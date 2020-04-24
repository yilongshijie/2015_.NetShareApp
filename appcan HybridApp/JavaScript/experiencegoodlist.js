appcan.define("experiencegoodlist", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '  <img data-index="<%=index%>" data-GoodExperienceID="<%=data.GoodExperienceID%>"  \
        data-GoodID="<%=data.GoodID%>" src="StyleSheet/white.png" data-original="<%=data.Image%>"/>';

        self._template = appcan.view.template(model);

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.add(self.option.data, self.option.index);
        }
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            if (data.length > 0) {
                $(self.option.selector).removeClass("uhide");
            }
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                $(self.option.selector).find("div").append(ele)

            }
        }

    }
    module.exports = function (option) {
        return new view(option);
    };


})
