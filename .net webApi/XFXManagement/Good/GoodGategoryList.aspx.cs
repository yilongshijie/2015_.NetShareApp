using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class GoodGategoryList : System.Web.UI.Page
    {
        protected GoodGategoryBLL goodGategoryBLL = new GoodGategoryBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("GoodGategoryDetail.aspx");
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindData();
        }
        protected void BindData(Func<GoodGategory, object> orderBy = null, string sort = "DESC")
        {

            Func<GoodGategory, bool> delegation = goodGategory => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = goodGategoryBLL.GetGoodGategory(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("GoodGategoryDetail.aspx?goodGategoryID=" + GridView1.SelectedDataKey.Value);
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                if (sortData[1] == "ASC")
                {
                    ViewState["sortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            else
            {
                ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
            }
            BindData();

        }

        private void Filter(ref Func<GoodGategory, object> orderBy, ref string sort, ref Func<GoodGategory, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "GoodGategoryID": orderBy = o => o.GoodGategoryID; break;
                    case "Name": orderBy = o => o.Name; break;
                    case "Describe": orderBy = o => o.Describe; break;
                    case "State": orderBy = o => o.State; break;
                    case "OrderBy": orderBy = o => o.OrderBy; break;
                }
            }
            bool stateBool = true;

            int state = 0;

            if (DropDownListState.SelectedValue != "-1")
            {
                stateBool = false;
                state = Convert.ToInt32(DropDownListState.SelectedValue);
            }
            delegation = o =>
                 (stateBool || o.State == state);
        }

    }
}