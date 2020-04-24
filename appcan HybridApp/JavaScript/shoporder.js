appcan.define("shoporder", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<li data-index="<%=index%>" class="backgoundColor">\
                <p class="shopcarImg"><img src="<%=data.img%>" /></p>\
                <div class="shopcarCenter">\
                    <p class="title"><%=data.title%></p>\
                    <p class="guigeText"><%for(var i in data.guige){%><var><%=data.guige[i]%></var> <%}%></p>\
                </div>\
                <div class="shopcarPrice">\
                    <var data-price="<%=data.price%>" data-num="<%=data.num%>"  data-LogisticsPrice="<%=data.LogisticsPrice%>"  data-baoyou="<%=data.baoyou%>" data-integral="<%=data.integral%>">￥<%=data.price%></var>\
                    <p>\
                        <b >×<%=data.num%></b>\
                    </p>\
                </div>\
            </li>'


        self.template = model;
        self._template = appcan.view.template(self.template);

        self.ele = $(self.option.selector);
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
        } 
    }
    module.exports = function (option) {
        return new view(option);
    };


})
