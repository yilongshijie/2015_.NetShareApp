using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiCache;
using XFXClassLibrary;


namespace XFXWebAPI.Controllers
{
    public class GoodGategoryController : ApiController
    {
        [OutputCacheWebApi(36000)]
        public IEnumerable<GoodGategory> Gets()
        {
            return new GoodGategoryBLL().GetGoodGategory(o => o.State == 1, o => o.OrderBy).Select(o =>
                  new GoodGategory()
                  {
                      Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
                      Name = o.Name,
                      Describe = o.Describe,
                      GoodGategoryID = o.GoodGategoryID
                  });
        }

        [OutputCacheWebApi(36000)]
        public IEnumerable<GoodGategory> GetIndex()
        {
            return new GoodGategoryBLL().GetGoodGategory(o => o.State == 1, o => o.OrderBy).Select(o =>
                   new GoodGategory()
                   {
                       Image = ConfigurationManager.AppSettings["UploadUrl"] + o.ImageHome,
                       Name = o.Name,
                       Describe = o.Describe,
                       GoodGategoryID = o.GoodGategoryID,
                       GoodHome = o.GoodHome.Select(oo => new GoodHome()
                       {
                           GoodID = oo.GoodID,
                           Title = oo.Title,
                           Label = oo.Label,
                           Image = ConfigurationManager.AppSettings["UploadUrl"] + oo.Image,
                           Flex = oo.Flex,

                       }).OrderByDescending(oo => oo.Flex).ToList()
                   });


        }

    }
}
