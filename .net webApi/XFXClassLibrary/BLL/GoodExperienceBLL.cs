using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    public class GoodExperienceBLL
    {
        private GoodExperienceDAL goodExperienceDAL = new GoodExperienceDAL();

        public List<GoodExperience> GetGoodExperience(Func<GoodExperience, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<GoodExperience, object> orderBy = null, string sort = "DESC")
        {
            return goodExperienceDAL.GetGoodExperience(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<GoodExperience> GetGoodExperience(Func<GoodExperience, bool> delegation, Func<GoodExperience, object> orderBy = null)
        {
            return goodExperienceDAL.GetGoodExperience(delegation, orderBy);
        }

        public List<GoodExperienceReply> GetGoodExperienceReply(Func<GoodExperienceReply, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<GoodExperienceReply, object> orderBy = null, string sort = "DESC")
        {
            return goodExperienceDAL.GetGoodExperienceReply(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        #region 转换
        public string StateText(int state)
        {
            List<string> typeText = new List<string>();

            if ((state & 1) > 0)
            {
                typeText.Add("未审核");
            }
            if ((state & 2) > 0)
            {
                typeText.Add("审核通过");
            }

            if ((state & 4) > 0)
            {
                typeText.Add("用户删除");
            }
            if ((state & 8) > 0)
            {
                typeText.Add("管理员删除");
            }

            return string.Join("、", typeText);
        }

        public string ReplyStateText(int state)
        {
            List<string> typeText = new List<string>();
            if ( state == 1 )
            {
               return ("用户删除");
            }
            if  (state ==2)
            {
                return ("管理员删除");
            }
            return ("正常");
        }
        #endregion
    }
}
