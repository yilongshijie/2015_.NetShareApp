using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XFXClassLibrary;
using System.Web.Caching;
using WebApiCache;

namespace XFXWebAPI.Controllers
{
    public class AdvertisementController : ApiController
    {
        [OutputCacheWebApi(36000)]
        [HttpGet]
        public IEnumerable<Advertisement> Get()
        {

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            int type;
            int.TryParse(request["type"], out type);
            return new AdvertisementBLL().GetAdvertisement(o => o.Type == type && o.State == 1, o => o.Type).Select(o =>
             new Advertisement()
             {
                 Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
                 Type = o.Type,
                 Link = o.Link,
                 Title = o.Title
             });
        }


    }
}
