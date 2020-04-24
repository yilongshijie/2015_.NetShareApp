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

    public partial class CirclePostDetail : System.Web.UI.Page
    {
        protected CirclePost circlePost;
        protected CirclePostBLL circlePostBLL = new CirclePostBLL();
        protected CircleTypeBLL circleTypeBLL = new CircleTypeBLL();
        protected UserBLL userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["circlePostID"] == null)
                {
                    Panel1.Visible = false;
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
                    CurrentMode_Init();
                }
                else
                {
                    ViewState["circlePostID"] = Request["circlePostID"];
                    using (Entity entity = new Entity())
                    {
                        entity.CirclePost.Find(Convert.ToInt32(Request["circlePostID"])).ComplaintUntreated = 0;
                        entity.SaveChanges();
                    }
                    BindData(Convert.ToInt32(Request["circlePostID"]));
                }


            }


        }

        protected void BindData(int circlePostID)
        {

            var list = circlePostBLL.GetCirclePost(circlePost => circlePost.CirclePostID == circlePostID);
            circlePost = list.First();

            circlePost.ImgList = XFXExt.imgList(circlePost.ImgList, ConfigurationManager.AppSettings["UploadUrl"]);

            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("CirclePostList.aspx");
            }
            DetailsView1.ChangeMode(e.NewMode);
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            using (Entity entity = new Entity())
            {
                var circlePost = entity.CirclePost.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                var tempshenhe = (circlePost.State & 2) == 0;
                var tempjiajing = (circlePost.State & 8) == 0;
                var tempshanchu = (circlePost.State & 32) == 0;
                circlePost.State = 0;
                var DropDownList_Province = ((DropDownList)DetailsView1.FindControl("DropDownList_Province"));
                circlePost.Province = DropDownList_Province.SelectedValue;
                foreach (ListItem item in CheckBoxList_State.Items)
                {
                    if (item.Selected)
                    {
                        circlePost.State = circlePost.State | Convert.ToInt32(item.Value);
                    }
                }
                if ((circlePost.State & 2) > 0 && (circlePost.State & 1) > 0)
                {
                    circlePost.State--;
                }
                if (((circlePost.State & 2) > 0) && tempshenhe)
                {
                    new ExperienceLevelBLL().CirclePostPass(circlePost.UserID);
                }
                if (((circlePost.State & 8) > 0) && tempjiajing)
                {
                    new ExperienceLevelBLL().CirclePostJiajing(circlePost.UserID);
                }

                if (tempshenhe)
                {
                    if ((circlePost.State & 2) > 0)
                    {


                        string text = string.Format("您的帖子 {0} 已经审核通过", circlePost.Title);
                        UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), circlePost.UserID, text, 1 | 16,circlePost.CirclePostID);

                    }

                }

                if (tempshanchu)
                {
                    if ((circlePost.State & 32) > 0)
                    {


                        string text = string.Format("您的帖子 {0} 已被管理员删除。{1}", circlePost.Title,
                            (e.NewValues["Mark"] == null ? "" : ("原因: " + e.NewValues["Mark"].ToString())));
                        UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), circlePost.UserID, text, 1 | 16);
                        if(e.NewValues["Mark"] != null)
                        {
                 
                            circlePost.Mark = e.NewValues["Mark"].ToString();
                        }
                    }

                }
                circlePost.UpdateTime = DateTime.Now;
                entity.SaveChanges();
            }
            Response.Redirect("CirclePostDetail.aspx?circlePostID=" + DetailsView1.DataKey.Value);
        }


        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var HiddenField1 = (HiddenField)DetailsView1.FindControl("HiddenField1");
                var HiddenField2 = (HiddenField)DetailsView1.FindControl("HiddenField2");
                var HiddenField3 = (HiddenField)DetailsView1.FindControl("HiddenField3");
                var HiddenField4 = (HiddenField)DetailsView1.FindControl("HiddenField4");
                var img_div = (HtmlGenericControl)DetailsView1.FindControl("img_div");
                img_div.InnerHtml = HiddenField2.Value;
                if (string.IsNullOrWhiteSpace(HiddenField1.Value))
                {
                    throw new Exception("内容不能为空");

                }
                var div_Detail = (HtmlGenericControl)DetailsView1.FindControl("div_Detail");
                div_Detail.InnerHtml = HiddenField1.Value;
                var circlePost = new CirclePost();
                if (e.Values["Title"] == null || e.Values["Title"].ToString().Length < 5)
                {
                    throw new Exception("标题不能少于五个字");

                }

                circlePost.Title = e.Values["Title"].ToString();
                circlePost.Detail = HiddenField1.Value;
                int templength = 20;
                if (HiddenField4.Value.Length < 20)
                {
                    templength = HiddenField4.Value.Length;
                }
                circlePost.DetailDigest = HiddenField4.Value.Substring(0, templength);
                circlePost.ImgList = HiddenField3.Value.Replace(ConfigurationManager.AppSettings["UploadUrl"], "");
                circlePost.CircleTypeID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_CircleTypeID")).SelectedValue);
                circlePost.UserID = Convert.ToInt32(((DropDownList)DetailsView1.FindControl("DropDownList_UserID")).SelectedValue);
                circlePost.CreateTime = DateTime.Now;
                circlePost.UpdateTime = DateTime.Now;
                circlePost.State = 1;
                using (Entity entity = new Entity())
                {
                    entity.CirclePost.Add(circlePost);
                    entity.SaveChanges();
                }
                Response.Redirect("CirclePostDetail.aspx?circlePostID=" + circlePost.CirclePostID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            var DropDownList_CircleTypeID = ((DropDownList)DetailsView1.FindControl("DropDownList_CircleTypeID"));
            if (DropDownList_CircleTypeID != null)
            {
                DropDownList_CircleTypeID.DataSource = circleTypeBLL.GetCircleType(circleType => true).Select(circleType =>
                new
                {
                    CircleTypeID = circleType.CircleTypeID,
                    Title = circleType.Title
                });
                DropDownList_CircleTypeID.DataBind();
            }
            var DropDownList_Province = ((DropDownList)DetailsView1.FindControl("DropDownList_Province"));
            if (circlePost != null && DropDownList_Province != null)
            {

                DropDownList_Province.SelectedValue = circlePost.Province;
            }


            var DropDownList_UserID = ((DropDownList)DetailsView1.FindControl("DropDownList_UserID"));
            if (DropDownList_UserID != null)
            {
                DropDownList_UserID.DataSource = userBLL.GetUsers(user => user.GetCounterfeit()).Select(user =>
                new
                {
                    UserID = user.UserID,
                    NickName = user.NickName
                });
                DropDownList_UserID.DataBind();
            }

            var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
            if (CheckBoxList_State != null && circlePost != null)
            {
                if ((circlePost.State & 1) > 0)
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if ((circlePost.State & 2) > 0)
                {
                    CheckBoxList_State.Items[1].Selected = true;
                }
                if ((circlePost.State & 4) > 0)
                {
                    CheckBoxList_State.Items[2].Selected = true;
                }
                if ((circlePost.State & 8) > 0)
                {
                    CheckBoxList_State.Items[3].Selected = true;
                }
                if ((circlePost.State & 16) > 0)
                {
                    CheckBoxList_State.Items[4].Selected = true;
                }
                if ((circlePost.State & 32) > 0)
                {
                    CheckBoxList_State.Items[5].Selected = true;
                }
                if ((circlePost.State & 64) > 0)
                {
                    CheckBoxList_State.Items[6].Selected = true;
                }
                if ((circlePost.State & 128) > 0)
                {
                    CheckBoxList_State.Items[7].Selected = true;
                }
            }
        }

    }
}