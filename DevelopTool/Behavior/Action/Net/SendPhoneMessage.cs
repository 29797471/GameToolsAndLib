using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CqBehavior.Task
{
    [Editor("发短信")]
    [MenuItemPath("添加/网络/发短信")]
    public class SendPhoneMessage : CqBehaviorNode
    {
        Dictionary<int,string> dic = new Dictionary<int, string>()
            {
                { -1, "没有该用户账户"},
                { -2, "接口密钥不正确,不是账户登陆密码"},
                { -21,"MD5接口密钥加密不正确"},
                {-3,"短信数量不足" },
                { -11,"该用户被禁用"},
                {-14,"短信内容出现非法字符" },
                { -4,"手机号格式不正确"},
                {-41,"手机号码为空" },
                { -42,"短信内容为空"},
                {-51,"短信签名格式不正确,接口签名格式为：【签名内容】" },
                {-6,"IP限制" },
                {1,"短信发送数量:1" },
                {2,"短信发送数量:2" },
                {3,"短信发送数量:3" }
            };
        private string url = "http://utf8.api.smschinese.cn/?Uid={0}&key={1}&smsMob={2}&smsText={3}:8888";
        //private string url = "https://api.dingdongcloud.com/v1/sms/sendtz?apikey={1}&mobile={2}&content={3}";
        [Button, Click("OnSend")]
        [Priority(5)]
        public string Btn { get { return "发送"; } }

        public string m_PhoneNumber;
        [Priority(3,1)]
        [TextBox("手机号码"),Width(200)]
        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set { m_PhoneNumber = value; Update("PhoneNumber"); }
        }

        public string mApiKey;
        [Priority(2, 2)]
        [TextBox("接口秘钥"), Width(250)]
        public string ApiKey
        {
            get { return mApiKey; }
            set { mApiKey = value; Update("ApiKey"); }
        }
        public string mUser;
        [Priority(2, 1)]
        [TextBox("网建帐号"), Width(200)]
        public string User
        {
            get { return mUser; }
            set { mUser = value; Update("User"); }
        }
        [Priority(4)]
        [TextBox("短信内容"), MinHeight(150), MinWidth(250), TextWrapping(System.Windows.TextWrapping.Wrap), AcceptsReturn(true)]
        public string Content
        {
            get { return mContent; }
            set { mContent = value; Update("Content"); }
        }
        public string mContent;

        public void OnSend(object obj)
        {
            string targeturl = string.Format(url, User, ApiKey, PhoneNumber, Content).Trim().ToString();
            string strRet;
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet=ser.ReadToEnd();
                EventMgr.MsgPrint.Notify("结果:\t" + dic[int.Parse(strRet)], 5);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


