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

    public partial class GoodHomeDetail : System.Web.UI.Page
    {
        protected GoodHome goodHome;
        protected GoodHomeBLL goodHomeBLL = new GoodHomeBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["goodHomeID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
                    CurrentMode_Init();

                }
                else
                {
                    BindData(Convert.ToInt32(Request["goodHomeID"]));
                }


            }
        }

        protected void BindData(int goodHomeID)
        {

            var list = goodHomeBLL.GetGoodHome(o => o.GoodHomeID == goodHomeID);
            goodHome = list.First();
            goodHome.Image = ConfigurationManager.AppSettings["UploadUrl"] + goodHome.Image;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("GoodHomeDetail.aspx");
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {

                using (Entity entity = new Entity())
                {
                    var goodHome = entity.GoodHome.Find(Convert.ToInt32(DetailsView1.DataKey.Value));

                    var fileHome_url = (HtmlInputHidden)DetailsView1.FindControl("fileHome_url");
                    if (!string.IsNullOrEmpty(fileHome_url.Value))
                    {
                        var imgHome_url = (HtmlImage)DetailsView1.FindControl("imgHome_url");
                        imgHome_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + fileHome_url.Value;
                        goodHome.Image = fileHome_url.Value;
                    }


                    if (e.NewValues["Title"] == null)
                    {
                        goodHome.Title = "";
                    }
                    else
                    {
                        goodHome.Title = e.NewValues["Title"].ToString();
                    }

                    if (e.NewValues["Label"] == null)
                    {
                        goodHome.Label = "";
                    }
                    else
                    {

                        goodHome.Label = e.NewValues["Label"].ToString();

                    }
                    if (e.NewValues["GoodID"] == null)
                    {
                        throw new Exception("对应商品ID不能为空");
                    }
                    if (e.NewValues["OrderBy"] != null)
                    {
                        goodHome.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    }
                    goodHome.GoodID = Convert.ToInt32(e.NewValues["GoodID"].ToString());
                    goodHome.GoodGategoryID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID")).SelectedValue);

                    goodHome.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                    goodHome.Flex = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Flex")).SelectedValue);

                    entity.SaveChanges();
                    Response.Redirect("GoodHomeDetail.aspx?goodHomeID=" + goodHome.GoodHomeID);
                }
               
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
                var goodHome = new GoodHome();

                var fileHome_url = (HtmlInputHidden)DetailsView1.FindControl("fileHome_url");
                if (!string.IsNullOrEmpty(fileHome_url.Value))
                {
                    var imgHome_url = (HtmlImage)DetailsView1.FindControl("imgHome_url");
                    imgHome_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + fileHome_url.Value;
                    goodHome.Image = fileHome_url.Value;
                }

                if (string.IsNullOrEmpty(fileHome_url.Value))
                {
                    throw new Exception("首页图片不能为空");
                }

                if (e.Values["Title"] == null)
                {
                    goodHome.Title = "";
                }
                else
                {
                    goodHome.Title = e.Values["Title"].ToString();
                }

                if (e.Values["Label"] == null)
                {
                    goodHome.Label = "";
                }
                else
                {

                    goodHome.Label = e.Values["Label"].ToString();

                }

                if (e.Values["GoodID"] == null)
                {
                    throw new Exception("对应商品ID不能为空");
                }
                if (e.Values["OrderBy"] != null)
                {
                    goodHome.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());
                }
                goodHome.GoodID = Convert.ToInt32(e.Values["GoodID"].ToString());
                goodHome.GoodGategoryID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID")).SelectedValue);

                goodHome.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                goodHome.Flex = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Flex")).SelectedValue);

                using (Entity entity = new Entity())
                {
                    entity.GoodHome.Add(goodHome);
                    entity.SaveChanges();
                }
                Response.Redirect("GoodHomeDetail.aspx?goodHomeID=" + goodHome.GoodHomeID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {

            if (goodHome != null)
            {
                ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = goodHome.State.ToString();
                ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Flex")).SelectedValue = goodHome.Flex.ToString();
            }
            var DropDownList_GoodGategoryID = ((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID"));
            if (DropDownList_GoodGategoryID != null)
            {
                DropDownList_GoodGategoryID.DataSource = new GoodGategoryBLL().GetGoodGategory(o => o.State == 1).Select(o =>
                new
                {
                    Name = o.Name,
                    GoodGategoryID = o.GoodGategoryID
                });
                DropDownList_GoodGategoryID.DataBind();

            }
            if (goodHome != null && DropDownList_GoodGategoryID != null)
            {
                DropDownList_GoodGategoryID.SelectedValue = goodHome.GoodGategoryID.ToString();
            }
        }

    }
}