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

    public partial class GoodExperienceDetail : System.Web.UI.Page
    {
        protected GoodExperience goodExperience;
        protected GoodExperienceBLL goodExperienceBLL = new GoodExperienceBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["goodExperienceID"] = Request["goodExperienceID"];
                BindData(Convert.ToInt32(Request["goodExperienceID"]));

            }
        }

        protected void BindData(int goodExperienceID)
        {

            var list = goodExperienceBLL.GetGoodExperience(o => o.GoodExperienceID == goodExperienceID);
            goodExperience = list.First();

            goodExperience.Images = XFXExt.imgList(goodExperience.Images, ConfigurationManager.AppSettings["UploadUrl"], false);

            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            using (Entity entity = new Entity())
            {
                var goodExperience = entity.GoodExperience.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));

                goodExperience.State = 0;
                foreach (ListItem item in CheckBoxList_State.Items)
                {
                    if (item.Selected)
                    {
                        goodExperience.State = goodExperience.State | Convert.ToInt32(item.Value);
                    }
                }
                if (e.NewValues["OrderBy"] != null)
                {
                    goodExperience.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"]);
                }
                if ((goodExperience.State & 2) > 0 && (goodExperience.State & 1) > 0)
                {
                    goodExperience.State--;
                }

                entity.SaveChanges();
            }
            Response.Redirect("GoodExperienceDetail.aspx?goodExperienceID=" + DetailsView1.DataKey.Value);
        }


        private void CurrentMode_Init()
        {

            var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
            if (CheckBoxList_State != null && goodExperience != null)
            {
                if ((goodExperience.State & 1) > 0)
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if ((goodExperience.State & 2) > 0)
                {
                    CheckBoxList_State.Items[1].Selected = true;
                }
                if ((goodExperience.State & 4) > 0)
                {
                    CheckBoxList_State.Items[2].Selected = true;
                }
                if ((goodExperience.State & 8) > 0)
                {
                    CheckBoxList_State.Items[3].Selected = true;
                }
            }
        }

    }
}