using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace XFXClassLibrary
{

    public static class CommonSecurity
    {
        public static string SHA1(string source)
        {

            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");

        }
        public static string MD5(string source)
        {

            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5"); ;

        }

        public static string SHA1MD5MD5(string source)
        {
            return MD5(MD5(SHA1(source)));
        }

    }
}
