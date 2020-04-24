using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    class CircleDAL
    {

        public List<CircleType> GetCircleType(Func<CircleType, bool> where)
        {

            using (Entity entity = new Entity())
            {
                return entity.CircleType.Where(where).OrderByDescending(o => o.OrderBy).ToList();
            }
        }
        public List<CircleType> GetCircleType(Func<CircleType, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<CircleType, object> orderBy = null, string sort = "DESC")
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.OrderBy;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.CircleType.Where(where);
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

        public int AddCircleType(int UserID, int CircleTypeID, int state)
        {

            using (Entity entity = new Entity())
            {
                CircleManage circleManage = entity.CircleManage.Where(o => o.UserID == UserID && o.CircleTypeID == CircleTypeID).FirstOrDefault();

                if (circleManage == null)
                {
                    entity.CircleManage.Add(new CircleManage() { UserID = UserID, CircleTypeID = CircleTypeID, State = state, CreateTime = DateTime.Now, UpdateTime = DateTime.Now });
                }
                else
                {
                    if (state == 0)
                    {
                        circleManage.State -= 1;
                    }
                    else
                    {
                        circleManage.State |= state;
                    }

                }
                return entity.SaveChanges();
            }
        }

    }
}
