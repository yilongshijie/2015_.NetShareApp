using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    public class CirclePostBLL
    {
        private CirclePostDAL circlePostDAL = new CirclePostDAL();
        public CirclePost GetCirclePost(int circlePostID)
        {
            return circlePostDAL.GetCirclePost(circlePostID);
        }

        public List<CirclePost> GetCirclePost(Func<CirclePost, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<CirclePost, object> orderBy = null, string sort = "DESC", Func<CirclePost, object> thenOrderBy = null)
        {
            return circlePostDAL.GetCirclePost(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort,   thenOrderBy );
        }
        public List<CirclePost> GetCirclePost(Func<CirclePost, bool> delegation, Func<CirclePost, object> orderBy = null, string sort = "DESC")
        {
            return circlePostDAL.GetCirclePost(delegation, orderBy, sort);
        }

        public List<CirclePost> GetCirclePostReply(int userID)
        {
            return circlePostDAL.GetCirclePostReply(userID);
        }


        public List<CirclePostReply> GetCirclePostReply(Func<CirclePostReply, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<CirclePostReply, object> orderBy = null, string sort = "DESC")
        {
            return circlePostDAL.GetCirclePostReply(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
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
                typeText.Add("置顶");
            }
            if ((state & 8) > 0)
            {
                typeText.Add("加精");
            }
            if ((state & 16) > 0)
            {
                typeText.Add("用户删除");
            }
            if ((state & 32) > 0)
            {
                typeText.Add("管理员删除");
            }
            if ((state & 64) > 0)
            {
                typeText.Add("匿名");
            }
            if ((state & 128) > 0)
            {
                typeText.Add("位置");
            }
            return string.Join("、", typeText);
        }

        public string ReplyStateText(int state)
        {
            List<string> typeText = new List<string>();
            if ((state & 1) > 0)
            {
                typeText.Add("用户删除");
            }
            if ((state & 2) > 0)
            {
                typeText.Add("管理员删除");
            }
            if ((state & 4) > 0)
            {
                typeText.Add("置顶");
            }
            return string.Join("、", typeText);
        }
        #endregion
    }
}
