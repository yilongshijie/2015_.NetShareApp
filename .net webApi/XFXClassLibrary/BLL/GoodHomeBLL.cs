using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class GoodHomeBLL
    {
        private GoodHomeDAL goodHomeDAL = new GoodHomeDAL();
 
        public List<GoodHome> GetGoodHome(Func<GoodHome, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<GoodHome, object> orderBy = null, string sort = "DESC")
        {
            return goodHomeDAL.GetGoodHome(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<GoodHome> GetGoodHome(Func<GoodHome, bool> delegation, Func<GoodHome, object> orderBy = null)
        {
            return goodHomeDAL.GetGoodHome(delegation);
        }


    }
}
