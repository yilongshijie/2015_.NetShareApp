appcan.define("shopcar", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<li data-index="<%=index%>"  data-goodID="<%=data.goodID%>"   data-goodChildID="<%=data.goodChildID%>" class="backgoundColor">\
                <p class="shopcarCheckbox"><var class="iconfont  icon-yigouxuan"></var></p>\
                <p class="shopcarImg"><img src="StyleSheet/white.png" data-original="<%=data.img%>" /></p>\
                <div class="shopcarCenter">\
                    <p class="title"><%=data.title%></p>\
                    <p class="guigeText"><%for(var i in data.guige){%><var><%=data.guige[i]%></var> <%}%></p>\
                    <div class="num">\
                        <p>\
                            <b class="iconfont icon-jianhao"></b>\
                            <var><%=data.num%></var>\
                            <b class="iconfont icon-jiahao2"></b>\
                        </p>\
                    </div>\
                </div>\
                <div class="shopcarPrice">\
                    <var data-price="<%=data.price%>"></var>\
                    <p>\
                        <b class="iconfont icon-lajixiang"></b>\
                    </p>\
                </div>\
            </li>'

        var html =
         '<div class="shopCarBottom backgoundColor">\
            <span class="quanxuan">\
                <var class="iconfont icon-gouxuankuang"></var>\
                <b>全选</b>\
            </span>\
            <span class="heji">\
            合计:\
                <var></var>\
            </span>\
            <span class="jiesuan">\
                <b>结 算</b>\
            </span>\
        </div>'
        self.template = model;
        self._template = appcan.view.template(self.template);
        self.afterElement = $(html);
        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.set(self.option.data);
        }
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            if (data.length == 0) {
                $(self.option.empty).removeClass("uhide");
            }
            else {
                $(self.option.selector).removeClass("uhide");
                $(self.option.empty).after(this.afterElement);
            }
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i,
                    option: self.option
                }));
                this.ele.append(ele);
            }

            var num = this.ele.find(".shopcarCenter .num");
            num.find(".icon-jiahao2").on("tap", function () {
                var element = $(this).parent().find("var");
                var elementNum = Number(element.html())
                if (elementNum >= 99) {
                    return;
                }
                element.html(elementNum + 1);
                var li = $(this).parent().parent().parent().parent();;
                var goodChildID = li.attr("data-goodChildID")
                self.__jiaJianShopCar(element.html(), goodChildID)
                self.__reTotalPrice();
            })
            num.find(".icon-jianhao").on("tap", function () {
                var element = $(this).parent().find("var");
                var elementNum = Number(element.html())
                if (elementNum <= 1) {
                    return;
                }
                element.html(elementNum - 1);
                var li = $(this).parent().parent().parent().parent();;
                var goodChildID = li.attr("data-goodChildID")
                self.__jiaJianShopCar(element.html(), goodChildID)
                self.__reTotalPrice();
            })

            self.ele.find(".shopcarImg").on("tap", function () {
                localStorage.setItem("request", $(this).parent().attr("data-goodID"));
                appcan.window.evaluateScript("shopdetail", 'uexWindow.close(0,0)');
                appcan.openWinWithUrl("shopdetail", "shopdetail.html", 2, 0, 400)
            })

            self.ele.find(".iconfont.icon-lajixiang").on("tap", function () {
                var li = $(this).parent().parent().parent();
                var goodChildID = li.attr("data-goodChildID");
                self.__removeShopCar(goodChildID, li);
                self.__reTotalPrice();
            })
            self.ele.find(".shopcarCheckbox").on("tap", function () {
                var element = $(this).find(".icon-gouxuankuang , .icon-yigouxuan")
                element.toggleClass("icon-gouxuankuang").toggleClass("icon-yigouxuan");
                self.__reTotalPrice();
            })

            self.afterElement.find(".quanxuan").on("tap", function () {
                var element = $(this).find(".icon-gouxuankuang , .icon-yigouxuan")
                element.toggleClass("icon-gouxuankuang").toggleClass("icon-yigouxuan");
                if (element.hasClass("icon-yigouxuan")) {
                    self.ele.find(".shopcarCheckbox").find(".icon-gouxuankuang , .icon-yigouxuan").removeClass("icon-gouxuankuang").addClass("icon-yigouxuan");
                }
                else {
                    self.ele.find(".shopcarCheckbox").find(".icon-gouxuankuang , .icon-yigouxuan").removeClass("icon-yigouxuan").addClass("icon-gouxuankuang");

                }
                self.__reTotalPrice();
            })

            self.afterElement.find(".jiesuan b").on("tap", function () {
                if (!$(this).hasClass("eable")) {
                    return;
                }
                var li = self.ele.find("li");
                var tempstr = "";
                var request_shopcar = { cache_shopcar: [] };
                _.each(li, function (element) {
                    if ($(element).find(".icon-yigouxuan").length == 0) return;
                    var num = Number($(element).find(".num  var").html());
                    var goodChildID = $(element).attr("data-goodChildID");
                    var order = { goodchildid: goodChildID, num: num };
                    request_shopcar["cache_shopcar"].push(order);
                })
                if (request_shopcar["cache_shopcar"].length > 0) {
                    localStorage.setItem("request", JSON.stringify(request_shopcar));
                    appcan.openWinWithUrl("shoporder", "shoporder.html", 2, 0, 400)
                }
            })

            self.__reTotalPrice();


        },
        __quanxuan: function () {
            if (this.ele.find(".shopcarCheckbox").find(".icon-gouxuankuang").length > 0 || this.ele.find(".shopcarCheckbox").find(".icon-yigouxuan").length == 0) {
                this.afterElement.find(".quanxuan").find(" .icon-yigouxuan").removeClass("icon-yigouxuan").addClass("icon-gouxuankuang")
            }
            else {
                this.afterElement.find(".quanxuan").find(" .icon-gouxuankuang").removeClass("icon-gouxuankuang").addClass("icon-yigouxuan")

            }
        },
        __reTotalPrice: function () {
            this.__quanxuan();
            var li = this.ele.find("li");
            var total = 0;
            _.each(li, function (element) {
                var num = Number($(element).find(".num  var").html());
                var price = $(element).find(".shopcarPrice var").attr("data-price");
                var oneTotal = num * price;
                $(element).find(".shopcarPrice var").html("￥" + oneTotal.toFixed(2));
                if ($(element).find(".shopcarCheckbox var").hasClass("icon-yigouxuan")) {
                    total = oneTotal + total;
                }
            })
            this.afterElement.find(".heji").find("var").html("￥" + total.toFixed(2));
            if (total != 0) {
                this.afterElement.find(".jiesuan b").addClass("eable");
            }
            else {

                this.afterElement.find(".jiesuan b").removeClass("eable");
            }
        },
        __removeShopCar: function (goodChildID, li) {
            var self = this;
            if (localStorage.getItem("authentication_token") == null) {
                var cache_shopcar = localStorage.getItem("cache_shopcar");
                if (cache_shopcar != null) {
                    cache_shopcar = JSON.parse(cache_shopcar);
                    var temp = _.filter(cache_shopcar.cache_shopcar, function (element) {
                        if (element.goodchildid == goodChildID) {
                            return false;
                        }
                        else {
                            return true;
                        }
                    });
                    cache_shopcar.cache_shopcar = temp;
                    localStorage.setItem("cache_shopcar", JSON.stringify(cache_shopcar));
                    li.remove();
                    self.__reTotalPrice();
                    if (self.option.page == "shopcar") {
                        appcan.window.evaluatePopoverScript("root", "content_3", 'uexWindow.closePopover("content_3")'); 
                    }
 
                }
            }
            else {
                appcan.request.ajax({
                    url: url + "/GoodCart/DeleteGoodCart",
                    dataType: "json",
                    type: "Post",
                    data: $.extend({ goodchildid: goodChildID }, sendtoken()),
                    success: function (data) {
                        li.remove();
                        self.__reTotalPrice();
                        if (self.option.page == "shopcar") {
                            appcan.window.evaluatePopoverScript("root", "content_3", 'reload();');
                        }
 
                    },
                    error: function () {
                        
                        appcan.window.openToast("请求失败", 2500, 5, 0)
                    }
                })
            }

        },
        __jiaJianShopCar: function (num, goodChildID) {
            var self = this;
            if (localStorage.getItem("authentication_token") == null) {
                var cache_shopcar = localStorage.getItem("cache_shopcar");
                if (cache_shopcar != null) {
                    cache_shopcar = JSON.parse(cache_shopcar);
                    var temp = _.map(cache_shopcar.cache_shopcar, function (element) {
                        if (element.goodchildid == goodChildID) {
                            element.num = num;
                        }
                        return element;

                    });
                    cache_shopcar.cache_shopcar = temp;
                    localStorage.setItem("cache_shopcar", JSON.stringify(cache_shopcar));
                    if (self.option.page == "shopcar") {
                        appcan.window.evaluatePopoverScript("root", "content_3", 'uexWindow.closePopover("content_3")'); 
                    }
                }
            }
            else {
                appcan.request.ajax({
                    url: url + "/GoodCart/NumGoodCart",
                    dataType: "json",
                    type: "Post",
                    data: $.extend({ goodchildid: goodChildID, num: num }, sendtoken()),
                    success: function (data) {
                        if (self.option.page == "shopcar") {
                            appcan.window.evaluatePopoverScript("root", "content_3", 'uexWindow.closePopover("content_3")');
          
                        }
                    },
                    error: function () {
                        
                        appcan.window.openToast("请求失败", 2500, 5, 0)
                    }
                })
            }


        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
