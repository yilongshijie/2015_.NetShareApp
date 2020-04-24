using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class GoodDAL
    {
        public List<Good> GetGood(Func<Good, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<Good, object> orderBy = null, string sort = "DESC")
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.UpdateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.Good.Include("GoodGategory").Where(where);
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
        public List<Good> GetGood(Func<Good, bool> where, Func<Good, object> orderBy = null, string sort = "DESC")
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.UpdateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.Good.Include("GoodGategory").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy).ThenByDescending(O => O.OrderBy).ThenByDescending(O => O.CreateTime).Skip(0).Take(300);
                }
                else
                {
                    items = items.OrderBy(orderBy).ThenByDescending(O => O.OrderBy).ThenByDescending(O => O.CreateTime).Skip(0).Take(300);
                }
                return items.ToList();
            }
        }
        public List<Good> GetGood(int goodID)
        {

            using (Entity entity = new Entity())
            {
                return entity.Good.Include("GoodChild").Where(o => o.GoodID == goodID && (o.State & 2) > 0).ToList();
            }
        }
        public List<GoodChild> GetGoodChild(Func<GoodChild, bool> where, Func<GoodChild, object> orderBy = null)
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
                var items = entity.GoodChild.Where(where);
                return items.ToList();
            }
        } 
    }
}
