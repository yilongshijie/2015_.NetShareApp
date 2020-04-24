using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class UserGradeLogList : System.Web.UI.Page
    {
        protected UserGradeLogBLL userGradeLogBLL = new UserGradeLogBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

 
        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void BindData(Func<UserGradeLog, object> orderBy = null, string sort = "DESC")
        {

            Func<UserGradeLog, bool> delegation = o => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = userGradeLogBLL.GetAdvertisement(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


 

        protected void Submit_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindData();
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

        private void Filter(ref Func<UserGradeLog, object> orderBy, ref string sort, ref Func<UserGradeLog, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "UserGradeLogID": orderBy = o => o.UserGradeLogID; break;
                    case "UserID": orderBy = o => o.UserID; break;
                    case "Value": orderBy = o => o.Value; break;
                    case "Type": orderBy = o => o.Type; break;
                    case "Mark": orderBy = o => o.Mark; break;
                    case "CreateTime": orderBy = o => o.CreateTime; break; 
                }
            }
            bool typeBool = true ,  userIDBool = true;
            int type = 0;
            int userID = 0;

            if (DropDownListType.SelectedValue != "-1")
            {
                typeBool = false;
                type = Convert.ToInt32(DropDownListType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);


            }
 
            delegation = o => (userIDBool || o.UserID == userID) && (typeBool || o.Type == type);
        }

    }
}