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

    public partial class GoodDetail : System.Web.UI.Page
    {
        protected Good good;
        protected GoodGategoryBLL goodGategoryBLL = new GoodGategoryBLL();
        protected GoodBLL goodBLL = new GoodBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["goodID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
                    CurrentMode_Init();
                    Panel1.Visible = false;
                }
                else
                {
                    ViewState["goodID"] = Request["goodID"];
                    BindData(Convert.ToInt32(Request["goodID"]));
                    Panel1.Visible = true;
                }


            }
        }

        protected void BindData(int GoodID)
        {

            var list = goodBLL.GetGood(o => o.GoodID == GoodID);
            good = list.First();

            good.ImageList = XFXExt.imgList(good.ImageList, ConfigurationManager.AppSettings["UploadUrl"]);
            good.Image = ConfigurationManager.AppSettings["UploadUrl"] + good.Image;
            good.Detail = good.Detail.Replace("***xing*fen*xiang*img*src***", ConfigurationManager.AppSettings["UploadUrl"]);
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("GoodDetail.aspx");
            }
            DetailsView1.ChangeMode(e.NewMode);
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var good = entity.Good.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var HiddenField1 = (HiddenField)DetailsView1.FindControl("HiddenField1");
                    var HiddenField2 = (HiddenField)DetailsView1.FindControl("HiddenField2");
                    var HiddenField3 = (HiddenField)DetailsView1.FindControl("HiddenField3");
                    var img_div = (HtmlGenericControl)DetailsView1.FindControl("img_div");
                    img_div.InnerHtml = HiddenField2.Value;

                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        good.Image = file_url.Value;
                    }


                    if (string.IsNullOrWhiteSpace(HiddenField1.Value))
                    {
                        throw new Exception("内容不能为空");

                    }

                    if (string.IsNullOrWhiteSpace(HiddenField3.Value))
                    {
                        throw new Exception("商品详细页滚动图片不能为空");

                    }

                    if (e.NewValues["Title"] == null)
                    {
                        throw new Exception("标题不能为空");
                    }
                    good.Title = e.NewValues["Title"].ToString();
                    good.SubTitle = e.NewValues["SubTitle"].ToString();
                    good.BidPrice = Convert.ToDecimal(e.NewValues["BidPrice"].ToString());
                    good.RealPrice = Convert.ToDecimal(e.NewValues["RealPrice"].ToString());
                    good.RealPrice = Convert.ToDecimal(e.NewValues["RealPrice"].ToString());

                    var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                    int tempState = good.State;
                    good.State = 0;
                    foreach (ListItem item in CheckBoxList_State.Items)
                    {
                        if (item.Selected)
                        {
                            good.State = good.State | Convert.ToInt32(item.Value);
                        }
                    }
                    if ((good.State & 2) > 0 && (good.State & 1) > 0)
                    {
                        good.State--;
                    }
                    if (!(good.GoodChild.Where(o => o.State == 1).Count() > 0))
                    {
                        if ((good.State & 2) > 0)
                        {
                            good.State = good.State ^ 2;
                        }
                    }

                    good.Detail = HiddenField1.Value.Replace(ConfigurationManager.AppSettings["UploadUrl"], "***xing*fen*xiang*img*src***");
                    good.ImageList = HiddenField3.Value.Replace(ConfigurationManager.AppSettings["UploadUrl"], "");

                    good.GoodGategoryID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID")).SelectedValue);
                    if (e.NewValues["SalesVolume"] != null)
                    {
                        good.SalesVolume = Convert.ToInt32(e.NewValues["SalesVolume"].ToString());

                    }
                    if (e.NewValues["EvaluateNum"] != null)
                    {
                        good.EvaluateNum = Convert.ToInt32(e.NewValues["EvaluateNum"].ToString());

                    }
                    if (e.NewValues["EvaluateValue"] != null)
                    {
                        good.EvaluateValue = Convert.ToDecimal(e.NewValues["EvaluateValue"].ToString());

                    }
                    if (e.NewValues["OrderBy"] != null)
                    {
                        good.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());

                    }
                    good.CreateTime = DateTime.Now;
                    good.UpdateTime = DateTime.Now;
                    good.UpdateTime = DateTime.Now;
                    good.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    entity.SaveChanges();
                    Response.Redirect("GoodDetail.aspx?goodID=" + good.GoodID);
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
                var HiddenField1 = (HiddenField)DetailsView1.FindControl("HiddenField1");
                var HiddenField2 = (HiddenField)DetailsView1.FindControl("HiddenField2");
                var HiddenField3 = (HiddenField)DetailsView1.FindControl("HiddenField3");
                var img_div = (HtmlGenericControl)DetailsView1.FindControl("img_div");
                img_div.InnerHtml = HiddenField2.Value;

                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;

                }

                if (string.IsNullOrWhiteSpace(HiddenField1.Value))
                {
                    throw new Exception("内容不能为空");

                }

                if (string.IsNullOrWhiteSpace(HiddenField3.Value))
                {
                    throw new Exception("商品详细页滚动图片不能为空");

                }
                if (string.IsNullOrEmpty(file_url.Value))
                {
                    throw new Exception("商品列表页图片不能为空");
                }
                var good = new Good();
                if (e.Values["Title"] == null)
                {
                    throw new Exception("标题不能为空");

                }

                good.Title = e.Values["Title"].ToString();
                good.SubTitle = e.Values["SubTitle"].ToString();
                good.BidPrice = Convert.ToDecimal(e.Values["BidPrice"].ToString());
                good.RealPrice = Convert.ToDecimal(e.Values["RealPrice"].ToString());
                good.RealPrice = Convert.ToDecimal(e.Values["RealPrice"].ToString());

                var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                good.State = 0;
                foreach (ListItem item in CheckBoxList_State.Items)
                {
                    if (item.Selected)
                    {
                        good.State = good.State | Convert.ToInt32(item.Value);
                    }
                }
                if ((good.State & 2) > 0)
                {
                    good.State = good.State ^ 2;
                }

                good.Detail = HiddenField1.Value.Replace(ConfigurationManager.AppSettings["UploadUrl"], "***xing*fen*xiang*img*src***");
                good.ImageList = HiddenField3.Value.Replace(ConfigurationManager.AppSettings["UploadUrl"], "");
                good.Image = file_url.Value;
                good.GoodGategoryID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID")).SelectedValue);
                if (e.Values["SalesVolume"] != null)
                {
                    good.SalesVolume = Convert.ToInt32(e.Values["SalesVolume"].ToString());

                }
                if (e.Values["EvaluateNum"] != null)
                {
                    good.EvaluateNum = Convert.ToInt32(e.Values["EvaluateNum"].ToString());

                }
                if (e.Values["EvaluateValue"] != null)
                {
                    good.EvaluateValue = Convert.ToDecimal(e.Values["EvaluateValue"].ToString());

                }
                if (e.Values["OrderBy"] != null)
                {
                    good.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());

                }

                good.CreateTime = DateTime.Now;
                good.UpdateTime = DateTime.Now;

                using (Entity entity = new Entity())
                {
                    entity.Good.Add(good);
                    entity.SaveChanges();
                }
                Response.Redirect("GoodDetail.aspx?goodID=" + good.GoodID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            var DropDownList_GoodGategoryID = ((DropDownList)DetailsView1.FindControl("DropDownList_GoodGategoryID"));
            if (DropDownList_GoodGategoryID != null)
            {
                DropDownList_GoodGategoryID.DataSource = goodGategoryBLL.GetGoodGategory(o => o.State == 1).Select(o =>
                new
                {
                    Name = o.Name,
                    GoodGategoryID = o.GoodGategoryID
                });
                DropDownList_GoodGategoryID.DataBind();
                if (good != null)
                {
                    DropDownList_GoodGategoryID.SelectedValue = good.GoodGategoryID.ToString();
                }
            }

            var HiddenField1 = (HiddenField)DetailsView1.FindControl("HiddenField1");
            if (HiddenField1 != null && good != null)
            {
                HiddenField1.Value = good.Detail;
            }

            var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
            if (CheckBoxList_State != null && good != null)
            {

                if ((good.State & 1) > 0)
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if ((good.State & 2) > 0)
                {
                    CheckBoxList_State.Items[1].Selected = true;
                }
                if ((good.State & 4) > 0)
                {
                    CheckBoxList_State.Items[2].Selected = true;
                }
                if ((good.State & 8) > 0)
                {
                    CheckBoxList_State.Items[3].Selected = true;
                }
                if ((good.State & 16) > 0)
                {
                    CheckBoxList_State.Items[4].Selected = true;
                }
                if ((good.State & 32) > 0)
                {
                    CheckBoxList_State.Items[5].Selected = true;
                }
                if ((good.State & 64) > 0)
                {
                    CheckBoxList_State.Items[6].Selected = true;
                }

            }
        }


    }
}