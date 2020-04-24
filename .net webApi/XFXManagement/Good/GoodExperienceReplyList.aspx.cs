using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class GoodExperienceReplyList : System.Web.UI.Page
    {
        protected GoodExperienceBLL goodExperienceBLL = new GoodExperienceBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request["goodExperienceID"] != null)
                {
                    ViewState["goodExperienceID"] = Request["goodExperienceID"];
                    BindData(Convert.ToInt32(Request["goodExperienceID"]));
                }
                else
                {
                    ViewState["goodExperienceID"] = 0;
                    BindData(0);
        
                }

            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "StateDelete")
            {
                int goodExperienceReplyID = Convert.ToInt32(e.CommandArgument);
                using (Entity entity = new Entity())
                {
                    entity.GoodExperienceReply.Find(goodExperienceReplyID).State = 2;
                    entity.SaveChanges();
                }
                BindData(Convert.ToInt32(ViewState["goodExperienceID"]));
            }
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData(Convert.ToInt32(ViewState["goodExperienceID"]));
        }
        protected void BindData(int goodExperienceID)
        {

            int pageTotal;
            var list = goodExperienceBLL.GetGoodExperienceReply(o => o.GoodExperienceID == goodExperienceID || goodExperienceID == 0 , AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal);
            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }
    }
}