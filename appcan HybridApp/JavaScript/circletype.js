appcan.define("circleType", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-id="<%=data.id%>" >\
                <%=data.title%>\
            </li>';

        self.template = model;
        self._template = appcan.view.template(self.template);
        self.ele = $(self.option.selector);
        self.set(self.option.data);


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

            self.ele.find("li").on("tap", function (event) {
                $(this).attr("data-id")
                self.switchover($(this).attr("data-index"))

            });
            self.switchover(0)

        },
        switchover: function (index) {
            this.ele.find("li").removeClass("focus").eq(index).addClass("focus")
            this.option.childCircleList.clear();
            this.option.childCircleList.set(this.option.data[index].list);

        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
