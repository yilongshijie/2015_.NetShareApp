appcan.define("circleDetailTopList", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-id="<%=data.CirclePostID%>" class="title" >\
                <span class="tag blue">置顶</span>   <%=data.Title%>\
            </li>';

        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
       
        if (self.option.data) {
            self.add(self.option.data);
        }
        self.initEvent();
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            if (data.length > 0)
            {
                self.ele.removeClass("uhide");
            }
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                self.ele.find("ul").append(ele)
            }

        },
        initEvent: function () {
            var self = this;
            $(self.ele).on("tap", "li", function (event) {
                localStorage.setItem("request", $(this).attr("data-id"));
                appcan.openWinWithUrl("circlepost", "circlepost.html", 2, 0, 400)
            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
