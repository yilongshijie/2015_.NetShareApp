using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XFXClassLibrary;

namespace XFXManagement
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Clear();
                if (Request["login"] != null)
                {
                    string error = "登陆失败".Replace("'", "\\'").Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", string.Format("<script>alert('{0}')</script>", error));
                }


            }
        }



        protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
        {


            string passwordTemp = CommonSecurity.SHA1MD5MD5(Login1.Password);
            using (Entity entity = new Entity())
            {
                User user = entity.User
                    .Where(o => o.Tel == Login1.UserName && o.PassWord == passwordTemp && (o.Type & 8) > 0 && o.State == 1).FirstOrDefault();

                if (user == null)
                {
                    Server.Transfer("default.aspx?login=error");


                }
                else
                {
                    Session["userID"] = user.UserID;
                    Server.Transfer("main.aspx");
                }
            }


        }

    }
}