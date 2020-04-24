appcan.define("shoporderwuliu", function ($, exports, module) {
    function view(option) {
        appcan.extend(this, appcan.eventEmitter);
        var self = this;
        self.option = $.extend({
            selector: null,
            empty: null,
            index: 0
        }, option, true);
        var model =
            '<li class="backgoundColor"  data-index="<%=index%>"  >\
              <span><%=data.time%></span><var><%=data.context%><var/>\
            </li>';
        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
        self.empty = $(self.option.empty);
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
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
