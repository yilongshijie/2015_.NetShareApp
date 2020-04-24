appcan.define("shoppingjia", function ($, exports, module) {
    function view(option) {
        appcan.extend(this, appcan.eventEmitter);
        var self = this;
        self.option = $.extend({
            selector: null,
            empty: null,
            index: 0
        }, option, true);
        var model =
            ' <li data-index="<%=index%>" data-id="<%=data.GoodID%>"  ><p>\
              <%=data.Detail%>\
            </p>\
           <p><%=data.User.NickName%> &nbsp; <%=new Date(data.Time ).Format("yyyy-MM-dd")%></p>\
            </li>';
        self._template = appcan.view.template(model);
        self.ele = $(self.option.selector);
 
    };
    view.prototype = {
        set: function (data) {
            var self = this;
            if (data.length  > 0) {
                self.ele.removeClass("uhide");
            }
            for (var i in data) {
           
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                self.ele.find("ul").append(ele);
            }
      
        } 
     
    }
    module.exports = function (option) {
        return new view(option);
    };


})
