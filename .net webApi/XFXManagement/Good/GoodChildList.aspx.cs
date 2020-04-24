using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class GoodChildList : System.Web.UI.Page
    {
        protected GoodBLL goodBLL = new GoodBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request["goodID"] != null)
                {
                    ViewState["goodID"] = Request["goodID"];
                    BindData(Convert.ToInt32(Request["goodID"]));
                }

            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("GoodChildDetail.aspx?goodID=" + ViewState["goodID"]);
        }


        protected void BindData(int goodID)
        {
            var list = goodBLL.GetGoodChild(goodID);
            GridView1.DataSource = list;
            GridView1.DataBind();
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("GoodChildDetail.aspx?goodChildID=" + GridView1.SelectedDataKey.Value+"&goodID=" + ViewState["goodID"]);
        }

    }
}