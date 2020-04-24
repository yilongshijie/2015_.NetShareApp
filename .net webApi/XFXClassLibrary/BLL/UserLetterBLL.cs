using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace XFXClassLibrary
{
    public class UserLetterBLL
    {
        private UserLetterDAL userLetterDAL = new UserLetterDAL();


        public List<UserLetter> GetUserLetter(Func<UserLetter, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<UserLetter, object> orderBy = null, string sort = "DESC")
        {
            return userLetterDAL.GetUserLetter(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }
        public List<UserLetter> GetUserLetter(Func<UserLetter, bool> delegation, string sort = "DESC")
        {
            return userLetterDAL.GetUserLetter(delegation, sort);
        }
        public List<UserLetter> GetUserLetterToplevel(int UserID)
        {
            return userLetterDAL.GetUserLetterToplevel(UserID);
        }

        public static UserLetter Create(int InitiativeUserID, int PassivityUserID, string text, int type, int? CirclePostID = null)
        {
            using (Entity entity = new Entity())
            {
                UserLetter userLetter = new UserLetter();
                userLetter.InitiativeUserID = InitiativeUserID;
                userLetter.PassivityUserID = PassivityUserID;
                userLetter.Text = text;
                userLetter.CreateTime = DateTime.Now;
                userLetter.Type = type;
                userLetter.State = 1;
                userLetter.CirclePostID = CirclePostID;
                entity.UserLetter.Add(userLetter);
                entity.SaveChanges();
                return userLetter;
            }

        }


    }
}
