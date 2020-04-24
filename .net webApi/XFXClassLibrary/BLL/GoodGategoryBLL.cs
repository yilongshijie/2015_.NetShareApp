using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class GoodGategoryBLL
    {
        private GoodGategoryDAL goodGategoryDAL = new GoodGategoryDAL();
 
        public List<GoodGategory> GetGoodGategory(Func<GoodGategory, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<GoodGategory, object> orderBy = null, string sort = "DESC")
        {
            return goodGategoryDAL.GetGoodGategory(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<GoodGategory> GetGoodGategory(Func<GoodGategory, bool> delegation, Func<GoodGategory, object> orderBy = null)
        {
            return goodGategoryDAL.GetGoodGategory(delegation);
        }
        public List<GoodGategory> GetGoodGategoryIndex(Func<GoodGategory, bool> delegation, Func<GoodGategory, object> orderBy = null)
        {
            return goodGategoryDAL.GetGoodGategoryIndex(delegation);
        }

    }
}
