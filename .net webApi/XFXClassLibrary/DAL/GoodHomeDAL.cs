using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class GoodHomeDAL
    {
        public List<GoodHome> GetGoodHome(Func<GoodHome, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<GoodHome, object> orderBy = null, string sort = "DESC")
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
                var items = entity.GoodHome.Include("GoodGategory").Include("Good").Where(where);
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
        public List<GoodHome> GetGoodHome(Func<GoodHome, bool> where, Func<GoodHome, object> orderBy = null)
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
                var items = entity.GoodHome.Include("GoodGategory").Include("Good").Where(where);
                return items.ToList();
            }
        } 
    }
}
