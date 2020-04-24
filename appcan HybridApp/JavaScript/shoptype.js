appcan.define("shopType", function ($, exports, module) {
    function view(option) {
        appcan.extend(this, appcan.eventEmitter);
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            ' <li data-index="<%=index%>" data-id="<%=data.GoodGategoryID%>"  data-name="<%=data.Name%>">\
               <img  src="StyleSheet/white.png" data-original="<%=data.Image%>"  />\
                <div>\
                    <h3><%=data.Name%></h3>\
                    <h6 class="sc-text"><%=data.Describe%></h6>\
                </div> \
                <p class="iconfont icon-jinru"></p>\
            </li>'
        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.set(self.option.data);
        }
        self.initEvent();
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            var container = $('<ul></ul>');
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                container.append(ele);
            }

            this.ele.append(container);
        },
        initEvent: function () {
            var self = this;
            $(self.ele).on("tap", "li", function (event) {
                localStorage.setItem("request", "{id:" + $(this).attr("data-id") + ",text:'" + $(this).attr("data-name") + "'}")
                appcan.openWinWithUrl("shoplist", "shoplist.html", 2, 0, 400)

            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
