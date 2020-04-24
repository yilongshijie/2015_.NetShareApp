function refresh(unfresh) {
    $(".page").addClass("uhide");

    var type =  window.localStorage.getItem("registlogin");
    if (!unfresh) {
        $(".mima,.yanzhengma,.tel").find("input[type=text],input[type=password]").val("");

    } else if (type == "forget") {
        $(".mima").find("input[type=text],input[type=password]").val("");
    }
    if (type == "regist") {
        $(".nav-center").html("注册");
        $(".mima").show();
        $(".tel").show();
        $(".yanzhengma").show();
        $("#submit").html("下一步");
        $(".yiyouzhanghao").show();
        $(".meiyouzhanghao").hide();
        $(".xingbie").hide();
    }
    else if (type == "login") {
        $(".nav-center").html("登录");
        $(".mima").show();
        $(".tel").show();
        $(".yanzhengma").hide();
        $("#submit").html("登录");
        $(".yiyouzhanghao").hide();
        $(".meiyouzhanghao").show();
        $(".xingbie").hide();
    }
    else if (type == "forget") {
        $(".nav-center").html("重置密码");
        $(".mima").hide();
        $(".tel").show();
        $(".yanzhengma").show();
        $("#submit").html("确认");
        $(".yiyouzhanghao").show();
        $(".meiyouzhanghao").hide();
        $(".xingbie").hide();
    }
    else if (type == "chongzhimima") {
        $(".nav-center").html("重置密码");
        $(".mima").show();
        $(".tel").hide();
        $(".yanzhengma").hide();
        $("#submit").html("重置");
        $(".yiyouzhanghao").hide();
        $(".meiyouzhanghao").hide();
        $(".xingbie").hide();
    }
    else if (type == "xuanzhexingbie") {
        $(".nav-center").html("选择性别");
        $(".mima").hide();
        $(".yanzhengma").hide();
        $("#submit").html("注册");
        $(".yiyouzhanghao").hide();
        $(".meiyouzhanghao").hide();
        $(".tel").hide();
        $(".xingbie").show();
    }

    $(".page").removeClass("uhide");
}
if (window.localStorage.getItem("registlogin") == null) {
    window.localStorage.setItem("registlogin", "login");
}
refresh();
$(".yiyouzhanghao span").on("tap", function () {
    window.localStorage.setItem("registlogin", "login");
    refresh();
})
$(".meiyouzhanghao span").on("tap", function () {
    window.localStorage.setItem("registlogin", "regist");
    refresh();
})
$(".meiyouzhanghao a").on("tap", function () {
    window.localStorage.setItem("registlogin", "forget");
    refresh();
})
$(".yanzhengma input[type=button]").on("tap", function () {
    var self = this;
    if ($(this).hasClass("disable")) return;
    if (!telcheck()) return;
    $(this).addClass("disable");
    appcan.request.ajax({
        url: url + "/user/getyanzhengma",
        type: "POST",
        data: { tel: $("input[name=tel]").val() },
        dataType: "json",
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.error) {
                appcan.window.openToast(data.error, 2500, 5, 0)
            }
        },
        error: function () {
            appcan.window.openToast("请求失败", 2500, 5, 0)
        }
    })
    var time = 60;
    var a = setInterval(function () {
        time--;
        if (time == 0) {
            clearInterval(a);
            $(self).val("验证码").removeClass("disable");
            return;
        } else {
            $(self).val(time + "秒");
        }
    }, 1000);

})
$("#submit").on("tap", function () {
    var type = window.localStorage.getItem("registlogin");
    switch (type) {
        case "login":
            login();
            break;
        case "regist":
            nextRegist();
            break;
        case "forget":
            forget();
            break;
        case "chongzhimima":
            chongzhimima();
            break;
        case "xuanzhexingbie":
            regist();
            break;
    }
})

$(".xingbie").on("tap", "span", function () {
    if ($(this).html() == "男") {
        $(".xingbie").addClass("nan").removeClass("nv");
    }
    else if ($(this).html() == "女") {
        $(".xingbie").addClass("nv").removeClass("nan");
    }
})



