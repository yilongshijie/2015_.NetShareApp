using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class ComplaintDAL
    {

        public List<Complaint> GetComplaint(Func<Complaint, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<Complaint, object> orderBy = null, string sort = "DESC")
        { 
            using (Entity entity = new Entity())
            {

                if (where == null)
                {
                    where = o => true;
                }
                if (orderBy == null)
                {
                    orderBy = o => o.Time;
                }
                var items = entity.Complaint.Where(where);
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
