using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;
namespace XFXManagement
{
    public partial class CirclePostReplyList : System.Web.UI.Page
    {
        protected CirclePostBLL circlePostBLL = new CirclePostBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["all"] != null)
                {
                    Add.Visible = false;
                    ViewState["circlePostID"] = 0;
                    BindData(0);
                }
                if (Request["circlePostID"] != null)
                {
                    filter.Visible = false;
                    ViewState["circlePostID"] = Request["circlePostID"];
                    BindData(Convert.ToInt32(Request["circlePostID"]));
                }

            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "StateDelete")
            {
                using (Entity entity = new Entity())
                {
                    int circlePostReplyID = Convert.ToInt32(e.CommandArgument);
                    var circlePostReply = entity.CirclePostReply.Find(circlePostReplyID);
                    circlePostReply.State = 2;
                    circlePostReply.CirclePost.ReplyNum = circlePostReply.CirclePost.ReplyNum - 1;
                    entity.SaveChanges();
                    string text = string.Format("您在\"{0}\"的一条回复已被管理员删除", circlePostReply.CirclePost.Title);
                    UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), circlePostReply.UserID, text, 1 | 16, circlePostReply.CirclePostID);
                }
                BindData(Convert.ToInt32(ViewState["circlePostID"]));
            }
            else if (e.CommandName == "StateZhiding")
            {
                using (Entity entity = new Entity())
                {
                    int circlePostReplyID = Convert.ToInt32(e.CommandArgument);
                    var circlePostReply = entity.CirclePostReply.Find(circlePostReplyID);
                    circlePostReply.State = circlePostReply.State | 4;

                    entity.SaveChanges();

                }
                BindData(Convert.ToInt32(ViewState["circlePostID"]));
            }
            else if (e.CommandName == "StateQuxiao")
            {
                using (Entity entity = new Entity())
                {
                    int circlePostReplyID = Convert.ToInt32(e.CommandArgument);
                    var circlePostReply = entity.CirclePostReply.Find(circlePostReplyID);
                    circlePostReply.State = circlePostReply.State - (circlePostReply.State & 4);

                    entity.SaveChanges();

                }
                BindData(Convert.ToInt32(ViewState["circlePostID"]));
            }
            else if (e.CommandName == "ChildDelete")
            {
                using (Entity entity = new Entity())
                {
                    int circlePostReplyChildID = Convert.ToInt32(e.CommandArgument);
                    var circlePostReplyChild = entity.CirclePostReplyChild.Find(circlePostReplyChildID);
                    string title = circlePostReplyChild.CirclePost.Title;
                    entity.CirclePostReplyChild.Remove(circlePostReplyChild);
                    entity.SaveChanges();
                    string text = string.Format("您在\"{0}\"的一条回复已被管理员删除", title);
                    UserLetterBLL.Create(Convert.ToInt32(Session["userID"]), circlePostReplyChild.InitiativeUserID, text, 1 | 16, circlePostReplyChild.CirclePostID);

                }

                BindData(Convert.ToInt32(ViewState["circlePostID"]));
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindData(Convert.ToInt32(ViewState["circlePostID"]));
        }


        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("CirclePostReplyDetail.aspx?circlePostID=" + ViewState["circlePostID"]);
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData(Convert.ToInt32(ViewState["circlePostID"]));
        }
        protected void BindData(int circlePostID)
        {

            int pageTotal;
            int circlePostIDTemp = 0, userID = 0;
            bool userIDBool = true, circlePostIDBool = true;
            if (!string.IsNullOrWhiteSpace(UserID.Value))
            {
                userIDBool = !int.TryParse(UserID.Value, out userID);
            }
            if (!string.IsNullOrWhiteSpace(CirclePostID.Value))
            {
                circlePostIDBool = !int.TryParse(CirclePostID.Value, out circlePostIDTemp);
            }
            Func<CirclePostReply, object> orderby = null;
            if (CheckBox1.Checked)
            {
                orderby = new Func<CirclePostReply, object>(delegate (CirclePostReply c) { return c.CirclePostReplyChild.Count() > 0 ? c.CirclePostReplyChild.Max(o => o.CreateTime) : DateTime.MinValue; });
            }
            var list = circlePostBLL.GetCirclePostReply(o =>
                           (userIDBool || o.UserID == userID || (o.CirclePostReplyChild.Where(ooo => ooo.User.UserID == userID)).Count() > 0) &&
   (circlePostIDBool || o.CirclePostID == circlePostIDTemp) &&
         (o.CirclePostID == circlePostID || circlePostID == 0)
         , AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderby);
            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            GridView GridViewChild1 = (GridView)e.Row.FindControl("GridViewChild1");
            if (GridViewChild1 == null)
            {
                return;
            }
            using (Entity entity = new Entity())
            {
                int circlePostReplyID = Convert.ToInt32(e.Row.Cells[2].Text);
                GridViewChild1.DataSource = entity.CirclePostReplyChild.Where(o => o.CirclePostReplyID == circlePostReplyID).ToList();
                GridViewChild1.DataBind();

            }
        }

    }
}