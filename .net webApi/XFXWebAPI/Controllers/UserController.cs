using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using XFXClassLibrary;
using XFXClassLibrary.Model;

namespace XFXWebAPI.Controllers
{
    public class UserController : ApiController
    {

        #region registlogin
        [HttpPost]
        public string regist()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string tel = request["tel"];
            string password = request["password"];
            string yanzhengma = request["yanzhengma"];
            string xingbie = request["xingbie"];
            return new UserBLL().regist(tel, password, yanzhengma, xingbie);
        }

        [HttpPost]
        public string login()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string tel = request["tel"];
            string password = request["password"];

            return new UserBLL().login(tel, password);
        }

        [HttpPost]
        public string forget()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string tel = request["tel"];
            string yanzhengma = request["yanzhengma"];

            return new UserBLL().forget(tel, yanzhengma);

        }

        [HttpPost]
        public string chongzhimima()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string tel = request["tel"];
            string password = request["password"];
            string yanzhengma = request["yanzhengma"];
            return new UserBLL().chongzhimima(tel, password, yanzhengma);
        }

        [HttpPost]
        public string getyanzhengma()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string tel = request["tel"];
            string ip = Request.GetClientIpAddress();
            return new UserBLL().getyanzhengma(tel, ip);
        }
        #endregion

        [HttpPost]
        public User getUser()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (string.IsNullOrEmpty(authentication.state))
            {
                var v = new UserBLL().GetUserView(authentication.userID);
                if (v.Count > 0)
                {
                    var user = v.First();
                    user.HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + user.HeadPortrait;
                    return user;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public int qiandao()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            var data = DateTime.Now.AddDays(-1);

            using (Entity entity = new Entity())
            {
                var num = entity.UserGradeLog.Where(o => o.UserID == authentication.userID && o.CreateTime > data && o.Source == "签到").Count();
                if (num > 0)
                {
                    return 0;
                }

                new ExperienceLevelBLL().QianDao(authentication.userID);
                entity.SaveChanges();
            }
            return 20;

        }

        [HttpPost]
        public int setUserAddress()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }

            using (Entity entity = new Entity())
            {
                UserAddress userAddress = entity.UserAddress.Where(o => o.UserID == authentication.userID).FirstOrDefault();
                if (userAddress == null)
                {
                    userAddress = new UserAddress()
                    {
                        ProvincialCityAddress = request["provincecity"],
                        AddressInfo = request["xiangxidizhi"],
                        Tel = request["tel"],
                        Person = request["people"],
                        UserID = authentication.userID,
                        State = 1
                    };
                    entity.UserAddress.Add(userAddress);
                }
                else
                {
                    userAddress.ProvincialCityAddress = request["provincecity"];
                    userAddress.AddressInfo = request["xiangxidizhi"];
                    userAddress.Tel = request["tel"];
                    userAddress.Person = request["people"];
                }
                return entity.SaveChanges();
            }
        }

        [HttpPost]
        public UserAddress getUserAddress()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {
                var userAddress = entity.UserAddress.Where(o => o.UserID == authentication.userID).FirstOrDefault();
                if (userAddress == null)
                {
                    return null;
                }
                return new UserAddress()
                {
                    AddressInfo = userAddress.AddressInfo,
                    Tel = userAddress.Tel,
                    Person = userAddress.Person,
                    ProvincialCityAddress = userAddress.ProvincialCityAddress
                };
            }
        }

        [HttpPost]
        public UserView GetUserView()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            using (Entity entity = new Entity())
            {
                int userID = Convert.ToInt32(request["UserID"]);
                var user = entity.User.Include("UserExtend").Where(o => o.UserID == userID).FirstOrDefault();

                return new UserView()
                {
                    UserID = user.UserID.ToString(),
                    ExperienceValue = user.UserExtend.ExperienceValue.ToString(),
                    ExperienceLevel = user.UserExtend.ExperienceLevel.ToString(),
                    ExperienceName = user.UserExtend.ExperienceName.ToString(),


                    Bust = user.UserExtend.Bust.ToString(),
                    Waist = user.UserExtend.Waist.ToString(),
                    Hips = user.UserExtend.Hips.ToString(),
                    Stature = user.UserExtend.Stature.ToString(),
                    Weight = user.UserExtend.Weight.ToString(),
                    ProfessionalTitle = user.UserExtend.ProfessionalTitle.ToString(),
                    ProfessionalDescription = user.UserExtend.ProfessionalDescription.ToString(),
                    Tel = user.Tel.ToString(),
                    NickName = user.NickName.ToString(),

                    Gender = user.Gender.ToString(),
                    Married = user.Married.ToString(),
                    SexualOrientation = user.SexualOrientation.ToString(),
                    Age = user.Age.ToString(),
                    Location = user.Location.ToString(),
                    Constellation = user.Constellation.ToString(),
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + user.HeadPortrait.ToString(),
                    type = user.Type.getUserType().ToString(),

                };
            }
        }

        [HttpPost]
        public int UpdateUserView()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }

            using (Entity entity = new Entity())
            {
                var user = entity.User.Include("UserExtend").Where(o => o.UserID == authentication.userID).FirstOrDefault();

                if (request["Bust"] != null)
                {
                    user.UserExtend.Bust = request["Bust"];
                }
                if (request["Waist"] != null)
                {
                    user.UserExtend.Waist = request["Waist"];
                }
                if (request["Hips"] != null)
                {
                    user.UserExtend.Hips = request["Hips"];
                }
                if (request["Stature"] != null)
                {
                    user.UserExtend.Stature = request["Stature"];
                }
                if (request["Weight"] != null)
                {
                    user.UserExtend.Weight = request["Weight"];
                }
                if (request["ProfessionalTitle"] != null)
                {
                    user.UserExtend.ProfessionalTitle = request["ProfessionalTitle"];
                }
                if (request["ProfessionalDescription"] != null)
                {
                    user.UserExtend.ProfessionalDescription = request["ProfessionalDescription"];
                }
                if (request["NickName"] != null)
                {
                    user.NickName = request["NickName"];
                }
                if (request["Gender"] != null)
                {
                    user.Gender = request["Gender"];
                }
                if (request["Married"] != null)
                {
                    user.Married = request["Married"];
                }
                if (request["SexualOrientation"] != null)
                {
                    user.SexualOrientation = request["SexualOrientation"];
                }
                if (request["Age"] != null)
                {
                    user.Age = request["Age"];
                }
                if (request["Location"] != null)
                {
                    user.Location = request["Location"];
                }
                if (request["Constellation"] != null)
                {
                    user.Constellation = request["Constellation"];
                }
                try
                {
                    entity.SaveChanges();
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }

        }

        [HttpPost]
        public List<UserBlackView> GetUsersBlack()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {
                return entity.UserBlacklist.Where(o => o.InitiativeUserID == authentication.userID).ToList().Select(o => new UserBlackView()
                {
                    UserID = o.User1.UserID,
                    NickName = o.User1.NickName,
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User1.HeadPortrait
                }).ToList();
            }
        }

        // 传参 PassivityUserID
        [HttpPost]
        public int DeleteUsersBlack()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }

            using (Entity entity = new Entity())
            {
                int PassivityUserID = Convert.ToInt32(request["PassivityUserID"]);
                var userBlacklist = entity.UserBlacklist.Where(o => o.InitiativeUserID == authentication.userID && o.PassivityUserID == PassivityUserID).FirstOrDefault();
                if (userBlacklist == null)
                {
                    return 0;
                }
                entity.UserBlacklist.Remove(userBlacklist);
                return entity.SaveChanges();
            }
        }

        // 传参 PassivityUserID
        [HttpPost]
        public int AddUsersBlack()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int PassivityUserID = Convert.ToInt32(request["PassivityUserID"]);
            DateTime dateTime = DateTime.Now;

            using (Entity entity = new Entity())
            {
                var length = entity.UserBlacklist.Where(o => o.InitiativeUserID == authentication.userID && o.PassivityUserID == PassivityUserID).Count();
                if (length > 0)
                {
                    return 1;
                }
                entity.UserBlacklist.Add(new UserBlacklist()
                {
                    InitiativeUserID = authentication.userID,
                    PassivityUserID = PassivityUserID,
                    CreateTime = dateTime

                });
                return entity.SaveChanges();
            }
        }

        [HttpPost]
        public int AddComplaint()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int PassivityUserID = Convert.ToInt32(request["PassivityUserID"]);
            Complaint complaint = new Complaint();
            complaint.InitiativeUserID = authentication.userID;
            complaint.PassivityUserID = PassivityUserID;
            complaint.Type = 1;
            complaint.Time = DateTime.Now;
            complaint.Reason = request["Reason"];

            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(PassivityUserID);
                user.UserExtend.ComplaintSum++;
                user.UserExtend.ComplaintUntreated++;
                entity.Complaint.Add(complaint);
                return entity.SaveChanges();
            }
        }


        [HttpPost]
        public string UpdateUserImg()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return "";
            }

            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(authentication.userID);
                user.HeadPortrait = request["Img"].Replace(ConfigurationManager.AppSettings["UploadUrl"], "");
                entity.SaveChanges();

                return ConfigurationManager.AppSettings["UploadUrl"] + user.HeadPortrait;
            }
        }

    }
}
