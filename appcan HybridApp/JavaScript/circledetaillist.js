appcan.define("circleDetailList", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
            '<li data-index="<%=index%>" data-pageIndex="<%=pageIndex%>" data-id="<%=data.CirclePostID%>" class ="backgoundColor <%= (data.State&8)?"tuijian":"" %>" >\
                <div class="title ut-s"><%=data.Title%></div>\
                <div class="label sc-text ut-s"><%=data.Detail%></div>\
                <%if(data.ImgList.length>0){%>\
                    <div class="imglist"><%for(var index in data.ImgList.split(",")){ if(index==3)break; %>\
                         <p data-original="<%= data.ImgList.split(",")[index]%>" style="background-image:url(StyleSheet/white.png)"></p>\<%}%>\
                    </div>\
                 <%}%>\
                <p class="foot sc-text">\
                    <span clas="left">\
                        <em><%=data.User.NickName%></em>\
                        <em class="tag  <%if(data.Gender=="男"){%>blue<%}else{%>pink<%}%>" ><%=data.User.UserExtend.ExperienceLevel%></em><em><%=data.Province%></em>\
                    </span>\
                    <span name="right">\
                        <em><%=new Date(data.CreateTime).Format("yyyy-MM-dd",true)%></em>\
                        <em><var class="iconfont icon-huifu"></var> <%=data.ReplyNum%></em>\
                    </span>\
                </p>\
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
        add: function (data,pageIndex) {
            var self = this;
            for (var i in data) {
        
                var ele = $(this._template({
                    data: data[i],
                    index: i,
                    pageIndex: pageIndex
                }));
                self.ele.find("ul").append(ele)
            }

        },
        initEvent:function(){
            var self = this;
            $(self.ele).on("tap","li",function (event) {
 
                localStorage.setItem("request", $(this).attr("data-id"));
                appcan.openWinWithUrl("circlepost", "circlepost.html", 2, 0, 400)
            })
        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
