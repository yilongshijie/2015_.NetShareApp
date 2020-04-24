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
    public partial class AdvertisementDetail : System.Web.UI.Page
    {
        protected Advertisement advertisement;
        protected AdvertisementBLL advertisementBLL = new AdvertisementBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["advertisementID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);

                }
                else
                {
                    BindData(Convert.ToInt32(Request["advertisementID"]));
                }
            }
        }

        protected void BindData(int advertisementID)
        {

            var list = advertisementBLL.GetAdvertisement(o => o.AdvertisementID == advertisementID);
            advertisement = list.First();
            advertisement.Image = ConfigurationManager.AppSettings["UploadUrl"] + advertisement.Image;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("AdvertisementDetail.aspx");
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var advertisement = entity.Advertisement.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        advertisement.Image = file_url.Value;
                    }
                    if (e.NewValues["Title"] != null)
                    {
                        advertisement.Title = e.NewValues["Title"].ToString();
                    }
                    if (e.NewValues["Link"] == null)
                    {
                        throw new Exception("对应ID不能为空");
                    }

                    if (e.NewValues["OrderBy"] != null)
                    {
                        advertisement.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    }
                    advertisement.Link = Convert.ToInt32(e.NewValues["Link"].ToString()).ToString();
                    advertisement.Type = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue);
                    advertisement.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                    entity.SaveChanges();
                }
                Response.Redirect("AdvertisementDetail.aspx?AdvertisementID=" + DetailsView1.DataKey.Value);
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
                var advertisement = new Advertisement();
                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                    advertisement.Image = file_url.Value;
                }
                else
                {
                    throw new Exception("图片不能为空");
                }
                if (e.Values["Title"] == null)
                {
                    throw new Exception("标题不能为空");
                }
                if (e.Values["Link"] == null)
                {
                    throw new Exception("对应ID不能为空");
                }

                if (e.Values["OrderBy"] != null)
                {
                    advertisement.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());
                }
                advertisement.Time = DateTime.Now;
                advertisement.Link = Convert.ToInt32(e.Values["Link"].ToString()).ToString();
                advertisement.Title = e.Values["Title"].ToString();
                advertisement.Type = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue);
                advertisement.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                using (Entity entity = new Entity())
                {
                    entity.Advertisement.Add(advertisement);
                    entity.SaveChanges();
                }
                Response.Redirect("AdvertisementDetail.aspx?AdvertisementID=" + advertisement.AdvertisementID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {

            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = advertisement.State.ToString();
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Type")).SelectedValue = advertisement.Type.ToString();
        }

    }
}