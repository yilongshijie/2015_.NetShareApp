using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    class GoodEvaluateDAL
    {

        public List<GoodEvaluate> GetGoodEvaluate(Func<GoodEvaluate, bool> where, Func<GoodEvaluate, object> orderBy = null, string sort = "DESC")
        {

            if (orderBy == null)
            {

                orderBy = o => o.Time;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.GoodEvaluate.Include("User").Include("Good").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy).ThenByDescending(o => o.Time).Skip(0).Take(300);
                }
                else
                {
                    items = items.OrderBy(orderBy).Skip(0).Take(300);
                }

                return items.ToList();
            }
        }



        public List<GoodEvaluate> GetGoodEvaluate(Func<GoodEvaluate, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<GoodEvaluate, object> orderBy = null, string sort = "DESC")
        {
            if (where == null)
            {
                where = o => true;
            }
            if (orderBy == null)
            {
                orderBy = o => o.Time;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.GoodEvaluate.Include("User").Include("Good").Where(where);
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
