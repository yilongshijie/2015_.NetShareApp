using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace XFXClassLibrary
{
    public class GoodOrderView
    {
        public IEnumerable<GoodCartView> GoodCartViewList;
        public string LogisticsPrice = ConfigurationManager.AppSettings["LogisticsPrice"];
        public int integral;
        public decimal integralMoney;
        public bool baoyou;
        public string huodong;
        public decimal shangpinzhongji;
        public decimal zhifuzongfeiyong;
    }
}