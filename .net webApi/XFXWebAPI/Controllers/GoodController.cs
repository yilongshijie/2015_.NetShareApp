using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using XFXClassLibrary;

namespace XFXWebAPI.Controllers
{
    public class GoodController : ApiController
    {

        public IEnumerable<Good> Gets()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string State = request["State"];
            int state = 2;
            if (!String.IsNullOrEmpty(State))
            {
                state = Convert.ToInt32(State);
            }
            string GoodGategoryID = request["GoodGategoryID"];
            int goodGategoryID = 0;
            bool goodGategoryIDBool = true;
            if (!String.IsNullOrEmpty(GoodGategoryID))
            {
                goodGategoryIDBool = false;
                goodGategoryID = Convert.ToInt32(GoodGategoryID);
            }

            string search = request["search"];
            bool searchBool = true;
            if (!String.IsNullOrEmpty(search))
            {
                searchBool = false;
            }

            string orderby = request["orderby"];
            Func<Good, object> orderBy = o => o.OrderBy;
            string sort = "DESC";
            if (orderby == "zonghe")
            {

            }
            else if (orderby == "jiageasc")
            {
                orderBy = o => o.RealPrice;
                sort = "ASC";
            }
            else if (orderby == "jiagedesc")
            {
                orderBy = o => o.RealPrice;
            }
            else if (orderby == "renqi")
            {
                orderBy = o => o.SalesVolume;
            }
            else if (orderby == "xinpin")
            {
                orderBy = o => (o.State & 8);
            }
            if (request["index"] != null)
            {
                int pageTotal;
                return new GoodBLL().GetGood(o =>
(o.State & 2) > 0 &&
(o.State & state) > 0 &&
(goodGategoryIDBool || o.GoodGategoryID == goodGategoryID) &&
(searchBool || (o.Title.Contains(search) || o.SubTitle.Contains(search)))
, 20, Convert.ToInt32(request["index"]), out pageTotal, orderBy, sort).Select(o =>
           new Good()
           {
               GoodID = o.GoodID,
               Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
               Title = o.Title,
               SubTitle = o.SubTitle,
               RealPrice = o.RealPrice,
               SalesVolume = o.SalesVolume,
               State = o.State
           });

            }
            else
            {
                return new GoodBLL().GetGood(o =>
(o.State & 2) > 0 &&
(o.State & state) > 0 &&
(goodGategoryIDBool || o.GoodGategoryID == goodGategoryID) &&
(searchBool || (o.Title.Contains(search) || o.SubTitle.Contains(search)))
, orderBy, sort).Select(o =>
        new Good()
        {
            GoodID = o.GoodID,
            Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
            Title = o.Title,
            SubTitle = o.SubTitle,
            RealPrice = o.RealPrice,
            SalesVolume = o.SalesVolume,
            State = o.State
        });
            }

        }
        [HttpPost]
        public IEnumerable<Good> myshoucang()
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
                var linq = entity.GoodCollection.Where(o => o.UserID == authentication.userID && o.State == 1).Select(o => o.GoodID).Distinct();

                return new GoodBLL().GetGood(o => linq.Contains(o.GoodID) && (o.State & 2) > 0).Select(o =>
                      new Good()
                      {
                          GoodID = o.GoodID,
                          Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
                          Title = o.Title,
                          SubTitle = o.SubTitle,
                          RealPrice = o.RealPrice,
                          SalesVolume = o.SalesVolume,
                          State = o.State
                      });
            }
        }
        [HttpPost]
        public Good Get()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            string GoodID = request["GoodID"];
            int goodID = Convert.ToInt32(GoodID);

            var v = new GoodBLL().GetGood(goodID).Select(good => new Good()
            {
                GoodID = good.GoodID,
                Title = good.Title,
                SubTitle = good.SubTitle,
                BidPrice = good.BidPrice,
                RealPrice = good.RealPrice,
                Detail = good.Detail.Replace("***xing*fen*xiang*img*src***", ConfigurationManager.AppSettings["UploadUrl"]),
                ImageList = XFXExt.imgList(good.ImageList, ConfigurationManager.AppSettings["UploadUrl"], false),
                SalesVolume = good.SalesVolume,
                EvaluateNum = good.EvaluateNum,
                EvaluateValue = good.EvaluateValue,
                GoodChild = good.GoodChild.Where(goodChild => goodChild.State == 1).OrderByDescending(o => o.OrderBy).Select(goodChild => new GoodChild()
                {
                    GoodChildID = goodChild.GoodChildID,
                    Specification = goodChild.Specification,
                    AddPrice = goodChild.AddPrice,
                    Image = ConfigurationManager.AppSettings["UploadUrl"] + goodChild.Image,
                    Repertory = (goodChild.Repertory > 10 ? 10 : goodChild.Repertory)
                }).ToList()

            }).FirstOrDefault();

            using (Entity entity = new Entity())
            {
                v.GoodCollection = (string.IsNullOrEmpty(authentication.state) ?
                (entity.GoodCollection.Where(o => o.GoodID == v.GoodID && o.UserID == authentication.userID).ToList().Select(goodCollection => new GoodCollection() { State = goodCollection.State }).ToList()) : null);
            }
            return v;
        }

        public int SetGoodCollection()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int goodID;
            if (!int.TryParse(request["GoodID"], out goodID))
            {
                return 0;
            }

            int state;
            if (!int.TryParse(request["State"], out state))
            {
                return 0;
            }
            using (Entity entity = new Entity())
            {
                var v = entity.GoodCollection.Where(o => o.UserID == authentication.userID && o.GoodID == goodID).FirstOrDefault();
                if (v == null)
                {
                    entity.GoodCollection.Add(new GoodCollection()
                    {
                        UserID = authentication.userID,
                        GoodID = goodID,
                        State = state

                    });
                }
                else
                {
                    v.State = state;
                }
                entity.SaveChanges();
                return v.State;
            }
        }

        public List<GoodEvaluate> getGoodEvaluate()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            int goodID = Convert.ToInt32(request["goodID"]);
            string numStr = request["num"] ?? "500";
            int num = Convert.ToInt32(numStr);

            using (Entity entity = new Entity())
            {
                var list = entity.GoodEvaluate.Include("User").Where(o => o.GoodID == goodID && o.State == 1).OrderByDescending(o => o.Time).ToList().Skip(0).Take(num);
                return list.Select(o => new GoodEvaluate()
                {

                    GoodID = o.GoodID,
                    User = new User()
                    {
                        NickName = o.User.NickName.setStart()
                    },
                    Time = o.Time,
                    Detail = o.Detail

                }).ToList();

            }
        }

    }
}
