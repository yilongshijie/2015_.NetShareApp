using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class UserList : System.Web.UI.Page
    {
        protected UserBLL userBLL = new UserBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["state"] != null && Request["state"] == "jubao")
            {
                CheckBoxList1.Items[0].Selected = true;
            }
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindData();
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserDetail.aspx");
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void BindData(Func<User, object> orderBy = null, string sort = "DESC")
        {

            Func<User, bool> delegation = user => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = userBLL.GetUsers(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("UserDetail.aspx?userID=" + GridView1.SelectedDataKey.Value);
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

        private void Filter(ref Func<User, object> orderBy, ref string sort, ref Func<User, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "UserID": orderBy = o => o.UserID; break;
                    case "Tel": orderBy = o => o.Tel; break;
                    case "NickName": orderBy = o => o.NickName; break;
                    case "Type": orderBy = o => o.Type; break;
                    case "State": orderBy = o => o.State; break;
                    case "UserExtend.ExperienceLevel": orderBy = o => (o.UserExtend ?? new UserExtend()).ExperienceLevel; break;
                    case "UserExtend.ComplaintUntreated": orderBy = o => (o.UserExtend ?? new UserExtend()).ComplaintUntreated; break;
                }
            }
            bool userIDBool = true, telBool = true, nickNameBool = true, userTypeBool = true;
            int userID = 0;
            string tel = null;
            string nickName = null;
            int userType = 0;
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);


            }
            if (!string.IsNullOrWhiteSpace(Tel.Value))
            {
                telBool = false;
                tel = Tel.Value.Trim();

            }

            if (!string.IsNullOrWhiteSpace(NickName.Value))
            {
                nickNameBool = false;
                nickName = NickName.Value.Trim();
            }
            if (UserTypeDropDownList.SelectedValue != "0")
            {
                userTypeBool = false;
                userType = Convert.ToInt32(UserTypeDropDownList.SelectedValue);
            }

            if (!CheckBoxList1.Items[0].Selected)
            {
                delegation = o =>
            (userIDBool || o.UserID == userID) &&
            (telBool || o.Tel == tel) &&
            (nickNameBool || o.NickName == nickName) &&
            (userTypeBool || ((o.Type & userType) > 0));
            }
            else
            {
                delegation = o =>
                    (userIDBool || o.UserID == userID) &&
                    (telBool || o.Tel == tel) &&
                    (nickNameBool || o.NickName == nickName) &&
                    (userTypeBool || ((o.Type & userType) > 0)) &&
                    o.UserExtend.ComplaintUntreated > 0;
            }

        }
    }
}