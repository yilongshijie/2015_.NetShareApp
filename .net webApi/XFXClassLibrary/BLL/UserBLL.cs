using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class UserBLL
    {
        private UserDAL userDAL = new UserDAL();

        #region 查询
        public User GetUser(int userID)
        {
            return userDAL.GetUser(userID);
        }
        public List<User> GetUserView(int userID)
        {
            return userDAL.GetUserView(userID);
        }


        public List<User> GetUsers(Func<User, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<User, object> orderBy = null, string sort = "DESC")
        {
            return userDAL.GetUsers(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }

        public List<User> GetUsers(Func<User, bool> delegation, Func<User, object> orderBy = null)
        {
            return userDAL.GetUsers(delegation);
        }


        #endregion

        #region 修改
        /// <summary>
        /// 更新为体验师
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int UpdateExperience(int userID)
        {
            return userDAL.AddState(userID, 2);
        }
        /// <summary>
        /// 更新为咨询师
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int UpdateCounselor(int userID)
        {
            return userDAL.AddState(userID, 4);
        }
        /// <summary>
        /// 去除体验师
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int RemoveExperience(int userID)
        {
            return userDAL.RemoveState(userID, 2);
        }
        /// <summary>
        /// 去除咨询师
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int RemoveCounselor(int userID)
        {
            return userDAL.RemoveState(userID, 4);
        }
        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int Freeze(int userID)
        {
            return userDAL.UpdateType(userID, 0);
        }
        /// <summary>
        /// 解冻
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int RemoveFreeze(int userID)
        {
            return userDAL.UpdateType(userID, 1);
        }
        #endregion

        #region 假数据


        #endregion

        #region 转换
        public string TypeText(int type)
        {
            List<string> typeText = new List<string>();

            if ((type & 2) > 0)
            {
                typeText.Add("体验师");
            }
            if ((type & 4) > 0)
            {
                typeText.Add("咨询师");
            }
            if ((type & 8) > 0)
            {
                typeText.Add("系统管理员");
            }
            if (typeText.Count == 0)
            {
                typeText.Add("普通用户");
            }

            return string.Join("、", typeText);
        }

        public String GetCounterfeit(int type)
        {
            if ((type & (int)Math.Pow(2, 30)) > 0)
                return "假";
            else
                return "真";
        }
        public String StateText(int state)
        {
            if (state == 0)
                return "冻结";
            else
                return "正常";
        }

        public string ComplaintUntreatedText(int userID, int top)
        {
            using (Entity entity = new Entity())
            {
                var list = entity.Complaint.Where(o => o.PassivityUserID == userID).Take(top).ToList(); ;
                string s = "";
                foreach (var item in list)
                {
                    s += item.Reason + ",";
                }
                return s;
            }
        }
        #endregion


        #region api

        public string regist(string tel, string password, string yanzhengma, string xingbie)
        {

            if (tel == null)
            {
                return "{error:'电话号码不能为空'}";
            }
            if (yanzhengma == null)
            {
                return "{error:'验证码不能为空'}";
            }
            if (xingbie == null || (xingbie != "男" && xingbie != "女"))
            {
                return "{error:'性别不正确'}";
            }
            string pattern = @"^(0|86|17951)?(1[234578])[0-9]{9}$";
            Regex rgx = new Regex(pattern);
            if (!rgx.IsMatch(tel))
            {
                return "{error:'电话号不正确'}";
            }
            if (password == null)
            {
                return "{error:'密码不能为空'}";
            }
            if (password.Length < 6)
            {
                return "{error:'密码长度不能小于6'}";
            }

            using (Entity entity = new Entity())
            {
                UserSMS userSMS = entity.UserSMS
            .Where(o => o.Tel == tel && o.State == 0)
            .OrderByDescending(o => o.SentTime)
            .FirstOrDefault();

                if (userSMS == null)
                {
                    return "{error:'验证码不正确'}";
                }
                if (entity.User.Where(o => o.Tel == tel).Count() > 0)
                {
                    return "{error:'用户已经存在'}";
                }
                string passwordTemp = CommonSecurity.SHA1MD5MD5(password);
                User user = new User()
                {
                    Tel = tel,
                    PassWord = passwordTemp,
                    NickName = "分享玩家",
                    CreatTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    State = 1,
                    Gender = xingbie
                };
                entity.User.Add(user);
                user.UserExtend = new UserExtend();
                user.UserExtend.Banned = 0;
                userSMS.State = 1;

                user.UserExtend.ExperienceLevel = 1;
                ExperienceLevel experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceLevelValue == user.UserExtend.ExperienceLevel).FirstOrDefault();
                user.UserExtend.ExperienceValue = experienceLevel.ExperienceValueMin;
                if (user.Gender == "男")
                {
                    user.UserExtend.ExperienceName = experienceLevel.NameMan;
                }
                else
                {
                    user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                }
                user.InitBeforeSave();
                entity.SaveChanges();
                Authentication authentication = new Authentication(user.Tel, password);
                if (!string.IsNullOrEmpty(authentication.state))
                {
                    return authentication.state;
                }
                return authentication.tokenUserTime();

            }
        }



        public string login(string tel, string password)
        {
            if (tel == null)
            {
                return "{error:'电话号码不能为空'}";
            }
            string pattern = @"^(0|86|17951)?(1[234578])[0-9]{9}$";
            Regex rgx = new Regex(pattern);
            if (!rgx.IsMatch(tel))
            {
                return "{error:'电话号不正确'}";
            }
            if (password == null)
            {
                return "{error:'密码不能为空'}";
            }
            if (password.Length < 6)
            {
                return "{error:'密码长度不能小于6'}";
            }
            string passwordTemp = CommonSecurity.SHA1MD5MD5(password);

            using (Entity entity = new Entity())
            {
                User user = entity.User
                    .Where(o => o.Tel == tel && o.PassWord == passwordTemp)
                    .FirstOrDefault();
                if (user == null)
                {
                    return "{error:'账号或密码不正确'}";
                }

                Authentication authentication = new Authentication(user.Tel, password);
                if (!string.IsNullOrEmpty(authentication.state))
                {
                    return "{error:'" + authentication.state + "'}";
                }
                return authentication.tokenUserTime();
            }
        }


        public string forget(string tel, string yanzhengma)
        {

            if (tel == null)
            {
                return "{error:'电话号码不能为空'}";
            }
            if (yanzhengma == null)
            {
                return "{error:'验证码不能为空'}";
            }
            using (Entity entity = new Entity())
            {

                UserSMS userSMS = entity.UserSMS
            .Where(o => o.Tel == tel && o.State == 0)
            .OrderByDescending(o => o.SentTime)
            .FirstOrDefault();
                if (userSMS == null)
                {
                    return "{error:'验证码不正确'}";
                }
            }
            return "{message:'成功'}";

        }


        public string chongzhimima(string tel, string password, string yanzhengma)
        {

            if (tel == null)
            {
                return "{error:'电话号码不能为空'}";
            }
            if (yanzhengma == null)
            {
                return "{error:'验证码不能为空'}";
            }

            string pattern = @"^(0|86|17951)?(1[234578])[0-9]{9}$";
            Regex rgx = new Regex(pattern);
            if (!rgx.IsMatch(tel))
            {
                return "{error:'电话号不正确'}";
            }
            if (password == null)
            {
                return "{error:'密码不能为空'}";
            }
            if (password.Length < 6)
            {
                return "{error:'密码长度不能小于6'}";
            }

            using (Entity entity = new Entity())
            {
                UserSMS userSMS = entity.UserSMS
            .Where(o => o.Tel == tel && o.State == 0)
            .OrderByDescending(o => o.SentTime)
            .FirstOrDefault();
                if (userSMS == null)
                {
                    return "{error:'验证码不正确'}";
                }
                User user = entity.User
                        .Where(o => o.Tel == tel)
                        .FirstOrDefault();
                if (user == null)
                {
                    return "{error:'账号不存在'}";
                }

                string passwordTemp = CommonSecurity.SHA1MD5MD5(password);
                user.PassWord = passwordTemp;
                userSMS.State = 1;
                entity.SaveChanges();
                Authentication authentication = new Authentication(user.Tel, password);
                return authentication.tokenUserTime();
            }
        }

        public string getyanzhengma(string tel, string ip)
        {

            DateTime dateTime = DateTime.Now;

            using (Entity entity = new Entity())
            {
                DateTime dt = dateTime.AddDays(-1);
                var registerList = entity.UserSMS.Where(o => o.Ip == ip && o.SentTime > dt);

                if (registerList.Count() > 0)
                {
                    if (registerList.Count() >= 20)
                    {
                        return "{error:'今天接收验证码数目已达上限'}";
                    }
                }
                registerList = entity.UserSMS.Where(o => o.Tel == tel && o.SentTime > dt);

                if (registerList.Count() > 0)
                {
                    if (registerList.Count() >= 20)
                    {
                        return "{error:'该手机号码今天接收验证码数目已达上限'}";
                    }
                }
                Random random = new Random();
                string yzm = random.Next(1000, 9999).ToString();

                UserSMS userSMS = new UserSMS() { Ip = ip, Tel = tel, SentTime = dateTime, Yanzhengma = yzm, State = 0 };
                userSMS.SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff" + random.Next(100, 999).ToString());

                entity.UserSMS.Add(userSMS);
                entity.SaveChanges();
                ThreadPool.QueueUserWorkItem(delegate (object a)
                {
                    string tt = @"您的验证码是" + userSMS.Yanzhengma + " 请在10分钟内使用!";
                    string bb = SMS.sendSMS(userSMS.Tel, tt, userSMS.SerialNumber);
                });
            }
            return "{message:'成功'}";

        }
        #endregion

    }





}
