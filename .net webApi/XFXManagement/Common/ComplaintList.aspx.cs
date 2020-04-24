using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;


namespace XFXManagement
{
    public partial class ComplaintList : System.Web.UI.Page
    {

        protected ComplaintBLL complaintBLL = new ComplaintBLL();
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

        protected void BindData(Func<Complaint, object> orderBy = null, string sort = "DESC")
        {

            Func<Complaint, bool> delegation = o => true;
            Filter(ref sort, ref delegation);
            int pageTotal;
            var list = complaintBLL.GetComplaint(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);
            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }



        private void Filter(ref string sort, ref Func<Complaint, bool> delegation)
        {

            bool userIDBool = true;

            int? userID = null;

            if (!string.IsNullOrWhiteSpace(UserIDInput.Value))
            {
                userIDBool = false;
                userID = Convert.ToInt32(UserIDInput.Value.Trim());

            }
            bool circlePostIDBool = true;

            int? circlePostID = null;

            if (!string.IsNullOrWhiteSpace(CirclePostIDInput.Value))
            {
                circlePostIDBool = false;
                circlePostID = Convert.ToInt32(CirclePostIDInput.Value.Trim());

            }
            bool typeBool = true;
            int  type = 0;
            if (DropDownListType.SelectedValue != "-1")
            {
                typeBool = false;
                type = Convert.ToInt32(DropDownListType.SelectedValue);

            }
            delegation = o => (userIDBool || o.PassivityUserID == userID) &&
            (circlePostIDBool || o.CirclePostID == circlePostID) &&
            (typeBool || o.Type == type);
        }
    }
}