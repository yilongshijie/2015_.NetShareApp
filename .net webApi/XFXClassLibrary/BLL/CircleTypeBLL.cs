using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class CircleTypeBLL
    {
        private CircleDAL circleDAL = new CircleDAL();


        public List<CircleType> GetCircleType(Func<CircleType, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<CircleType, object> orderBy = null, string sort = "DESC")
        {
            return circleDAL.GetCircleType(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<CircleType> GetCircleType(Func<CircleType, bool> delegation, Func<CircleType, object> orderBy = null)
        {
            return circleDAL.GetCircleType(delegation);
        }
 
        public int AddCircleType(int UserID, int CircleTypeID,int state)
        {
            return circleDAL.AddCircleType(UserID, CircleTypeID, state);
        }
    }
}
