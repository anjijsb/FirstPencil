using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace FirstPencilWeb.Helps
{
    /// <summary>
    /// 微信操作类
    /// </summary>
    public class WeiXinHelpers
    {
        #region 获取用户信息

        public static WeiXinUserSampleInfo GetUserSamplerInfo(string code)
        {
            string url = string.Format(WeiXinConst.WeiXin_User_OpenIdUrl, code);

            WeiXinUserSampleInfo info = HttpClientHelper.GetResponse<WeiXinUserSampleInfo>(url);

            return info;
        }

        /// <summary>
        /// 根据用户Code获取用户信息（包括OpenId的简单信息）
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string GetUserOpenId(string code)
        {
            return GetUserSamplerInfo(code).OpenId;
        }

        /// <summary>
        /// 根据OpenId获取用户基本信息（需关注公众号）
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WeiXinUserInfo GetUserInfo(string openId)
        {
            var token = AccessToken.Instance;

            string url = string.Format(WeiXinConst.WeiXin_User_GetInfoUrl, token.Access_Token, openId);

            string result = HttpClientHelper.GetResponse(url);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            WeiXinUserInfo info = JsonConvert.DeserializeObject<WeiXinUserInfo>(result);

            //解析用户信息失败，判断 失败Code ，40001 为AccessToken失效，重新创建Token并获取用户信息]
            if (info == null || string.IsNullOrEmpty(info.OpenId))
            {
                return GetUserInfoByNewAccessToken(openId);
            }
            else
            {
                return info;
            }
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static string GetJsapiTicket(string access_token)
        {
            access_token = access_token.Replace("\"", "");
            string url = string.Format(WeiXinConst.Jsapi_TicketUrl, access_token);
            string result = HttpClientHelper.GetResponse(url);
            if (string.IsNullOrEmpty(result))
            {
                return null;
            }
            WeiXinTicket info = JsonConvert.DeserializeObject<WeiXinTicket>(result);
            if (info == null)
            {
                return null;
            }
            else
            {
                return info.ticket;
            }
        }




        /// <summary>
        /// 创建新的AccessToken并或回去用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private static WeiXinUserInfo GetUserInfoByNewAccessToken(string openId)
        {
            var token = AccessToken.Instance;
            string url = string.Format(WeiXinConst.WeiXin_User_GetInfoUrl, token.Access_Token, openId);
            WeiXinUserInfo info = HttpClientHelper.GetResponse<WeiXinUserInfo>(url);
            return info;
        }



        #endregion
    }

    /// <summary>
    /// AccessToken类，公众号通过此token 获取相关信息 （单例类）
    /// </summary>
    public sealed class AccessToken
    {
        private static AccessToken _Token = new AccessToken();

        private static object lock_Object = new object();

        /// <summary>
        /// 此处 会判断是否过期，没过期返回原存储的Token
        /// </summary>
        public static AccessToken Instance
        {
            get
            {
                if (_Token.Expired)
                {
                    lock (lock_Object)
                    {
                        if (_Token.Expired)
                        {
                            _Token.CreateTime = DateTime.Now;
                            _Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(HttpClientHelper.GetAccessToken()));
                        }
                    }
                }

                return _Token;
            }
        }

        /// <summary>
        /// 此处会创建新的Token返回，只有在调用接口提示AccessToken过期时 才调用这个接口。
        /// </summary>
        /// <returns></returns>
        public static AccessToken CreateNewInstance()
        {
            lock (lock_Object)
            {
                _Token.CreateTime = DateTime.Now;
                _Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(HttpClientHelper.GetAccessToken()));
            }
            return _Token;
        }

        private AccessToken()
        {
            CreateTime = DateTime.Now;
            _expiresIn = -1;
        }

        private string _accessToken;
        private int _expiresIn;
        public DateTime CreateTime;
        public string Access_Token
        {
            get { return _accessToken; }
            set { _accessToken = value; }
        }

        /// <summary>
        /// 有效时间，秒
        /// </summary>
        public int Expires_In
        {
            get { return _expiresIn; }
            set { _expiresIn = value; }
        }

        public bool Expired
        {
            get
            {
                DateTime expiredTime = CreateTime.AddSeconds(_expiresIn);

                if (DateTime.Now > expiredTime)
                    return true;

                return false;
            }
        }

        public void CopyModel(AccessToken token)
        {
            //token 为空，将 过期时间 设置为 -1
            if (token == null)
            {
                this.Expires_In = -1;
                return;
            }

            this.Access_Token = token.Access_Token;
            this.Expires_In = token.Expires_In;

        }
    }


    /// <summary>
    /// 微信 需要用到的URL\JSON常量
    /// </summary>
    public class WeiXinConst
    {
        #region Value Const

        /// <summary>
        /// 微信开发者 AppId
        /// </summary>
        public const string AppId = "wx13b3ff8fdcc0d04f";


        /// <summary>
        /// 微信开发者 Secret
        /// </summary>
        public const string Secret = "0bef2378e056169f55daa426aa7fadbc";


        /// <summary>
        /// V2:支付请求中 用于加密的秘钥Key，可用于验证商户的唯一性，对应支付场景中的AppKey
        /// </summary>
        public static string PaySignKey = "V2.PaySignKey";


        /// <summary>
        /// V2:财付通签名key
        /// V3:商户支付密钥 Key。登录微信商户后台，进入栏目【账户设置】 【密码安全 】【API 安全】 【API 密钥】 ，进入设置 API 密钥。
        /// </summary>
        public const string PartnerKey = "PartnerKey";

        /// <summary>
        /// 商户号
        /// </summary>
        public const string PartnerId = "PartnerId";


        /// <summary>
        /// 百度地图Api  Ak
        /// </summary>
        public const string BaiduAk = "BaiduAk";

        /// <summary>
        /// 用于验证 请求 是否来自 微信
        /// </summary>
        public const string Token = "Token";

        /// <summary>
        /// 证书文件 路径
        /// </summary>
        public const string CertPath = @"E:\cert\apiclient_cert.pem";


        /// <summary>
        /// 证书文件密码（默认为商户号）
        /// </summary>
        public const string CertPwd = "111";

        #endregion

        #region Url Const

        #region AccessTokenUrl

        /// <summary>
        /// 公众号 获取Access_Token的Url(需Format  0.AppId 1.Secret)
        /// </summary>
        private const string AccessToken_Url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 公众号 获取Token的Url
        /// </summary>
        public static string WeiXin_AccessTokenUrl { get { return string.Format(AccessToken_Url, AppId, Secret); } }

        /// <summary>
        /// 公众号 获取Jsapi_Ticket的url
        /// </summary>
        public const string Jsapi_TicketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";


        #endregion

        #region 获取用户信息Url

        /// <summary>
        /// 根据Code 获取用户OpenId Url
        /// </summary>
        private const string User_GetOpenIdUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 根据Code 获取用户OpenId的Url 需要Format 0.code
        /// </summary>
        public static string WeiXin_User_OpenIdUrl { get { return string.Format(User_GetOpenIdUrl, AppId, Secret, "{0}"); } }

        /// <summary>
        /// 根据OpenId 获取用户基本信息 Url（需要Format0.access_token 1.openid）
        /// </summary>
        public const string WeiXin_User_GetInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        #endregion

        #region OAuth2授权Url

        /// <summary>
        /// OAuth2授权Url，需要Format0.AppId  1.Uri  2.state
        /// </summary>
        private const string OAuth2_Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        /// <summary>
        /// OAuth2授权Url，需要Format  0.Uri  1.state
        /// </summary>
        public static string WeiXin_User_OAuth2Url { get { return string.Format(OAuth2_Url, AppId, "{0}", "{1}"); } }

        #endregion

        #region QrCode Url

        /// <summary>
        /// 创建获取QrCode的Ticket Url  需要Format 0 access_token
        /// </summary>
        public const string WeiXin_Ticket_CreateUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码图片Url,需要Format 0.ticket
        /// </summary>
        public const string WeiXin_QrCode_GetUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        #endregion

        #region Baidu 逆地理编码Url

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.ak  1.经度  2.纬度
        /// </summary>
        private const string BaiduGeoCoding_ApiUrl = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=json&pois=0";

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.经度  1.纬度 
        /// </summary>
        public static string Baidu_GeoCoding_ApiUrl
        {
            get
            {
                return string.Format(BaiduGeoCoding_ApiUrl, BaiduAk, "{0}", "{1}");
            }
        }

        #endregion

        #region Menu Url

        /// <summary>
        /// 创建菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_CreateUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 获取菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_GetUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        /// 删除菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXin_Menu_DeleteUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";


        #endregion

        #region 支付相关Url

        /// <summary>
        /// 生成预支付账单Url ，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_PrePayUrl = "https://api.weixin.qq.com/pay/genprepay?access_token={0}";

        /// <summary>
        /// 订单查询Url ，需替换0 access_token
        /// </summary>
        public const string WeiXin_Pay_OrderQueryUrl = "https://api.weixin.qq.com/pay/orderquery?access_token={0}";

        /// <summary>
        /// 发货通知Url，需替换 0 access_token
        /// </summary>
        public const string WeiXin_Pay_DeliverNotifyUrl = "https://api.weixin.qq.com/pay/delivernotify?access_token={0}";

        #region 统一支付相关Url （V3接口）

        /// <summary>
        /// 统一预支付Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 订单查询Url 
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 退款申请Url
        /// </summary>
        public const string WeiXin_Pay_UnifiedOrderRefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        #endregion


        #endregion

        #endregion

        #region Json Const

        /// <summary>
        /// 获取二维码 所需Ticket 需要上传的Json字符串（需要Format 0.scene_id）
        /// </summary>
        /// <remarks>scene_id场景值ID  永久二维码时最大值为100000（目前参数只支持1--100000）</remarks>
        public const string WeiXin_QrCodeTicket_Create_JsonString = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":{0}}}}";

        #endregion
    }

    /// <summary>
    /// 用户详细信息
    /// </summary>
    public class WeiXinUserInfo
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Conuntry { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string Subscribe_Time { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string UnionId { get; set; }
    }

    /// <summary>
    /// jsapi_ticket
    /// </summary>
    public class WeiXinTicket
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public string expires_in { get; set; }
    }

    /// <summary>
    /// 只包含OpenId的用户信息
    /// </summary>
    public class WeiXinUserSampleInfo
    {
        public string Access_Token { get; set; }

        public string Expires_In { get; set; }

        public string Refresh_Token { get; set; }

        public string OpenId { get; set; }

        public string Scope { get; set; }
    }

    /// <summary>
    /// 包含通过HttpClient发起get或post请求的方法，所有调用微信接口的操作都通过此类。
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        public static T GetResponse<T>(string url)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            T result = default(T);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class,new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(xmlString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpClient httpClient = new HttpClient();

            T result = default(T);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = XmlDeserialize<T>(s);
            }
            return result;
        }

        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class,new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }

        /// <summary>
        /// 获取Access_Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            string url = WeiXinConst.WeiXin_AccessTokenUrl;
            string result = HttpClientHelper.GetResponse(url);
            return result;
        }
    }
}