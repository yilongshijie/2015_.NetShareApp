using System;
using System.Linq;
using System.Web;

namespace XFXClassLibrary
{
    public class Authentication
    {
        public int userID;
        public string time;
        public string token;
        public static string seed = "08129asd817246iop172375@#$65123+_+(*";
        public string state;
        public int userType;
        public Authentication(HttpRequestBase request)
        {

            string token = request["authentication_token"];
            string userID = request["authentication_userID"];
            string time = request["authentication_time"];
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(token))
            {
                state = "缺少请求参数";
                return;
            }
            if (!int.TryParse(userID, out this.userID))
            {
                state = "用户ID不正确";
                return;
            }
            this.time = time;
            this.token = token;
            string source = userID + "^" + time + "^" + seed;
            if (token != CommonSecurity.SHA1(source))
            {
                state = "登陆失败";
                return;
            }
        }

        public Authentication(string userID, string time, string token)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(token))
            {
                state = "缺少请求参数";
                return;
            }
            if (!int.TryParse(userID, out this.userID))
            {
                state = "用户ID不正确";
                return;
            }
            this.time = time;
            this.token = token;
            string source = userID + "^" + time + "^" + seed;
            if (token != CommonSecurity.SHA1(source))
            {
                throw new Exception();
            }
        }

        public Authentication(string tel, string password)
        {
            if (string.IsNullOrEmpty(tel) || string.IsNullOrEmpty(password))
            {
                state = "缺少请求参数";
                return;
            }
            password = CommonSecurity.SHA1MD5MD5(password);
            using (Entity entity = new Entity())
            {
                var user = entity.User.Where(o => o.Tel == tel && o.PassWord == password).FirstOrDefault();
                if (user == null)
                {
                    state = "用户名或密码不对";
                    return;
                }
                if (user.State == 0)
                {
                    state = "该用户已经冻结";
                    return;
                }
                this.userID = user.UserID;
                this.time = CommonTime.GetTimeStamp();
                string source = this.userID + "^" + this.time + "^" + seed;
                this.token = CommonSecurity.SHA1(source);
                this.userType = user.Type;
            }

        }

        public string tokenUserTime()
        {
            return string.Format("{{authentication_token:'{0}',authentication_userID:'{1}',authentication_time:'{2}',authentication_userType:'{3}'}}",
 token, userID, time, userType);
        }


    }
}