appcan.define("shoporderlist", function ($, exports, module) {
    function view(option) {
        appcan.extend(this, appcan.eventEmitter);
        var self = this;
        self.option = $.extend({
            selector: null,
            empty: null,
            index: 0
        }, option, true);
        var model =
            '<li class="backgoundColor"  data-index="<%=index%>" data-id="<%=data.OrderID%>"  data-state="<%=data.State%>" >\
                <p><span>订单号:<var><%=data.OrderID%></var></span><b class="colorUseBc-head"><%=data.StateText%></b></p>\
                <div>\
                    <img src="<%=data.Image%>" data-GoodID="<%=data.GoodID%>"  />\
                    <p>\
                       <span  class="eventspan"> 订单时间：<var ><%=data._createTime%></var></span><br />\
      <span   class="eventspan">数量：<var  ><%=data.Num%></var></span><br />\
                        <span class="colorUseBc-head eventspan">订单总金额：<var  ><%=data.PaymentPrice%></var></span>\
    <% if(new Date(Date.now()).diff(new Date(data.CreateTime))<50 && (data.State&4)>0 && (data.State&16)==0 ){%>\
        <span class="pingjia uhide"  data-id="<%=data.OrderID%>"" >去评价</span>\
    <% }  %>\
                    </p>\
                </div>\
            </li>';
        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
        self.empty = $(self.option.empty);
        if (self.option.data) {
            self.set(self.option.data);
        }
        self.initEvent();

    };
    view.prototype = {
        set: function (data) {
            var self = this;
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                self.ele.append(ele);
            }
            if (data.length == 0) {

                self.empty.removeClass("uhide");
            }
            else {
                self.ele.removeClass("uhide");
            }
        },
        initEvent: function () {
            var self = this;
            $(self.ele).on("tap", "li", function (event) {

                $(this).attr("data-id")

            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
