using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiCache;
using XFXClassLibrary;

namespace XFXWebAPI.Controllers
{
    public class WelfareController : ApiController
    {
        [OutputCacheWebApi(36000)]
        [HttpGet]
        public Welfare GetWelfare()
        {
            using (Entity entity = new Entity())
            {
                return entity.Welfare.First();
            }
        }


        [OutputCacheWebApi(36000)]
        [HttpGet]
        public string GetWholeField()
        {
            using (Entity entity = new Entity())
            {
                var v = entity.WholeFieldActivity.OrderByDescending(o => o.Type);
                string s = "";
                foreach (var vv in v)
                {
                    s += vv.Title + " ";
                }
                return s.Trim();
            }
        }


    }

}
