using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{

    public partial class OrderDetail : System.Web.UI.Page
    {
        protected Order order;
        protected OrderBLL orderBLL = new OrderBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ViewState["orderID"] = Request["orderID"];
                BindData(Request["orderID"]);
                Panel1.Visible = true;

            }
        }

        protected void BindData(string orderID)
        {

            var list = orderBLL.GetOrder(orderID);
            order = list.First();

            order.Image = ConfigurationManager.AppSettings["UploadUrl"] + order.Image;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            BindData(DetailsView1.DataKey.Value.ToString());

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {

                using (Entity entity = new Entity())
                {
                    var order = entity.Order.Find(DetailsView1.DataKey.Value.ToString());

                    var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                    order.State = 0;
                    int fahuoTemp = order.State & 4;
                    foreach (ListItem item in CheckBoxList_State.Items)
                    {
                        if (item.Selected)
                        {
                            order.State = order.State | Convert.ToInt32(item.Value);
                        }
                    }

                    if (fahuoTemp == 0)
                    {
                        if ((order.State & 4) > 0)
                        {
                            OrderLog orderLog = new OrderLog();
                            orderLog.OrderID = order.OrderID;
                            orderLog.State = 2;
                            orderLog.CreateTime = DateTime.Now;
                            orderLog.Mark = "卖家发货";
                            orderLog.UserId = Convert.ToInt32(Session["userID"]);
                            entity.OrderLog.Add(orderLog);
                            if (order.UserID != null)
                            {
                                string text = string.Format("您的订单{0}已经发货", order.OrderID);
                                UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), order.UserID.Value, text, 1 | 8);

                                ThreadPool.QueueUserWorkItem(delegate (object a)
                                {
                                    string tt = @"您的订单 " + order.OrderID + " 已经发货,不日则到。请注意查收您的订单 ";
                                    using (Entity entity1 = new Entity())
                                    {
                                        UserSMS userSMS = new UserSMS() { Tel = entity1.User.Find(order.UserID).Tel };

                                        string bb = SMS.sendSMS(userSMS.Tel, tt, userSMS.SerialNumber);
                                    }
                                });
                            }
                        }
                    }


                    var DropDownList1 = ((DropDownList)DetailsView1.FindControl("DropDownList1"));
                    order.LogisticsCompany = DropDownList1.SelectedValue;
                    if (e.NewValues["LogisticsNumber"] != null)
                    {
                        order.LogisticsNumber = e.NewValues["LogisticsNumber"].ToString();
                    }
                    order.UpdateTime = DateTime.Now;

                    entity.SaveChanges();

                    Response.Redirect("OrderDetail.aspx?OrderID=" + order.OrderID);
                }
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }



        private void CurrentMode_Init()
        {


            var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
            if (CheckBoxList_State != null && order != null)
            {

                if ((order.State & 1) > 0)
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if ((order.State & 2) > 0)
                {
                    CheckBoxList_State.Items[1].Selected = true;
                }
                if ((order.State & 4) > 0)
                {
                    CheckBoxList_State.Items[2].Selected = true;
                }
                if ((order.State & 8) > 0)
                {
                    CheckBoxList_State.Items[3].Selected = true;
                }
                if ((order.State & 16) > 0)
                {
                    CheckBoxList_State.Items[4].Selected = true;
                }
                if ((order.State & 32) > 0)
                {
                    CheckBoxList_State.Items[5].Selected = true;
                }
                if ((order.State & 64) > 0)
                {
                    CheckBoxList_State.Items[6].Selected = true;
                }
                if ((order.State & 128) > 0)
                {
                    CheckBoxList_State.Items[7].Selected = true;
                }

                if ((order.State & 256) > 0)
                {
                    CheckBoxList_State.Items[8].Selected = true;
                }

                if ((order.State & 512) > 0)
                {
                    CheckBoxList_State.Items[9].Selected = true;
                }

                if ((order.State & 1024) > 0)
                {
                    CheckBoxList_State.Items[10].Selected = true;
                }

                if ((order.State & 2048) > 0)
                {
                    CheckBoxList_State.Items[11].Selected = true;
                }

            }
            var DropDownList1 = ((DropDownList)DetailsView1.FindControl("DropDownList1"));
            if (DropDownList1 != null && order != null)
            {
                DropDownList1.SelectedValue = order.LogisticsCompany;
            }

        }


    }
}