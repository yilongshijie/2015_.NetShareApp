using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using XFXClassLibrary;

namespace XFXWebAPI.Controllers
{

    public class OrderController : ApiController
    {

        [HttpPost]
        public GoodOrderView GetGoodOrderView()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string cache_shopcar = request["cache_shopcar"];

            GoodCartLocalStorage goodCartLocalStorage = JsonConvert.DeserializeObject<GoodCartLocalStorage>(cache_shopcar);
            List<GoodCartView> GoodCartViewList = new List<GoodCartView>();
            GoodOrderView goodOrderView = new GoodOrderView();
            goodOrderView.GoodCartViewList = GoodCartViewList;

            using (Entity entity = new Entity())
            {
                foreach (var v in goodCartLocalStorage.cache_shopcar)
                {

                    int goodChildID = v.goodchildid;
                    int num = v.num;
                    var goodChild = entity.GoodChild.Include("Good").Where(o => o.GoodChildID == goodChildID && o.State == 1).FirstOrDefault();
                    if (goodChild != null)
                    {
                        if ((goodChild.Good.State & 2) == 0)
                        {
                            continue;
                        }
                        var t = GoodCartViewList.Find(o => o.goodChildID == goodChildID);
                        if (t != null)
                        {
                            t.num += num;
                        }
                        else
                        {
                            GoodCartViewList.Add(new GoodCartView()
                            {
                                img = ConfigurationManager.AppSettings["UploadUrl"] + goodChild.Image,
                                title = goodChild.Good.Title,
                                guige = goodChild.Specification,
                                num = num,
                                price = goodChild.AddPrice + goodChild.Good.RealPrice,
                                goodChildID = goodChild.GoodChildID,
                            });
                        }
                        if (goodOrderView.baoyou == false)
                        {
                            goodOrderView.baoyou = (goodChild.Good.State & 32) > 0;
                        }

                    }
                }

                goodOrderView.shangpinzhongji = goodOrderView.GoodCartViewList.Sum(o => o.price * o.num);
                goodOrderView.zhifuzongfeiyong = goodOrderView.shangpinzhongji;
                WholeFieldActivity wholeFieldActivity = entity.WholeFieldActivity.Where(o => o.Type == 0).FirstOrDefault();
                if (wholeFieldActivity != null)
                {
                    if (goodOrderView.zhifuzongfeiyong >= wholeFieldActivity.FillPrice)
                    {
                        if (wholeFieldActivity.DiscountPrice != null)
                        {
                            goodOrderView.zhifuzongfeiyong -= wholeFieldActivity.DiscountPrice.Value;
                            goodOrderView.huodong += wholeFieldActivity.Title + " ";
                        }
                    }
                }
                if (!goodOrderView.baoyou)
                {

                    wholeFieldActivity = entity.WholeFieldActivity.Where(o => o.Type == 1).FirstOrDefault();
                    if (wholeFieldActivity != null)
                    {
                        if (goodOrderView.zhifuzongfeiyong >= wholeFieldActivity.FillPrice)
                        {
                            goodOrderView.baoyou = true;

                            goodOrderView.huodong += wholeFieldActivity.Title;

                        }
                    }
                    if (!goodOrderView.baoyou)
                    {
                        goodOrderView.zhifuzongfeiyong += Convert.ToInt32(ConfigurationManager.AppSettings["LogisticsPrice"]);
                    }
                }
                else
                {
                    goodOrderView.huodong += "包邮";
                }

            }
            //Authentication authentication = new Authentication(request);
            //if (string.IsNullOrEmpty(authentication.state))
            //{
            //    goodOrderView.integralMoney = entity.UserExtend.Find(authentication.userID).Integral / 100;
            //    if (goodOrderView.integralMoney > goodOrderView.zhifuzongfeiyong)
            //    {
            //        goodOrderView.integralMoney = goodOrderView.zhifuzongfeiyong;
            //    }
            //    goodOrderView.integral =Convert.ToInt32( goodOrderView.integralMoney * 100);
            //}

            return goodOrderView;

        }
        [HttpPost]
        public GoodOrderMyView GetGoodOrderMyView()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string orderid = request["orderid"];

