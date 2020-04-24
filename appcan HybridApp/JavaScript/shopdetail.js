appcan.define("shopDetail", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = option;
        var model =
            '<div class="productTitle backgoundColor">\
                <p class="title"><%=data.Title%></p>\
                <p class="label"><%=data.SubTitle%></p>\
            </div>\
            <div class="priceAndNum backgoundColor">\
                <p class="price"><span>￥<%=data.RealPrice%></span><del>市场价￥<%=data.BidPrice%></del></p>\
                <p class="num"><var class="iconfont icon-num_limit"></var><span><%=data.SalesVolume%>销量</span></p>\
            </div>\
            <ul class="promise">\
               <% for(var i in  data.promise){%>\
                    <li><b class="iconfont icon-duigou"></b><%=data.promise[i]%></li>\
                <%}%>\
            </ul>\
                <div class="backgoundColor baoyou">\
       </div>'


        var modelChildPage =
        ' <span class="close iconfont icon-chahao"></span>\
        <div class="goods">\
            <p><img class="guigeImg" src="<%=data.GoodChild[0].Image%>" /></p>\
            <p class="goodsText" data-price="<%=data.RealPrice%>">\
                <span>￥<%=data.RealPrice%></span>\
            </p>\
        </div>\
        <div class="model">\
          <div class="guige">\
                  <ul>\
           <% for(i in data.GoodChild){%>\
 <li data-text="<%=data.GoodChild[i].Specification%>" data-add="<%=data.GoodChild[i].AddPrice%>" data-GoodChildID="<%=data.GoodChild[i].GoodChildID%>"  data-Image="<%=data.GoodChild[i].Image%>"  class="<%=(data.GoodChild[i].Repertory<=0 ? "no":"")%>  <%=(data.GoodChild[i].Repertory > 0  && data.GoodChild.length==1 ? "guigeSelect":"")%>" ><b class="iconfont icon-gouxuanjiaobiao"></b><%=data.GoodChild[i].Specification%></li>\
               <% }%>\
                </ul>\
           </div>\
            <div class="num">\
                <span>购买数量</span>\
                <div>\
                    <b class="iconfont icon-jianhao"></b>\
                    <var>1</var>\
                    <b class="iconfont icon-jiahao2"></b>\
                </div>\
            </div>\
        </div>\
        <div class="buttonDiv">\
            <span class="input" id="submit" >确认</span>\
        </div>\
        <input type="hidden" name="buyOrshopCar" />\
    </div>';
        self._template = appcan.view.template(model);
        self._templateChildPage = appcan.view.template(modelChildPage);
        self.ele = $(self.option.selector);
        self.eleChildPage = $(self.option.selectorChildPage);
        if (self.option.data) {
            self.set(self.option.data);
        }
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            if (self.option.experience != true) {
                var ele = $(this._template({
                    data: data
                }));
                self.ele.append(ele);
                $(self.option.selectorNeirong).append(data.Detail)
            }

            var eleChildPage = $(this._templateChildPage({
                data: data
            }));
            self.eleChildPage.append(eleChildPage);
            self.setEvent();

        },
        setEvent: function () {
            var payChildPage = $(".shopDetailChildPage");
            var productBottom = $(".productBottom");

            productBottom.find(".pbright span").on("tap", function () {
                $(".backgroundCover").removeClass("uhide");
                payChildPage.removeClass("closePage").addClass("openPage")
                payChildPage.find("[name=buyOrshopCar]").val($(this).attr("data-mark"));
            })
            payChildPage.find(".close").on("tap", function () {
                payChildPage.removeClass("openPage").addClass("closePage")
                $(".backgroundCover").addClass("uhide");

            })
            payChildPage.find(".guige li").on("tap", function () {
                if ($(this).hasClass("no")) return;
                $(this).parent().children().removeClass("guigeSelect");
                $(this).addClass("guigeSelect");
                $(".guigeImg").attr("src", $(this).attr("data-Image"));
                reTotal();
            })



            function reTotal() {

                var select = payChildPage.find(".model li.guigeSelect");
                var goodsText = payChildPage.find(".goodsText")
                var price = Number(goodsText.attr("data-price"));
                goodsText.find("var").remove();
                _.each(select, function (i) {
                    if (!isNaN($(i).attr("data-add"))) {
                        price += Number($(i).attr("data-add"));
                    }
                    goodsText.append("<var>" + $(i).attr("data-text") + "</var>");
                });
                goodsText.find("span").html("￥" + price.toFixed(2));

            };
            reTotal();

            var num = payChildPage.find(".model .num");
            num.find(".icon-jiahao2").on("tap", function () {
                var element = $(this).parent().find("var");
                var elementNum = Number(element.html())
                if (elementNum >= 99) {
                    return;
                }
                element.html(elementNum + 1);
                reTotal();
            })
            num.find(".icon-jianhao").on("tap", function () {
                var element = $(this).parent().find("var");
                var elementNum = Number(element.html())
                if (elementNum <= 1) {
                    return;
                }
                element.html(elementNum - 1);
                reTotal();
            })

            appcan.button("#submit", "ani-act", function () {
                var self = this;
                var tempbool = true;
                _.each($(".guige"), function (ele) {
                    if ($(ele).find(".guigeSelect").length == 0) {
                        tempbool = false;
                    }
                })
                if (tempbool) {
                    var goodchildid = $(".guige").find(".guigeSelect").attr("data-goodchildid");
                    var num = $(".model .num var").html();
                    var order = { goodchildid: goodchildid, num: num };
                    payChildPage.removeClass("openPage").addClass("closePage");
                    $(".backgroundCover").addClass("uhide");
                    var buyOrshopCar = payChildPage.find("[name=buyOrshopCar]").val();
                    if (buyOrshopCar == "shopcar") {
                        if (localStorage.getItem("authentication_token") == null) {
                            if (localStorage.getItem("cache_shopcar") == null) {
                                var cache_shopcar = { cache_shopcar: [order] };
                                cache_shopcar = JSON.stringify(cache_shopcar);
                                localStorage.setItem("cache_shopcar", cache_shopcar);
                            }
                            else {
                                var cache_shopcar = localStorage.getItem("cache_shopcar");
                                cache_shopcar = JSON.parse(cache_shopcar);
                                cache_shopcar["cache_shopcar"].push(order);
                                cache_shopcar = JSON.stringify(cache_shopcar);
                                localStorage.setItem("cache_shopcar", cache_shopcar);
                            }
                            appcan.window.openToast("添加成功", 2500, 5, 0)
                            $(".icon-gouwuche").addClass("you");

                            appcan.window.evaluatePopoverScript("root", "content_3", 'reload();');

                        }
                        else {
                            appcan.request.ajax({
                                url: url + "/GoodCart/SetGoodCart",
                                type: "POST",
                                dataType: "json",
                                data: $.extend({ goodchildid: goodchildid, num: num }, sendtoken()),
                                success: function (data) {
                                    appcan.window.openToast("添加成功", 2500, 5, 0)
                                    $(".icon-gouwuche").addClass("you");

                                    appcan.window.evaluatePopoverScript("root", "content_3", 'reload();');
                                },
                                error: function () {

                                    appcan.window.openToast("请求失败", 2500, 5, 0)
                                }
                            })

                        }
                    }
                    else {
                        var cache_shopcar = { cache_shopcar: [order] };
                        localStorage.setItem("request", JSON.stringify(cache_shopcar));
                        appcan.openWinWithUrl("shoporder", "shoporder.html", 2, 0, 400)
                    }
                }
                else {
                    appcan.window.openToast("请选择产品属性", 2500, 5, 0);
                }
            })
        }

    }
    module.exports = function (option) {
        return new view(option);
    };


})
