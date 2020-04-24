using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XFXClassLibrary;
using XFXClassLibrary.Model;

namespace XFXWebAPI.Controllers
{

    public class UploadController : ApiController
    {
        [HttpPost]
        public ImageView upload()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;



            byte[] imageBytes = Convert.FromBase64String(request["base64"].ToString().Split(',')[1]);
            MemoryStream ms = new System.IO.MemoryStream(imageBytes);
            Image image = Image.FromStream(ms);


            string date = DateTime.Now.ToString("yyyyMMdd");
            string dir = Path.Combine(ConfigurationManager.AppSettings["UploadPath"], date);
            string guid = Guid.NewGuid().ToString("N");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filename = guid + ".jpeg";
            string path = Path.Combine(dir, filename);
            image.Save(path);

            ImageResizeOptions imageResizeOptions = new ImageResizeOptions();
            if (context.Request["type"] == "touxiang")
            {

                imageResizeOptions.cut = true;
                imageResizeOptions.Width = 200;
                imageResizeOptions.Height = 200;
            }
            else if (context.Request["type"] == "tiyanshi")
            {

                imageResizeOptions.cut = true;
                imageResizeOptions.Width = 600; 
                imageResizeOptions.Height = 700;
            }
            else
            {

                imageResizeOptions.cut = false;
                imageResizeOptions.Width = 600;
                imageResizeOptions.Height = 0;
            }
            string src = date + "/" + ImageManager.ResizeImage(path, imageResizeOptions);
            ImageView imageView = new ImageView()
            {
                src = src,
                fileID = request["fileID"],
                url = ConfigurationManager.AppSettings["UploadUrl"]
            };
            return imageView;


        }

    }
}
