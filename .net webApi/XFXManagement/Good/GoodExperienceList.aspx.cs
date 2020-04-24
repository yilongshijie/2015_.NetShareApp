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
    public partial class GoodExperienceList : System.Web.UI.Page
    {
        protected GoodExperienceBLL goodExperienceBLL = new GoodExperienceBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["state"] != null && Request["state"] == "1")
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                BindData();

                DropDownListGoodGategory.DataSource = new GoodGategoryBLL().GetGoodGategory(o => o.State == 1);
                DropDownListGoodGategory.DataTextField = "Name";
                DropDownListGoodGategory.DataValueField = "GoodGategoryID";

                DropDownListGoodGategory.DataBind();
                DropDownListGoodGategory.Items.Add(new ListItem() { Text = "全部", Value = "0" });
                DropDownListGoodGategory.SelectedValue = "0";
            }

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

        protected void BindData(Func<GoodExperience, object> orderBy = null, string sort = "DESC")
        {

            Func<GoodExperience, bool> delegation = circleType => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = goodExperienceBLL.GetGoodExperience(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("GoodExperienceDetail.aspx?goodExperienceID=" + GridView1.SelectedDataKey.Value);
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


        private void Filter(ref Func<GoodExperience, object> orderBy, ref string sort, ref Func<GoodExperience, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "UserID": orderBy = o => o.UserID; break;
                    case "GoodID": orderBy = o => o.GoodID; break;
                    case "Title": orderBy = o => o.Title; break;
                    case "ReplyNum": orderBy = o => o.ReplyNum; break;
                    case "UpdateTime": orderBy = o => o.UpdateTime; break;
                    case "OrderBy": orderBy = o => o.OrderBy; break;
                }
            }
            bool userIDBool = true, goodGategoryIDBool = true, goodIDBool = true, StateBool = true;
            int userID = 0;
            int goodGategoryID = 0;
            int goodID = 0;
            int State = 0;
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);


            }

            if (DropDownListGoodGategory.SelectedValue != "0")
            {
                goodGategoryIDBool = !int.TryParse(DropDownListGoodGategory.SelectedValue, out goodGategoryID);
            }

            if (!string.IsNullOrWhiteSpace(GoodID.Value))
            {
                goodIDBool = !int.TryParse(GoodID.Value, out goodID);
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
                 (userIDBool || o.UserID == userID) &&
                 (goodGategoryIDBool || o.GoodGategoryID == goodGategoryID) &&
                 (goodIDBool || o.GoodID == goodID) &&
                 (StateBool || (o.State & State) > 0);
        }


    }
}