using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    class GoodExperienceDAL
    {

        public List<GoodExperience> GetGoodExperience(Func<GoodExperience, bool> where, Func<GoodExperience, object> orderBy = null, string sort = "DESC")
        {

            if (orderBy == null)
            {

                orderBy = o => o.UpdateTime;
            }

            using (Entity entity = new Entity())
            {
                var items = entity.GoodExperience.Include("User").Include("Good").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy).ThenByDescending(o => o.UpdateTime).Skip(0).Take(300);
                }
                else
                {
                    items = items.OrderBy(orderBy).ThenByDescending(o => o.UpdateTime).Skip(0).Take(300);
                }

                return items.ToList();
            }
        }



        public List<GoodExperience> GetGoodExperience(Func<GoodExperience, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<GoodExperience, object> orderBy = null, string sort = "DESC")
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
                var items = entity.GoodExperience.Include("User").Include("Good").Where(where);
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

        public List<GoodExperienceReply> GetGoodExperienceReply(Func<GoodExperienceReply, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<GoodExperienceReply, object> orderBy = null, string sort = "DESC")
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
                var items = entity.GoodExperienceReply.Include("User").Where(where);
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
