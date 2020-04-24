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

    public partial class CircleTypeDetail : System.Web.UI.Page
    {
        protected CircleType circleType;
        protected CircleTypeBLL circleTypeBLL = new CircleTypeBLL();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["circleTypeID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);

                }
                else
                {
                    BindData(Convert.ToInt32(Request["circleTypeID"]));
                }


            }
        }

        protected void BindData(int circleTypeID)
        {

            var list = circleTypeBLL.GetCircleType(circleType => circleType.CircleTypeID == circleTypeID);
            circleType = list.First();
            circleType.Image = ConfigurationManager.AppSettings["UploadUrl"] + circleType.Image;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("CircleTypeList.aspx");
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var circleType = entity.CircleType.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        circleType.Image = file_url.Value;
                    }

                    if (e.NewValues["Title"] != null)
                    {
                        circleType.Title = e.NewValues["Title"].ToString();
                    }
                    if (e.NewValues["SubTitle"] != null)
                    {
                        circleType.SubTitle = e.NewValues["SubTitle"].ToString();
                    }
                    if (e.NewValues["OrderBy"] != null)
                    {
                        circleType.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    }

                    circleType.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                    entity.SaveChanges();
                }
                Response.Redirect("CircleTypeDetail.aspx?circleTypeID=" + DetailsView1.DataKey.Value);
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
                var circleType = new CircleType();
                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                    circleType.Image = file_url.Value;
                }
                else
                {
                    throw new Exception("图片不能为空");
                }
                if (e.Values["Title"] == null)
                {
                    throw new Exception("标题不能为空");
                }
                if (e.Values["SubTitle"] == null)
                {
                    throw new Exception("子标题不能为空");
                }

                if (e.Values["OrderBy"] != null)
                {
                    circleType.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());
                }
                circleType.Title = e.Values["Title"].ToString();
                circleType.SubTitle = e.Values["SubTitle"].ToString();
                circleType.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                using (Entity entity = new Entity())
                {
                    entity.CircleType.Add(circleType);
                    entity.SaveChanges();
                }
                Response.Redirect("CircleTypeDetail.aspx?circleTypeID=" + circleType.CircleTypeID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = circleType.State.ToString();
        }


    }
}