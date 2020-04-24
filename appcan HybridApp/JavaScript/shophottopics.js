appcan.define("shophottopics", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var modelHome =
            '<h2>热门专题</h2> \
            <p  id="<%=data.id%>">\
               <img src="StyleSheet/white.png" data-original="<%=data.img%>" />\
            </p>';
        var model =
    '<h2>热门专题</h2> \
            <p  id="<%=data.id%>">\
               <img src="StyleSheet/white.png" data-original="<%=data.img%>" />\
            </p>';
        self._template = appcan.view.template(model);
        self._templateHome = appcan.view.template(modelHome);

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.add(self.option.data, self.option.index);
        }
        self.initEvent();
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            for (var i in data) {
                if (self.home) {
                    var ele = $(this._templateHome({
                        data: data[i],
                        index: i
                    }));
                    self.ele.append(ele);
                }
                else {
                    var ele = $(this._template({
                        data: data[i],
                        index: i
                    }));
                    self.ele.append(ele);
                }
            }
        },
        initEvent: function () {
            var self = this;
            self.ele.on("tap", "img", function () {
                console.log($(this).attr("id"))
                appcan.openWinWithUrl("shopdetail", "shopdetail.html", 2, 0, 400)

            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
