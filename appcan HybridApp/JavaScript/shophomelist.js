appcan.define("shophomelist", function ($, exports, module) {
    function view(option) {
        var self = this;
        self.option = $.extend({
            selector: null,
            index: 0
        }, option, true);
        self.model =
            '<div class="shopHomeModel backgoundColor">\
            <h2 data-id="<%=data.GoodGategoryID%>"  data-Name="<%=data.Name%>"><%=data.Name%></h2>\
            <h3  data-id="<%=data.GoodGategoryID%>"  data-Name="<%=data.Name%>"><img src="StyleSheet/white.png" data-original="<%=data.Image%>"/></h3>\
            <h4>\
            <span class="flex2"><label><%=data.Flex2[0].Title%><br/><b><%=data.Flex2[0].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[0].Title%><br/><b><%=data.Flex1[0].Label%></b></label></span>\
            </h4>\
            <p class="p1">\
                <img class="img2" src="StyleSheet/white.png" data-original="<%=data.Flex2[0].Image%>" data-id="<%=data.Flex2[0].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[0].Image%>" data-id="<%=data.Flex1[0].GoodID%>"/>\
            </p>\
            <h4>\
            <span class="flex1"><label><%=data.Flex1[1].Title%><br/><b><%=data.Flex1[1].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[2].Title%><br/><b><%=data.Flex1[2].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[3].Title%><br/><b><%=data.Flex1[3].Label%></b></label></span>\
            </h4>\
            <p>\
<img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[1].Image%>"  data-id="<%=data.Flex1[1].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[2].Image%>"  data-id="<%=data.Flex1[2].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[3].Image%>"  data-id="<%=data.Flex1[3].GoodID%>"/>\
            </p>\
            </div>';
        self.modelNoFlex2 =
    '<div class="shopHomeModel backgoundColor">\
            <h2 data-id="<%=data.GoodGategoryID%>"  data-Name="<%=data.Name%>"><%=data.Name%></h2>\
            <h3  data-id="<%=data.GoodGategoryID%>"  data-Name="<%=data.Name%>"><img src="StyleSheet/white.png" data-original="<%=data.Image%>"/></h3>\
            <h4>\
            <span class="flex1"><label><%=data.Flex1[0].Title%><br/><b><%=data.Flex1[0].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[1].Title%><br/><b><%=data.Flex1[1].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[2].Title%><br/><b><%=data.Flex1[2].Label%></b></label></span>\
            </h4>\
            <p class="p1">\
<img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[0].Image%>"  data-id="<%=data.Flex1[0].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[1].Image%>" data-id="<%=data.Flex1[1].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[2].Image%>"  data-id="<%=data.Flex1[2].GoodID%>"/>\
            </p>\
            <h4>\
            <span class="flex1"><label><%=data.Flex1[3].Title%><br/><b><%=data.Flex1[3].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[4].Title%><br/><b><%=data.Flex1[4].Label%></b></label></span>\
            <span class="flex1"><label><%=data.Flex1[5].Title%><br/><b><%=data.Flex1[5].Label%></b></label></span>\
            </h4>\
            <p>\
<img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[3].Image%>"  data-id="<%=data.Flex1[3].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[4].Image%>"  data-id="<%=data.Flex1[4].GoodID%>"/><img class="img1" src="StyleSheet/white.png" data-original="<%=data.Flex1[5].Image%>"  data-id="<%=data.Flex1[5].GoodID%>"/>\
            </p>\
            </div>';
   
        

        self.ele = $(self.option.selector);
        if (self.option.data) {
            self.add(self.option.data, self.option.index);
        }
        self.initEvent();
    };
    view.prototype = {
        add: function (data) {
            var self = this;
            if (data[0].Flex2)
            {
                self._template = appcan.view.template(self.model);
            }
            else
            {
                self._template = appcan.view.template(self.modelNoFlex2);
            }
          
            for (var i in data) {
                var ele = $(this._template({
                    data: data[i],
                    index: i
                }));
                this.ele.append(ele);
            }
        },
        initEvent: function () {
            var self = this;
            self.ele.on("tap", ".shopHomeModel p >img", function () {
 
                localStorage.setItem("request", $(this).attr("data-id"))
                appcan.openWinWithUrl("shopdetail", "shopdetail.html", 2, 0, 400)
            })

            self.ele.on("tap", ".shopHomeModel h2 ,  .shopHomeModel h3", function () {
                localStorage.setItem("request", "{id:" + $(this).attr("data-id") + ",text:'"+$(this).attr("data-name")+"'}")
                appcan.openWinWithUrl("shoplist", "shoplist.html", 2, 0, 400)
            })



        }

    }
    module.exports = function (option) {
        return new view(option);
    };


})
