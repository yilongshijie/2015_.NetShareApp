using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Com.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string private_key = "";
        private static string public_key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            partner = "2088021868018114";

            //商户的私钥
            private_key =
                @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAOS5PHxGID1M9ZGE
3C0R4NU56BhbS+CXIHwz7R83ascyobYWxl/zu9y9VGfiQN/cWTWTKHuoPvayuXxI
3s1QlnYug6LrMPllcyCWZRsSnwVU/TOzqqaksSlJibIbbnWiUmBKAuHDeBL2MAQD
0hwiS/pFrIO6HuESBA/eDWWVpzVTAgMBAAECgYEAicR1hVlsA+B+Lfe78z3Ikka9
4SXkr1P4UjeoxVvI6lE6DIbtWFBkQNKdq7EhaHW+GkIYaVtvCYOfrBfsc/jtlwBC
Y+2v/6wrh2ELlSDBFttOha/VuQIj4qPsZiiCYo1gAWD7Lay+k/3LTZ7z+WNEcxcb
DZ5Mxh4q7JcryGTVANkCQQD6YdDSbvFewElBKHvrX2mYhy7E1hx4AZXBdIWfQ7WF
wyhk5KXbFgMEoTjmnlTCecMAeUqbUesEamGeNsOpqwT9AkEA6dsD1KAyN51R6THr
jWM9d0gkwnMuAKIVCmT3U73iZmHlPFvYSKwku4JH6yIfxXhNzXBBoGI58+L1IzeY
JQ7cjwJAaAgIWcba5wGB7l7BzjQgjc1tMz+7KGmQLYcalefuHhvORs1x1Cu7KUtL
dxbGJN+ulB3RT+OjgHwq/y/F8FMsiQJALhH+pDHXEsvEaktyW01Uu54T19b3FhrY
SE7xxqae5oqcrZJufoqjRel5n6H+XlnAAhv+YclYH2rz3jdCmvJdkwJBALqU54Co
dzNo5C0dVgPcTyEAyZSd12ofiirdyHHpF8DZA/b+7uaLxnUFSay+iZdRkR0TQ9sC
75AHND+MoT/cjr8=";

            //支付宝的公钥，无需修改该值
            public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：RSA、DSA、MD5
            sign_type = "RSA";
        }

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}