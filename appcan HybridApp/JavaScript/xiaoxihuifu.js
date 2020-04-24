appcan.define("xiaoxihuifu", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model =
         '<div class="circlePostItem backgoundColor  huifu  " data-id="<%=data.UserLetterID%>" data-userid="<%=data.InitiativeUserID%>" >\
            <div class="people <%= data.self %>">\
                 <div class="touxiang"><img src="<%=data.User.HeadPortrait%>"  data-userid="<%=data.User.UserID%>"/> </div> \
                <div class="divright">\
                    <p class="pleft">\
                        <span class="name"><%=data.User.NickName%></span>\
                        <span class="level"><var><%=data.User.UserExtend.ExperienceLevel%></var><%=data.User.UserExtend.ExperienceName%></span><br/>\
                        <span class="time"><%=new Date(data.CreateTime ).Format("yyyy-MM-dd hh:mm:ss")%></span>\
                    </p>\
                </div>\
            </div>\
            <div class="article " <% if(data.CirclePostID!=null){%> \
        data-circlepostid = <%=data.CirclePostID%> <%}%> >\
                <%=data.Text%>\
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
                if ((data[i].Type & 4) > 0)
                {
                    $(".huiFuKuang").removeClass("uhide");
                }
                else
                {
                    $(".huiFuKuang").addClass("uhide");
                }
                if (data[i].InitiativeUserID == localStorage.getItem("authentication_userID"))
                {
                    data[i].self = "self";
                }
                else
                {
                    data[i].self = ""
                }
         
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
