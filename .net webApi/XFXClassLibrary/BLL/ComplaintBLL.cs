using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class ComplaintBLL
    {
        private ComplaintDAL complaintDAL = new ComplaintDAL();

        #region 查询
        public List<Complaint> GetComplaint(Func<Complaint, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<Complaint, object> orderBy = null, string sort = "DESC")
        {
            return complaintDAL.GetComplaint(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }

 
        #endregion

   
    }

     


}
