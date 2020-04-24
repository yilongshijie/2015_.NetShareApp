appcan.define("circleList", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-id="<%=data.CircleTypeID%>" >\
                <img  src="StyleSheet/white.png" data-original="<%=data.Image%>" />\
                <div>\
                    <h3><%=data.Title%></h3>\
                    <h6 class="sc-text"><%=data.SubTitle%></h6>\
                </div>\
                <%if(option.hideAdd!=true){%><p>\
                     <a  class="colorUseBc-head <%if(data.State>0){%>uhide<%}%> "  >加入</a>\
                     <a  class="colorUserBc-bg <%if(data.State==0 ){%>uhide<%}%>" > 退出 </a>\
                </p><%}%>\
            </li>'

        self.template = model;
        self._template = appcan.view.template(self.template);

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.set(self.option.data);
        }
        self.initEvent();
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            self.ele.find("ul").remove();
            if (self.option.removeHide && data.length > 0) {
                $(self.option.removeHide).removeClass("uhide");
            }
            var container = $('<ul></ul>');
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i,
                    option: self.option
                }));
                container.append(ele);
            }

            self.ele.append(container);
        },
        clear: function () {
            this.ele.find("ul").remove();
        },
        initEvent: function () {
            var self = this;
            var selfTemp = this;
            if (self.option.lievent) {
                self.ele.on("tap", "li>div,li>img", function (event) {
                    localStorage.setItem("request", $(this).parent().attr("data-id"));
                    appcan.openWinWithUrl("circledetaillist", "circledetaillist.html", 2, 0, 400)
                });
            }

            self.ele.on("tap", "li a", function (event) {
                if (!islogin()) return;
                var self = this;
                appcan.request.ajax({
                    url: url + "/circle/AddMyCircle",
                    type: "post",
                    dataType: "json",
                    data: $.extend({ id: $(self).parent().parent().attr("data-id") }, sendtoken()),
                    success: function (data) {
                        data = eval("(" + data + ")");
                        if (data.message) {
                            if (selfTemp.option.lievent) {
                                
                                appcan.window.openToast($(self).parent().prev().find("h3").html() + " " + "添加成功", 2500, 5, 0);
                                $(self).hide();
                                $("#myCircleList ul").append($(self).parent().parent());
                                $("#myCircleTitle ,#myCircleList").removeClass("uhide");
                                if ($("#recommendCircleList li").length == 0) {
                                    $("#recommendCircleTitle ,#recommendCircleList").addClass("uhide");
                                }

                            }
                            else {
                                if (data.message == 2) {
                                    $(self).addClass("uhide").parent().find(".colorUserBc-bg").removeClass("uhide");
                                }
                                else {
                                    $(self).addClass("uhide").parent().find(".colorUseBc-head ").removeClass("uhide");

                                }
                                appcan.window.evaluatePopoverScript("root", "content_0", 'window.reload()');

                            }




                        }
                    },
                    error: function () {
                        
                        appcan.window.openToast("请求失败", 2500, 5, 0)
                    }
                })


                $(this).parent().parent().attr("data-id")
            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
