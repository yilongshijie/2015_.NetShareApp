using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;


namespace XFXManagement
{
    public partial class UserSMSList : System.Web.UI.Page
    {

        protected UserSMSBLL userSMSBLL = new UserSMSBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void BindData(Func<UserSMS, object> orderBy = null, string sort = "DESC")
        {

            Func<UserSMS, bool> delegation = o => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = userSMSBLL.GetUsers(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
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

        private void Filter(ref Func<UserSMS, object> orderBy, ref string sort, ref Func<UserSMS, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "UserSMSID": orderBy = o => o.UserSMSID; break;
                    case "Tel": orderBy = o => o.Tel; break;
                    case "Yanzhengma": orderBy = o => o.Yanzhengma; break;
                    case "State": orderBy = o => o.State; break;
                    case "SentTime": orderBy = o => o.SentTime; break;
                    case "Ip": orderBy = o => o.Ip; break;
                }
            }
            bool telBool = true;

            string tel = null;

            if (!string.IsNullOrWhiteSpace(Tel.Value))
            {
                telBool = false;
                tel = Tel.Value.Trim();

            }

            delegation = o => (telBool || o.Tel == tel);
        }
    }
}