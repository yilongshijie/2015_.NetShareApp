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

    public partial class GoodChildDetail : System.Web.UI.Page
    {
        protected GoodChild goodChild;
        protected GoodBLL goodBLL = new GoodBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["goodID"] = Request["goodID"];
                if (Request["goodChildID"] == null)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);

                }
                else
                {
                    BindData(Convert.ToInt32(Request["goodChildID"]));
                }


            }
        }

        protected void BindData(int goodChildID)
        {
            Func<GoodChild, bool> delegation = goodChild => goodChild.GoodChildID == goodChildID;
            var list = goodBLL.GetGoodChild(delegation);
            goodChild = list.First();
            goodChild.Image = ConfigurationManager.AppSettings["UploadUrl"] + goodChild.Image;

            DetailsView1.DataSource = list;
            DetailsView1.DataBind();
            CurrentMode_Init();
        }

        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {

            DetailsView1.ChangeMode(e.NewMode);
            if (DetailsView1.DataKey.Value == null)
            {
                Response.Redirect("GoodChildDetail.aspx?goodID=" + ViewState["goodID"]);
            }
            BindData(Convert.ToInt32(DetailsView1.DataKey.Value));

        }
        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("GoodChildList.aspx?goodID=" + ViewState["goodID"]);
        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                using (Entity entity = new Entity())
                {
                    var goodChild = entity.GoodChild.Find(Convert.ToInt32(DetailsView1.DataKey.Value));
                    var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                    if (!string.IsNullOrEmpty(file_url.Value))
                    {
                        var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                        img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                        goodChild.Image = file_url.Value;
                    }


                    if (e.NewValues["Specification"] == null)
                    {
                        throw new Exception("规格不能为空");
                    }
                    if (e.NewValues["SalesVolume"] != null)
                    {
                        goodChild.SalesVolume = Convert.ToInt32(e.NewValues["SalesVolume"].ToString());
                    }

                    if (e.NewValues["OrderBy"] != null)
                    {
                        goodChild.OrderBy = Convert.ToInt32(e.NewValues["OrderBy"].ToString());
                    }
                    goodChild.Repertory = Convert.ToInt32(e.NewValues["Repertory"].ToString());
                    goodChild.Specification = e.NewValues["Specification"].ToString();
                    goodChild.AddPrice = Convert.ToDecimal(e.NewValues["AddPrice"].ToString());
                    var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                    goodChild.State = 0;
                    foreach (ListItem item in CheckBoxList_State.Items)
                    {
                        if (item.Selected)
                        {
                            goodChild.State = goodChild.State | Convert.ToInt32(item.Value);
                        }
                    }


                    goodChild.UpdateTime = DateTime.Now;

                    entity.SaveChanges();
                    Response.Redirect("GoodChildDetail.aspx?goodChildID=" + goodChild.GoodChildID + "&goodID=" + goodChild.GoodId);
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
                var goodChild = new GoodChild();
                var file_url = (HtmlInputHidden)DetailsView1.FindControl("file_url");
                if (!string.IsNullOrEmpty(file_url.Value))
                {
                    var img_url = (HtmlImage)DetailsView1.FindControl("img_url");
                    img_url.Src = ConfigurationManager.AppSettings["UploadUrl"] + file_url.Value;
                    goodChild.Image = file_url.Value;
                }

                if (string.IsNullOrEmpty(file_url.Value))
                {
                    throw new Exception("图片不能为空");
                }

                if (e.Values["Specification"] == null)
                {
                    throw new Exception("规格不能为空");
                }

                if (e.Values["SalesVolume"] != null)
                {
                    goodChild.SalesVolume = Convert.ToInt32(e.Values["SalesVolume"].ToString());
                }
                if (e.Values["OrderBy"] != null)
                {
                    goodChild.OrderBy = Convert.ToInt32(e.Values["OrderBy"].ToString());
                }
                goodChild.Repertory = Convert.ToInt32(e.Values["Repertory"].ToString());
                goodChild.Specification = e.Values["Specification"].ToString();
                goodChild.AddPrice = Convert.ToDecimal(e.Values["AddPrice"].ToString());
                var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
                goodChild.State = 0;
                foreach (ListItem item in CheckBoxList_State.Items)
                {
                    if (item.Selected)
                    {
                        goodChild.State = goodChild.State | Convert.ToInt32(item.Value);
                    }
                }


                goodChild.CreateTime = DateTime.Now;
                goodChild.UpdateTime = DateTime.Now;
                using (Entity entity = new Entity())
                {
                    entity.GoodChild.Add(goodChild);
                    goodChild.GoodId = Convert.ToInt32(ViewState["goodID"]);
                    var good = entity.Good.Find(goodChild.GoodId);
                    goodChild.GoodGategoryID = good.GoodGategoryID;
                    entity.SaveChanges();
                }
                Response.Redirect("GoodChildDetail.aspx?goodChildID=" + goodChild.GoodChildID + "&goodID=" + goodChild.GoodId);
            }
            catch (Exception exception)
            {
                string error = exception.GetErrorMessage().Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
            }
        }


        private void CurrentMode_Init()
        {
            var CheckBoxList_State = ((CheckBoxList)DetailsView1.FindControl("CheckBoxList_State"));
            if (CheckBoxList_State != null && goodChild != null)
            {
                if ((goodChild.State % 2) == 0)
                {
                    CheckBoxList_State.Items[0].Selected = true;
                }
                if ((goodChild.State & 1) > 0)
                {
                    CheckBoxList_State.Items[1].Selected = true;
                }
            }
        }


    }
}