using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XFXClassLibrary
{

    public static class XFXExt
    {
        public static void SetCounterfeit(this User user)
        {
            user.Type = user.Type | (int)Math.Pow(2, 30);
        }
        public static bool GetCounterfeit(this User user)
        {
            return (user.Type & (int)Math.Pow(2, 30)) > 0;
        }
        public static User InitBeforeSave(this User user)
        {
            if (string.IsNullOrWhiteSpace(user.HeadPortrait))
            {
                if (user.Gender == "男")
                {
                    user.HeadPortrait = "init/man.jpg";
                }
                else
                {
                    user.HeadPortrait = "init/woman.jpg";
                }
            }
            user.SexualOrientation = user.SexualOrientation ?? "";
            user.Age = user.Age ?? "";
            user.Location = user.Location ?? "";
            user.Constellation = user.Constellation ?? "";
            user.UserExtend.Bust = user.UserExtend.Bust ?? "";
            user.UserExtend.Waist = user.UserExtend.Waist ?? "";
            user.UserExtend.Hips = user.UserExtend.Hips ?? "";
            user.UserExtend.Stature = user.UserExtend.Stature ?? "";
            user.UserExtend.Weight = user.UserExtend.Weight ?? "";
            user.UserExtend.ProfessionalTitle = user.UserExtend.ProfessionalTitle ?? "";
            user.UserExtend.ProfessionalDescription = user.UserExtend.ProfessionalDescription ?? "";
            user.Married = user.Married ?? "保密";
            user.Type = user.Type == 0 ? 1 : user.Type; 


            return user;
        }

        public static string GetErrorMessage(this Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception.Message;
            }
            else
            {
                return exception.InnerException.GetErrorMessage();
            }
        }

        public static string imgList(string imglist, string src, bool tag = true, int num = int.MaxValue)

        {
            if (imglist == null) return "";
            var imglist1 = imglist.Split(',');
            List<string> imglist2 = new List<string>();
            foreach (string v in imglist1)
            {
                if (string.IsNullOrEmpty(v)) continue;
                num--;
                if (tag)
                {

                    imglist2.Add(string.Format("<img src=\"{0}{1}\"/>", src, v));
                }
                else
                {
                    imglist2.Add(string.Format("{0}{1}", src, v));
                }
                if (num == 0) break;
            }
            return string.Join(",", imglist2);
        }

        public static string setStart(this string str)
        {

            char[] strarr = new char[str.Length];
            for (int i = 1; i < strarr.Length - 1; i++)
            {
                strarr[i] = '*';
            }
            strarr[0] = str[0];
            strarr[str.Length - 1] = str[str.Length - 1];
            return new string(strarr);


        }

        public static int getUserType(this int userType)
        {
            var type = ((userType & 1) | (userType & 2) | (userType & 4));
            if (type == 3)
            {
                type = 2;
            }
            if (type == 5)
            {
                type = 4;
            }
            if (type == 6)
            {
                type = 7;
            }
            if (type == 0)
            {
                type = 1;
            }
            return type;
        }

        public static string removeTag(this string str)
        {
            string regexstr = @"<(?!img|br|p|/p).*?>";   //去除所有标签，只剩img,br,p
            return  Regex.Replace(str, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

    }
}
