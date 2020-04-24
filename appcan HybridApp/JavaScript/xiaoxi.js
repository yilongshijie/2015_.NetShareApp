appcan.define("xiaoxi", function ($, exports, module) {
    function view(option) {
        var nav_right = option.nav_right;
        nav_right.append('<span class="iconfont icon-zhengguiicon40 "></span>');
        nav_right.on("tap", function () {
            if (!islogin()) return;
            appcan.openWinWithUrl("xiaoxilist", "xiaoxilist.html", 2, 0, 400)
        })
 
        window.xiaoxiloop = function () {
            clearTimeout(window.Interval)
            if (localStorage.getItem("authentication_token") == null) {
                nav_right.find("var").remove();
                return;
            }
            appcan.request.ajax({
                url: url + "/UserLetter/GetCount",
                type: "POST",
                dataType: "json",
                data: $.extend(sendtoken()),
                success: function (data) {
                    window.Interval = setTimeout(window.xiaoxiloop, 10000)
                    if (data) {
                        if (data.lettercount)
                        {
                            nav_right.find("var").remove();
                            nav_right.append('<var class="xiaoxiNum">' + data.lettercount + '</var> ');
                        }
                        if (data.lettercount == 0)
                        {
                            nav_right.find("var").remove();
                        }
                        localStorage.setItem("authentication_userType", data.userType);
                        localStorage.setItem("authentication_userState", data.userState);
                        localStorage.setItem("authentication_userLevel", data.userLevel);
                        localStorage.setItem("authentication_userBanned", data.userBanned);

                        if (data.userState == 0) {
                            logout();
                            appcan.window.openToast("不好意思，该账号被冻结了...", 2500, 5, 0);
                        }
                    }
                    else {
                        nav_right.find("var").remove();
                    }
                   

                },
                error: function () {
          
                    window.Interval = setTimeout(window.xiaoxiloop, 10000)
                }
            })
        }
        window.xiaoxiloop();
    };

    module.exports = function (option) {
        return new view(option);
    };
})
