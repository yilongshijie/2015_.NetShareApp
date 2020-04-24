window.url = "http://www.xfx.ren:8803"
function islogin() {
    if (localStorage.getItem("authentication_token") != null && localStorage.getItem("authentication_userID") != null && localStorage.getItem("authentication_time") != null) {
        return true;
    }
    else {
        window.localStorage.setItem("registlogin", "login")
        appcan.openWinWithUrl("registlogin", "registlogin.html", 2, 0, 400);
        return false;
    }

}

function sendtoken() {
    return {
        authentication_token: localStorage.getItem("authentication_token"),
        authentication_userID: localStorage.getItem("authentication_userID"),
        authentication_time: localStorage.getItem("authentication_time")
    }
}

function logout() {
    localStorage.clear();
    appcan.window.evaluateScript("root", 'window.conArray[3]=false');
    appcan.window.evaluatePopoverScript("root", "content_3", 'uexWindow.closePopover("content_3")');
    appcan.window.evaluatePopoverScript("root", "content_0", 'reload();');
    appcan.window.evaluatePopoverScript("root", "content_4", 'reload()');
}

Date.prototype.Format = function (fmt, agoif) { //author: meizz
    if (agoif) {
        var ago = this.ago();
        if (ago) {
            return ago;
        }
    }

    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

Date.prototype.ago = function () {
    var past = this.getTime(),
        now = new Date().getTime(),
        diffValue = now - past,
        result = '',
        minute = 1000 * 60,
        hour = minute * 60,
        day = hour * 24,
        month = day * 30,
        year = month * 12;

    var _year = diffValue / year,
        _month = diffValue / month,
        _week = diffValue / (7 * day),
        _day = diffValue / day,
        _hour = diffValue / hour,
        _min = diffValue / minute;

    if (_year >= 1) result = false; // parseInt(_year) + "年前";
    else if (_month >= 1) result = false; //parseInt(_month) + "个月前";
    else if (_week >= 1) result = parseInt(_week) + "周前";
    else if (_day >= 1) result = parseInt(_day) + "天前";
    else if (_hour >= 1) result = parseInt(_hour) + "小时前";
    else if (_min >= 1) result = parseInt(_min) + "分钟前";
    else result = "刚刚";
    return result;
}