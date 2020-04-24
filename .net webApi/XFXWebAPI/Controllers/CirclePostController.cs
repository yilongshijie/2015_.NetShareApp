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
    public class CirclePostController : ApiController
    {

        public IEnumerable<CirclePost> Gets()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string circleTypeIDTemp = request["id"];
            if (circleTypeIDTemp == null)
            {
                return null;
            }
            int circleTypeID;
            if (!int.TryParse(circleTypeIDTemp, out circleTypeID))
            {
                return null;
            }
            if (request["index"] != null)
            {
                int pageTotal;
                return new CirclePostBLL().GetCirclePost(o => o.CircleTypeID == circleTypeID && ((o.State & 16) == 0) && ((o.State & 32) == 0) && ((o.State & 2) > 0), 20, Convert.ToInt32(request["index"]), out pageTotal, o => (o.State & 12) > 0, "DESC", o => o.UpdateTime).ToList().Select(o => new CirclePost()
                {
                    CirclePostID = o.CirclePostID,
                    CircleTypeID = o.CircleTypeID,
                    Title = o.Title.removeTag(),
                    ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false, 3),
                    Detail = o.DetailDigest.removeTag(),
                    User = new User()
                    {
                        NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,

                        },
                        Gender = o.User.Gender,
                        Type = o.User.Type.getUserType()
                    },
                    UserID = o.UserID,
                    ReplyNum = o.ReplyNum,
                    State = o.State,
                    CreateTime = o.UpdateTime,
                    Province = ((o.State & 128) > 0) ? (o.Province ?? "") : ""

                }).ToList();

            }
            else
            {
                return new CirclePostBLL().GetCirclePost(o => o.CircleTypeID == circleTypeID && ((o.State & 16) == 0) && ((o.State & 32) == 0) && ((o.State & 2) > 0)).ToList().Select(o => new CirclePost()
                {
                    CirclePostID = o.CirclePostID,
                    CircleTypeID = o.CircleTypeID,
                    Title = o.Title,
                    ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false, 3),
                    Detail = o.DetailDigest,
                    User = new User()
                    {
                        NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,

                        },
                        Gender = o.User.Gender,
                        Type = o.User.Type.getUserType()
                    },
                    UserID = o.UserID,
                    ReplyNum = o.ReplyNum,
                    State = o.State,
                    CreateTime = o.CreateTime,
                    Province = ((o.State & 128) > 0) ? (o.Province ?? "") : ""

                }).ToList();

            }

        }



        public IEnumerable<CirclePost> GetSearch()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            string circleTypeIDTemp = request["id"];
            if (circleTypeIDTemp == null)
            {
                return null;
            }
            int pageTotal;
            return new CirclePostBLL().GetCirclePost(o => o.Title.Contains(circleTypeIDTemp) && ((o.State & 16) == 0) && ((o.State & 32) == 0) && ((o.State & 2) > 0), 20, Convert.ToInt32(request["index"]), out pageTotal, o => (o.State & 12) > 0, "DESC", o => o.UpdateTime).ToList().Select(o => new CirclePost()
            {
                CirclePostID = o.CirclePostID,
                CircleTypeID = o.CircleTypeID,
                Title = o.Title.removeTag(),
                ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false, 3),
                Detail = o.DetailDigest.removeTag(),
                User = new User()
                {
                    NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
                    UserExtend = new UserExtend()
                    {
                        ExperienceLevel = o.User.UserExtend.ExperienceLevel,

                    },
                    Gender = o.User.Gender,
                    Type = o.User.Type.getUserType()
                },
                UserID = o.UserID,
                ReplyNum = o.ReplyNum,
                State = o.State,
                CreateTime = o.UpdateTime,
                Province = ((o.State & 128) > 0) ? (o.Province ?? "") : ""

            }).ToList();



        }

        [HttpPost]
        public IEnumerable<CirclePost> GetsFujin()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string city = request["city"];
            string province = request["province"];

            if (request["index"] != null)
            {
                int pageTotal;
                return new CirclePostBLL().GetCirclePost(o => o.Province == province && ((o.State & 16) == 0) && ((o.State & 32) == 0) && ((o.State & 2) > 0) && ((o.State & 128) > 0), 20, Convert.ToInt32(request["index"]), out pageTotal, o => o.CreateTime, "DESC")
                .Select(o => new CirclePost()
                {
                    CirclePostID = o.CirclePostID,
                    CircleTypeID = o.CircleTypeID,
                    Title = o.Title.removeTag(),
                    ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false, 3),
                    Detail = o.DetailDigest.removeTag(),
                    User = new User()
                    {
                        NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,

                        },
                        Gender = o.User.Gender,
                        Type = o.User.Type.getUserType()
                    },
                    UserID = o.UserID,
                    ReplyNum = o.ReplyNum,
                    State = o.State,
                    CreateTime = o.CreateTime,
                    Province = o.Province ?? ""

                });
            }
            else
            {
                return new CirclePostBLL().GetCirclePost(o => o.Province == province && ((o.State & 16) == 0) && ((o.State & 32) == 0) && ((o.State & 2) > 0) && ((o.State & 128) > 0), o => o.CreateTime, "DESC")
  .Select(o => new CirclePost()
  {
      CirclePostID = o.CirclePostID,
      CircleTypeID = o.CircleTypeID,
      Title = o.Title,
      ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false, 3),
      Detail = o.DetailDigest,
      User = new User()
      {
          NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
          UserExtend = new UserExtend()
          {
              ExperienceLevel = o.User.UserExtend.ExperienceLevel,

          },
          Gender = o.User.Gender,
          Type = o.User.Type.getUserType()
      },
      UserID = o.UserID,
      ReplyNum = o.ReplyNum,
      State = o.State,
      CreateTime = o.CreateTime,
      Province = o.Province ?? ""

  });
            }
        }
        public IEnumerable<CirclePost> Get()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string circlePostIDTemp = request["id"];
            if (circlePostIDTemp == null)
            {
                return null;
            }
            int circlePostID;
            if (!int.TryParse(circlePostIDTemp, out circlePostID))
            {
                return null;
            }
            return new CirclePostBLL().GetCirclePost(o => o.CirclePostID == circlePostID && ((o.State & 16) == 0) && ((o.State & 32) == 0))
                .Select(o => new CirclePost()
                {
                    CirclePostID = o.CirclePostID,
                    CircleTypeID = o.CircleTypeID,
                    Title = o.Title.removeTag(),
                    ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false),
                    Detail = o.Detail.removeTag(),
                    User = new User()
                    {

                        NickName = o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,
                            ExperienceName = o.User.UserExtend.ExperienceName

                        },
                        Gender = o.User.Gender,
                        HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
                        Type = o.User.Type.getUserType()
                    },
                    ReplyNum = o.ReplyNum,
                    State = o.State,
                    CreateTime = o.CreateTime,
                    UserID = o.UserID,
                    Province = o.Province ?? ""

                }).ToList();

        }

        [HttpPost]
        public int Delete()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            string circlePostIDTemp = request["id"];
            if (circlePostIDTemp == null)
            {
                return 0;
            }
            int circlePostID;
            if (!int.TryParse(circlePostIDTemp, out circlePostID))
            {
                return 0;
            }
            using (Entity entity = new Entity())
            {
                return entity.Database.ExecuteSqlCommand(@" 
update CirclePost set State |=16 where CirclePostID =" + circlePostID + " and UserID = " + authentication.userID + @"; 
delete CirclePostReplyChild where  CirclePostID = " + circlePostID+@"; 
delete CirclePostReply where  CirclePostID = " + circlePostID);
            }


        }
        public IEnumerable<CirclePostReply> GetRepeat()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            string circlePostIDTemp = request["id"];
            if (circlePostIDTemp == null)
            {
                return null;
            }
            int circlePostID;
            if (!int.TryParse(circlePostIDTemp, out circlePostID))
            {
                return null;
            }

            string circlePostReplyIDTemp = request["circlePostReplyID"];
            bool circlePostReplyBool = true;

            int circlePostReplyID;
            if (int.TryParse(circlePostReplyIDTemp, out circlePostReplyID))
            {
                circlePostReplyBool = false;
            }

            using (Entity entity = new Entity())
            {
                return entity.CirclePostReply.Include("User").Include("User.UserExtend").Include("CirclePostReplyChild").Where(o => o.CirclePostID == circlePostID &&
               (o.State & 2) == 0 &&
               (o.State & 1) == 0 &&
               (circlePostReplyBool || o.CirclePostReplyID == circlePostReplyID)
            ).ToList()
    .Select(o => new CirclePostReply()
    {
        CirclePostReplyID = o.CirclePostReplyID,
        CirclePostID = o.CirclePostID,
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
            UserID = o.UserID,
            Gender = o.User.Gender,
            Location = o.User.Location,
            HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
            Type = o.User.Type.getUserType()
        },
        UserID = o.UserID,
        Floor = o.Floor,
        State = o.State,
        CreateTime = o.CreateTime,
        CirclePostReplyChild = o.CirclePostReplyChild.OrderBy(oo => oo.CreateTime).Select(ooo => new CirclePostReplyChild()
        {
            InitiativeUserID = ooo.InitiativeUserID,
            Detail = ooo.Detail,
            ImgList = XFXExt.imgList(ooo.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false),
            User = new User()
            {
                NickName = ooo.User.NickName,
                UserID = ooo.User.UserID
            }
        }).ToList()


    }).OrderByDescending(ooo => (ooo.State & 4)).ThenBy(ooo => ooo.CreateTime).ToList();
            }
        }

        [HttpPost]
        public IEnumerable<CirclePostReply> CreateReply()
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
                UserExtend userExtend = entity.UserExtend.Find(authentication.userID);
                if (userExtend.Banned == 1 && userExtend.BannedEndTime > DateTime.Now)
                {
                    throw new Exception("禁言中...");
                }
            }
            string circlePostIDTemp = request["id"];
            if (circlePostIDTemp == null)
            {
                return null;
            }
            if (request["text"] == null) return null;
            int circlePostID;
            if (!int.TryParse(circlePostIDTemp, out circlePostID))
            {
                return null;
            }
            using (Entity entity = new Entity())
            {
                CirclePost circlePost = entity.CirclePost.Find(circlePostID);
                circlePost.ReplyNum++;
                circlePost.UpdateTime = DateTime.Now;
                CirclePostReply circlePostReply = new CirclePostReply()
                {
                    UserID = authentication.userID,
                    CirclePostID = circlePostID,
                    Detail = request["text"],
                    ImgList = request["imglist"],
                    State = 0,
                    CreateTime = DateTime.Now,
                    Floor = circlePost.ReplyNum
                };
                circlePost.CirclePostReply.Add(circlePostReply);
                new ExperienceLevelBLL().CirclePostReply(authentication.userID);
                entity.SaveChanges();
                if (authentication.userID != circlePost.UserID)
                {
                    var user = entity.User.Find(authentication.userID);
                    string text = string.Format(" {0} 回复了您的帖子 {1}", user.NickName, circlePost.Title);
                    UserLetterBLL.Create(1, circlePost.UserID, text, 1 | 16, circlePost.CirclePostID);
                }


                return entity.CirclePostReply.Include("User").Include("User.UserExtend").Where(o => o.CirclePostReplyID == circlePostReply.CirclePostReplyID).ToList()
        .Select(o => new CirclePostReply()
        {
            CirclePostReplyID = o.CirclePostReplyID,
            CirclePostID = o.CirclePostID,
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
                UserID = o.UserID,
                Gender = o.User.Gender,
                Location = o.User.Location,
                HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + o.User.HeadPortrait,
                Type = o.User.Type.getUserType()

            },
            UserID = o.UserID,
            Floor = o.Floor,
            State = o.State,
            CreateTime = o.CreateTime,


        });
            }
        }


        [HttpPost]
        public CirclePostReplyChild CreateReplyChild()
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
                UserExtend userExtend = entity.UserExtend.Find(authentication.userID);
                if (userExtend.Banned == 1 && userExtend.BannedEndTime > DateTime.Now)
                {
                    throw new Exception("禁言中...");
                }
            }
            string circlePostIDTemp = request["circlePostID"];
            if (circlePostIDTemp == null)
            {
                return null;
            }
            if (request["text"] == null) return null;
            int circlePostID;
            if (!int.TryParse(circlePostIDTemp, out circlePostID))
            {
                return null;
            }
            int CirclePostReplyID = Convert.ToInt32(request["circlePostReplyID"]);
            using (Entity entity = new Entity())
            {
                CirclePost circlePost = entity.CirclePost.Find(circlePostID);
                circlePost.ReplyNum++;
                circlePost.UpdateTime = DateTime.Now;
                CirclePostReplyChild circlePostReplyChild = new CirclePostReplyChild()
                {
                    InitiativeUserID = authentication.userID,
                    PassivityUserID = circlePost.UserID,
                    CirclePostID = circlePostID,
                    CirclePostReplyID = CirclePostReplyID,
                    Detail = request["text"],
                    ImgList = request["imglist"],
                    State = 0,
                    CreateTime = DateTime.Now,
                };
                circlePost.CirclePostReplyChild.Add(circlePostReplyChild);
                new ExperienceLevelBLL().CirclePostReply(authentication.userID);
                entity.SaveChanges();
                if (authentication.userID != circlePost.UserID)
                {
                    var user = entity.User.Find(authentication.userID);
                    string text = string.Format(" {0} 回复了您的帖子 {1}", user.NickName, circlePost.Title);
                    UserLetterBLL.Create(1, circlePost.UserID, text, 1 | 16, circlePost.CirclePostID);
                }

                return entity.CirclePostReplyChild.Include("User").Where(o => o.CirclePostReplyChildID == circlePostReplyChild.CirclePostReplyChildID).ToList().Select(o =>
                          new CirclePostReplyChild()
                          {
                              InitiativeUserID = o.InitiativeUserID,
                              Detail = o.Detail,
                              ImgList = XFXExt.imgList(o.ImgList, ConfigurationManager.AppSettings["UploadUrl"], false),
                              User = new User()
                              {
                                  NickName = o.User.NickName,
                                  UserID = o.User.UserID
                              }
                          }
                ).First();

            }
        }

        [HttpPost]
        public string CreatePost()
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
                UserExtend userExtend = entity.UserExtend.Find(authentication.userID);
                if (userExtend.Banned == 1 && userExtend.BannedEndTime > DateTime.Now)
                {
                    throw new Exception("禁言中...");
                }
            }
            string circleTypeIDTemp = request["id"];
            if (circleTypeIDTemp == null)
            {
                return null;
            }
            int circleTypeID;
            if (!int.TryParse(circleTypeIDTemp, out circleTypeID))
            {
                return null;
            }
            CirclePost circlePost = new CirclePost();
            circlePost.Title = request["Title"];
            circlePost.Detail = request["Detail"];
            circlePost.ImgList = request["ImgList"];
            string DetailDigest = request["DetailDigest"] ?? "";
            if (DetailDigest.Length > 50)
            {
                DetailDigest = DetailDigest.Substring(0, 50);
            }
            circlePost.DetailDigest = DetailDigest;
            circlePost.UserID = authentication.userID;
            circlePost.CircleTypeID = circleTypeID;
            circlePost.CreateTime = DateTime.Now;
            circlePost.UpdateTime = DateTime.Now;
            circlePost.State = 1;
            if (request["didian"] == "1")
            {
                circlePost.State |= 128;
            }
            float x;
            float.TryParse(request["longitude"], out x);
            circlePost.CoordX = x;
            float y;
            float.TryParse(request["latitude"], out y);
            circlePost.CoordY = y;
            circlePost.Province = request["adre"];
            circlePost.City = request["city"];

            using (Entity entity = new Entity())
            {
                entity.CirclePost.Add(circlePost);
                entity.SaveChanges();
            }
            return "{message:'" + circlePost.CirclePostID.ToString() + "'}";
        }

        [HttpPost]
        public IEnumerable<CirclePost> GetMy()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }

            List<CirclePost> CirclePostList = new CirclePostBLL().GetCirclePost(o => o.UserID == authentication.userID && ((o.State & 16) == 0) && ((o.State & 32) == 0), o => o.CreateTime)
    .Select(o => new CirclePost()
    {
        CirclePostID = o.CirclePostID,
        CircleTypeID = o.CircleTypeID,
        Title = o.Title,

        User = new User()
        {
            NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
            UserExtend = new UserExtend()
            {
                ExperienceLevel = o.User.UserExtend.ExperienceLevel,

            },
            UserID = o.UserID,
            Gender = o.User.Gender,
            Location = o.User.Location,
            Type = o.User.Type.getUserType()
        },
        UserID = o.UserID,
        ReplyNum = o.ReplyNum,
        Detail = ((o.State & 1) > 0) ? "未审核" : (((o.State & 32) > 0) ? "管理员删除" : (((o.State & 16) > 0) ? "用户删除" : "")),
        CreateTime = o.CreateTime,
        State = ((o.State & 1) > 0) ? 1 : (((o.State & 32) > 0) ? 0 : (((o.State & 16) > 0) ? 0 : 1)),

    }).ToList();

            return CirclePostList.OrderByDescending(o => o.CreateTime);

        }

        [HttpPost]
        public IEnumerable<CirclePost> GetMyRepeat()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return null;
            }
            List<CirclePost> CirclePostReplayList = new CirclePostBLL().GetCirclePostReply(authentication.userID)
                .Select(o => new CirclePost()
                {
                    CirclePostID = o.CirclePostID,
                    CircleTypeID = o.CircleTypeID,
                    Title = o.Title,

                    User = new User()
                    {
                        NickName = ((o.State & 64) > 0) ? "匿名" : o.User.NickName,
                        UserExtend = new UserExtend()
                        {
                            ExperienceLevel = o.User.UserExtend.ExperienceLevel,

                        },
                        Gender = o.User.Gender,
                        Location = o.User.Location,
                        Type = o.User.Type.getUserType()
                    },
                    UserID = o.UserID,
                    ReplyNum = o.ReplyNum,
                    Detail = ((o.State & 1) > 0) ? "未审核" : (((o.State & 32) > 0) ? "管理员删除" : (((o.State & 16) > 0) ? "用户删除" : "")),
                    CreateTime = o.CreateTime,
                    State = ((o.State & 1) > 0) ? 1 : (((o.State & 32) > 0) ? 0 : (((o.State & 16) > 0) ? 0 : 1)),
                }).ToList();


            return CirclePostReplayList.OrderByDescending(o => o.CreateTime);

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
            int CirclePostID = Convert.ToInt32(request["CirclePostID"]);
            Complaint complaint = new Complaint();
            complaint.InitiativeUserID = authentication.userID;
            complaint.CirclePostID = CirclePostID;
            complaint.Type = 2;
            complaint.Time = DateTime.Now;
            complaint.Reason = request["Reason"];

            using (Entity entity = new Entity())
            {
                var circlePost = entity.CirclePost.Find(CirclePostID);
                circlePost.ComplaintNum++;
                circlePost.ComplaintUntreated++;
                entity.Complaint.Add(complaint);

                return entity.SaveChanges();
            }
        }



    }
}
