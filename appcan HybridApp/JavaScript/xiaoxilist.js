appcan.define("xiaoxilist", function ($, exports, module) {
    function view(option) {
        appcan.extend(this, appcan.eventEmitter);
        var self = this;
        self.option = $.extend({
            selector: null,
            empty:null,
            index: 0
        }, option, true);
        var model =
            ' <li data-index="<%=index%>" data-id="<%=data.UserLetterID%>"  class= "<% if(data.State&1){%>weidu<%}%>">\
              <img class="lievent"  src="StyleSheet/white.png" data-original="<%=data.User.HeadPortrait%>"/>\
            <p>\
                <span  class="lievent" ><%=data.Text%></span>\
                <span><b    class="lievent" style="float:left;color:#aaa;"><%=formant(data.Type)%></b><%=new Date(data.CreateTime ).Format("yyyy-MM-dd")%> &nbsp;<b class="iconfont icon-lajixiang"></b></span>\
            </p>\
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
                    index: i,
                    formant: function (type) {
                        if ((type & 8) > 0)
                            return "订单消息";
                        if ((type & 16) > 0)
                            return "系统消息";
                        return "私信";
                    }
                }));
                self.ele.append(ele);
            }
            if(data.length==0)
            {
                self.ele.hide();
                self.empty.removeClass("uhide");
            }
        } 
    }
    module.exports = function (option) {
        return new view(option);
    };


})