            using (Entity entity = new Entity())
            {
                Order order = entity.Order.Where(o => o.OrderID == orderid).FirstOrDefault();
                if (order == null)
                {
                    return null;
                }

                GoodOrderMyView goodOrderMyView = new GoodOrderMyView();
                goodOrderMyView.orderDetailsModel = JsonConvert.DeserializeObject<OrderDetailsModel>(order.Detail.ToString());
                goodOrderMyView.LogisticsNumber = order.LogisticsNumber;
                goodOrderMyView.LogisticsCompany = order.LogisticsCompany;
                goodOrderMyView.LogisticsAddress = order.LogisticsAddress;
                goodOrderMyView.LogisticsTel = order.LogisticsTel;
                goodOrderMyView.LogisticsPerson = order.LogisticsPerson;
                goodOrderMyView.LogisticsPrice = order.OrderExtend.LogisticsPrice.ToString("#0");
                goodOrderMyView.PaymentPrice = order.OrderExtend.PaymentPrice.ToString("#0.00");
                goodOrderMyView.Remark1 = order.Remark1;
                goodOrderMyView.StateText = OrderBLL.GetText(order.State);
                goodOrderMyView.State = order.State;
                goodOrderMyView.createOrder = new XFXClassLibrary.CreateOrder();
                goodOrderMyView.createOrder.orderid = order.OrderID;
                goodOrderMyView.createOrder.subject = "分享";
                goodOrderMyView.createOrder.body = "分享";
                goodOrderMyView.createOrder.fee = order.OrderExtend.PaymentPrice.ToString("#0.00");
                return goodOrderMyView;
            }
        }
        [HttpPost]
        public CreateOrder CreateOrder()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string jsonText = request["data"];


            CreateOrder createOrder = new CreateOrder();
            GoodOrderView goodOrderView = JsonConvert.DeserializeObject<GoodOrderView>(jsonText);
            string payway = request["payway"];
            //string jifen = request["jifen"];
            string liuyan = request["liuyan"];

            int? userID = null;
            Authentication authentication = new Authentication(request);
            if (string.IsNullOrEmpty(authentication.state))
            {
                userID = authentication.userID;
            }

            bool baoyou = false;
            OrderDetailsModel orderDetailsModel = new OrderDetailsModel();
            Order order = new Order();

            using (Entity entity = new Entity())
            {
                foreach (var goodCartView in goodOrderView.GoodCartViewList)
                {

                    int goodChildID = goodCartView.goodChildID;
                    int num = goodCartView.num;
                    var goodChild = entity.GoodChild.Include("Good").Where(o => o.GoodChildID == goodChildID && o.State == 1).FirstOrDefault();
                    if (goodChild == null)
                    {
                        createOrder.error = "商品不存在或者已经下架";
                        return createOrder;
                    }
                    if (goodChild.Repertory == 0)
                    {
                        createOrder.error = "商品库存不够";
                        return createOrder;
                    }
                    if ((goodChild.Good.State & 32) > 0)
                    {
                        baoyou = true;
                    }
                    order.Num += num;
                    orderDetailsModel.OrderDetailModelList.Add(new OrderDetailModel()
                    {
                        GoodID = goodChild.Good.GoodID,
                        Title = goodChild.Good.Title,
                        SubTitle = goodChild.Good.SubTitle,
                        RealPrice = goodChild.Good.RealPrice,
                        GoodChildID = goodChild.GoodChildID,
                        Specification = goodChild.Specification,
                        AddPrice = goodChild.AddPrice,
                        Image = goodChild.Image,
                        num = num
                    });
                    if (string.IsNullOrEmpty(order.Image))
                    {
                        order.Image = goodChild.Image;
                    }
                    if (order.GoodID == 0)
                    {
                        order.GoodID = goodChild.Good.GoodID;
                    }
                    if (string.IsNullOrEmpty(order.Title))
                    {
                        order.Title = goodChild.Good.Title;
                    }
                    if (userID != null)
                    {
                        var t = entity.GoodCart.Where(o => o.UserID == userID && o.GoodChildID == goodChildID).FirstOrDefault();
                        if (t != null)
                        {
                            entity.GoodCart.Remove(t);
                        }
                    }
                }


                order.OrderID = DateTime.Now.ToString("yyMMddhhmmssfff") + new Random().Next(100, 1000).ToString();
                order.UserID = userID;
                order.State = 1;
                order.Detail = JsonConvert.SerializeObject(orderDetailsModel);
                order.Remark1 = liuyan;
                order.CreateTime = DateTime.Now;
                order.UpdateTime = DateTime.Now;
                order.LogisticsAddress = request["dizhi"];
                order.LogisticsTel = request["Tel"];
                order.LogisticsPerson = request["shouhuoren"];
                order.OrderExtend = new OrderExtend();
                order.OrderExtend.TotalPrice = orderDetailsModel.OrderDetailModelList.Sum(o => (o.RealPrice + o.AddPrice) * o.num);


                order.OrderExtend.DiscountPrice = 0;
                order.OrderExtend.LogisticsPrice = 0;
                order.OrderExtend.PaymentPrice = 0;
                order.OrderExtend.ThirdPartyPayment = payway;


                WholeFieldActivity wholeFieldActivity = entity.WholeFieldActivity.Where(o => o.Type == 0).FirstOrDefault();
                if (wholeFieldActivity != null)
                {
                    if (order.OrderExtend.TotalPrice >= wholeFieldActivity.FillPrice)
                    {
                        if (wholeFieldActivity.DiscountPrice != null)
                        {
                            order.OrderExtend.DiscountPrice = wholeFieldActivity.DiscountPrice.Value;

                        }
                    }
                }
                order.OrderExtend.PaymentPrice = order.OrderExtend.TotalPrice - order.OrderExtend.DiscountPrice;
                if (!baoyou)
                {
                    order.OrderExtend.LogisticsPrice = Convert.ToInt32(ConfigurationManager.AppSettings["LogisticsPrice"]);
                    wholeFieldActivity = entity.WholeFieldActivity.Where(o => o.Type == 1).FirstOrDefault();
                    if (wholeFieldActivity != null)
                    {
                        if (order.OrderExtend.PaymentPrice >= wholeFieldActivity.FillPrice)
                        {
                            order.OrderExtend.LogisticsPrice = 0;

                        }
                    }
                }

                order.OrderExtend.PaymentPrice = order.OrderExtend.TotalPrice - order.OrderExtend.DiscountPrice + order.OrderExtend.LogisticsPrice;
                //if (jifen == "true" && !string.IsNullOrEmpty(authentication.state))
                //{
                //    var user = entity.User.Find(authentication.userID);
                //    order.OrderExtend.UseIntegral = user.UserExtend.Integral;
                //    order.OrderExtend.PaymentPrice -= (order.OrderExtend.UseIntegral / 100);
                //}

                //order.OrderExtend.GainIntegral = Convert.ToInt32(order.OrderExtend.TotalPrice - order.OrderExtend.DiscountPrice + order.OrderExtend.LogisticsPrice);

                OrderLog orderLog = new OrderLog();
                orderLog.State = order.State;
                orderLog.CreateTime = DateTime.Now;
                orderLog.UserId = order.UserID;
                orderLog.Mark = "用户下单";
                order.OrderLog.Add(orderLog);
                entity.Order.Add(order);

                entity.SaveChanges();
            }
            string subject = "分享";
            string body = "";

            foreach (var v in orderDetailsModel.OrderDetailModelList)
            {
                body += v.Title + " ";
            }
            body = body.Trim();

            createOrder.orderid = order.OrderID;
            createOrder.subject = subject;
            createOrder.body = body;
            createOrder.fee = order.OrderExtend.PaymentPrice.ToString("#0.00");
            return createOrder;


        }


        [HttpPost]
        public List<OrderModel> GetOrderList()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            Authentication authentication = new Authentication(request);
            if (string.IsNullOrEmpty(authentication.state))
            {
                return new OrderBLL().getOrderView(authentication.userID);
            }
            else
            {
                string orderIDs = request["orderIDs"];
                return new OrderBLL().getOrderView(orderIDs.Split(','));

            }
        }

        [HttpPost]
        public int SetOrderList()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            string orderIDs = request["orderIDs"];
            string[] orderIDsArray = orderIDs.Split(',');

            using (Entity entity = new Entity())
            {
                foreach (var v in orderIDsArray)
                {
                    var order = entity.Order.Find(v);
                    if (order != null)
                    {
                        if (order.UserID == null)
                        {
                            order.UserID = authentication.userID;
                        }
                    }
                }

                return entity.SaveChanges();
            }
        }

        [HttpPost]
        public int GoodPingjia()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;

            Authentication authentication = new Authentication(request);
            if (!string.IsNullOrEmpty(authentication.state))
            {
                return 0;
            }
            string orderID = request["orderID"];

            using (Entity entity = new Entity())
            {
                Order order = entity.Order.Find(orderID);
                GoodEvaluate goodEvaluate = new GoodEvaluate();
                goodEvaluate.GoodID = order.GoodID;
                goodEvaluate.GoodGategoryID = entity.Good.Find(order.GoodID).GoodGategoryID;
                goodEvaluate.Detail = request["Detail"];
                goodEvaluate.UserID = authentication.userID;
                goodEvaluate.State = 1;
                goodEvaluate.Time = DateTime.Now;
                entity.GoodEvaluate.Add(goodEvaluate);
                order.State |= 16;
                return entity.SaveChanges();
            }
        }

        [HttpPost]
        public string GetWuLiu()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            string type = "";
            switch (request["wuliugongsi"])
            {
                case "申通": type = "shentong"; break;
                case "顺丰": type = "shunfeng"; break;
                case "中通速递": type = "zhongtong"; break;
                case "圆通速递": type = "yuantong"; break;
                case "韵达快运": type = "yunda"; break;
                case "天天快递": type = "tiantian"; break;
                case "德邦": type = "debangwuliu"; break;
                case "ems快递": type = "ems"; break;

            }
            string wuliuhao = request["wuliuhao"];
            string url = "http://m.kuaidi100.com/query";

            string data = string.Format("type=" + type + "&postid=" + wuliuhao + "&id=1&valicode=&temp=0.17727931282809095");
            string a= SMS.HttpGet(url, data);
            return a;
        }

    }
}
