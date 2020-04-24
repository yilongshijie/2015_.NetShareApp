appcan.define("mycirclelist", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-id="<%=data.CirclePostID%>"  data-state="<%=data.State%>"  class ="backgoundColor  >\
                <p class="title ut-s"><%=data.Title%></p>\
                <p class="foot sc-text">\
                    <span clas="left">\
                        <em><%=data.User.NickName%></em>\
                        <em class="tag  <%if(data.Gender=="男"){%>blue<%}else{%>pink<%}%>" ><%=data.User.UserExtend.ExperienceLevel%></em><em><%=data.User.Location%></em>\
                    </span>\
                    <span name="right">\
                        <em><%=new Date(data.CreateTime ).Format("yyyy-MM-dd")%></em>\
                        <em><var class="iconfont icon-huifu"></var> <%=data.ReplyNum%></em>\
                       <%if(shanchu){%> <b class="iconfont icon-lajixiang" style="padding: 0 0.3em;"></b><%}%>\
                    </span>\
                </p>\
                <p class="tx-r sc-text-active"><%=data.Detail%></p>\
              </li>'

        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
        self.ele.removeClass("uhide");
        if (self.option.data) {
            self.add(self.option.data);
        }
        self.initEvent();
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            if (data.length == 0)
            {
                $(".empty").removeClass("uhide");
            }
            else
            {
                $(".empty").addClass("uhide");
                
            }

            for (var i in data) {
 
                var ele = $(this._template({
                    data: data[i],
                    index: i,
                    shanchu: self.option.shanchu
                }));
                self.ele.find("ul").append(ele)
            }

        },
        initEvent: function () {
            var self = this;
            $(self.ele).on("tap", "li", function (event) {
                if ($(this).attr("data-state") == "0") {
                    
                    appcan.window.openToast("已经删除了哦", 2500, 5, 0);
                    return;
                }
                localStorage.setItem("request", $(this).attr("data-id"));
                appcan.openWinWithUrl("circlepost", "circlepost.html", 2, 0, 400)
            })
            $(self.ele).on("tap", "li b.icon-lajixiang", function (event) {
                if (confirm("确定删除该帖子？")) {
                    var li = $(this).parentsUntil("ul").filter("li");
                    li.remove();
                    appcan.request.ajax({
                        url: url + "/CirclePost/Delete",
                        type: "POST",
                        dataType: "json",
                        data: $.extend(sendtoken(), { id: li.attr("data-id") }),
                        success: function (data) {

                        },
                        error: function () {
                        }
                    })
                    return false;
                } 
                return false;
             
            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
