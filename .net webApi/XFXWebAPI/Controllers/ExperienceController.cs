using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XFXClassLibrary;


namespace XFXWebAPI.Controllers
{
    public class ExperienceController : ApiController
    {
        GoodExperienceBLL goodExperienceBLL = new GoodExperienceBLL();

        [HttpPost]
        public string CreateExperience()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }
            string goodIDTemp = request["id"];
            if (goodIDTemp == null)
            {
                return null;
            }
            int goodID;
            if (!int.TryParse(goodIDTemp, out goodID))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {
                GoodExperience goodExperience = new GoodExperience();
                goodExperience.GoodID = goodID;
                goodExperience.GoodGategoryID = entity.Good.Find(goodID).GoodGategoryID;
                goodExperience.UserID = authentication.userID;
                goodExperience.CreateTime = DateTime.Now;
                goodExperience.UpdateTime = goodExperience.CreateTime;
                goodExperience.State = 1;
                goodExperience.Title = request["Title"];
                goodExperience.Deatil = request["Deatil"];
                goodExperience.Images = request["ImgList"];
                goodExperience.Image = goodExperience.Images.Split(',')[0];
                entity.GoodExperience.Add(goodExperience);
                entity.SaveChanges();

                return "{message:'" + goodExperience.GoodExperienceID.ToString() + "'}";
            }
        }

        public GoodExperience Get()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            int ID;
            if (!int.TryParse(request["id"], out ID))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {
                GoodExperience goodExperience = entity.GoodExperience.Where(o => o.GoodExperienceID == ID).FirstOrDefault();
                if (goodExperience == null)
                {
                    return null;
                }
                goodExperience.Images = XFXExt.imgList(goodExperience.Images, ConfigurationManager.AppSettings["UploadUrl"], false);
                return new GoodExperience()
                {
                    GoodExperienceID = goodExperience.GoodExperienceID,
                    Deatil = goodExperience.Deatil.removeTag(),
                    Images = goodExperience.Images,
                    ReplyNum = goodExperience.ReplyNum,
                    Title = goodExperience.Title,
                    GoodID = goodExperience.GoodID,
                    User = new User()
                    {
                        NickName = goodExperience.User.NickName,
                        HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.User.HeadPortrait,
                        UserID = goodExperience.UserID,
                        Type = goodExperience.User.Type.getUserType()
                    },
                    Good = new Good()
                    {
                        Image = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.Good.Image,
                        Title = goodExperience.Good.Title,
                        RealPrice = goodExperience.Good.RealPrice
                    }
                };
            }
        }

        public List<GoodExperience> GetByGoodID()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            int GoodID;
            if (!int.TryParse(request["GoodID"], out GoodID))
            {
                return null;
            }
            return goodExperienceBLL.GetGoodExperience(o => o.GoodID == GoodID && (o.State & 2) > 0, o => o.OrderBy).Select(goodExperience => new GoodExperience()
            {

                GoodExperienceID = goodExperience.GoodExperienceID,
                Image = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.Image,
                GoodID = goodExperience.GoodID
            }).ToList();
        }
        public List<GoodExperience> Gets()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            if (request["index"] != null)
            {
                int pageTotal;
                return goodExperienceBLL.GetGoodExperience(o => (o.State & 2) > 0, 20, Convert.ToInt32(request["index"]), out pageTotal, o => o.OrderBy).Select(goodExperience => new GoodExperience()
                {

                    GoodExperienceID = goodExperience.GoodExperienceID,
                    Image = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.Image,
                    ReplyNum = goodExperience.ReplyNum,
                    Title = goodExperience.Title.removeTag(),
                    GoodID = goodExperience.GoodID,
                    User = new User()
                    {
                        NickName = goodExperience.User.NickName,
                        HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.User.HeadPortrait,
                        Type = goodExperience.User.Type.getUserType(),
                        UserID = goodExperience.User.UserID

                    }
                }).ToList();
            }
            else
            {
                return goodExperienceBLL.GetGoodExperience(o => (o.State & 2) > 0, o => o.OrderBy).Select(goodExperience => new GoodExperience()
                {

                    GoodExperienceID = goodExperience.GoodExperienceID,
                    Image = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.Image,
                    ReplyNum = goodExperience.ReplyNum,
                    Title = goodExperience.Title.removeTag(),
                    GoodID = goodExperience.GoodID,
                    User = new User()
                    {
                        NickName = goodExperience.User.NickName,
                        HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.User.HeadPortrait,
                        Type = goodExperience.User.Type.getUserType(),
                        UserID = goodExperience.User.UserID

                    }
                }).ToList();
            }

        }

        [HttpPost]
        public List<GoodExperience> GetsMy()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }

            return goodExperienceBLL.GetGoodExperience(o => (o.State & 4) == 0 && o.UserID == authentication.userID, o => o.OrderBy).Select(goodExperience => new GoodExperience()
            {

                GoodExperienceID = goodExperience.GoodExperienceID,
                Image = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.Image,
                ReplyNum = goodExperience.ReplyNum,
                Title = goodExperience.Title.removeTag(),
                GoodID = goodExperience.GoodID,
                User = new User()
                {
                    NickName = goodExperience.User.NickName,
                    HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + goodExperience.User.HeadPortrait,
                    Type = goodExperience.User.Type.getUserType(),
                    UserID = goodExperience.User.UserID
                },
                State = goodExperience.State
            }).ToList();
        }


        public IEnumerable<GoodExperienceReply> GetRepeat()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            string IDTemp = request["id"];

            int goodExperienceID;
            if (!int.TryParse(IDTemp, out goodExperienceID))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {

                return entity.GoodExperienceReply.Include("User").Include("User.UserExtend").Where(o => o.GoodExperienceID == goodExperienceID &&
               (o.State & 2) == 0 &&
               (o.State & 1) == 0
            ).ToList()
    .Select(o => new GoodExperienceReply()
    {
        GoodExperienceReplyID = o.GoodExperienceReplyID,
        GoodExperienceID = o.GoodExperienceID,
        Detail = o.Detail.removeTag(),
        ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false),
        User = new User()
        {

            NickName = o.User.NickName,
            UserExtend = new UserExtend()
            {
                ExperienceLevel = o.User.UserExtend.ExperienceLevel,
                ExperienceName = o.User.UserExtend.ExperienceName

            },
            Gender = o.User.Gender,
            Location = o.User.Location,
            HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
            Type = o.User.Type.getUserType(),
            UserID = o.User.UserID
        },
        UserID = o.UserID,
        Floor = o.Floor,
        State = o.State,
        CreateTime = o.CreateTime,


    });

            }
        }

        [HttpPost]
        public IEnumerable<GoodExperienceReply> CreateReply()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }
            string IDTemp = request["id"];

            int goodExperienceID;
            if (!int.TryParse(IDTemp, out goodExperienceID))
            {
                return null;
            }

            if (request["text"] == null) return null;


            using (Entity entity = new Entity())
            {
                GoodExperience goodExperience = entity.GoodExperience.Find(goodExperienceID);
                goodExperience.ReplyNum++;
                GoodExperienceReply goodExperienceReply = new GoodExperienceReply()
                {
                    UserID = authentication.userID,
                    GoodExperienceID = goodExperienceID,
                    GoodID = goodExperience.GoodID,
                    Detail = request["text"],
                    ImgList = request["imglist"],
                    State = 0,
                    CreateTime = DateTime.Now,
                    Floor = goodExperience.ReplyNum
                };
                goodExperience.GoodExperienceReply.Add(goodExperienceReply);
                entity.SaveChanges();

                return entity.GoodExperienceReply.Include("User").Where(o => o.GoodExperienceReplyID == goodExperienceReply.GoodExperienceReplyID &&
             (o.State & 2) == 0 &&
             (o.State & 1) == 0
          ).ToList()
    .Select(o => new GoodExperienceReply()
    {
        GoodExperienceReplyID = o.GoodExperienceReplyID,
        GoodExperienceID = o.GoodExperienceID,
        Detail = o.Detail,
        ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false),
        User = new User()
        {

            NickName = o.User.NickName,
            UserExtend = new UserExtend()
            {
                ExperienceLevel = o.User.UserExtend.ExperienceLevel,
                ExperienceName = o.User.UserExtend.ExperienceName

            },
            Gender = o.User.Gender,
            Location = o.User.Location,
            HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
            Type = o.User.Type.getUserType(),
            UserID = o.User.UserID
        },
        Floor = o.Floor,
        State = o.State,
        CreateTime = o.CreateTime,


    }).ToList();
            }
        }
        [HttpPost]
        public GoodExperience GetReplyNum()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            int ID;
            if (!int.TryParse(request["id"], out ID))
            {
                return null;
            }

            using (Entity entity = new Entity())
            {
                GoodExperience goodExperience = entity.GoodExperience.Where(o => o.GoodExperienceID == ID &&
               (o.State & 2) == 0 &&
               (o.State & 1) == 0).FirstOrDefault();
                if (goodExperience == null)
                {
                    return new GoodExperience()
                    {
                        ReplyNum = 0,
                    };
                }
                return new GoodExperience()
                {
                    ReplyNum = goodExperience.ReplyNum,
                };
            }
        }

    }
}
