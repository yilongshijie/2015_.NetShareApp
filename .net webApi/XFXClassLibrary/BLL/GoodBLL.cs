using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class GoodBLL
    {
        private GoodDAL goodDAL = new GoodDAL();

        public List<Good> GetGood(Func<Good, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<Good, object> orderBy = null, string sort = "DESC")
        {
            return goodDAL.GetGood(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<Good> GetGood(Func<Good, bool> delegation, Func<Good, object> orderBy = null, string sort = "DESC")
        {
            return goodDAL.GetGood(delegation, orderBy, sort);
        }

        public List<GoodChild> GetGoodChild(int goodID)
        {
            Func<GoodChild, bool> delegation = goodChild => goodChild.GoodId == goodID;
            return goodDAL.GetGoodChild(delegation);

        }
        public List<Good> GetGood(int goodID)
        {

            return goodDAL.GetGood(goodID);

        }
        public List<GoodChild> GetGoodChild(Func<GoodChild, bool> delegation)
        {

            return goodDAL.GetGoodChild(delegation);

        }

        public string StateText(int state)
        {
            List<string> stateText = new List<string>();
            if ((state & 1) > 0)
            {
                stateText.Add("下架");
            }
            if ((state & 2) > 0)
            {
                stateText.Add("上架");
            }
            if ((state & 4) > 0)
            {
                stateText.Add("限时特价");
            }
            if ((state & 8) > 0)
            {
                stateText.Add("周一新品");
            }
            if ((state & 16) > 0)
            {
                stateText.Add("HOT热卖");
            }
            if ((state & 32) > 0)
            {
                stateText.Add("包邮专区");
            }
            if ((state & 64) > 0)
            {
                stateText.Add("女神必备");
            }

            return string.Join("、", stateText);
        }
    }
}
