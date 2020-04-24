using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    class CirclePostDAL
    {
        public CirclePost GetCirclePost(int circlePostID)
        {
            using (Entity entity = new Entity())
            {
                return entity.CirclePost.Find(circlePostID);
            }
        }


        public List<CirclePost> GetCirclePost(Func<CirclePost, bool> where, Func<CirclePost, object> orderBy = null, string sort = "DESC")
        {
            if (orderBy == null)
            {
                orderBy = o => o.UpdateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.CirclePost.Include("User").Include("CircleType").Include("User.UserExtend").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy);
                }
                else
                {
                    items = items.OrderBy(orderBy);
                }
                return items.ToList();
            }
        }

        public List<CirclePost> GetCirclePostReply(int userID)
        {

            using (Entity entity = new Entity())
            {
                var linq = entity.CirclePostReply.Where(o => o.UserID == userID).Select(o => o.CirclePostID).Distinct();
                return entity.CirclePost.Include("User").Include("User.UserExtend").Include("CircleType").Where(o => linq.Contains(o.CirclePostID) &&  ((o.State & 16) == 0) && ((o.State & 32) == 0)).ToList();
            }
        }

        public List<CirclePost> GetCirclePost(Func<CirclePost, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<CirclePost, object> orderBy = null, string sort = "DESC", Func<CirclePost, object> thenOrderBy = null)
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.UpdateTime;
            }
            if (thenOrderBy == null)
            {
                thenOrderBy = o => o.UpdateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.CirclePost.Include("User").Include("User.UserExtend").Include("CircleType").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy).ThenByDescending(thenOrderBy);
     
                }
                else
                {
                    items = items.OrderBy(orderBy).ThenBy(thenOrderBy);
                }
 
                pageTotal = items.Count();
                return items.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize).ToList();
            }
        }
        public List<CirclePostReply> GetCirclePostReply(Func<CirclePostReply, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<CirclePostReply, object> orderBy = null, string sort = "DESC")
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.CreateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.CirclePostReply.Include("User").Include("User.UserExtend").Include("CirclePostReplyChild").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy);
                }
                else
                {
                    items = items.OrderBy(orderBy);
                }
                pageTotal = items.Count();
                return items.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize).ToList();
            }
        }

    }
}
