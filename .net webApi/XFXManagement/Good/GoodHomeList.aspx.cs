using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class GoodHomeList : System.Web.UI.Page
    {
        protected GoodHomeBLL goodHomeBLL = new GoodHomeBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                DropDownListGoodGategory.DataSource = new GoodGategoryBLL().GetGoodGategory(o => o.State == 1);
                DropDownListGoodGategory.DataTextField = "Name";
                DropDownListGoodGategory.DataValueField = "GoodGategoryID";

                DropDownListGoodGategory.DataBind();
                DropDownListGoodGategory.Items.Add(new ListItem() { Text = "全部", Value = "0" });
                DropDownListGoodGategory.SelectedValue = "0";
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("GoodHomeDetail.aspx");
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
        protected void BindData(Func<GoodHome, object> orderBy = null, string sort = "DESC")
        {

            Func<GoodHome, bool> delegation = goodGategory => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = goodHomeBLL.GetGoodHome(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("GoodHomeDetail.aspx?goodHomeID=" + GridView1.SelectedDataKey.Value);
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

        private void Filter(ref Func<GoodHome, object> orderBy, ref string sort, ref Func<GoodHome, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "GoodGategoryID": orderBy = o => o.GoodGategoryID; break;
                    case "GoodID": orderBy = o => o.GoodID; break;
                    case "Title": orderBy = o => o.Title; break;
                    case "Label": orderBy = o => o.Label; break;
                    case "State": orderBy = o => o.State; break;
                    case "Flex": orderBy = o => o.Flex; break;
                    case "OrderBy": orderBy = o => o.OrderBy; break;
                }
            }
            bool stateBool = true, goodGategoryIDBool = true;

            int state = 0;
            int goodGategoryID = 0;
            if (DropDownListGoodGategory.SelectedValue != "0")
            {
                goodGategoryIDBool = !int.TryParse(DropDownListGoodGategory.SelectedValue, out goodGategoryID);
            }


            if (DropDownListState.SelectedValue != "-1")
            {
                stateBool = false;
                state = Convert.ToInt32(DropDownListState.SelectedValue);
            }
            delegation = o =>
               (goodGategoryIDBool || o.GoodGategoryID == goodGategoryID) &&
                 (stateBool || o.State == state);
        }

    }
}