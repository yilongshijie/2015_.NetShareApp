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
    public partial class UserAddressDetail : System.Web.UI.Page
    {
        protected UserAddress userAddress = new UserAddress();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData(Convert.ToInt32(Request["userID"]));
            }
        }

        protected void BindData(int userID)
        {

            using (Entity entity = new Entity())
            {
                var list = entity.UserAddress.Where(o => o.UserID == userID).ToList();
                userAddress = list.FirstOrDefault();
                if (userAddress != null)
                {
                    DetailsView1.DataSource = list;
                    DetailsView1.DataBind();
                }
            }
        }

    }
}