//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace XFXClassLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderExtend
    {
        public string OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal LogisticsPrice { get; set; }
        public decimal PaymentPrice { get; set; }
        public string DiscountRemark { get; set; }
        public string ThirdPartyPayment { get; set; }
        public string ThirdPartyPaymentNumber { get; set; }
        public int GainIntegral { get; set; }
        public int UseIntegral { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
