appcan.define("shopordermy", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-GoodID="<%=data.GoodID%>" class="backgoundColor">\
                <p class="shopcarImg"><img src="<%=data.ImageHref%>" /></p>\
                <div class="shopcarCenter">\
                    <p class="title"><%=data.Title%></p>\
                    <p class="guigeText"><var><%=data.Specification%></var></p>\
                </div>\
                <div class="shopcarPrice">\
                    <var data-price="<%=data.price%>"   >￥<%=data.TotalPrice%></var>\
                    <p>\
                        <b >×<%=data.num%></b>\
                    </p>\
                </div>\
            </li>'


        self.template = model;
        self._template = appcan.view.template(self.template);

        self.ele = $(self.option.selector);
        self.initEvent();
        if (self.option.data) {
            self.set(self.option.data);
        }
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i,
                    option: self.option
                }));
                this.ele.append(ele);
            }
        },
        initEvent: function () {
            var self = this;
            self.ele.on("tap", "li", function () {

                localStorage.setItem("request", $(this).attr("data-GoodID"))

                appcan.openWinWithUrl("shopdetail", "shopdetail.html", 2, 0, 400)

            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
