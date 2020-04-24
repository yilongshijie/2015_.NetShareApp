using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public  class AdvertisementBLL
    {
        private AdvertisementDAL circleDAL = new AdvertisementDAL();


        public List<Advertisement> GetAdvertisement(Func<Advertisement, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<Advertisement, object> orderBy = null, string sort = "DESC")
        {
            return circleDAL.GetCircleType(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<Advertisement> GetAdvertisement(Func<Advertisement, bool> delegation ,Func<Advertisement, object> orderBy = null)
        {
            return circleDAL.GetCircleType(delegation, orderBy);
        }

    }
}
