$(".jiesuan b").one("tap",jiesuanFunc);

function jiesuanFunc () {
    if ($(".shouhuodizhi .shouhuorenTel").html().length < 5) {
        $(".jiesuan b").one("tap",jiesuanFunc);
        $(".shouhuodizhi").trigger("tap");
        return;
    }
    $(".backgroundCover").addClass("uhide");
    appcan.request.ajax({
        url: url + "/Order/CreateOrder",
        dataType: "json",
        type: "Post",
        data: $.extend({ data: window.data },
            {
                payway: $(".payway .icon-danxuanxuanzhong").attr("data-type"),
                jifen: $(".jifenli .iconfont").hasClass("icon-yigouxuan"),
                liuyan: $(".orderliuyan input").val(),
                dizhi: $(".shouhuodizhi .shohuodizhi").html(),
                Tel: $(".shouhuodizhi .shouhuorenTel").html(),
                shouhuoren: $(".shouhuodizhi .shouhuoren").html()
            }, sendtoken()),
        success: function (data) {
            if (data.error) {
                
                appcan.window.openToast(data.error, 2500, 5, 0)
            }
            else {
                if (localStorage.getItem("authentication_token") == null) {
                    var cache_orderIDs = localStorage.getItem("cache_orderIDs");
                    if (cache_orderIDs == null || $.trim(cache_orderIDs) == "") {
                        localStorage.setItem("cache_orderIDs", data.orderid)
                    }
                    else {
                        localStorage.setItem("cache_orderIDs", cache_orderIDs + "," + data.orderid);
                    }

                    var cache_shopcar = localStorage.getItem("cache_shopcar");
                    if (cache_shopcar != null) {
                        cache_shopcar = JSON.parse(cache_shopcar);
                        var goodChildIDList = _.map(window.data.GoodCartViewList, function (item) { return item.goodChildID });
                        var temp = _.filter(cache_shopcar.cache_shopcar, function (element) {
                            _.indexOf(goodChildIDList, element.goodchildid) > -1
                        });
                        cache_shopcar.cache_shopcar = temp;
                        localStorage.setItem("cache_shopcar", JSON.stringify(cache_shopcar));
                    }
                }
                uexAliPay.onStatus = function () {
  
                    appcan.window.evaluatePopoverScript("root", "content_3", ' window.reload()');
                    appcan.window.evaluateScript("shopcar", 'uexWindow.close(0,0)');
                    $(".backgroundCover").removeClass("uhide");

                    appcan.openWinWithUrl("shoporderlist", "shoporderlist.html", 2, 0, 400);

                };
                uexWidgetOne.cbError = function () {

                    $(".backgroundCover").removeClass("uhide");
                };
                uexAliPay.setPayInfo(data.partner, data.seller, data.rsaPrivate, data.rsaPublic, data.notifyUrl)
                uexAliPay.pay(data.orderid, data.subject, data.body, data.fee)
            }
        },
        error: function () {
            $(".backgroundCover").removeClass("uhide");
            appcan.window.openToast("请求失败", 2500, 5, 0)
        }
    })

}


$(".shouhuodizhi").on("tap", function () {

    appcan.window.open({
        name: "address",
        data: "address.html",
        aniId: 0,
        type: 0,
        animDuration: 0
    });
})

$(".payway li").on("tap", function () {
    if ($(this).find("span.iconfont").hasClass("icon-danxuanxuanzhong")) {
        return;
    }
    else {
        $(".payway").find("span.icon-danxuanxuanzhong").removeClass("icon-danxuanxuanzhong").addClass("icon-danxuanmeixuan");
        $(this).find("span.iconfont").addClass("icon-danxuanxuanzhong").removeClass("icon-danxuanmeixuan");
    }
})
$(".jifenli .iconfont").on("tap", function () {
    $(this).toggleClass("icon-gouxuankuang").toggleClass("icon-yigouxuan")
})

function initView(data) {
    appcan.shoporder({
        selector: ".shopcarUl",
        data: data.GoodCartViewList
    });
    if (data.baoyou) {
        $(".youfei").html("包邮");
    }
    else {
        $(".youfei").html("￥" + data.LogisticsPrice);
    }
    if (data.huodong) {
        $(".huodongli").removeClass("uhide").find(".huodong").html(data.huodong);
    }
    //if (data.integralMoney > 0) {
    //    $(".jifenli").removeClass("uhide").find("var").eq(0).html(data.integral);
    //    $(".jifenli").removeClass("uhide").find("var").eq(1).html("￥" + data.integralMoney.toFixed(2));
    //}
    $(".shangpinzongji").html("￥" + data.shangpinzhongji.toFixed(2));
    $(".zhifuzongfeiyong").html("￥" + data.zhifuzongfeiyong.toFixed(2));
    $(".orderpage").removeClass("uhide");
}