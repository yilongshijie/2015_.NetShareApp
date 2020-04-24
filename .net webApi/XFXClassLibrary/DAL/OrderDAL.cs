using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFXClassLibrary
{
    public class OrderDAL
    {

        public List<OrderModel> getOrderView(string[] orderIDs)
        {
            using (Entity entity = new Entity())
            {
                return entity.Order.Where(o => orderIDs.Contains(o.OrderID)).OrderByDescending(o => o.CreateTime).Select(o => new OrderModel()
                {
                    CreateTime = o.CreateTime,
                    State = o.State,
                    OrderID = o.OrderID,
                    Num = o.Num,
                    PaymentPrice = o.OrderExtend.PaymentPrice,
                    Image = o.Image,
                    GoodID = o.GoodID
                }).ToList();
            }
        }

        public List<OrderModel> getOrderView(int userID)
        {
            using (Entity entity = new Entity())
            {
                return entity.Order.Where(o => o.UserID == userID).OrderByDescending(o => o.CreateTime).Select(o => new OrderModel()
                {
                    CreateTime = o.CreateTime,
                    State = o.State,
                    OrderID = o.OrderID,
                    Num = o.Num,
                    PaymentPrice = o.OrderExtend.PaymentPrice,
                    Image = o.Image,
                    GoodID = o.GoodID
                }).ToList();
            }
        }
        public List<Order> GetOrder(Func<Order, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<Order, object> orderBy = null, string sort = "DESC")
        {
            using (Entity entity = new Entity())
            {
                if (where == null)
                {
                    where = o => true;
                }
                if (orderBy == null)
                {
                    orderBy = o => o.UpdateTime;
                }
                var items = entity.Order.Include("OrderExtend").Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy);
                }
                else
                {
                    items = items.OrderBy(orderBy);
                }
                pageTotal = items.Count();
                return items.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize).ToList();
            }
        }

        public List<Order> GetOrder(string orderID)
        {
            using (Entity entity = new Entity())
            {
                return entity.Order.Include("OrderExtend").Where(o => o.OrderID == orderID).ToList();
            }
        }

        public List<OrderLog> GetOrderLog(Func<OrderLog, bool> where, int pageSize, int pageIndex, out int pageTotal, Func<OrderLog, object> orderBy = null, string sort = "DESC")
        {
            using (Entity entity = new Entity())
            {
                if (where == null)
                {
                    where = o => true;
                }
                if (orderBy == null)
                {
                    orderBy = o => o.CreateTime;
                }
                var items = entity.OrderLog.Where(where);
                if (sort == "DESC")
                {
                    items = items.OrderByDescending(orderBy);
                }
                else
                {
                    items = items.OrderBy(orderBy);
                }
                pageTotal = items.Count();
                return items.Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize).ToList();
            }
        }
    }
}
