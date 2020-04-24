appcan.define("circlePost", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
         '<div class="circlePostItem backgoundColor <%if(data.louzhu==null){%>huifu<%}%>" data-id="<%=data.CirclePostID%>" <%if(data.louzhu==null){%>data-CirclePostReplyID="<%=data.CirclePostReplyID%>"  data-floor="<%=data.Floor%>"<%}%>  data-userid="<%=data.UserID%>"  >\
            <div class="people">\
                 <div class="touxiang"><img src="StyleSheet/white.png" data-original="<%=data.User.HeadPortrait%>"  data-userid="<%=data.UserID%>" data-type="<%=data.User.Type%>"/> </div> \
                <div class="divright">\
                    <p class="pleft">\
                        <span class="name"><%=data.User.NickName%></span>\
                        <%if(data.louzhu){%><span class="louzhu">楼主</span><%}%>\
                        <span class="level"><var><%=data.User.UserExtend.ExperienceLevel%></var><%=data.User.UserExtend.ExperienceName%></span><br />\
                        <span class="time"><%=new Date(data.CreateTime).Format("yyyy-MM-dd hh:mm:ss")%></span>\
                    </p>\
                    <p class="pright">\
                        <%if(data.Floor){%><span class="louceng"><%=data.Floor%>F</span><%}%>\
                    </p>\
                </div>\
            </div>\
            <div class="article ">\
               <div> <%if(data.Title) {print(data.Title)}%></div>\
                <%=data.Detail%>\
                <% if( data.ImgList){ for(var index in data.ImgList.split(",")){ %>\
                 <img   src="StyleSheet/white.png" data-original="<%=data.ImgList.split(",")[index]%>"/><%}}%>\
   <ul class="replayChild">\
                <%if(data.CirclePostReplyChild.length>0){%>\
                 <%for(var indexReplyChild in data.CirclePostReplyChild){   %>\
                         <li>\
                         <span class="name" data-userid=<%= data.CirclePostReplyChild[indexReplyChild].User.UserID%>><%= data.CirclePostReplyChild[indexReplyChild].User.NickName%>：</span>\
 <div class="article ">\
              <%=data.CirclePostReplyChild[indexReplyChild].Detail%>\
                <% if( data.CirclePostReplyChild[indexReplyChild].ImgList){ for(var indexReplyChild1 in data.CirclePostReplyChild[indexReplyChild].ImgList.split(",")){ %>\
                 <img   src="StyleSheet/white.png" data-original="<%=data.CirclePostReplyChild[indexReplyChild].ImgList.split(",")[indexReplyChild1]%>"/><%}}%>\
           </div>\
                         </li>\<%}%>\
                 <%}%>\
  </ul>\
            </div>\
        </div>';

        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
        self.ele.removeClass("uhide");
        if (self.option.data) {
            self.add(self.option.data);
        }
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                self.ele.append(ele)
            }

        }
    }
    module.exports = function (option) {
        return new view(option);
    };


})
