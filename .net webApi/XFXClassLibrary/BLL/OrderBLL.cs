using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class OrderBLL
    {
        public OrderDAL orderDAL = new OrderDAL();
        public List<OrderModel> getOrderView(string[] orderIDs)
        {
            return orderDAL.getOrderView(orderIDs).Select(o => new OrderModel()
            {
                CreateTime = o.CreateTime,
                State = o.State,
                StateText = OrderBLL.GetText(o.State),
                OrderID = o.OrderID,
                Num = o.Num,
                PaymentPrice = o.PaymentPrice,
                Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
                GoodID = o.GoodID
            }).ToList();
        }

        public List<OrderModel> getOrderView(int userID)
        {
            return orderDAL.getOrderView(userID).Select(o => new OrderModel()
            {
                CreateTime = o.CreateTime,
                State = o.State,
                StateText = OrderBLL.GetText(o.State),
                OrderID = o.OrderID,
                Num = o.Num,
                PaymentPrice = o.PaymentPrice,
                Image = ConfigurationManager.AppSettings["UploadUrl"] + o.Image,
                GoodID = o.GoodID
            }).ToList();
        }

        public List<Order> GetOrder(Func<Order, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<Order, object> orderBy = null, string sort = "DESC")
        {
            return orderDAL.GetOrder(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }

        public List<Order> GetOrder(string orderID)
        {
            return orderDAL.GetOrder(orderID);

        }

        public List<OrderLog> GetOrderLog(Func<OrderLog, bool> delegation, int pageSize, int pageIndex, out int pageTotal, Func<OrderLog, object> orderBy = null, string sort = "DESC")
        {
            return orderDAL.GetOrderLog(delegation, pageSize, pageIndex, out pageTotal, orderBy, sort);
        }

        public static string GetText(int state)
        {
            if ((state & 2048) > 0)
            {
                return "已退款";
            }
            if ((state & 1024) > 0)
            {
                return "已收货";
            }
            if ((state & 512) > 0)
            {
                return "已退货";
            }
            if ((state & 256) > 0)
            {
                return "同意退货";
            }
            if ((state & 128) > 0)
            {
                return "退货处理";
            }
            if ((state & 64) > 0)
            {
                return "申请退货";
            }
            if ((state & 16) > 0)
            {
                return "已评价";
            }
            if ((state & 8) > 0)
            {
                return "已收货";

            }
            if ((state & 4) > 0)
            {
                return "已发货";
            }
            if ((state & 2) > 0)
            {
                return "已支付";
            }
            if ((state & 1) > 0)
            {
                return "未支付";

            }
            return "";
        }
    }
}
