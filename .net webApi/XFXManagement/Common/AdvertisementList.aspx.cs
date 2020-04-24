using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class AdvertisementList : System.Web.UI.Page
    {
        protected AdvertisementBLL advertisementBLL = new AdvertisementBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdvertisementDetail.aspx");
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            BindData();
        }

        protected void BindData(Func<Advertisement, object> orderBy = null, string sort = "DESC")
        {

            Func<Advertisement, bool> delegation = o => true;
            Filter(ref orderBy, ref sort, ref delegation);
            int pageTotal;
            var list = advertisementBLL.GetAdvertisement(delegation, AspNetPager1.PageSize, AspNetPager1.CurrentPageIndex, out pageTotal, orderBy, sort);

            GridView1.DataSource = list;
            GridView1.DataBind();
            AspNetPager1.RecordCount = pageTotal;
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Response.Redirect("AdvertisementDetail.aspx?AdvertisementID=" + GridView1.SelectedDataKey.Value);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            BindData();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                if (sortData[1] == "ASC")
                {
                    ViewState["sortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            else
            {
                ViewState["sortExpression"] = e.SortExpression + " " + "ASC";
            }
            BindData();

        }

        private void Filter(ref Func<Advertisement, object> orderBy, ref string sort, ref Func<Advertisement, bool> delegation)
        {
            if (ViewState["sortExpression"] != null)
            {
                string[] sortData = ViewState["sortExpression"].ToString().Split(' ');
                sort = sortData[1];
                switch (sortData[0])
                {
                    case "AdvertisementID": orderBy = o => o.AdvertisementID; break;
                    case "Title": orderBy = o => o.Title; break;
                    case "OrderBy": orderBy = o => o.OrderBy; break;
                    case "State": orderBy = o => o.State; break;
                    case "Type": orderBy = o => o.Type; break;
                    case "Link": orderBy = o => o.Link; break;
                    case "Time": orderBy = o => o.Time; break;
                }
            }
            bool typeBool = true, stateBool = true;
            int type = 0;
            int state = 0;

            if (DropDownListType.SelectedValue != "-1")
            {
                typeBool = false;
                type = Convert.ToInt32(DropDownListType.SelectedValue);
            }

            if (DropDownListState.SelectedValue != "-1")
            {
                stateBool = false;
                state = Convert.ToInt32(DropDownListState.SelectedValue);
            }
            delegation = o =>
                 (stateBool || o.State == state) &&
                 (typeBool || o.Type == type);
        }

    }
}