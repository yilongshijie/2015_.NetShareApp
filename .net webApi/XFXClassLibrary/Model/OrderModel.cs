using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class OrderDetailModel
    {
        public int GoodID;
        public string Title;
        public string SubTitle;
        public decimal RealPrice;
        public int GoodChildID;
        public string Specification;
        public decimal AddPrice;
        public string Image;
        public int num;
        public string ImageHref {
            get { return ConfigurationManager.AppSettings["UploadUrl"]  + Image; }
        } 
        public decimal TotalPrice
        {
            get { return RealPrice + AddPrice; }
        }
    }

    public class OrderDetailsModel
    {
        public List<OrderDetailModel> OrderDetailModelList = new List<OrderDetailModel>();

    }

    public class OrderModel
    {
        public OrderDetailsModel orderDetailsModel;
        public string _createTime { get { return CreateTime.ToString("yyyy-MM-dd"); } }
        public DateTime CreateTime;
        public int State;
        public string StateText;
        public string OrderID;
        public int Num;
        public decimal PaymentPrice;
        public string Image;
        public int GoodID;
    }
}
