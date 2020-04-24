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
using XFXClassLibrary;

namespace XFXWebAPI.Controllers
{
    public class GoodCartController : ApiController
    {

        [HttpPost]
        public int SetGoodCart()
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
                int goodchildid = Convert.ToInt32(request["goodchildid"]);
                var goodChild = entity.GoodChild.Find(goodchildid);
                if (goodChild == null || goodChild.State == 0 || (goodChild.Good.State & 2) == 0)
                {
                    return 0;
                }
                var t = entity.GoodCart.Where(o => o.UserID == authentication.userID && o.GoodChildID == goodChild.GoodChildID).FirstOrDefault();
                if (t == null)
                {
                    GoodCart goodCart = new GoodCart();
                    goodCart.UserID = authentication.userID;
                    goodCart.GoodChildID = goodchildid;
                    goodCart.GoodID = goodChild.GoodId;
                    goodCart.CreateTime = DateTime.Now;
                    goodCart.Num = Convert.ToInt32(request["num"]);
                    entity.GoodCart.Add(goodCart);
                }
                else
                {
                    t.Num += Convert.ToInt32(request["num"]);
                }

                return entity.SaveChanges();
            }
        }
        [HttpPost]
        public int DeleteGoodCart()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int goodchildid = Convert.ToInt32(request["goodchildid"]);

            using (Entity entity = new Entity())
            {
                var goodCart = entity.GoodCart.Where(o => o.UserID == authentication.userID && o.GoodChildID == goodchildid).FirstOrDefault();
                if (goodCart != null)
                {
                    entity.GoodCart.Remove(goodCart);
                    return entity.SaveChanges();
                }
            }
            return 0;

        }
        [HttpPost]
        public int NumGoodCart()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            int goodchildid = Convert.ToInt32(request["goodchildid"]);

            using (Entity entity = new Entity())
            {
                var goodCart = entity.GoodCart.Where(o => o.UserID == authentication.userID && o.GoodChildID == goodchildid).FirstOrDefault();
                if (goodCart != null)
                {
                    goodCart.Num = Convert.ToInt32(request["num"]);
                    return entity.SaveChanges();
                }
            }
            return 0;
        }
        [HttpPost]
        public IEnumerable<GoodCartView> GetGoodCart()
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
                var GoodCartList = entity.GoodCart.Include("Good").Include("GoodChild").Where(o => o.UserID == authentication.userID).OrderByDescending(o => o.CreateTime).ToList();
                foreach (var v in GoodCartList)
                {
                    if ((v.Good.State & 2) == 0)
                    {
                        entity.GoodCart.Remove(v);
                        GoodCartList.Remove(v);
                    }
                    else
                    {
                        if (v.GoodChild.State == 0)
                        {
                            entity.GoodCart.Remove(v);
                            GoodCartList.Remove(v);
                        }
                    }
                }
                entity.SaveChanges();
                return GoodCartList.Select(o => new GoodCartView()
                {
                    img = ConfigurationManager.AppSettings["UploadUrl"] + o.GoodChild.Image,
                    title = o.Good.Title,
                    guige = o.GoodChild.Specification,

                    num = o.Num,
                    price = o.GoodChild.AddPrice + o.GoodChild.Good.RealPrice,
                    goodChildID = o.GoodChild.GoodChildID,
                    goodID = o.Good.GoodID

                });
            }
        }

        [HttpPost]
        public IEnumerable<GoodCartView> GetGoodCartLocalStorage()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string cache_shopcar = request["cache_shopcar"];

            GoodCartLocalStorage goodCartLocalStorage = JsonConvert.DeserializeObject<GoodCartLocalStorage>(cache_shopcar);

            using (Entity entity = new Entity())
            {
                List<GoodCartView> GoodCartViewList = new List<GoodCartView>();
                foreach (var v in goodCartLocalStorage.cache_shopcar)
                {

                    int goodChildID = v.goodchildid;
                    int num = v.num;
                    var goodChild = entity.GoodChild.Include("Good").Where(o => o.GoodChildID == goodChildID && o.State == 1).FirstOrDefault();
                    if (goodChild != null)
                    {
                        if ((goodChild.Good.State & 2) == 0)
                        {
                            continue;
                        }
                        var t = GoodCartViewList.Find(o => o.goodChildID == goodChildID);
                        if (t != null)
                        {
                            t.num += num;
                        }
                        else
                        {
                            GoodCartViewList.Add(new GoodCartView()
                            {
                                img = ConfigurationManager.AppSettings["UploadUrl"] + goodChild.Image,
                                title = goodChild.Good.Title,
                                guige = goodChild.Specification,
                                num = num,
                                price = goodChild.AddPrice + goodChild.Good.RealPrice,
                                goodChildID = goodChild.GoodChildID,
                                goodID = goodChild.Good.GoodID
                            });
                        }

                    }
                }
                return GoodCartViewList;
            }
        }

        [HttpPost]
        public int SetGoodCartLocalStorage()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            string cache_shopcar = request["cache_shopcar"];
            GoodCartLocalStorage goodCartLocalStorage = JsonConvert.DeserializeObject<GoodCartLocalStorage>(cache_shopcar);

            using (Entity entity = new Entity())
            {
                List<GoodCartView> GoodCartViewList = new List<GoodCartView>();
                foreach (var v in goodCartLocalStorage.cache_shopcar)
                {

                    int goodChildID = v.goodchildid;
                    int num = v.num;
                    var goodChild = entity.GoodChild.Include("Good").Where(o => o.GoodChildID == goodChildID && o.State == 1).FirstOrDefault();
                    if (goodChild != null)
                    {
                        if ((goodChild.Good.State & 2) == 0)
                        {
                            continue;
                        }

                        var t = entity.GoodCart.Where(o => o.UserID == authentication.userID && o.GoodChildID == goodChild.GoodChildID).FirstOrDefault();
                        if (t == null)
                        {
                            GoodCart goodCart = new GoodCart();
                            goodCart.UserID = authentication.userID;
                            goodCart.GoodChildID = goodChildID;
                            goodCart.GoodID = goodChild.GoodId;
                            goodCart.CreateTime = DateTime.Now;
                            goodCart.Num = num;
                            entity.GoodCart.Add(goodCart);
                        }
                        else
                        {
                            t.Num += num;
                        }

                    }
                }
                return entity.SaveChanges();
            }

        }

    }
}
