using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class ExperienceLevelBLL
    {

        public int CirclePostReply(int userID)
        {
            using (Entity entity = new Entity())
            {
                var date = DateTime.Now.AddDays(-1);
                var count = entity.UserGradeLog.Where(o => o.UserID == userID && o.CreateTime > date && (o.Source != "回帖")).Count();
                if (count > 10)
                {
                    return 0;
                }
                var user = entity.User.Find(userID);
                user.UserExtend.ExperienceValue += 1;
                var experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceValueMin < user.UserExtend.ExperienceValue && o.ExperienceValueMax > user.UserExtend.ExperienceValue).FirstOrDefault();
                if (experienceLevel != null)
                {
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }
                    user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                }
                UserGradeLog userGradeLog = new UserGradeLog()
                {
                    UserID = userID,
                    Value = 1,
                    Type = 2,
                    Source = "回帖",
                    CreateTime = DateTime.Now
                };
                entity.UserGradeLog.Add(userGradeLog);
                return entity.SaveChanges();
            }
        }

        public int CirclePostPass(int userID)
        {

            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.UserExtend.ExperienceValue += 3;
                var experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceValueMin < user.UserExtend.ExperienceValue && o.ExperienceValueMax > user.UserExtend.ExperienceValue).FirstOrDefault();
                if (experienceLevel != null)
                {
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }
                    user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                }
                UserGradeLog userGradeLog = new UserGradeLog()
                {
                    UserID = userID,
                    Value = 3,
                    Type = 2,
                    Source = "发帖",
                    CreateTime = DateTime.Now
                };
                entity.UserGradeLog.Add(userGradeLog);
                return entity.SaveChanges();

            }
        }

        public int CirclePostJiajing(int userID)
        {
            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.UserExtend.ExperienceValue += 10;
                var experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceValueMin < user.UserExtend.ExperienceValue && o.ExperienceValueMax > user.UserExtend.ExperienceValue).FirstOrDefault();
                if (experienceLevel != null)
                {
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }
                    user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                }
                UserGradeLog userGradeLog = new UserGradeLog()
                {
                    UserID = userID,
                    Value = 10,
                    Type = 2,
                    Source = "加精",
                    CreateTime = DateTime.Now
                };
                entity.UserGradeLog.Add(userGradeLog);
                return entity.SaveChanges();

            }
        }
        public int QianDao(int userID)
        {
            using (Entity entity = new Entity())
            {

                var user = entity.User.Find(userID);
                user.UserExtend.ExperienceValue += 20;
                var experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceValueMin < user.UserExtend.ExperienceValue && o.ExperienceValueMax > user.UserExtend.ExperienceValue).FirstOrDefault();
                if (experienceLevel != null)
                {
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }
                    user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                }
                UserGradeLog userGradeLog = new UserGradeLog()
                {
                    UserID = userID,
                    Value = 20,
                    Type = 2,
                    Source = "签到",
                    CreateTime = DateTime.Now
                };
                entity.UserGradeLog.Add(userGradeLog);
                return entity.SaveChanges();
            }
        }

        public int Jinyan(int userID)
        {

            using (Entity entity = new Entity())
            {
                var user = entity.User.Find(userID);
                user.UserExtend.ExperienceValue += -50;
                var experienceLevel = entity.ExperienceLevel.Where(o => o.ExperienceValueMin < user.UserExtend.ExperienceValue && o.ExperienceValueMax > user.UserExtend.ExperienceValue).FirstOrDefault();
                if (experienceLevel != null)
                {
                    if (user.Gender == "男")
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameMan;
                    }
                    else
                    {
                        user.UserExtend.ExperienceName = experienceLevel.NameWoman;
                    }
                    user.UserExtend.ExperienceLevel = experienceLevel.ExperienceLevelValue;
                }
                UserGradeLog userGradeLog = new UserGradeLog()
                {
                    UserID = userID,
                    Value = -50,
                    Type = 2,
                    Source = "禁言",
                    CreateTime = DateTime.Now
                };
                entity.UserGradeLog.Add(userGradeLog);
                return entity.SaveChanges();

            }
        }

    }
}
