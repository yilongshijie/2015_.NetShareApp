using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Com.Alipay;
using XFXClassLibrary;
using System.Web;
using System.Collections.Specialized;
using System.Threading;
using System.Configuration;

namespace XFXWebAPI.Controllers
{
    public class PayController : ApiController
    {
        [HttpPost]
        public string AlipayNotify()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, request["notify_id"], request["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号                
                    string out_trade_no = request["out_trade_no"];

                    //支付宝交易号                
                    string trade_no = request["trade_no"];

                    //交易状态
                    string trade_status = request["trade_status"];


                    if (request["trade_status"] == "TRADE_FINISHED")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的

                        using (Entity entity = new Entity())
                        {
                            Order order = entity.Order.Find(out_trade_no);
                            if ((order.State & 2) == 0)
                            {
                                order.State |= 2;
                                OrderDetailsModel orderDetailsModel = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailsModel>(order.Detail);
                                foreach (var v in orderDetailsModel.OrderDetailModelList)
                                {
                                    entity.Good.Find(v.GoodID).SalesVolume++;
                                    entity.GoodChild.Find(v.GoodChildID).SalesVolume++;
                                }
                                OrderLog orderLog = new OrderLog();
                                orderLog.OrderID = order.OrderID;
                                orderLog.State = 2;
                                orderLog.CreateTime = DateTime.Now;
                                orderLog.Mark = "支付宝返回成功";
                                entity.OrderLog.Add(orderLog);
                                order.OrderExtend.ThirdPartyPaymentNumber = trade_no;

                                entity.SaveChanges();
                            }
                        }

                    }
                    else if (request["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //付款完成后，支付宝系统发送该交易状态通知
                        //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的

                        using (Entity entity = new Entity())
                        {
                            Order order = entity.Order.Find(out_trade_no);
                            if ((order.State & 2) == 0)
                            {
                                order.State |= 2;
                                OrderDetailsModel orderDetailsModel = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailsModel>(order.Detail);
                                foreach (var v in orderDetailsModel.OrderDetailModelList)
                                {
                                    entity.Good.Find(v.GoodID).SalesVolume++;
                                    entity.GoodChild.Find(v.GoodChildID).SalesVolume++;
                                }
                                OrderLog orderLog = new OrderLog();
                                orderLog.OrderID = order.OrderID;
                                orderLog.State = 2;
                                orderLog.CreateTime = DateTime.Now;
                                orderLog.Mark = "支付宝返回成功";
                                entity.OrderLog.Add(orderLog);
                                order.OrderExtend.ThirdPartyPaymentNumber = trade_no;
                                entity.SaveChanges();
                                ThreadPool.QueueUserWorkItem(delegate (object a)
                                {
                                    string tt = @"有用户已经支付订单 " + order.OrderID + "金额 " + order.OrderExtend.PaymentPrice + " 请尽快发货";
                                    UserSMS userSMS = new UserSMS() {   Tel = ConfigurationManager.AppSettings["SMSAdmin"] };
                                    Random random = new Random();
                                    userSMS.SerialNumber = DateTime.Now.ToString("yyyyMMddhhmmssfff" + random.Next(100, 999).ToString());
                                    string bb = SMS.sendSMS(userSMS.Tel, tt, userSMS.SerialNumber);
                                });
                            }
                        }
                    }
                    else
                    {
                        OrderLog orderLog = new OrderLog();
                        orderLog.OrderID = out_trade_no;

                        orderLog.CreateTime = DateTime.Now;
                        orderLog.Mark = "支付宝返回" + request["trade_status"];

                        using (Entity entity = new Entity())
                        {
                            entity.OrderLog.Add(orderLog);
                            entity.SaveChanges();
                        }

                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    return "success";  //请不要修改或删除

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    return "fail";
                }
            }
            else
            {
                return "无通知参数";
            }
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];
            HttpRequestBase request = context.Request;
            coll = request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}
