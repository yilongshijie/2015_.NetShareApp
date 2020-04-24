appcan.define("shoplist", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<div data-index="<%=index%>" data-id="<%=data.GoodID%>" class="backgoundColor">\
                <div>\
                   <img class="lazy" src="StyleSheet/white.png" data-original="<%=data.Image%>"   />\
                </div>\
                <h2><%=data.Title%></h2>\
                <h3><%=data.SubTitle%></h3>\
                <p>\
                    <span>￥<%=data.RealPrice%></span>\
                    <span>销量:<%=data.SalesVolume%></span>\
                </p>\
            </div>';

        self._template = appcan.view.template(model);

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.add(self.option.data, self.option.index);
        }
        self.initEvent();
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            if (data.length > 0) {
                self.ele.removeClass("uhide");
                $(".empty").addClass("uhide");
            }
            else {
                self.ele.addClass("uhide");
                $(".empty").removeClass("uhide");
            }

            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                this.__append(ele)
            }

        },
        __append: function (ele) {
            var self = this;
            var element1 = $(self.option.selector + " li.li1>div");
            var element2 = $(self.option.selector + " li.li2>div")
            if (element1.length > element2.length) {
                $(self.option.selector + " li.li2").append(ele)
            }
            else {
                $(self.option.selector + " li.li1").append(ele)
            }
            //if (element1.length == 0) {
            //    $(self.option.selector + " li.li1").append(ele)
            //    return;
            //}
            //if (element2.length == 0) {
            //    $(self.option.selector + " li.li2").append(ele)
            //    return;
            //}
            //var li1offset = element1.last().offset()
            //var li2offset = element2.last().offset()
            //var li1Y = li1offset.top + li1offset.height;
            //var li2Y = li2offset.top + li2offset.height;
            //if (li1Y > li2Y) {

            //    $(self.option.selector + " li.li2").append(ele);
            //}
            //else {
            //    $(self.option.selector + " li.li1").append(ele);
            //}
        },
        initEvent: function () {
            var self = this;
            self.ele.on("tap", "li>div", function () {

                localStorage.setItem("request", $(this).attr("data-id"))

                appcan.openWinWithUrl("shopdetail", "shopdetail.html", 2, 0, 400)

            })
        },
        clear: function () {
            $(this.option.selector + " li>div").remove()

        }


    }
    module.exports = function (option) {
        return new view(option);
    };


})
