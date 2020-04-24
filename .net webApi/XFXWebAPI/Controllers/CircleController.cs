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
    public class CircleController : ApiController
    {

        [HttpPost]
        public IEnumerable<CircleType> Get()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            int circleTypeID;
            int.TryParse(request["id"], out circleTypeID);
            Authentication authentication = new Authentication(request);

            using (Entity entity = new Entity())
            {
                return new CircleTypeBLL().GetCircleType(o => o.State == 1 && o.CircleTypeID == circleTypeID)
.Select(o => new CircleType()
{
    CircleTypeID = o.CircleTypeID,
    Title = o.Title,
    Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
    SubTitle = o.SubTitle,
    State = string.IsNullOrEmpty(authentication.state) ?
    entity.CircleManage.Where(circleManage => circleManage.UserID == authentication.userID
    && circleManage.CircleTypeID == o.CircleTypeID && (circleManage.State & 1) > 0).Count() : 0

}).ToList();
            }
        }

        [HttpPost]
        [HttpGet]
        public IEnumerable<CircleType> GetAll()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            Authentication authentication = new Authentication(request);

            using (Entity entity = new Entity())
            {
                return new CircleTypeBLL().GetCircleType(o => o.State == 1
                && o.CircleTypeID != 1
                && o.CircleTypeID != 2
                && o.CircleTypeID != 3)
     .Select(o => new CircleType()
 {
     CircleTypeID = o.CircleTypeID,
     Title = o.Title,
     Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
     SubTitle = o.SubTitle,
     State = string.IsNullOrEmpty(authentication.state) ?
    entity.CircleManage.Where(circleManage => circleManage.UserID == authentication.userID
    && circleManage.CircleTypeID == o.CircleTypeID && (circleManage.State & 1) > 0).Count() : 0
 }).ToList();
               
            }
        }


        [HttpPost]
        public string AddMyCircle()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return "{error:'" + authentication.state + "'}";
            }

            using (Entity entity = new Entity())
            {
                int circleTypeID;
                if (int.TryParse(request["id"], out circleTypeID))
                {
                    int state = 0;
                    var circleManage = entity.CircleManage.Where(o => o.UserID == authentication.userID && o.CircleTypeID == circleTypeID).FirstOrDefault();
                    if (circleManage == null)
                    {
                        state = 1;
                    }
                    else
                    {
                        state = (circleManage.State & 1) > 0 ? 0 : 1;
                    }
                    new CircleTypeBLL().AddCircleType(authentication.userID, circleTypeID, state);
                    return "{message:" + (state + 1).ToString() + "}";
                }

                return "{error:'参数不正确'}";
            }



        }
    }
}
