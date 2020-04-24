using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    public class GoodEvaluateBLL
    {
        private GoodEvaluateDAL goodEvaluateDAL = new GoodEvaluateDAL();

        public List<GoodEvaluate> GetGoodEvaluate(Func<GoodEvaluate, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<GoodEvaluate, object> orderBy = null, string sort = "DESC")
        {
            return goodEvaluateDAL.GetGoodEvaluate(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<GoodEvaluate> GetGoodEvaluate(Func<GoodEvaluate, bool> delegation, Func<GoodEvaluate, object> orderBy = null)
        {
            return goodEvaluateDAL.GetGoodEvaluate(delegation, orderBy);
        }



    }
}
