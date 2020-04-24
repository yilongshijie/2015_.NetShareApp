using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    class UserLetterDAL
    {

        public List<UserLetter> GetUserLetter(Func<UserLetter, bool> where, string sort = "DESC")
        {
            using (Entity entity = new Entity())
            {
                if (sort == "DESC")
                {
                    return entity.UserLetter.Where(where).OrderByDescending(o => o.CreateTime).Skip(0).Take(100).ToList();
                }
                else
                {
                    return entity.UserLetter.Where(where).OrderBy(o => o.CreateTime).Skip(0)
                                .Take(100).ToList();
                }

            }
        }
        public List<UserLetter> GetUserLetterToplevel(int UserID)
        {
            using (Entity entity = new Entity())
            { 
                return entity.Database.SqlQuery<UserLetter>(@"select top 100 T1.* from UserLetter as T1  join   (
select   MAX(CreateTime) as MaxCreateTime, PassivityUserID, InitiativeUserID from UserLetter  where PassivityUserID=" + UserID+@" group by PassivityUserID, InitiativeUserID
) as T2 on T1.CreateTime = T2.MaxCreateTime and T1.InitiativeUserID = T2.InitiativeUserID and T1.PassivityUserID = T2.PassivityUserID
order by CreateTime desc").ToList();
            }
        }

        public List<UserLetter> GetUserLetter(Func<UserLetter, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<UserLetter, object> orderBy = null, string sort = "DESC")
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
                var items = entity.UserLetter.Include("User").Include("User.UserExtend").Where(where);
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
