appcan.define("circlePostReplayChild", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null
        }, option, true);
        var model = '<li>\
                         <span class="name" data-userid=<%= data.User.UserID%>><%= data.User.NickName%>：</span>\
 <div class="article ">\
              <%=data.Detail%>\
                <% if( data.ImgList){ for(var indexReplyChild1 in data.ImgList.split(",")){ %>\
                 <img   src="<%=data.ImgList.split(",")[indexReplyChild1]%>"/><%}}%>\
           </div></li>';

        self._template = appcan.view.template(model);
    
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            var ele = $(this._template({
                data: data
            }));
            $(self.option.selector).append(ele)
        }

    }

    module.exports = function (option) {
        return new view(option);
    };
})
