using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class WholeFieldActivityList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["quanchang"] == "disable")
            {
                Add.Visible = false;
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("WholeFieldActivityDetail.aspx");
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void BindData()
        {
            using (Entity entity = new Entity())
            {
                GridView1.DataSource = entity.WholeFieldActivity.ToList();
            }
            GridView1.DataBind();
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("WholeFieldActivityDetail.aspx?wholeFieldActivityID=" + GridView1.SelectedDataKey.Value);
        }

    }
}