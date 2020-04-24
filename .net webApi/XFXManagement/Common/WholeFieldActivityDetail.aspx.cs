using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class WholeFieldActivityDetail : System.Web.UI.Page
    {
        protected WholeFieldActivity wholeFieldActivity;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["wholeFieldActivityID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);

                }
                else
                {
                    BindData(Convert.ToInt32(Request["wholeFieldActivityID"]));
                }
            }
        }

        protected void BindData(int wholeFieldActivityID)
        {
            using (Entity entity = new Entity())
            {
                var list = entity.WholeFieldActivity.Where(o => o.WholeFieldActivityID == wholeFieldActivityID).ToList();
                wholeFieldActivity = list.First();
                DetailsView1.DataSource = list;

            }
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("WholeFieldActivityDetail.aspx");
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var wholeFieldActivity = entity.WholeFieldActivity.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    if (e.NewValues["Title"] == null)
                    {
                        throw new Exception("标题不能为空");
                    }
                    if (e.NewValues["FillPrice"] == null)
                    {
                        throw new Exception("满金额不能为空");
                    }
                    wholeFieldActivity.Type = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue);
                    wholeFieldActivity.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                    if (wholeFieldActivity.Type == 0)
                    {
                        if (e.NewValues["DiscountPrice"] == null)
                        {
                            throw new Exception("当为满减时，减的额度不能为空");

                        }
                        wholeFieldActivity.DiscountPrice = Convert.ToDecimal(e.NewValues["DiscountPrice"]);
                    }
                    wholeFieldActivity.FillPrice = Convert.ToDecimal(e.NewValues["FillPrice"]);
                    wholeFieldActivity.Title = e.NewValues["Title"].ToString();
                    entity.SaveChanges();
                }
                Response.Redirect("WholeFieldActivityDetail.aspx?wholeFieldActivityID=" + wholeFieldActivity.WholeFieldActivityID);

            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var wholeFieldActivity = new WholeFieldActivity();


                if (e.Values["Title"] == null)
                {
                    throw new Exception("标题不能为空");
                }
                if (e.Values["FillPrice"] == null)
                {
                    throw new Exception("满金额不能为空");
                }
                wholeFieldActivity.Type = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue);
                wholeFieldActivity.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                if (wholeFieldActivity.Type == 0)
                {
                    if (e.Values["DiscountPrice"] == null)
                    {
                        throw new Exception("当为满减时，减的额度不能为空");
                    }
                    wholeFieldActivity.DiscountPrice = Convert.ToDecimal(e.Values["DiscountPrice"]);
                }
                wholeFieldActivity.Title = e.Values["Title"].ToString();
                wholeFieldActivity.FillPrice = Convert.ToDecimal(e.Values["FillPrice"]);
                wholeFieldActivity.StartTime = DateTime.MinValue;
                wholeFieldActivity.EndTime = DateTime.MinValue;
                using (Entity entity = new Entity())
                {
                    entity.WholeFieldActivity.Add(wholeFieldActivity);
                    entity.SaveChanges();
                }
                Response.Redirect("WholeFieldActivityDetail.aspx?wholeFieldActivityID=" + wholeFieldActivity.WholeFieldActivityID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = wholeFieldActivity.State.ToString();
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue = wholeFieldActivity.Type.ToString();
            if (ConfigurationManager.AppSettings["quanchang"] == "disable")
            {
                ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).Enabled = false;
                ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).Enabled = false;
            }
        }

    }
}