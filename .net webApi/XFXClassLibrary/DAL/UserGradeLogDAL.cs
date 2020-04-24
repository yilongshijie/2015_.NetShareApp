using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class UserGradeLogDAL
    {
        public List<UserGradeLog> GetUserGradeLog(Func<UserGradeLog, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<UserGradeLog, object> orderBy = null, string sort = "DESC")
        {
            using (Entity entity = new Entity())
            {
                if (where == null)
                {
                    where = o => true;
                }
                if (orderBy == null)
                {
                    orderBy = o => o.CreateTime;
                }
                var items = entity.UserGradeLog.Where(where);
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
