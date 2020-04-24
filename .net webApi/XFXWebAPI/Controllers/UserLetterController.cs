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
    public class UserLetterController : ApiController
    {

        UserLetterBLL userLetterBLL = new UserLetterBLL();
        [HttpPost]
        public List<UserLetter> GetUserLetterList()
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
                return userLetterBLL.GetUserLetterToplevel(authentication.userID).Select(o => new UserLetter()
                {
                    InitiativeUserID = o.InitiativeUserID,
                    PassivityUserID = o.PassivityUserID,
                    Text = o.Text.removeTag(),
                    Type = o.Type,
                    CreateTime = o.CreateTime,
                    User =
                   entity.User.Where(oo => oo.UserID == o.InitiativeUserID).ToList().Select(oo =>
                     new User()
                     {
                         HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + oo.HeadPortrait,
                         UserID = oo.UserID,
                         NickName = oo.NickName
                     }
                   ).First()
                 ,
                    UserLetterID = o.UserLetterID,
                    State = o.State
                }).ToList();
            }
        }
        [HttpPost]
        public List<UserLetter> GetUserLetter()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }
            int ID = Convert.ToInt32(request["ID"]);

            using (Entity entity = new Entity())
            {
                var userLetter = entity.UserLetter.Find(ID);
                var user = entity.User.Include("UserExtend").Where(o => o.UserID == authentication.userID).ToList().Select(o => new User()
                {
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.HeadPortrait,
                    UserID = o.UserID,
                    NickName = o.NickName,
                    UserExtend = new UserExtend()
                    {
                        ExperienceLevel = o.UserExtend.ExperienceLevel,
                        ExperienceName = o.UserExtend.ExperienceName,
                    }
                }).First();
                var user1 = entity.User.Include("UserExtend").Where(o => o.UserID == userLetter.InitiativeUserID).ToList().Select(o => new User()
                {
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.HeadPortrait,
                    UserID = o.UserID,
                    NickName = o.NickName,
                    UserExtend = new UserExtend()
                    {
                        ExperienceLevel = o.UserExtend.ExperienceLevel,
                        ExperienceName = o.UserExtend.ExperienceName,
                    }
                }).First();
                var templist = userLetterBLL.GetUserLetter(o => (o.PassivityUserID == authentication.userID && o.InitiativeUserID == userLetter.InitiativeUserID) ||
                 o.PassivityUserID == userLetter.InitiativeUserID && o.InitiativeUserID == authentication.userID, "ASC"
                ).Select(o => new UserLetter()
                {
                    InitiativeUserID = o.InitiativeUserID,
                    PassivityUserID = o.PassivityUserID,
                    Text = o.Text.removeTag(),
                    Type = o.Type,
                    CreateTime = o.CreateTime,
                    User = ((o.InitiativeUserID == user.UserID) ? user : user1),
                    State = o.State,
                    UserLetterID = o.UserLetterID,
                    CirclePostID = o.CirclePostID
                }).ToList();
                var stateList = templist.Where(o => o.State != 2 && o.PassivityUserID == authentication.userID);
                foreach (var v in stateList)
                {
                    v.State = 2;
                }
                entity.SaveChanges();
                return templist;


            }

        }
        [HttpPost]
        public List<UserLetter> AddUserLetter()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return new List<UserLetter>();
            }
            using (Entity entity = new Entity())
            {
                UserExtend userExtend = entity.UserExtend.Find(authentication.userID);
                if (userExtend.Banned == 1 && userExtend.BannedEndTime > DateTime.Now)
                {
                    return null;
                }
            }
            using (Entity entity = new Entity())
            {

                int ID = Convert.ToInt32(request["ID"]);

                int PassivityUserID = entity.UserLetter.Find(ID).InitiativeUserID;

                var black = entity.UserBlacklist.Where(userBlacklisto => userBlacklisto.InitiativeUserID == PassivityUserID && userBlacklisto.PassivityUserID == authentication.userID).Count() > 0;
                if (black)
                {
                    return null;
                }

                UserLetter o = UserLetterBLL.Create(authentication.userID, PassivityUserID, request["Text"], 5);
                var UserLetterList = new List<UserLetter>();
                o.User = entity.User.Find(authentication.userID);
                UserLetterList.Add(new UserLetter()
                {
                    InitiativeUserID = o.InitiativeUserID,
                    PassivityUserID = o.PassivityUserID,
                    Text = o.Text,
                    Type = o.Type,
                    CreateTime = o.CreateTime,
                    User = new User()
                    {
                        HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
                        UserID = o.User.UserID,
                        NickName = o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,
                            ExperienceName = o.User.UserExtend.ExperienceName,
                        }
                    },
                    UserLetterID = o.UserLetterID
                });
                return UserLetterList;
            }

        }
        [HttpPost]
        public int AddUserLetterByUser()
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
                UserExtend userExtend = entity.UserExtend.Find(authentication.userID);
                if (userExtend.Banned == 1 && userExtend.BannedEndTime > DateTime.Now)
                {
                    return 0;
                }
            }
            using (Entity entity = new Entity())
            {
                int ID = Convert.ToInt32(request["ID"]);
                var black = entity.UserBlacklist.Where(userBlacklisto => userBlacklisto.InitiativeUserID == ID && userBlacklisto.PassivityUserID == authentication.userID).Count() > 0;
                if (black)
                {
                    return 0;
                }
                UserLetter o = UserLetterBLL.Create(authentication.userID, ID, request["Text"], 5);
                var UserLetterList = new List<UserLetter>();
                o.User = entity.User.Find(authentication.userID);
                return 1;
            }
        }
        [HttpPost]
        public List<UserLetter> GetUserLetterLoop()
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
                int ID = Convert.ToInt32(request["ID"]);
                int InitiativeUserID = entity.UserLetter.Find(ID).InitiativeUserID;


                var user1 = entity.User.Include("UserExtend").Where(o => o.UserID == InitiativeUserID).ToList().Select(o => new User()
                {
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.HeadPortrait,
                    UserID = o.UserID,
                    NickName = o.NickName,
                    UserExtend = new UserExtend()
                    {
                        ExperienceLevel = o.UserExtend.ExperienceLevel,
                        ExperienceName = o.UserExtend.ExperienceName,
                    }
                }).First();

                var templist = userLetterBLL.GetUserLetter(o => (o.PassivityUserID == authentication.userID && o.InitiativeUserID == InitiativeUserID) && o.State == 1, "ASC"
                ).Select(o => new UserLetter()
                {
                    InitiativeUserID = o.InitiativeUserID,
                    PassivityUserID = o.PassivityUserID,
                    Text = o.Text,
                    Type = o.Type,
                    CreateTime = o.CreateTime,
                    User = user1,
                    UserLetterID = o.UserLetterID
                }).ToList();
                foreach (var v in templist)
                {
                    entity.UserLetter.Find(v.UserLetterID).State = 2;
                }
                entity.SaveChanges();
                return templist;
            }

        }

        [HttpPost]
        public UserLoop GetCount()
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
                UserLoop userLoop = new UserLoop();
                userLoop.lettercount = entity.UserLetter.Where(o => o.PassivityUserID == authentication.userID && o.State == 1).Count();

                var user = entity.User.Find(authentication.userID);
                userLoop.userType = user.Type;
                userLoop.userState = user.State;
                userLoop.userBanned = user.UserExtend.Banned == 1 && user.UserExtend.BannedEndTime > DateTime.Now;
                userLoop.userLevel = user.UserExtend.ExperienceLevel;
                return userLoop;
            }
        }


        [HttpPost]
        public int DeleteUserLetter()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int ID = Convert.ToInt32(request["ID"]);

            using (Entity entity = new Entity())
            {
                var userLetter = entity.UserLetter.Find(ID);

                return entity.Database.ExecuteSqlCommand(@"delete [UserLetter] where (PassivityUserID = " + authentication.userID + " and InitiativeUserID = " + userLetter.InitiativeUserID + ") or (PassivityUserID = " + userLetter.InitiativeUserID + " and InitiativeUserID = " + authentication.userID + ")");


            }

        }
    }


}
