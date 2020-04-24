using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using XFXClassLibrary;

namespace XFXManagement.uploadify
{
    /// <summary>
    /// UploadImageHandler 的摘要说明
    /// </summary>
    public class UploadImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpPostedFile file = context.Request.Files[0];
            string date = DateTime.Now.ToString("yyyyMMdd");
            string dir = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], date);
            string guid = Guid.NewGuid().ToString("N");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filename = guid + Path.GetExtension(file.FileName);
            string path = Path.Combine(dir, filename);
            file.SaveAs(path);
            ImageResizeOptions imageResizeOptions = new ImageResizeOptions();
            if (context.Request["cut"] == "false")
            {
                imageResizeOptions.cut = false;
            }
            imageResizeOptions.Width = Convert.ToInt32(context.Request["width"]);
            imageResizeOptions.Height = Convert.ToInt32(context.Request["height"]);
            context.Response.Write(date + "/" + ImageManager.ResizeImage(path, imageResizeOptions));
 
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}