using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public  class UserGradeLogBLL
    {
        private UserGradeLogDAL userGradeLogDAL = new UserGradeLogDAL();


        public List<UserGradeLog> GetAdvertisement(Func<UserGradeLog, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<UserGradeLog, object> orderBy = null, string sort = "DESC")
        {
            return userGradeLogDAL.GetUserGradeLog(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
 

    }
}
