using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class SMS
    {
        ///<summary>

        ///采用https协议访问网络

        ///</summary>

        ///<param name="URL">url地址</param>

        ///<param name="strPostdata">发送的数据</param>

        ///<returns></returns>

        public static string OpenReadWithHttps(string URL, string strPostdata, string strEncoding)
        {

            Encoding encoding = Encoding.UTF8;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

            request.Method = "post";

            request.Accept = "text/html, application/xhtml+xml, */*";

            request.ContentType = "application/x-www-form-urlencoded";

            byte[] buffer = encoding.GetBytes(strPostdata);

            request.ContentLength = buffer.Length;

            request.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(strEncoding)))
            {

                return reader.ReadToEnd();
            }

        }

        ///<summary>

        ///采用https协议访问网络

        ///</summary>

        ///<param name="URL">url地址</param>

        ///<param name="strPostdata">发送的数据</param>

        ///<returns></returns>

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string sendSMS(string tel, string message, string serialNumber)
        {

            string url = "http://www.upunin.com.cn/sms.aspx";
            string userid = "98";
            string account = "dp_cjwh0317";
            string password = "A12345678";
            string content = message;
            string mobile = tel;
            string sendTime = "";
            string action = "send";
            string extno = "";
            string data = string.Format("userid={0}&account={1}&password={2}&content={3}&mobile={4}&sendTime={5}&action={6}&extno={7}",
                userid, account, password, content, mobile, sendTime, action, extno, extno);
 
            return OpenReadWithHttps(url, data, "UTF-8");

            //string url = "http://sms.api.ums86.com:8899/sms/Api/Send.do";
            //string SpCode = "108712";
            //string LoginName = "chuangjie";
            //string Password = "chuangjie2014";
            //string MessageContent = message;
            //string UserNumber = tel;
            //string ScheduleTime = "";
            //string SerialNumber = serialNumber;
            //string data = string.Format("SpCode={0}&LoginName={1}&Password={2}&MessageContent={3}&UserNumber={4}&ScheduleTime={5}&SerialNumber={6}", SpCode, LoginName, Password, MessageContent, UserNumber, ScheduleTime, SerialNumber);
            //return OpenReadWithHttps(url, data, "GB2312");
        }
    }
}
