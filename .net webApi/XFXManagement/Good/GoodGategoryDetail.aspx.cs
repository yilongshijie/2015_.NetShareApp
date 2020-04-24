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

    public partial class GoodGategoryDetail : System.Web.UI.Page
    {
        protected GoodGategory goodGategory;
        protected GoodGategoryBLL goodGategoryBLL = new GoodGategoryBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["goodGategoryID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);

                }
                else
                {
                    BindData(Convert.ToInt32(Request["goodGategoryID"]));
                }


            }
        }

        protected void BindData(int goodGategoryID)
        {

            var list = goodGategoryBLL.GetGoodGategory(goodGategory => goodGategory.GoodGategoryID == goodGategoryID);
            goodGategory = list.First();
            goodGategory.Image = ConfigurationManager.AppSettings["UploadUrl"] + goodGategory.Image;
            goodGategory.ImageHome = ConfigurationManager.AppSettings["UploadUrl"] + goodGategory.ImageHome;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("GoodGategoryDetail.aspx");
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var goodGategory = entity.GoodGategory.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        goodGategory.Image = file_url.Value;
                    }
                    var fileHome_url = (HtmlInputHidden)DetailsView1.FindControl("fileHome_url");
                    if (!string.IsNullOrEmpty(fileHome_url.Value))
                    {
                        var imgHome_url = (HtmlImage)DetailsView1.FindControl("imgHome_url");
                        imgHome_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + fileHome_url.Value;
                        goodGategory.ImageHome = fileHome_url.Value;
                    }
                    if (e.NewValues["Name"] != null)
                    {
                        goodGategory.Name = e.NewValues["Name"].ToString();
                    }
                    if (e.NewValues["Describe"] != null)
                    {
                        goodGategory.Describe = e.NewValues["Describe"].ToString();
                    }
                    if (e.NewValues["OrderBy"] != null)
                    {
                        goodGategory.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    }

                    goodGategory.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                    entity.SaveChanges();
                }
                Response.Redirect("GoodGategoryDetail.aspx?goodGategoryID=" + goodGategory.GoodGategoryID);
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
                var goodGategory = new GoodGategory();
                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                    goodGategory.Image = file_url.Value;
                }
                var fileHome_url = (HtmlInputHidden)DetailsView1.FindControl("fileHome_url");
                if (!string.IsNullOrEmpty(fileHome_url.Value))
                {
                    var imgHome_url = (HtmlImage)DetailsView1.FindControl("imgHome_url");
                    imgHome_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + fileHome_url.Value;
                    goodGategory.ImageHome = fileHome_url.Value;
                }
                if (string.IsNullOrEmpty(file_url.Value))
                {
                    throw new Exception("图标不能为空");
                }
                if (string.IsNullOrEmpty(fileHome_url.Value))
                {
                    throw new Exception("首页图片不能为空");
                }
                if (e.Values["Name"] == null)
                {
                    throw new Exception("名称不能为空");
                }
                if (e.Values["Describe"] == null)
                {
                    throw new Exception("描述不能为空");
                }

                if (e.Values["OrderBy"] != null)
                {
                    goodGategory.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());
                }
                goodGategory.Name = e.Values["Name"].ToString();
                goodGategory.Describe = e.Values["Describe"].ToString();
                goodGategory.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                using (Entity entity = new Entity())
                {
                    entity.GoodGategory.Add(goodGategory);
                    entity.SaveChanges();
                }
                Response.Redirect("GoodGategoryDetail.aspx?goodGategoryID=" + goodGategory.GoodGategoryID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = goodGategory.State.ToString();
        }


    }
}