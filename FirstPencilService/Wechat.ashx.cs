using FirstPencilService;
using FirstPencilService.Models;
//using FirstPencil.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace FirstPencil
{
    /// <summary>
    /// Wechat 的摘要说明
    /// </summary>
    public class Wechat : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (HttpContext.Current.Request.HttpMethod.ToLower() == "post")
            {
                var db = new ModelContext();
                string ret = "";
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                var postStr = Encoding.UTF8.GetString(b);

                WriteTxt(postStr);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(postStr);
                XmlElement rootElement = doc.DocumentElement;

                XmlNode MsgType = rootElement.SelectSingleNode("MsgType");

                RequestXML requestXML = new RequestXML();
                requestXML.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                requestXML.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                requestXML.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                requestXML.MsgType = MsgType.InnerText;



                if (requestXML.MsgType == "event")
                {
                    WriteTxt(requestXML.Event);
                    requestXML.Event = rootElement.SelectSingleNode("Event").InnerText;
                    var node = rootElement.SelectSingleNode("EventKey");
                    requestXML.EventKey = node == null ? "" : node.InnerText;
                    if (requestXML.Event.ToLower() == "subscribe" || requestXML.Event.ToLower() == "scan")
                    {
                        //查找user 没找到就插入
                        User user = db.UserSet.FirstOrDefault(u => u.OpenId == requestXML.FromUserName);
                        if (user == null)
                        {
                            user = new User
                            {
                                OpenId = requestXML.FromUserName,
                                Point = 0,
                                IsSalesman = false,
                                SubscribeTime = DateTime.Now,
                            };
                            db.UserSet.Add(user);
                        }


                        db.SaveChanges();
                        //get user info
                        //var t = new System.Threading.Tasks.Task(() => WechatHelper.GetUserInfo(user));
                        //t.Start();

                        WechatHelper.GetUserInfo(user);

                        if (!string.IsNullOrEmpty(requestXML.EventKey))
                        {

                            requestXML.EventKey = requestXML.EventKey.Replace("qrscene_", "");
                            var eve = db.ScanEventSet.FirstOrDefault(item => item.EventKey == requestXML.EventKey && item.IsActive == true);
                            if (eve != null)
                            {
                                if (eve.Type == EventType.AddSalesman)
                                {
                                    var smId = int.Parse(eve.Remarks);
                                    var sm = db.SalesmanSet.FirstOrDefault(item => item.SalesmanId == smId);
                                    var meetingAddress = System.Configuration.ConfigurationManager.AppSettings["meetingurl"];
                                    if (sm != null)
                                    {
                                        user.IsSalesman = true;
                                        user.SalesmanId = smId;
                                        eve.IsActive = false;
                                        //db.SaveChanges();
                                        ret = string.Format("您好，{0}。欢迎莅临本次80周年主题活动，请点击查看<a href=\"{1}\">大会议程</a>。直接回复您的祝福，参与互动，赢得奖品。", sm.Name, meetingAddress);
                                        user.Point = db.AddPointSet.FirstOrDefault(item => item.Type == AddPointType.RegisterSalesman).Count;
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                ret = string.Format("Hi~{0},欢迎关注中华文具。", user.NickName);
                            }

                            //ret = Helper.WechatHelper.GetEventString(requestXML.FromUserName, requestXML.ToUserName, requestXML.EventKey);
                        }
                        else
                        {
                            ret = "event key is null!";
                        }
                    }

                }// end event if
                else if (requestXML.MsgType == "text")
                {
                    requestXML.Content = rootElement.SelectSingleNode("Content").InnerText.Trim();
                    var user = db.UserSet.FirstOrDefault(item => item.OpenId == requestXML.FromUserName);
                    if (user == null)
                    {

                    }
                    if (requestXML.Content.Length < 31)
                    {
                        db.DiscussMsgSet.Add(new DiscussMsg
                        {
                            UserId = user.UserId,
                            CreateDate = DateTime.Now,
                            Content = requestXML.Content,
                        });
                        ret = "感谢您的留言。";
                        db.SaveChanges();
                    }
                    else
                    {
                        ret = "留言长度不能超过30个字，请精简之后重新发送。";
                    }
                }
                HttpContext.Current.Response.Write(checkXML(requestXML, ret));
                HttpContext.Current.Response.End();
            }
            else
            {
                string echoString = HttpContext.Current.Request.QueryString["echostr"];
                string signature = HttpContext.Current.Request.QueryString["signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];

                if (!string.IsNullOrEmpty(echoString))
                {

                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }

        }
        /// <summary>
        /// 整理文字消息格式
        /// </summary>
        /// <param name="requestXML"></param>
        /// <param name="mesg"></param>
        /// <returns></returns>
        private string checkXML(RequestXML requestXML, string mesg)
        {
            string now = "";
            now = "<xml><ToUserName><![CDATA[" + requestXML.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + requestXML.ToUserName + "]]></FromUserName><CreateTime>" + ConvertDateTimeInt(DateTime.Now) +
                                "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" +
                                mesg +
                                "]]></Content><FuncFlag>0</FuncFlag></xml>";
            return now;
        }

        /// <summary>  
        /// datetime转换为unixtime  
        /// </summary>  
        /// <param name="time"></param>  
        /// <returns></returns>  
        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }



        /// <summary>
        /// 1. 将token、timestamp、nonce三个参数进行字典序排序
        /// 2. 将三个参数字符串拼接成一个字符串进行sha1加密
        /// 3. 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        /// </summary>
        /// <param name="firmToken">厂家Token</param>
        /// <param name="signature">密文</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        private bool Verity(string firmToken, string signature, string timestamp, string nonce)
        {

            string[] tArr = new string[] { firmToken, timestamp, nonce };
            Array.Sort(tArr);
            string tString = string.Join("", tArr);
            string res = FormsAuthentication.HashPasswordForStoringInConfigFile(tString, "SHA1").ToLower();
            return signature == res;
        }

        /// <summary>  
        /// 记录bug，以便调试  
        /// </summary>  
        /// <returns></returns>  
        private bool WriteTxt(string str)
        {
            try
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("/bugLog.txt"), FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入  
                sw.WriteLine(str + "\r\n" + DateTime.Now, ToString() + "\r\n" + "\r\n");
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


        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }
    }


    public class RequestXML
    {

        public string ToUserName;
        public string FromUserName;
        public string CreateTime;
        public string MsgType;
        public string Location_X;
        public string Location_Y;
        public string Scale;
        public string Label;
        public string Content;
        public string PicUrl;
        public string EventKey;
        public string Event;
        public string Precision;
    }

}