appcan.define("experiencelist", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<div data-index="<%=index%>" data-GoodExperienceID="<%=data.GoodExperienceID%>"  \
        data-GoodID="<%=data.GoodID%>" class="backgoundColor">\
                <div>\
                    <img class="lazy" src="StyleSheet/white.png" data-original="<%=data.Image%>"   />\
                </div>\
                <h2><%=data.Title%></h2>\
                <% if(data.State){ if(data.State==1){%> <h3>未审核</h3> <%};\
if(data.State==2){%> <h3>审核通过</h3> <%};\
if(data.State==8){%> <h3>管理员删除</h3> <%};}%>\
                <p>\
                    <span><img class="lazy" src="StyleSheet/white.png" data-original="<%=data.User.HeadPortrait%>"/> <%=data.User.NickName%></span>\
                    <span> <b class="iconfont icon-huifu"></b> <%=data.ReplyNum%></span>\
                </p>\
            </div>';


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
                self.__append(ele)

            }
        },
        __append: function (ele) {
            var self = this;
            var element1 = $(self.option.selector + " li.li1>div");
            var element2 = $(self.option.selector + " li.li2>div")

            if (element1.length > element2.length)
            {
                $(self.option.selector + " li.li2").append(ele)
            }
            else
            {
                $(self.option.selector + " li.li1").append(ele)
            }
 
        }

    }
    module.exports = function (option) {
        return new view(option);
    };


})
