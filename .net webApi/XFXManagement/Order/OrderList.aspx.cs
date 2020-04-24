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

    public partial class OrderList : System.Web.UI.Page
    {
        protected GoodBLL goodBLL = new GoodBLL();
        protected OrderBLL orderBLL = new OrderBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["state"] != null && Request["state"] == "s2f4")
                {
                    CheckBoxList_State.Items[1].Selected = true;
                    CheckBoxList1.Items[0].Selected = true;
                }
                BindData();

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

        protected void BindData(Func<Order, object> orderBy = null, string sort = "DESC")
        {

            Func<Order, bool> delegation = circleType => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = orderBLL.GetOrder(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("OrderDetail.aspx?orderID=" + GridView1.SelectedDataKey.Value);
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


        private void Filter(ref Func<Order, object> orderBy, ref string sort, ref Func<Order, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "OrderID": orderBy = o => o.OrderID; break;
                    case "UserID": orderBy = o => o.UserID; break;
                    case "State": orderBy = o => o.State; break;
                    case "Num": orderBy = o => o.Num; break;
                    case "LogisticsNumber": orderBy = o => o.LogisticsNumber; break;
                    case "UpdateTime": orderBy = o => o.UpdateTime; break;
                    case "OrderExtend.PaymentPrice": orderBy = o => o.OrderExtend.PaymentPrice; break;
                    case "OrderExtend.ThirdPartyPayment": orderBy = o => o.OrderExtend.ThirdPartyPayment; break;
                    case "OrderExtend.ThirdPartyPaymentNumber": orderBy = o => o.OrderExtend.ThirdPartyPaymentNumber; break;
                }
            }
            bool orderIDBool = true, StateBool = true, userIDBool = true, logisticsNumberBool= true;
            string orderID = null;
            int userID = 0;
            int State = 0;
            string logisticsNumber = "";

            if (!string.IsNullOrWhiteSpace(OrderID.Value))
            {
                orderIDBool = false;
                orderID = OrderID.Value;
            }
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);
            }
            if (!string.IsNullOrWhiteSpace(LogisticsNumber.Value))
            {
                logisticsNumberBool = false;
                logisticsNumber = LogisticsNumber.Value;
            }


            foreach (ListItem item in CheckBoxList_State.Items)
            {
                if (item.Selected)
                {
                    State = State | Convert.ToInt32(item.Value);
                    StateBool = false;
                }
            }
            if (CheckBoxList1.Items[0].Selected)
            {
                delegation = o =>
    (orderIDBool || (o.OrderID == orderID)) &&
     (userIDBool || (o.UserID == userID)) &&
        (logisticsNumberBool || (o.LogisticsNumber == logisticsNumber)) &&
    (StateBool || (o.State & State) > 0) && ((o.State & 2) > 0 && (o.State & 4) == 0);
            }
            else
            {
                delegation = o =>
    (orderIDBool || (o.OrderID == orderID)) &&
     (userIDBool || (o.UserID == userID)) &&
        (logisticsNumberBool || (o.LogisticsNumber == logisticsNumber)) &&
    (StateBool || (o.State & State) > 0);
            }


        }


    }
}