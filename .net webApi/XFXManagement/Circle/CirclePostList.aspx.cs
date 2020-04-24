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
    public partial class CirclePostList : System.Web.UI.Page
    {
        protected CirclePostBLL circlePostBLL = new CirclePostBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["state"] != null && Request["state"] == "1")
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if (Request["state"] != null && Request["state"] == "jubao")
                {
                    CheckBoxList1.Items[0].Selected = true;
                    CheckBoxList_State.Items[0].Selected = false;
                }
                BindData();

                CircleTypeID.DataSource = new CircleTypeBLL().GetCircleType(o => true);
                CircleTypeID.DataTextField = "Title";
                CircleTypeID.DataValueField = "CircleTypeID";

                CircleTypeID.DataBind();
                CircleTypeID.Items.Add(new ListItem() { Text = "全部", Value = "0" });
                CircleTypeID.SelectedValue = "0";


            }

        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("CirclePostDetail.aspx");
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

        protected void BindData(Func<CirclePost, object> orderBy = null, string sort = "DESC")
        {

            Func<CirclePost, bool> delegation = circleType => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = circlePostBLL.GetCirclePost(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("CirclePostDetail.aspx?circlePostID=" + GridView1.SelectedDataKey.Value);
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


        private void Filter(ref Func<CirclePost, object> orderBy, ref string sort, ref Func<CirclePost, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "CirclePostID": orderBy = o => o.CirclePostID; break;
                    case "CircleTypeID": orderBy = o => o.CircleTypeID; break;
                    case "Title": orderBy = o => o.Title; break;
                    case "ReplyNum": orderBy = o => o.ReplyNum; break;
                    case "ComplaintNum": orderBy = o => o.ComplaintNum; break;
                    case "CreateTime": orderBy = o => o.CreateTime; break;
                    case "ComplaintUntreated": orderBy = o => o.ComplaintUntreated; break;
                    case "UpdateTime": orderBy = o => o.UpdateTime; break;
                }
            }
            bool userIDBool = true, circleTypeIDBool = true, circlePostIDBool = true, StateBool = true;
            int userID = 0;
            int circleTypeID = 0;
            int circlePostID = 0;
            int State = 0;
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);


            }

            if (CircleTypeID.SelectedValue != "0")
            {
                circleTypeIDBool = !int.TryParse(CircleTypeID.SelectedValue, out circleTypeID);
            }

            if (!string.IsNullOrWhiteSpace(CirclePostID.Value))
            {
                circlePostIDBool = !int.TryParse(CirclePostID.Value, out circlePostID);
            }

            foreach (ListItem item in CheckBoxList_State.Items)
            {
                if (item.Selected)
                {
                    State = State | Convert.ToInt32(item.Value);
                    StateBool = false;
                }
            }
            if (!CheckBoxList1.Items[0].Selected)
            {
                delegation = o =>
   (userIDBool || o.UserID == userID) &&
   (circleTypeIDBool || o.CircleTypeID == circleTypeID) &&
   (circlePostIDBool || o.CirclePostID == circlePostID) &&
   (StateBool || (o.State & State) > 0);
            }
            else
            {

                delegation = o =>
   (userIDBool || o.UserID == userID) &&
   (circleTypeIDBool || o.CircleTypeID == circleTypeID) &&
   (circlePostIDBool || o.CirclePostID == circlePostID) &&
   (StateBool || (o.State & State) > 0) &&
    o.ComplaintUntreated > 0;
            }

        }


    }
}