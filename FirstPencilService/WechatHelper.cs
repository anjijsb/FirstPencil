using FirstPencilService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace FirstPencilService
{
    class WechatHelper
    {
        static public string GetToken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            var context = new ModelContext();
            var token = context.AccessTokenSet.First();

            //现有token是否可用
            if (DateTime.Now.Subtract(token.GetTime.Value).TotalSeconds < 7000)
            {
                return token.Token;
            }


            url = string.Format(url, System.Configuration.ConfigurationManager.AppSettings["appid"], System.Configuration.ConfigurationManager.AppSettings["appsecrect"]);
            #region  获得新的token
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url); ;
            req.Method = "GET";
            req.Timeout = 2000;

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);

            var retString = sr.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            var retDic = js.Deserialize<Dictionary<string, string>>(retString);

            string newToken = "";
            if (retDic.ContainsKey("access_token"))
            {
                newToken = retDic["access_token"];
            }
            #endregion
            token.Token = newToken;
            token.GetTime = DateTime.Now;
            context.SaveChanges();

            return newToken;
        }
        public bool WriteTxt(string str)
        {
            try
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/bugLog.txt"), FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入  
                sw.WriteLine(str + "\r\n" + DateTime.Now);
                //清空缓冲区  
                sw.Flush();
                //关闭流  
                sw.Close();
                fs.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        static public User GetUserInfo(User user)
        {
            var db = new ModelContext();
            //var user = db.UserSet.FirstOrDefault(u => u.OpenId == userOpenId);
            //if (userOpenId == null)
            //{
            //    throw new NotFindCustomerOpenIDException(userOpenId);
            //}

            //if (firm == null)
            //{
            //    throw new NotFindFirmTokenException();
            //}

            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            var access = GetToken();
            url = string.Format(url, access, user.OpenId);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Timeout = 2000;
            req.Method = "GET";

            var res = (HttpWebResponse)req.GetResponse();
            var s = res.GetResponseStream();
            var sr = new StreamReader(s);
            var resString = sr.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            var dic = js.Deserialize<Dictionary<string, string>>(resString);
            if (dic.Keys.Contains("errcode"))
            {
                return null;
            }

            user.NickName = dic["nickname"];
            user.Sex = dic["sex"] == "1" ? Sex.男 : dic["sex"] == "2" ? Sex.女 : Sex.未知;
            user.City = dic["city"];
            user.Country = dic["country"];
            user.Province = dic["province"];
            user.Language = dic["language"];
            user.Headimgurl = dic["headimgurl"];
            db.SaveChanges();

            return user;
        }


        static public ScanEvent GetCodeForMeeting(Salesman sm)
        {
            var db = new ModelContext();

            var ran = new Random();
            int ranNum = 0;
        getRan: ranNum = ran.Next(99999) + 100000;
            var ranStr = ranNum.ToString();
            if (db.ScanEventSet.Any(item => item.EventKey == ranStr))
            {
                goto getRan;
            }

            var eve = WechatHelper.GetCode(CodeType.QR_SCENE, ranNum.ToString());
            if (eve != null)
            {
                eve.IsActive = true;
                eve.Type = EventType.AddSalesman;
            }
            return eve;
        }


        static public ScanEvent GetCode(CodeType type, string eventKey)
        {
            var ret = new ScanEvent();
            ret.EventKey = eventKey;
            var at = WechatHelper.GetToken();

            string requrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + at;

            #region 获取新的二维码编号
            var ran = new Random();
            // getNumber: var number = ran.Next(99999);
            //if (db.CardSet.Any(c => c.EventNumber == number.ToString()))
            //{
            //    goto getNumber;
            //}
            #endregion

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requrl);

            string data = "";
            if (type == CodeType.QR_LIMIT_SCENE)
            {
                data = string.Format("{{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {{\"scene\": {{\"scene_id\": {0}}}}}}}", eventKey);
                ret.CodeType = CodeType.QR_LIMIT_SCENE;
            }
            else if (type == CodeType.QR_SCENE)
            {
                data = string.Format("{{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\": {{\"scene\": {{\"scene_id\": 123}}}}}}", eventKey);
                ret.CodeType = CodeType.QR_SCENE;
            }
            else
            {
                return null;
            }
            request.Timeout = 2000;
            request.Method = "POST";

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.Default;

            string res = client.UploadString(requrl, data);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var retDic = js.Deserialize<Dictionary<string, string>>(res);

            if (!retDic.Keys.Contains("ticket"))
            {
                return null;
            }
            ret.CodeUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + retDic["ticket"];
            ret.CodeContent = retDic["url"];

            return ret;
        }
    }
}
