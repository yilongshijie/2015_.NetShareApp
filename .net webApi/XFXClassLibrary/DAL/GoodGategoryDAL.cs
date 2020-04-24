using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class GoodGategoryDAL
    {
        public List<GoodGategory> GetGoodGategory(Func<GoodGategory, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<GoodGategory, object> orderBy = null, string sort = "DESC")
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
                var items = entity.GoodGategory.Where(where);
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
        public List<GoodGategory> GetGoodGategory(Func<GoodGategory, bool> where, Func<GoodGategory, object> orderBy = null)
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
                var items = entity.GoodGategory.Include("GoodHome").Where(where);
                return items.ToList();
            }
        }
        public List<GoodGategory> GetGoodGategoryIndex(Func<GoodGategory, bool> where, Func<GoodGategory, object> orderBy = null)
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
                var items = entity.GoodGategory.Include("GoodHome").Where(where);
                return items.ToList();
            }
        } 
    }
}
