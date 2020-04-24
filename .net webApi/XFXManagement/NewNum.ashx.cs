using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XFXClassLibrary;
using System.Web.SessionState;

namespace XFXManagement
{
    /// <summary>
    /// NewNum 的摘要说明
    /// </summary>
    public class NewNum : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int tiezi = 0, dingdan = 0, jubaoyonghu = 0, jubaotiezi = 0, tiyanshi = 0;

            using (Entity entity = new Entity())
            {
                tiezi = entity.CirclePost.Where(o => (o.State & 1) > 0).Count();
                dingdan = entity.Order.Where(o => ((o.State & 2) > 0) && ((o.State & 4) == 0)).Count();
                jubaoyonghu = entity.UserExtend.Sum(o => o.ComplaintUntreated);
                jubaotiezi = entity.CirclePost.Sum(o => o.ComplaintUntreated);
                tiyanshi = entity.GoodExperience.Where(o => o.State == 1).Count();
                context.Response.Write(string.Format("{0},{1},{2},{3},{4},{5}", tiezi, dingdan, jubaotiezi, jubaoyonghu, tiyanshi, context.Session["userID"] ?? 0));
            }
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