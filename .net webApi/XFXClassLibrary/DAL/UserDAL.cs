using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    class UserDAL
    {

        public User GetUser(int userID)
        {
            using (Entity entity = new Entity())
            {
                return entity.User.Find(userID);
            }
        }
        public List<User> GetUserView(int userID)
        {
            using (Entity entity = new Entity())
            {
                return entity.User.Include("UserExtend").Where(o => o.UserID == userID).ToList().
                Select(o => new User()
                {
                    HeadPortrait = o.HeadPortrait,
                    NickName = o.NickName,
                    Gender = o.Gender,
                    UserExtend = new UserExtend()
                    {
                        ExperienceLevel = o.UserExtend.ExperienceLevel,
                        ExperienceName = o.UserExtend.ExperienceName,
                    }
                }).ToList();
            }
        }
        public List<User> GetUsers(Func<User, bool> where)
        {
            using (Entity entity = new Entity())
            {
                return entity.User.Include("UserExtend").Where(where).ToList();
            }
        }

        public List<User> GetUsers(Func<User, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<User, object> orderBy = null, string sort = "DESC")
        {
            using (Entity entity = new Entity())
            {
                if (where == null)
                {
                    where = o => true;
                }
                if (orderBy == null)
                {
                    orderBy = o => o.UpdateTime;
                }
                var items = entity.User.Include("UserExtend").Where(where);
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

        public int AddState(int userID, int state)
        {
            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.State = user.State | state;
                return entity.SaveChanges();
            }
        }

        public int RemoveState(int userID, int state)
        {
            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.State = user.State ^ state;
                return entity.SaveChanges();
            }
        }

        public int UpdateType(int userID, int type)
        {
            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.Type = type;
                return entity.SaveChanges();
            }
        }
        
    }
}
