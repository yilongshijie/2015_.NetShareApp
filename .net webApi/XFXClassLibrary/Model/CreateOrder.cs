using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class CreateOrder
    {
        public string error;
        public string orderid;
        public string partner = "2088021868018114";
        public string seller = "4032974@qq.com";
        public string rsaPrivate = @"MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAOS5PHxGID1M9ZGE
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
        public string rsaPublic = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
        public string notifyUrl = ConfigurationManager.AppSettings["AlipayNotify"];
        public string subject;
        public string body;
        public string fee;
    }


}
