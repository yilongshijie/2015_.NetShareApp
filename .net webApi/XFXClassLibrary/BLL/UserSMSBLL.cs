using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class UserSMSBLL
    {
        private UserSMSDAL userSMSDAL = new UserSMSDAL();

        #region 查询
        public List<UserSMS> GetUsers(Func<UserSMS, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<UserSMS, object> orderBy = null, string sort = "DESC")
        {
            return userSMSDAL.GetUserSMS(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }

 
        #endregion

   
    }

     


}
