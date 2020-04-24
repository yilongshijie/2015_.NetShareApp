appcan.define("shophot", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-state="<%=data.state%>" data-name="<%=data.label%>" >\
                <div  class=" <%=data.icon%>"></div>\
                <span class="ut-s "><%=data.label%> </span>\
            </li>'

        self._template = appcan.view.template(model);

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.set(self.option.data);
        }
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
            container.find("li").on("tap", function (event) {
                localStorage.setItem("request", "{state:" + $(this).attr("data-state") + ",text:'" + $(this).attr("data-name") + "'}")
                appcan.openWinWithUrl("shoplist", "shoplist.html", 2, 0, 400)

            });
            this.ele.append(container);
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
