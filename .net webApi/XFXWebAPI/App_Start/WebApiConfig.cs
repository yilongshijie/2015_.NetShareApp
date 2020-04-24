using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace XFXWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "Api",
            routeTemplate: "{controller}/{action}"
            );

            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling
              = Newtonsoft.Json.DateTimeZoneHandling.Local;

            //配置返回的时间类型数据格式
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
            //    new Newtonsoft.Json.Converters.IsoDateTimeConverter()
            //    {
            //        DateTimeFormat = "yyyy-MM-dd hh:mm:ss"
            //    }
            //);
        }
    }
}
