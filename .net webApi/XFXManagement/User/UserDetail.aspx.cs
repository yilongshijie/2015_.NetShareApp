using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class UserDetail : System.Web.UI.Page
    {
        protected User user;
        protected UserBLL userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["userID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
                    Panel1.Visible = false;
                }
                else
                {

                    using (Entity entity = new Entity())
                    {
                        entity.User.Find(Convert.ToInt32(Request["userID"])).UserExtend.ComplaintUntreated = 0;
                        entity.SaveChanges();
                    }
                    ViewState["userID"] = Request["userID"];
                    BindData(Convert.ToInt32(Request["userID"]));

                }


            }
        }

        protected void BindData(int userID)
        {

            var list = userBLL.GetUsers(user => user.UserID == userID);
            user = list.First();
            user.HeadPortrait = ConfigurationManager.AppSettings["UploadUrl"] + user.HeadPortrait;
            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("UserList.aspx");
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
                    var user = entity.User.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var CheckBoxList_Type = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_Type"));
                    user.Type = 0;
                    foreach (ListItem item in CheckBoxList_Type.Items)
                    {
                        if (item.Selected)
                        {
                            user.Type = user.Type | Convert.ToInt32(item.Value);
                        }
                    }
                    var RadioButtonList_Statet = ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State"));
                    user.State = Convert.ToInt32(RadioButtonList_Statet.SelectedValue);

                    var RadioButtonList_Banned = ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Banned"));
                    int bannedTemp = user.UserExtend.Banned;
                    user.UserExtend.Banned = Convert.ToInt32(RadioButtonList_Banned.SelectedValue);
                    if (user.UserExtend.Banned == 1 && bannedTemp != 1)
                    {

                        new ExperienceLevelBLL().Jinyan(user.UserID);
                        user.UserExtend.BannedStartTime = DateTime.Now;
                        user.UserExtend.BannedEndTime = user.UserExtend.BannedStartTime.Value.AddDays(3);
                        UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), Convert.ToInt32(ViewState["userID"]), "您的发言被用户举报，已经被禁言三天。", 1 | 16);
                    }
                    else if (user.UserExtend.Banned == 0)
                    {
                        user.UserExtend.BannedEndTime = user.UserExtend.BannedStartTime = null;
                    }

                    if (e.NewValues["UserExtend.ExperienceLevel"] != null)
                    {

                        int level = Convert.ToInt32(e.NewValues["UserExtend.ExperienceLevel"]);
                        if (level == 0)
                        {
                            level = 1;
                        }
                        if (user.UserExtend.ExperienceLevel != level)
                        {

                            ExperienceLevel experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceLevelValue == level).FirstOrDefault();
                            if (experienceLevel == null)
                            {
                                throw new Exception("经验等级和等级不对应");
                            }
                            user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                            user.UserExtend.ExperienceValue = experienceLevel.ExperienceValueMin;
                            if (user.Gender == "男")
                            {
                                user.UserExtend.ExperienceName = experienceLevel.NameMan;
                            }
                            else
                            {
                                user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                            }


                        }

                    }
                    entity.SaveChanges();
                }
                Response.Redirect("UserDetail.aspx?userID=" + DetailsView1.DataKey.Value);
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
                var user = new User();
                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                    user.HeadPortrait = file_url.Value;
                }

                if (e.Values["Tel"] == null)
                {
                    throw new Exception("用户电话不能为空");
                }

                if (e.Values["PassWord"] == null)
                {
                    throw new Exception("密码不能为空");
                }
                user.Tel = (e.Values["Tel"]).ToString().Trim();

                user.PassWord = CommonSecurity.SHA1MD5MD5((e.Values["PassWord"]).ToString().Trim());
                string pattern = @"^(0|86|17951)?(1[234578])[0-9]{9}$";
                Regex rgx = new Regex(pattern);
                if (!rgx.IsMatch(user.Tel))
                {
                    throw new Exception("电话号不正确");
                }
                if (user.PassWord.Length < 6)
                {
                    throw new Exception("密码长度不能小于6");
                }
                if (e.Values["NickName"] == null)
                {
                    user.NickName = "分享玩家";
                }
                else
                {
                    user.NickName = (e.Values["NickName"]).ToString().Trim();
                }

                user.Gender = ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Gender")).SelectedValue;
                user.Married = ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Married")).SelectedValue;

                var CheckBoxList_Type = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_Type"));
                if (CheckBoxList_Type.Items[0].Selected)
                {
                    user.Type = user.Type | 1;
                }
                if (CheckBoxList_Type.Items[1].Selected)
                {
                    user.Type = user.Type | 2;
                }
                if (CheckBoxList_Type.Items[2].Selected)
                {
                    user.Type = user.Type | 4;
                }
                if (CheckBoxList_Type.Items[3].Selected)
                {
                    user.Type = user.Type | 8;
                }
                user.State = Convert.ToInt32(((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue);
                user.CreatTime = DateTime.Now;
                user.UpdateTime = DateTime.Now;
                user.UserExtend = new UserExtend();
                if (e.Values["UserExtend.ExperienceLevel"] != null)
                {
                    int temp;
                    if (!int.TryParse(e.Values["UserExtend.ExperienceLevel"].ToString(), out temp))
                    {
                        throw new Exception("经验等级必须是整数");
                    }
                    if (temp == 0)
                    {
                        temp = 1;
                    }
                    user.UserExtend.ExperienceLevel = temp;
                }

                using (Entity entity = new Entity())
                {
                    ExperienceLevel experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceLevelValue == user.UserExtend.ExperienceLevel).FirstOrDefault();
                    if (experienceLevel == null)
                    {
                        throw new Exception("经验等级和等级不对应");
                    }
                    user.UserExtend.ExperienceValue = experienceLevel.ExperienceValueMin;
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }

                    user.InitBeforeSave();
                    user.SetCounterfeit();
                    entity.User.Add(user);
                    entity.SaveChanges();
                }
                Response.Redirect("UserDetail.aspx?userID=" + user.UserID);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }

        private void CurrentMode_Init()
        {
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Gender")).SelectedValue = user.Gender;

            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Married")).SelectedValue = user.Married;
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_SexualOrientation")).SelectedValue = user.SexualOrientation;

            var CheckBoxList_Type = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_Type"));
            if ((user.Type & 1) > 0)
            {
                CheckBoxList_Type.Items[0].Selected = true;
            }
            if ((user.Type & 2) > 0)
            {
                CheckBoxList_Type.Items[1].Selected = true;
            }
            if ((user.Type & 4) > 0)
            {
                CheckBoxList_Type.Items[2].Selected = true;
            }
            if ((user.Type & 8) > 0)
            {
                CheckBoxList_Type.Items[3].Selected = true;
            }
            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_State")).SelectedValue = user.State.ToString();

            ((RadioButtonList)DetailsView1.FindControl("RadioButtonList_Banned")).SelectedValue = user.UserExtend.Banned.ToString();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBox1.Text))
            {
                return;
            }
            UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), Convert.ToInt32(ViewState["userID"]), TextBox1.Text, 1 | 16);
            BindData(Convert.ToInt32(ViewState["userID"]));
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", "私信发送成功"));
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                if (user.Gender == "男")
                {
                    user.HeadPortrait = "init/man.jpg";
                }
                else
                {
                    user.HeadPortrait = "init/woman.jpg";
                }
                entity.SaveChanges();
            } 
            BindData(Convert.ToInt32(ViewState["userID"]));
            UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), Convert.ToInt32(ViewState["userID"]), "您上传的头像违规，已经被管理员设置为默认头像。", 1 | 16);

            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", "修改头像成功"));
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                user.NickName = "分享玩家";
                entity.SaveChanges();
            }
            BindData(Convert.ToInt32(ViewState["userID"]));
            UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), Convert.ToInt32(ViewState["userID"]), "您的昵称违规，已经被管理员设置为默认昵称。", 1 | 16);
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", "修改昵称成功"));
        }
    }
}