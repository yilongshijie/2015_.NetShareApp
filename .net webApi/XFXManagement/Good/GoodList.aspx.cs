using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{

    public partial class GoodList : System.Web.UI.Page
    {
        protected GoodBLL goodBLL = new GoodBLL();
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
            Response.Redirect("GoodDetail.aspx");
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

        protected void BindData(Func<Good, object> orderBy = null, string sort = "DESC")
        {

            Func<Good, bool> delegation = circleType => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = goodBLL.GetGood(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("GoodDetail.aspx?goodID=" + GridView1.SelectedDataKey.Value);
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


        private void Filter(ref Func<Good, object> orderBy, ref string sort, ref Func<Good, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "GoodGategoryID": orderBy = o => o.GoodGategoryID; break;
                    case "BidPrice": orderBy = o => o.BidPrice; break;
                    case "RealPrice": orderBy = o => o.RealPrice; break;
                    case "State": orderBy = o => o.State; break;
                    case "Repertory": orderBy = o => o.Repertory; break;
                    case "SalesVolume": orderBy = o => o.SalesVolume; break;
                    case "EvaluateNum": orderBy = o => o.EvaluateNum; break;
                    case "EvaluateValue": orderBy = o => o.EvaluateValue; break;
                    case "UpdateTime": orderBy = o => o.UpdateTime; break;

                }
            }
            bool goodIDBool = true, goodGategoryIDBool = true, StateBool = true;
            int goodID = 0;
            int goodGategoryID = 0;
            int State = 0;

            if (!string.IsNullOrWhiteSpace(GoodID.Value))
            {
                goodIDBool = !int.TryParse(GoodID.Value, out goodID);
            }

            if (DropDownListGoodGategory.SelectedValue != "0")
            {
                goodGategoryIDBool = !int.TryParse(DropDownListGoodGategory.SelectedValue, out goodGategoryID);
            }


            foreach (ListItem item in CheckBoxList_State.Items)
            {
                if (item.Selected)
                {
                    State = State | Convert.ToInt32(item.Value);
                    StateBool = false;
                }
            }

            delegation = o =>
                 (goodIDBool || o.GoodID == goodID) &&
                 (goodGategoryIDBool || o.GoodGategoryID == goodGategoryID) &&
                 (StateBool || (o.State & State) > 0);
        }


    }
}