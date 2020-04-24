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

    public partial class OrderLogList : System.Web.UI.Page
    {
        protected OrderBLL orderBLL = new OrderBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["orderID"] = Request["orderID"];
                BindData(ViewState["orderID"].ToString());
            }

        }


        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData(ViewState["orderID"].ToString());
        }

 
        protected void BindData(string orderID)
        {
            Func<OrderLog, object> orderBy = null;
            string sort = "DESC";
            Func<OrderLog, bool> delegation = o => o.OrderID == orderID;

            int pageTotal;
            var list = orderBLL.GetOrderLog(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }







    }
}