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

    public partial class CirclePostReplyDetail : System.Web.UI.Page
    {
        protected CirclePostReply circlePostReply;

        protected UserBLL userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["circlePostID"] = Request["circlePostID"];
                DetailsView1.ChangeMode(DetailsViewMode.Insert);
                CurrentMode_Init();
            }
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var DropDownList_UserID = ((DropDownList)DetailsView1.FindControl("DropDownList_UserID"));
                using (Entity entity = new Entity())
                {
                    CirclePost circlePost = entity.CirclePost.Find(Convert.ToInt32(ViewState["circlePostID"]));
                    circlePost.ReplyNum++;
                    circlePost.UpdateTime = DateTime.Now;
                    CirclePostReply circlePostReply = new CirclePostReply()
                    {
                        UserID = Convert.ToInt32(DropDownList_UserID.SelectedValue),
                        CirclePostID = Convert.ToInt32(ViewState["circlePostID"]),

                        State = 0,
                        CreateTime = DateTime.Now,
                        Floor = circlePost.ReplyNum
                    };
                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    circlePostReply.ImgList = "";
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        circlePostReply.ImgList = file_url.Value;
                    }
                    var HiddenField1 = (HiddenField)DetailsView1.FindControl("HiddenField1");
                    circlePostReply.Detail = HiddenField1.Value;
                    if (string.IsNullOrWhiteSpace(circlePostReply.ImgList) && string.IsNullOrWhiteSpace(circlePostReply.Detail))
                    {
                        throw new Exception("回复不能为空");
                    }
                    entity.CirclePostReply.Add(circlePostReply);
                    entity.SaveChanges();
                }
                Response.Redirect("CirclePostReplyList.aspx?circlePostID=" + ViewState["circlePostID"]);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            var DropDownList_UserID = ((DropDownList)DetailsView1.FindControl("DropDownList_UserID"));

            DropDownList_UserID.DataSource = userBLL.GetUsers(user => user.GetCounterfeit()).Select(user =>
            new
            {
                UserID = user.UserID,
                NickName = user.NickName
            });
            DropDownList_UserID.DataBind();

        }

    }
}