function login() {
    if (!telcheck()) return;
    if (!passwordcheck()) return;
    appcan.request.ajax({
        url: url + "/user/login",
        type: "post",
        data: { tel: $("input[name=tel]").val(), password: $("input[name=password]").val() },
        dataType: "json",
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.error) {
                appcan.window.openToast(data.error, 2500, 5, 0)
                window.localStorage.setItem("registlogin", "login");
                refresh(true);
                return;
            }
            pass(data);
        },
        error: function () {

            appcan.window.openToast("请求失败", 2500, 5, 0)
        }
    })
}
function nextRegist() {
    if (!telcheck()) return;
    if (!passwordcheck()) return;
    if (!yanzhengmacheck()) return;
    window.localStorage.setItem("registlogin", "xuanzhexingbie");
    refresh(true);
}
function regist() {
    window.localStorage.removeItem("registlogin");
    appcan.request.ajax({
        url: url + "/user/regist",
        type: "post",
        data: {
            tel: $("input[name=tel]").val(),
            password: $("input[name=password]").val(),
            yanzhengma: $("input[name=yzmtext]").val(),
            xingbie: $(".xingbie").hasClass("nan") ? "男" : "女"
        },
        dataType: "json",
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.error) {
                appcan.window.openToast(data.error, 2500, 5, 0)
                window.localStorage.setItem("registlogin", "regist");
                refresh(true);
                return;
            }
            pass(data);

        },
        error: function () {

            appcan.window.openToast("请求失败", 2500, 5, 0);
            window.localStorage.setItem("registlogin", "regist");
            refresh(true);
        }
    })
}
function forget() {
    if (!telcheck()) return;
    if (!yanzhengmacheck()) return;
    appcan.request.ajax({
        url: url + "/user/forget",
        type: "post",
        data: {
            tel: $("input[name=tel]").val(),
            yanzhengma: $("input[name=yzmtext]").val()
        },
        dataType: "json",
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.error) {
                appcan.window.openToast(data.error, 2500, 5, 0)
                return;
            }
            window.localStorage.setItem("registlogin", "chongzhimima");
            refresh(true);
        },
        error: function () {
            appcan.window.openToast("请求失败", 2500, 5, 0)
        }
    })
}
function chongzhimima() {
    if (!telcheck()) return;
    if (!yanzhengmacheck()) return;
    if (!passwordcheck()) return;
    appcan.request.ajax({
        url: url + "/user/chongzhimima",
        type: "post",
        dataType: "json",
        data: {
            tel: $("input[name=tel]").val(),
            password: $("input[name=password]").val(),
            yanzhengma: $("input[name=yzmtext]").val()
        }, 
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.error) {
                appcan.window.openToast(data.error, 2500, 5, 0);
                window.localStorage.setItem("registlogin", "forget");
                refresh(true);
                return;
            }
            pass(data);

        },
        error: function () {

            appcan.window.openToast("请求失败", 2500, 5, 0)
        }
    })

}
function telcheck() {
    var tel = $("input[name=tel]").val();
    var Reg = !!tel.match(/^(0|86|17951)?(1[234578])[0-9]{9}$/);
    if (Reg == false) {
        appcan.window.openToast("手机号码不正确", 2500, 5, 0)
        return false;
    }
    return true;
}
function passwordcheck() {
    var password = $("input[name=password]").val();
    if (password.length < 6) {
        appcan.window.openToast("密码长度不能小于6位", 2500, 5, 0);
        return false;
    }
    return true;
}
function yanzhengmacheck() {
    var yzmtext = $("input[name=yzmtext]").val();
    var Reg = yzmtext.match(/^[0-9]+$/);
    if (!Reg) {
        appcan.window.openToast("验证码不正确", 2500, 5, 0)
        return false;
    }
    return true;
}

function pass(data) {
    var loginid = {
        authentication_token: data.authentication_token,
        authentication_userID: data.authentication_userID,
        authentication_time: data.authentication_time
    }
    if (localStorage.getItem("cache_shopcar") != null) {
        appcan.request.ajax({
            url: url + "/GoodCart/SetGoodCartLocalStorage",
            dataType: "json",
            type: "Post",
            data: $.extend({ cache_shopcar: localStorage.getItem("cache_shopcar") }, loginid),
            success: function (data) {
            },
            error: function () {
            }
        })
    }
    if (localStorage.getItem("cache_orderIDs") != null) {
        appcan.request.ajax({
            url: url + "/Order/SetOrderList",
            dataType: "json",
            type: "Post",
            data: $.extend({ orderIDs: localStorage.getItem("cache_orderIDs") }, loginid),
            success: function (data) {
            },
            error: function () {
            }
        })
    }

    window.localStorage.clear();
    window.localStorage.setItem("authentication_token", data.authentication_token);
    window.localStorage.setItem("authentication_userID", data.authentication_userID);
    window.localStorage.setItem("authentication_userType", data.authentication_userType);
    window.localStorage.setItem("authentication_time", data.authentication_time);
    appcan.window.evaluatePopoverScript("root", "content_0", 'reload();xiaoxiloop();');
    appcan.window.evaluatePopoverScript("root", "content_3", 'reload();xiaoxiloop();');
    appcan.window.evaluatePopoverScript("root", "content_4", 'reload();xiaoxiloop();');

    appcan.window.evaluatePopoverScript("root", "content_2", 'xiaoxiloop();');

    appcan.window.close(0, 0);

}
