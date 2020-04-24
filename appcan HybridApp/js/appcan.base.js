/*! appcan v0.1.18 |  from 3g2win.com */

;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:appcan 基础对象
    created:2014,08.18
    update:2013.08.22 by dushaobin


*/
/* global window */
; (function (global) {
    var appcan = {};
    var isUexReady = false;
    var readyQueue = [];
    var isAppcan = false;
    //appcan 相关模块
    appcan.modules = {};

    //获取唯一id
    var getUID = function () {
        var maxId = 65536;
        var uid = 0;
        return function () {
            uid = (uid + 1) % maxId;
            return uid;
        };
    }();

    //获取随机的唯一id，随机不重复，长度固定
    var getUUID = function (len) {
        len = len || 6;
        len = parseInt(len, 10);
        len = isNaN(len) ? 6 : len;
        var seed = '0123456789abcdefghijklmnopqrstubwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ';
        var seedLen = seed.length - 1;
        var uuid = '';
        while (len--) {
            uuid += seed[Math.round(Math.random() * seedLen)]
        }
        return uuid;
    }

    //是否是函数
    var isFunction = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Function]';
    };
    //是否是字符串
    var isString = function (obj) {
        return Object.prototype.toString.call(obj) === '[object String]';
    };
    //是否是object对象
    var isObject = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Object]';
    };
    //是否是数组
    var isArray = function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    };
    //是否是window对象
    var isWindow = function (obj) {
        return obj != null && obj == obj.window;
    };
    //是否是纯对象
    var isPlainObject = function (obj) {
        return isObject(obj) && !isWindow(obj) && Object.getPrototypeOf(obj) == Object.prototype;
    };
    //扩展对象
    var extend = function (target, source, deep) {
        var key = null;
        for (key in source) {
            if (deep && (isPlainObject(source[key]) || isArray(source[key]))) {
                if (isPlainObject(source[key]) && !isPlainObject(target[key])) {
                    target[key] = {};
                }
                if (isArray(source[key]) && !isArray(target[key])) {
                    target[key] = [];
                }
                extend(target[key], source[key], deep);
            }
            else if (source[key] !== undefined) {
                target[key] = source[key];
            }
        }
        return target;
    };

    //添加appcan 版本
    appcan.version = '0.1.18';

    var errorInfo = {
        moduleName: '模块的名字必须为字符串并且不能为空！',
        moduleFactory: '模块构造对象必须是函数！'
    };

    //定义一个模块，或者插件
    appcan.define = function (name, factory) {
        if (isFunction(name)) {
            name = '';
            factory = name;
        }
        if (!name || !isString(name)) {
            throw new Error(errorInfo.moduleName);
        }
        if (!isFunction(factory)) {
            throw new Error(errorInfo.moduleFactory);
        }
        var mod = { exports: {} };
        var res = factory.call(this, appcan.require('dom'), mod.exports, mod);
        var exports = mod.exports || res;
        //模块已经存在
        if (name in appcan) {
            appcan[name] = [appcan.name];
            appcan[name].push(exports);
        } else {
            appcan[name] = exports;
        }
        return exports;
    };

    /*
    对模块进行扩展
    @param String name 要扩展的对象
    @param Function factory 扩展函数


    */
    appcan.extend = function (name, factory) {
        if (arguments.length > 1 && isPlainObject(name)) {
            return extend.apply(appcan, arguments);
        }
        if (isFunction(name) || isPlainObject(name)) {
            factory = name;
            name = '';
        }
        name = name ? name : this;
        var extendTo = appcan.require(name);
        extendTo = extendTo ? extendTo : this;
        var mod = { exports: {} };
        var res = null;
        var exports = mod.exports;
        if (isFunction(factory)) {
            res = factory.call(this, extendTo, exports, mod);
            res = res || mod.exports;
            extend(extendTo, res);
        }
        if (isPlainObject(factory)) {
            extend(extendTo, factory);
        }
        return extendTo;
    };

    //加载依赖的文件
    appcan.require = function (name) {
        if (!name) {
            throw new Error(errorInfo.moduleName);
        }
        if (!isString(name)) {
            return name;
        }
        var res = appcan[name];
        if (isArray(res) && res.length < 2) {
            return res[0];
        }
        return res || null;
    };

    //代码入口
    appcan.use = function (modules, factory) {
        if (isFunction(modules)) {
            factory = modules;
            modules = [];
        }
        if (isString(modules)) {
            modules = [modules];
            factory = factory;
        }
        if (!isArray(modules)) {
            throw new Error('以来模块参数不正确！');
        }
        var args = [];
        args.push(appcan.require('dom'));
        for (var i = 0, len = modules.length; i < len; i++) {
            args.push(appcan.require(modules[i]));
        }
        return factory.apply(appcan, args);
    };

    /*
    是否在appcan内运行
    */

    appcan.extend({
        isPlainObject: isPlainObject,
        isFunction: isFunction,
        isString: isString,
        isArray: isArray,
        isAppcan: isAppcan,
        getOptionId: getUID,
        getUID: getUUID
    });

    /*
        继承类

    */
    appcan.inherit = function (parent, protoProps, staticProps) {
        if (!isFunction(parent)) {
            staticProps = protoProps;
            protoProps = parent;
            parent = function () { };
        } else {
            parent = parent;
        }
        var child;
        if (protoProps && (protoProps.hasOwnProperty('constructor'))) {
            child = protoProps.constructor;
        } else {
            child = function () {
                parent.apply(this, arguments);
                this.initated && (this.initated.apply(this, arguments));
                return this;
            };
        }
        extend(child, parent);
        extend(child, staticProps);
        var Surrogate = function () { this.constructor = child; };
        Surrogate.prototype = parent.prototype;
        child.prototype = new Surrogate();
        if (protoProps) {
            extend(child.prototype, protoProps);
        }
        child.__super__ = parent.prototype;
        return child;
    };

    /*
    执行添加到ready中的方法

    */
    function execReadyQueue() {
        for (var i = 0, len = readyQueue.length; i < len; i++) {
            readyQueue[i].call(appcan);
        }
        readyQueue.length = 0;
    }

    /*
    检查是ready
    @param Function callback 回调函数

    */
    function ready(callback) {
        callback = isFunction(callback) ? callback : function () { };
        readyQueue.push(callback);
        if (isUexReady) {
            execReadyQueue();
            return;
        }
        if ('uexWindow' in window) {
            isUexReady = true;
            execReadyQueue();
            return;
        } else {
            //判断uex插件是否ready
            if (readyQueue.length > 1) {
                return;
            }
            if (isFunction(window.uexOnload)) {
                readyQueue.unshift(window.uexOnload);
            }
            window.uexOnload = function (type) {
                isAppcan = true;
                appcan.isAppcan = true;
                if (!type) {
                    isUexReady = true;
                    execReadyQueue();
                }
            };
        }
    }

    //设置uexReady
    appcan.ready = ready;
    global.appcan = appcan;
})(this);
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展zepto到appcan dom 对象上
    扩展Backbone到appcan Backbone 对象上
    扩展underscore到appcan _ 对象上
    created:2014,08.18
    update:


*/
/*global appcan,Zepto,Backbone,_,uexLog,window*/

//把zepto，导入到appcan.dom 上
window.appcan && window.appcan.define('dom', function ($, exports, module) {
    module.exports = jQuery;
});

//把Backbone，导入到appcan.Backbone 上
window.appcan && appcan.define('Backbone', function ($, exports, module) {
    module.exports = Backbone;
});

//把underscore，导入到appcan._ 上
window.appcan && appcan.define('_', function ($, exports, module) {
    module.exports = _;
});

//把underscore，导入到appcan.underscore 上
window.appcan && appcan.define('underscore', function ($, exports, module) {
    module.exports = _;
});

//扩展appcan基础库能力
window.appcan && appcan.extend(function (ac, exports, module) {

    /*
    打印日志到控制台，如果是appcan应用打印到，响应的控制台
    @param * obj 任何类型

    */
    var logs = function (obj) {
        try {
            if (window.uexLog) {
                window.uexLog && uexLog.sendLog(obj);
            } else {
                console && console.log(obj);
            }
        } catch (e) {
            return e;
        }
    };

    ac.logs = logs;

});

//扩展原声的dom对象
window.appcan && appcan.extend('dom', function (dom, exports, module) {
    if (!appcan.isAppcan) {
        return;
    }

});
;/*

    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan view模块
    create:2014.08.25
    update:______/___author___


*/
/* global appcan,window,document*/
appcan && appcan.define('detect', function (ac, exports, module) {
    var os = {};
    var browser = {};
    var ua = window.navigator.userAgent;
    var webkit = ua.match(/Web[kK]it[\/]{0,1}([\d.]+)/),
      android = ua.match(/(Android);?[\s\/]+([\d.]+)?/),
      osx = ua.match(/\(Macintosh\; Intel .*OS X ([\d_.]+).+\)/),
      ipad = ua.match(/(iPad).*OS\s([\d_]+)/),
      ipod = ua.match(/(iPod)(.*OS\s([\d_]+))?/),
      iphone = !ipad && ua.match(/(iPhone\sOS)\s([\d_]+)/),
      webos = ua.match(/(webOS|hpwOS)[\s\/]([\d.]+)/),
      wp = ua.match(/Windows Phone ([\d.]+)/),
      touchpad = webos && ua.match(/TouchPad/),
      kindle = ua.match(/Kindle\/([\d.]+)/),
      silk = ua.match(/Silk\/([\d._]+)/),
      blackberry = ua.match(/(BlackBerry).*Version\/([\d.]+)/),
      bb10 = ua.match(/(BB10).*Version\/([\d.]+)/),
      rimtabletos = ua.match(/(RIM\sTablet\sOS)\s([\d.]+)/),
      playbook = ua.match(/PlayBook/),
      chrome = ua.match(/Chrome\/([\d.]+)/) || ua.match(/CriOS\/([\d.]+)/),
      firefox = ua.match(/Firefox\/([\d.]+)/),
      ie = ua.match(/MSIE\s([\d.]+)/) || ua.match(/Trident\/[\d](?=[^\?]+).*rv:([0-9.].)/),
      webview = !chrome && ua.match(/(iPhone|iPod|iPad).*AppleWebKit(?!.*Safari)/),
      safari = webview || ua.match(/Version\/([\d.]+)([^S](Safari)|[^M]*(Mobile)[^S]*(Safari))/);

    // Todo: clean this up with a better OS/browser seperation:
    // - discern (more) between multiple browsers on android
    // - decide if kindle fire in silk mode is android or not
    // - Firefox on Android doesn't specify the Android version
    // - possibly devide in os, device and browser hashes

    if (browser.webkit = !!webkit) {
        browser.version = webkit[1];
    }
    //android
    if (android) {
        os.name = 'android';
        os.android = true;
        os.version = android[2];
    }

    if (iphone && !ipod) {
        os.name = 'iphone';
        os.ios = os.iphone = true;
        os.version = iphone[2].replace(/_/g, '.');
    }

    if (ipad) {
        os.name = 'ipad';
        os.ios = os.ipad = true;
        os.version = ipad[2].replace(/_/g, '.');
    }
    if (ipod) {
        os.name = 'ipod';
        os.ios = os.ipod = true;
        os.version = ipod[3] ? ipod[3].replace(/_/g, '.') : null;
    }
    if (wp) {
        os.name = 'wp';
        os.wp = true;
        os.version = wp[1];
    }
    if (webos) {
        os.name = 'webos';
        os.webos = true;
        os.version = webos[2];
    }

    if (touchpad) {
        os.name = 'touchpad';
        os.touchpad = true;
    }

    if (blackberry) {
        os.name = 'blackberry';
        os.blackberry = true;
        os.version = blackberry[2];
    }

    if (bb10) {
        os.name = 'bb10';
        os.bb10 = true;
        os.version = bb10[2];
    }

    if (rimtabletos) {
        os.name = 'rimtabletos';
        os.rimtabletos = true;
        os.version = rimtabletos[2];
    }

    if (playbook) {
        browser.name = 'playbook';
        browser.playbook = true;
    }

    if (kindle) {
        os.name = 'kindle';
        os.kindle = true;
        os.version = kindle[1];
    }
    if (silk) {
        browser.name = 'silk';
        browser.silk = true;
        browser.version = silk[1];
    }
    if (!silk && os.android && ua.match(/Kindle Fire/)) {
        browser.name = 'silk';
        browser.silk = true;
    }
    if (chrome) {
        browser.name = 'chrome';
        browser.chrome = true;
        browser.version = chrome[1];
    }
    if (firefox) {
        browser.name = 'firefox';
        browser.firefox = true;
        browser.version = firefox[1];
    }
    if (ie) {
        browser.name = 'ie';
        browser.ie = true;
        browser.version = ie[1];
    }
    if (safari && (osx || os.ios)) {
        browser.name = 'safari';
        browser.safari = true;
        if (osx) {
            browser.version = safari[1];
        }
    }
    if (osx) {
        os.name = 'osx';
        os.version = osx[1].split('_').join('.');
    }
    if (webview) {
        browser.name = 'webview';
        browser.webview = true;
    }
    //appcan navive 应用
    if (!!(appcan.isAppcan)) {
        browser.name = 'appcan';
        browser.appcan = true;
    }
    os.tablet = !!(ipad || playbook || (android && !ua.match(/Mobile/)) ||
      (firefox && ua.match(/Tablet/)) || (ie && !ua.match(/Phone/) && ua.match(/Touch/)));
    os.phone = !!(!os.tablet && !os.ipod && (android || iphone || webos || blackberry || bb10 ||
      (chrome && ua.match(/Android/)) || (chrome && ua.match(/CriOS\/([\d.]+)/)) ||
      (firefox && ua.match(/Mobile/)) || (ie && ua.match(/Touch/))));

    //检查是否支持touch事件
    var checkTouchEvent = function () {
        if (('ontouchstart' in window) || window.DocumentTouch && window.document instanceof window.DocumentTouch) {
            return true;
        }
        return false;
    };

    //判断是否支持css3d,todo：避免多次创建
    var supports3d = function () {
        var div = document.createElement('div'),
			ret = false,
			properties = ['perspectiveProperty', 'WebkitPerspective'];
        for (var i = properties.length - 1; i >= 0; i--) {
            ret = ret ? ret : div.style[properties[i]] !== undefined;
        }

        //如果webkit 3d transforms被禁用,虽然语法上检查没问题，但是还是不支持
        if (ret) {
            var st = document.createElement('style');
            // webkit allows this media query to succeed only if the feature is enabled.
            // "@media (transform-3d),(-o-transform-3d),(-moz-transform-3d),(-ms-transform-3d),(-webkit-transform-3d),(modernizr){#modernizr{height:3px}}"
            st.textContent = '@media (-webkit-transform-3d){#test3d{height:3px}}';
            document.getElementsByTagName('head')[0].appendChild(st);
            div.id = 'test3d';
            document.documentElement.appendChild(div);
            ret = (div.offsetHeight === 3);
            st.parentNode.removeChild(st);
            div.parentNode.removeChild(div);
        }
        return ret;
    };

    //事件的支持度检测
    var events = {
        supportTouch: checkTouchEvent()
    };

    //css的支持度检测
    var css = {
        support3d: supports3d()
    };

    //Mozilla/5.0 (MeeGo; NokiaN9) AppleWebKit/534.13 (KHTML, like Gecko) NokiaBrowser/8.5.0 Mobile Safari/534.13
    module.exports = {
        browser: browser,
        os: os,
        event: events,
        css: css,
        ua: ua
    };

});
;/*
 author:dushaobin
 email:shaobin.du@3g2win.com
 description:扩展encrypt 到appcan对象上
 created:2014,08.21
 update:

 */
/*global appcan*/
appcan && appcan.define('crypto', function ($, exports, module) {
    /*
     扰乱s-box
     @param String key 字符串长度为0-256位

     */
    function rc4Init(key) {
        var s = [],
            j = 0,
            x;
        for (var i = 0; i < 256; i++) {
            s[i] = i;
        }
        for (i = 0; i < 256; i++) {
            j = (j + s[i] + key.charCodeAt(i % key.length)) % 256;
            x = s[i];
            s[i] = s[j];
            s[j] = x;
        }
        return s;
    }

    /*
     用rc4 进行加解密
     @param String str 要加密的数据
     @param Array s 初始化好的s-box

     */
    function rc4Encrypt(str, s) {
        var i = 0;
        var j = 0;
        var res = '';
        var k = [];
        var x = null;
        k = k.concat(s);
        for (var y = 0; y < str.length; y++) {
            i = (i + 1) % 256;
            j = (j + k[i]) % 256;
            x = k[i];
            k[i] = k[j];
            k[j] = x;
            var dest = str.charCodeAt(y) ^ k[(k[i] + k[j]) % 256] || str.charCodeAt(y)
            res += String.fromCharCode(dest);

        }
        return res;
    }

    /*
     直接加密数据合并两个方法

     */
    function rc4EncryptWithKey(key, content) {
        if (!key || !content) {
            return '';
        }
        var sbox = rc4Init(key);
        return rc4Encrypt(content, sbox);
    }

    function MD5(data) {

        // convert number to (unsigned) 32 bit hex, zero filled string
        function to_zerofilled_hex(n) {
            var t1 = (n >>> 0).toString(16)
            return "00000000".substr(0, 8 - t1.length) + t1
        }

        // convert array of chars to array of bytes
        function chars_to_bytes(ac) {
            var retval = []
            for (var i = 0; i < ac.length; i++) {
                retval = retval.concat(str_to_bytes(ac[i]))
            }
            return retval
        }

        // convert a 64 bit unsigned number to array of bytes. Little endian
        function int64_to_bytes(num) {
            var retval = []
            for (var i = 0; i < 8; i++) {
                retval.push(num & 0xFF)
                num = num >>> 8
            }
            return retval
        }

        //  32 bit left-rotation
        function rol(num, places) {
            return ((num << places) & 0xFFFFFFFF) | (num >>> (32 - places))
        }

        // The 4 MD5 functions
        function fF(b, c, d) {
            return (b & c) | (~b & d)
        }

        function fG(b, c, d) {
            return (d & b) | (~d & c)
        }

        function fH(b, c, d) {
            return b ^ c ^ d
        }

        function fI(b, c, d) {
            return c ^ (b | ~d)
        }

        // pick 4 bytes at specified offset. Little-endian is assumed
        function bytes_to_int32(arr, off) {
            return (arr[off + 3] << 24) | (arr[off + 2] << 16) | (arr[off + 1] << 8) | (arr[off])
        }

        /*
         Conver string to array of bytes in UTF-8 encoding
         See:
         http://www.dangrossman.info/2007/05/25/handling-utf-8-in-javascript-php-and-non-utf8-databases/
         http://stackoverflow.com/questions/1240408/reading-bytes-from-a-javascript-string
         How about a String.getBytes(<ENCODING>) for Javascript!? Isn't it time to add it?
         */
        function str_to_bytes(str) {
            var retval = []
            for (var i = 0; i < str.length; i++)
                if (str.charCodeAt(i) <= 0x7F) {
                    retval.push(str.charCodeAt(i))
                } else {
                    var tmp = encodeURIComponent(str.charAt(i)).substr(1).split('%')
                    for (var j = 0; j < tmp.length; j++) {
                        retval.push(parseInt(tmp[j], 0x10))
                    }
                }
            return retval
        }

        // convert the 4 32-bit buffers to a 128 bit hex string. (Little-endian is assumed)
        function int128le_to_hex(a, b, c, d) {
            var ra = ""
            var t = 0
            var ta = 0
            for (var i = 3; i >= 0; i--) {
                ta = arguments[i]
                t = (ta & 0xFF)
                ta = ta >>> 8
                t = t << 8
                t = t | (ta & 0xFF)
                ta = ta >>> 8
                t = t << 8
                t = t | (ta & 0xFF)
                ta = ta >>> 8
                t = t << 8
                t = t | ta
                ra = ra + to_zerofilled_hex(t)
            }
            return ra
        }

        // conversion from typed byte array to plain javascript array
        function typed_to_plain(tarr) {
            var retval = new Array(tarr.length)
            for (var i = 0; i < tarr.length; i++) {
                retval[i] = tarr[i]
            }
            return retval
        }

        // check input data type and perform conversions if needed
        var databytes = null
        // String
        var type_mismatch = null
        if (typeof data == 'string') {
            // convert string to array bytes
            databytes = str_to_bytes(data)
        } else if (data.constructor == Array) {
            if (data.length === 0) {
                // if it's empty, just assume array of bytes
                databytes = data
            } else if (typeof data[0] == 'string') {
                databytes = chars_to_bytes(data)
            } else if (typeof data[0] == 'number') {
                databytes = data
            } else {
                type_mismatch = typeof data[0]
            }
        } else if (typeof ArrayBuffer != 'undefined') {
            if (data instanceof ArrayBuffer) {
                databytes = typed_to_plain(new Uint8Array(data))
            } else if ((data instanceof Uint8Array) || (data instanceof Int8Array)) {
                databytes = typed_to_plain(data)
            } else if ((data instanceof Uint32Array) || (data instanceof Int32Array) || (data instanceof Uint16Array) || (data instanceof Int16Array) || (data instanceof Float32Array) || (data instanceof Float64Array)) {
                databytes = typed_to_plain(new Uint8Array(data.buffer))
            } else {
                type_mismatch = typeof data
            }
        } else {
            type_mismatch = typeof data
        }

        if (type_mismatch) {
            alert('MD5 type mismatch, cannot process ' + type_mismatch)
        }

        function _add(n1, n2) {
            return 0x0FFFFFFFF & (n1 + n2)
        }

        return do_digest()

        function do_digest() {

            // function update partial state for each run
            function updateRun(nf, sin32, dw32, b32) {
                var temp = d
                d = c
                c = b
                //b = b + rol(a + (nf + (sin32 + dw32)), b32)
                b = _add(b, rol(_add(a, _add(nf, _add(sin32, dw32))), b32))
                a = temp
            }

            // save original length
            var org_len = databytes.length

            // first append the "1" + 7x "0"
            databytes.push(0x80)

            // determine required amount of padding
            var tail = databytes.length % 64
            // no room for msg length?
            if (tail > 56) {
                // pad to next 512 bit block
                for (var i = 0; i < (64 - tail) ; i++) {
                    databytes.push(0x0)
                }
                tail = databytes.length % 64
            }
            for (i = 0; i < (56 - tail) ; i++) {
                databytes.push(0x0)
            }
            // message length in bits mod 512 should now be 448
            // append 64 bit, little-endian original msg length (in *bits*!)
            databytes = databytes.concat(int64_to_bytes(org_len * 8))

            // initialize 4x32 bit state
            var h0 = 0x67452301
            var h1 = 0xEFCDAB89
            var h2 = 0x98BADCFE
            var h3 = 0x10325476

            // temp buffers
            var a = 0,
                b = 0,
                c = 0,
                d = 0

            // Digest message
            for (i = 0; i < databytes.length / 64; i++) {
                // initialize run
                a = h0
                b = h1
                c = h2
                d = h3

                var ptr = i * 64

                // do 64 runs
                updateRun(fF(b, c, d), 0xd76aa478, bytes_to_int32(databytes, ptr), 7)
                updateRun(fF(b, c, d), 0xe8c7b756, bytes_to_int32(databytes, ptr + 4), 12)
                updateRun(fF(b, c, d), 0x242070db, bytes_to_int32(databytes, ptr + 8), 17)
                updateRun(fF(b, c, d), 0xc1bdceee, bytes_to_int32(databytes, ptr + 12), 22)
                updateRun(fF(b, c, d), 0xf57c0faf, bytes_to_int32(databytes, ptr + 16), 7)
                updateRun(fF(b, c, d), 0x4787c62a, bytes_to_int32(databytes, ptr + 20), 12)
                updateRun(fF(b, c, d), 0xa8304613, bytes_to_int32(databytes, ptr + 24), 17)
                updateRun(fF(b, c, d), 0xfd469501, bytes_to_int32(databytes, ptr + 28), 22)
                updateRun(fF(b, c, d), 0x698098d8, bytes_to_int32(databytes, ptr + 32), 7)
                updateRun(fF(b, c, d), 0x8b44f7af, bytes_to_int32(databytes, ptr + 36), 12)
                updateRun(fF(b, c, d), 0xffff5bb1, bytes_to_int32(databytes, ptr + 40), 17)
                updateRun(fF(b, c, d), 0x895cd7be, bytes_to_int32(databytes, ptr + 44), 22)
                updateRun(fF(b, c, d), 0x6b901122, bytes_to_int32(databytes, ptr + 48), 7)
                updateRun(fF(b, c, d), 0xfd987193, bytes_to_int32(databytes, ptr + 52), 12)
                updateRun(fF(b, c, d), 0xa679438e, bytes_to_int32(databytes, ptr + 56), 17)
                updateRun(fF(b, c, d), 0x49b40821, bytes_to_int32(databytes, ptr + 60), 22)
                updateRun(fG(b, c, d), 0xf61e2562, bytes_to_int32(databytes, ptr + 4), 5)
                updateRun(fG(b, c, d), 0xc040b340, bytes_to_int32(databytes, ptr + 24), 9)
                updateRun(fG(b, c, d), 0x265e5a51, bytes_to_int32(databytes, ptr + 44), 14)
                updateRun(fG(b, c, d), 0xe9b6c7aa, bytes_to_int32(databytes, ptr), 20)
                updateRun(fG(b, c, d), 0xd62f105d, bytes_to_int32(databytes, ptr + 20), 5)
                updateRun(fG(b, c, d), 0x2441453, bytes_to_int32(databytes, ptr + 40), 9)
                updateRun(fG(b, c, d), 0xd8a1e681, bytes_to_int32(databytes, ptr + 60), 14)
                updateRun(fG(b, c, d), 0xe7d3fbc8, bytes_to_int32(databytes, ptr + 16), 20)
                updateRun(fG(b, c, d), 0x21e1cde6, bytes_to_int32(databytes, ptr + 36), 5)
                updateRun(fG(b, c, d), 0xc33707d6, bytes_to_int32(databytes, ptr + 56), 9)
                updateRun(fG(b, c, d), 0xf4d50d87, bytes_to_int32(databytes, ptr + 12), 14)
                updateRun(fG(b, c, d), 0x455a14ed, bytes_to_int32(databytes, ptr + 32), 20)
                updateRun(fG(b, c, d), 0xa9e3e905, bytes_to_int32(databytes, ptr + 52), 5)
                updateRun(fG(b, c, d), 0xfcefa3f8, bytes_to_int32(databytes, ptr + 8), 9)
                updateRun(fG(b, c, d), 0x676f02d9, bytes_to_int32(databytes, ptr + 28), 14)
                updateRun(fG(b, c, d), 0x8d2a4c8a, bytes_to_int32(databytes, ptr + 48), 20)
                updateRun(fH(b, c, d), 0xfffa3942, bytes_to_int32(databytes, ptr + 20), 4)
                updateRun(fH(b, c, d), 0x8771f681, bytes_to_int32(databytes, ptr + 32), 11)
                updateRun(fH(b, c, d), 0x6d9d6122, bytes_to_int32(databytes, ptr + 44), 16)
                updateRun(fH(b, c, d), 0xfde5380c, bytes_to_int32(databytes, ptr + 56), 23)
                updateRun(fH(b, c, d), 0xa4beea44, bytes_to_int32(databytes, ptr + 4), 4)
                updateRun(fH(b, c, d), 0x4bdecfa9, bytes_to_int32(databytes, ptr + 16), 11)
                updateRun(fH(b, c, d), 0xf6bb4b60, bytes_to_int32(databytes, ptr + 28), 16)
                updateRun(fH(b, c, d), 0xbebfbc70, bytes_to_int32(databytes, ptr + 40), 23)
                updateRun(fH(b, c, d), 0x289b7ec6, bytes_to_int32(databytes, ptr + 52), 4)
                updateRun(fH(b, c, d), 0xeaa127fa, bytes_to_int32(databytes, ptr), 11)
                updateRun(fH(b, c, d), 0xd4ef3085, bytes_to_int32(databytes, ptr + 12), 16)
                updateRun(fH(b, c, d), 0x4881d05, bytes_to_int32(databytes, ptr + 24), 23)
                updateRun(fH(b, c, d), 0xd9d4d039, bytes_to_int32(databytes, ptr + 36), 4)
                updateRun(fH(b, c, d), 0xe6db99e5, bytes_to_int32(databytes, ptr + 48), 11)
                updateRun(fH(b, c, d), 0x1fa27cf8, bytes_to_int32(databytes, ptr + 60), 16)
                updateRun(fH(b, c, d), 0xc4ac5665, bytes_to_int32(databytes, ptr + 8), 23)
                updateRun(fI(b, c, d), 0xf4292244, bytes_to_int32(databytes, ptr), 6)
                updateRun(fI(b, c, d), 0x432aff97, bytes_to_int32(databytes, ptr + 28), 10)
                updateRun(fI(b, c, d), 0xab9423a7, bytes_to_int32(databytes, ptr + 56), 15)
                updateRun(fI(b, c, d), 0xfc93a039, bytes_to_int32(databytes, ptr + 20), 21)
                updateRun(fI(b, c, d), 0x655b59c3, bytes_to_int32(databytes, ptr + 48), 6)
                updateRun(fI(b, c, d), 0x8f0ccc92, bytes_to_int32(databytes, ptr + 12), 10)
                updateRun(fI(b, c, d), 0xffeff47d, bytes_to_int32(databytes, ptr + 40), 15)
                updateRun(fI(b, c, d), 0x85845dd1, bytes_to_int32(databytes, ptr + 4), 21)
                updateRun(fI(b, c, d), 0x6fa87e4f, bytes_to_int32(databytes, ptr + 32), 6)
                updateRun(fI(b, c, d), 0xfe2ce6e0, bytes_to_int32(databytes, ptr + 60), 10)
                updateRun(fI(b, c, d), 0xa3014314, bytes_to_int32(databytes, ptr + 24), 15)
                updateRun(fI(b, c, d), 0x4e0811a1, bytes_to_int32(databytes, ptr + 52), 21)
                updateRun(fI(b, c, d), 0xf7537e82, bytes_to_int32(databytes, ptr + 16), 6)
                updateRun(fI(b, c, d), 0xbd3af235, bytes_to_int32(databytes, ptr + 44), 10)
                updateRun(fI(b, c, d), 0x2ad7d2bb, bytes_to_int32(databytes, ptr + 8), 15)
                updateRun(fI(b, c, d), 0xeb86d391, bytes_to_int32(databytes, ptr + 36), 21)

                // update buffers
                h0 = _add(h0, a)
                h1 = _add(h1, b)
                h2 = _add(h2, c)
                h3 = _add(h3, d)
            }
            // Done! Convert buffers to 128 bit (LE)
            return int128le_to_hex(h3, h2, h1, h0).toUpperCase()
        }

    }

    /**
*
*  Base64 encode / decode
*
**/
    // private property
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    // public method for encoding
    function encode(input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = _utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output + _keyStr.charAt(enc1) + _keyStr.charAt(enc2) + _keyStr.charAt(enc3) + _keyStr.charAt(enc4);

        }

        return output;
    }

    // public method for decoding
    function decode(input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = _utf8_decode(output);

        return output;

    }

    // private method for UTF-8 encoding
    function _utf8_encode(string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    }

    // private method for UTF-8 decoding
    function _utf8_decode(utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }

    function Arcfour(key, txt) {
        var S = [], result = "", i = 0, j = 0;

        for (i = 0; i < 256; i++) {
            S[i] = i;
            j = (j + S[i] + key.charCodeAt(i % key.length)) % 256;
            S[j] = [S[i], S[i] = S[j]][0];  // Swap S[i] for S[j]
        }
        i = j = 0;
        for (var n = 0; n < txt.length; n++) {
            i = (i + 1) % 256;
            j = (j + S[i]) % 256;
            S[i] = [S[j], S[j] = S[i]][0];  // Swap S[i] for S[j]
            result += String.fromCharCode(txt.charCodeAt(n) ^ S[(S[i] + S[j]) % 256]) || txt.charCodeAt(n);
        }
        return result;
    }


    module.exports = {
        /*
         rc4:{
         encryptWithKey:rc4EncryptWithKey
         }*/
        rc4: rc4EncryptWithKey,
        rc4New: Arcfour,
        md5: MD5,
        base64Encode: encode,
        base64Decode: decode
    };

});
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展 db 到appcan 对象上
    created:2014,08.22
    update:


*/
/*global appcan,window*/
appcan && appcan.define('database', function ($, exports, module) {

    var eventEmitter = appcan.require('eventEmitter');
    //var uexDataBaseMgr = window.uexDataBaseMgr || {};
    /*
    获取唯一操作符

    */
    var getOptId = appcan.getOptionId;

    /*
    数据库操作类
    @param String name 数据名

    */
    var DB = function (name) {
        this.name = name;
    };
    var dbProto = {
        constructor: DB,
        select: function (sql, callback) {
            var that = this;
            var optId = getOptId();
            if (arguments.length === 1 && appcan.isPlainObject(sql)) {
                callback = sql.callback;
                sql = sql.sql;
            }
            if (appcan.isFunction(callback)) {
                if (!sql) {
                    return callback(new Error('sql 为空'));
                }
                uexDataBaseMgr.cbSelectSql = function (optId, dataType, data) {
                    if (dataType != 1) {
                        return callback(new Error('select error'));
                    }
                    callback(null, data, dataType, optId);
                    that.emit('select', null, data, dataType, optId);
                };
            }
            uexDataBaseMgr.selectSql(this.name, optId, sql);
        },
        exec: function (sql, callback) {
            var that = this;
            var optId = getOptId();
            if (arguments.length === 1 && appcan.isPlainObject(sql)) {
                callback = sql.callback;
                sql = sql.sql;
            }
            if (appcan.isFunction(callback)) {
                if (!sql) {
                    return callback(new Error('sql 为空'));
                }
                uexDataBaseMgr.cbExecuteSql = function (optId, dataType, data) {
                    if (dataType != 2) {
                        return callback(new Error('exec sql error'));
                    }
                    callback(null, data, dataType, optId);
                    that.emit('select', null, data, dataType, optId);
                };
            }
            uexDataBaseMgr.executeSql(this.name, optId, sql);
        },
        //执行事务
        transaction: function (sqlFun, callback) {
            var that = this;
            var optId = getOptId();
            if (arguments.length === 1 && appcan.isPlainObject(sqlFun)) {
                callback = sqlFun.callback;
                sqlFun = sqlFun.sqlFun;
            }
            if (appcan.isFunction(callback)) {
                if (!appcan.isFunction(sqlFun)) {
                    return callback(new Error('exec transaction error'));
                }
                window.uexDataBaseMgr.cbTransaction = function (optId, dataType, data) {
                    if (dataType != 2) {
                        return callback(new Error('exec transaction!'));
                    }
                    callback(null, data, dataType, optId);
                    that.emit('transaction', null, data, dataType, optId);
                };
            }
            uexDataBaseMgr.transaction(this.name, optId, sqlFun);
        }
    };
    //实现eventEmitter能力
    appcan.extend(dbProto, eventEmitter);
    DB.prototype = dbProto;

    var database = {
        /*
        创建一个数据库
        @param String name 数据库名字
        @param String optId 操作Id
        @param Function callback 创建完成回调


        */
        create: function (name, optId, callback) {
            var argObj = null;
            if (arguments.length === 1 && appcan.isPlainObject(name)) {
                argObj = name;
                name = argObj.name;
                optId = argObj.optId;
                callback = argObj.callback;
            }
            if (!name) {
                callback(new Error('数据库名字不能为空！'));
                return;
            }
            if (appcan.isFunction(optId)) {
                callback = optId;
                optId = '';
            }
            if (appcan.isFunction(callback)) {
                uexDataBaseMgr.cbOpenDataBase = function (optId, type, data) {
                    if (type != 2) {
                        return callback(new Error('open database error'));
                    }
                    var db = new DB(name);
                    callback(null, data, db, type, optId);
                    this.emit('open', null, data, db, type, optId);
                };
            }
            uexDataBaseMgr.openDataBase(name, optId);
        },
        /*
        销户一个数据库
        @param String name 数据库名字
        @param String optId 操作Id
        @param Function callback 删除完成回调


        */
        destory: function (name, optId, callback) {
            var argObj = null;
            if (arguments.length === 1 && appcan.isPlainObject(name)) {
                argObj = name;
                name = argObj.name;
                optId = argObj.optId;
                callback = argObj.callback;
            }
            if (!name) {
                return;
            }
            if (appcan.isFunction(optId)) {
                callback = optId;
                optId = '';
            }
            if (appcan.isFunction(callback)) {
                if (!name) {
                    callback(new Error('数据库名字不能为空！'));
                    return;
                }
                uexDataBaseMgr.cbCloseDataBase = function (optId, dataType, data) {
                    if (dataType != 2) {
                        return callback(new Error('close database error'));
                    }
                    callback(null, data, dataType, optId);
                    this.emit('close', null, data, dataType, optId);
                };
            }

            uexDataBaseMgr.closeDataBase(name, optId);

        },
        /*
        查询数据库数据
        @param String name 数据库名
        @param String sql sql语句
        @param Function callback 查询结果回调


        */
        select: function (name, sql, callback) {
            if (arguments.length === 1 && appcan.isPlainObject(name)) {
                sql = name.sql;
                callback = name.callback;
                name = name.name;
            }
            if (!name || !appcan.isString(name)) {
                return callback(new Error('数据库名不为空'));
            }
            var db = new DB(name);
            db.select(sql, callback);
        },
        exec: function (name, sql, callback) {
            if (arguments.length === 1 && appcan.isPlainObject(name)) {
                sql = name.sql;
                callback = name.callback;
                name = name.name;
            }
            if (!name || !appcan.isString(name)) {
                return callback(new Error('数据库名不为空'));
            }
            var db = new DB(name);
            db.exec(sql, callback);
        },
        translaction: function (name, sqlFun, callback) {
            if (arguments.length === 1 && appcan.isPlainObject(name)) {
                sqlFun = name.sqlFun;
                callback = name.callback;
                name = name.name;
            }
            if (!name || !appcan.isString(name)) {
                return callback(new Error('数据库名不为空'));
            }
            var db = new DB(name);
            db.transaction(sqlFun, callback);
        }
    };

    database = appcan.extend(database, eventEmitter);

    module.exports = database;

});
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展zepto到appcan dom 对象上
    扩展Backbone到appcan Backbone 对象上
    扩展underscore到appcan _ 对象上
    created:2014,08.18
    update:


*/
/*global window,appcan*/
window.appcan && appcan.define('device', function ($, exports, module) {

    var completeCount = 0;
    //var uexDevice = window.uexDevice || {};
    /*
    启动设备震动一段时间
    @param Int millisecond 震动的毫秒数

    */
    function vibrate(millisecond) {
        millisecond = parseInt(millisecond, 10);
        millisecond = isNaN(millisecond) ? 0 : millisecond;
        uexDevice.vibrate(millisecond);
    }

    /*
    取消设备的震动

    */
    function cancelVibrate() {
        uexDevice.cancelVibrate();
    }

    /*
    获取设备相关的信息
    @param Int infoId 设备信息Id
    @param Function callback 获取信息成功后的回调

    */
    function getInfo(infoId, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(infoId)) {
            callback = infoId.callback;
            infoId = infoId.infoId;
        }
        if (appcan.isFunction(callback)) {
            uexDevice.cbGetInfo = function (optId, dataType, data) {
                if (dataType != 1) {
                    return callback(new Error('get info error' + infoId));
                }
                callback(null, data, dataType, optId);
            };
        }
        uexDevice.getInfo(infoId);
    }

    /*
    获取所有设备相关的信息
    @param Function callback 每个结果的回调
    todo: 由于只能通过for循环获取系统信息所以用for

    */
    function getDeviceInfo(callback) {
        var deviceInfo = {};
        for (var i = 0, len = 18; i < len; i++) {
            getInfo(i, function (err, data) {
                completeCount++;
                if (err) {
                    return callback(err);
                }
                var singleInfo = JSON.parse(data);
                appcan.extend(deviceInfo, singleInfo);
                callback(deviceInfo, singleInfo, i, len, completeCount);
            });
        }
        return deviceInfo;
    }

    //更新设备信息
    /*appcan.ready(function(){
        updateDeviceInfo(function(completeCount){
            if(completeCount > 17){
                deviceInfo.isUpdatedAll = true;
            }else{
                deviceInfo.isUpdateAll = false;
            }
            deviceInfo.completeCount = completeCount;
            appcan.extend(deviceRes,deviceInfo);
        });
    });*/

    //相关信息扩展到对象上

    module.exports = {
        vibrate: vibrate,
        cancelVibrate: cancelVibrate,
        getInfo: getInfo,
        getDeviceInfo: getDeviceInfo
    };

});
;/*

    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan eventEmitter模块
    create:2014.08.18
    update:______/___author___


*/
/*global appcan*/
appcan && appcan.define('eventEmitter', function ($, exports, module) {
    //事件对象
    var eventEmitter = {
        on: function (name, callback) {
            if (!this.__events) {
                this.__events = {};
            }
            if (this.__events[name]) {
                this.__events[name].push(callback);
            } else {
                this.__events[name] = [callback];
            }
        },
        off: function (name, callback) {
            if (!this.__events) {
                return;
            }
            if (name in this.__events) {
                for (var i = 0, len = this.__events[name].length; i < len; i++) {
                    if (this.__events[name][i] === callback) {
                        this.__events[name].splice(i, 1);
                        return;
                    }
                }
            }
        },
        once: function (name, callback) {
            var that = this;
            var tmpcall = function () {
                callback.apply(that, arguments);
                that.off(tmpcall);
            };
            this.on(name, tmpcall);
        },
        addEventListener: function () {
            return this.on.apply(this, arguments);
        },
        removeEventListener: function () {
            return this.off.apply(this, arguments);
        },
        trigger: function (name, context) {
            var args = [].slice.call(arguments, 2);
            if (!this.__events || !appcan.isString(name)) {
                return;
            }
            context = context || this;
            if (name && (name in this.__events)) {
                for (var i = 0, len = this.__events[name].length; i < len; i++) {
                    this.__events[name][i].apply(context, args);
                }
            }
        },
        emit: function () {
            return this.trigger.apply(this, arguments);
        }

    };
    //扩展到appan基础对象上
    appcan.extend(eventEmitter);
    module.exports = eventEmitter;
});
;;
/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    descript:构建appcan file 模块
    created:2014.08.19
    update:____/____

*/
/*global appcan,uexFileMgr*/

appcan && appcan.define('file', function ($, exports, module) {

    //var uexFileMgr = window.uexFileMgr;
    /*
    获取操作 id

    */
    var getOptionId = appcan.getOptionId;

    /*
    文件是否存在
    @param String filePath 文件路径
    @param Function callback 又返回结果时的回调

    */

    var existQueue = {};//出来是否存在的队列
    var writeGlobalQueue = [];//写队列
    var readGlobalQueue = [];//读队列
    var readOpenGlobalQueue = [];//读队列
    var statQueue = [];//stat方法使用队列
    var statQueueUsed = [];

    function processExistCall(optId, dataType, data) {
        //var callback = existQueue['exist_call_'+optId];
        var callback = existQueue['exist_call_' + optId].cb;
        var filePath = existQueue['exist_call_' + optId].fp;
        if (appcan.isFunction(callback)) {
            if (dataType == 2) {
                callback(null, data, dataType, optId, filePath);
            } else {
                callback(new Error('exist file error'), data, dataType, optId, filePath);
            }
        }
        //当调用一次后释放掉
        delete existQueue['exist_call_' + optId];
    }

    function exists(filePath, callback, optId) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            callback = argObj.callback;
            optId = argObj.optId;
        }
        optId = optId || getOptionId();
        if (appcan.isFunction(callback)) {
            existQueue['exist_call_' + optId] = { cb: callback, fp: filePath };
            uexFileMgr.cbIsFileExistByPath = function (optId, dataType, data) {
                processExistCall.apply(null, arguments);
            };
        }
        uexFileMgr.isFileExistByPath(optId, filePath);
        close(optId);
    }

    /*
    返回文件的相关信息
    @param String filePath
    @param Function callback
    */

    function stat(filePath, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            callback = argObj.callback;
        }

        if (statQueue.length > 0) {
            statQueue.push({ fp: filePath, cb: callback });
        } else {
            statQueue.push({ fp: filePath, cb: callback });
            execStatQueue();
        }
    }

    //执行存在队列
    function execStatQueue() {
        if (statQueue.length < 1) {
            return;
        }
        var execStat = statQueue[0];
        var filePath = execStat.fp;
        var callback = execStat.cb;

        if (appcan.isFunction(callback)) {
            uexFileMgr.cbGetFileTypeByPath = function (optId, dataType, data) {
                if (dataType != 2) {
                    callback(new Error('get file type error'), null, dataType, optId, filePath);
                    //processStatGlobalQueue(new Error('get file type error'),null,dataType,optId);
                    return;
                }
                var res = {};
                if (data == 0) {
                    res.isFile = true;
                }
                if (data == 1) {
                    res.isDirectory = true;
                }
                callback(null, res, dataType, optId, filePath);
                //processStatGlobalQueue(null,res,dataType,optId);
                statQueue.shift();
                if (statQueue.length) {
                    execStatQueue();
                } else {
                    //alert('exec over');
                }
            };
        } else {
            statQueue.shift();
            if (statQueue.length) {
                execStatQueue();
            }
        }
        uexFileMgr.getFileTypeByPath(filePath);
    }

    /*
                        处理全局回调read消息
        @param string msg 传递过来的消息
    
    */
    function processReadGlobalQueue(err, data, dataType, optId) {
        if (readGlobalQueue.length > 0) {
            $.each(readGlobalQueue, function (i, v) {
                if (v && v.cb && appcan.isFunction(v.cb)) {
                    if (v.readOptId == optId) {
                        v.cb(err, data, dataType, optId);
                    }
                }
            });
        }
        return
    }

    /*
                         处理全局回调readOpen消息
         @param string msg 传递过来的消息
     
     */
    function processReadOpenGlobalQueue(err, data, dataType, optId) {
        if (readOpenGlobalQueue.length > 0) {
            $.each(readOpenGlobalQueue, function (i, v) {
                if (v && v.cb && appcan.isFunction(v.cb)) {
                    if (v.optId == optId) {
                        v.cb(err, data, dataType, optId);
                    }
                }
            });
        }
        return
    }

    /*
    读取文件内容
    @param String filePath 文件路径
    @param String callback 结果回调
    */
    function read(filePath, length, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            length = argObj.length;
            callback = argObj.callback;
        }
        if (!filePath) {
            return callback(new Error('file name is empty'));
        }
        if (appcan.isFunction(length)) {
            callback = length;
            length = -1;
        }
        callback = appcan.isFunction(callback) ? callback : function () { };
        length = length || -1;
        var optId = getOptionId();
        readGlobalQueue.push({ fPath: filePath, cb: callback, readOptId: optId });
        exists(filePath, function (err, res, dataType, optId, filePath) {
            if (err) {
                $.each(readGlobalQueue, function (i, v) {
                    if (v && v.fPath == filePath) {
                        return v.cb(err);
                    }
                })
                //return callback(err);
            }
            if (!res) {
                $.each(readGlobalQueue, function (i, v) {
                    if (v && v.fPath == filePath) {
                        return v.cb(new Error('文件不存在'));
                    }
                })
                //return callback(new Error('文件不存在'));
            }
            stat(filePath, function (err, fileInfo, dataType, optId, filePath) {
                if (err) {
                    $.each(readGlobalQueue, function (i, v) {
                        if (v && v.fPath == filePath) {
                            return v.cb(err);
                        }
                    })
                    //return callback(err);
                }
                if (!fileInfo.isFile) {
                    $.each(readGlobalQueue, function (i, v) {
                        if (v && v.fPath == filePath) {
                            return v.cb(new Error('该路径不是文件'));
                        }
                    })
                    //return callback(new Error('该路径不是文件'));
                }
                uexFileMgr.cbReadFile = function (optId, dataType, data) {
                    if (dataType != 0) {
                        //callback(new Error('read file error'),data,dataType,optId);
                        processReadGlobalQueue(new Error('read file error'), data, dataType, optId);
                    }
                    //callback(null,data,dataType,optId);
                    processReadGlobalQueue(null, data, dataType, optId);
                };
                readOpen(filePath, 1, function (err, data, dataType, optId) {
                    uexFileMgr.readFile(optId, length);
                    close(optId);
                });
            });
        }, optId);
    }


    /*
    读加密的文件内容
    
    
    */

    function readSecure(filePath, length, key, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            length = argObj.length;
            key = argObj.key;
            callback = argObj.callback;
        }
        if (!filePath) {
            return callback(new Error('file name is empty'));
        }
        callback = appcan.isFunction(callback) ? callback : function () { };
        length = length || -1;
        exists(filePath, function (err, res) {
            if (err) {
                return callback(err);
            }
            if (!res) {
                return callback(new Error('文件不存在'));
            }
            stat(filePath, function (err, fileInfo) {
                if (err) {
                    return callback(err);
                }
                if (!fileInfo.isFile) {
                    return callback(new Error('该路径不是文件'));
                }
                uexFileMgr.cbReadFile = function (optId, dataType, data) {
                    if (dataType != 0) {
                        callback(new Error('read file error'), data, dataType, optId);
                    }
                    callback(null, data, dataType, optId);
                };
                openSecure(filePath, 1, key, function (err, data, dataType, optId) {
                    uexFileMgr.readFile(optId, length);
                    close(optId);
                });
            });
        });
    }

    /*
    获取文件的json形式
    @param String filePath 文件的路径
    @param Function callback 文件读取完成之后的回调

    */
    function readJSON(filePath, callback) {
        read({
            filePath: filePath,
            callback: function (err, data) {
                var res = null;
                if (err) {
                    return callback(err);
                }
                try {
                    if (!data) {
                        res = {};
                    } else {
                        res = JSON.parse(data);
                    }
                    callback(null, res);
                } catch (e) {
                    return callback(e);
                }
            }
        });
    }

    /*
       处理全局回调openwrite消息
       @param string msg 传递过来的消息
   
   */
    function processWriteGlobalQueue(err, data, dataType, optId) {
        if (writeGlobalQueue.length > 0) {
            $.each(writeGlobalQueue, function (i, v) {
                if (v && v.cb && appcan.isFunction(v.cb)) {
                    if (v.optId == optId) {
                        v.cb(err, data, dataType, optId, v.ct);
                    }
                }
            });
        }
        return
    }

    /*
    写文件
    @param String filePath 文件路径
    @param String mode  写入方式
    @param String content 写入内容

    */
    function write(filePath, content, callback, mode) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            content = argObj.content;
            mode = argObj.mode;
            callback = argObj.callback;
        }
        mode = mode || 0;
        if (appcan.isFunction(content)) {
            callback = content;
            content = '';
        }
        writeOpen(filePath, 2, function (err, data, dataType, optId, contents) {
            if (err) {
                return callback(err);
            }
            uexFileMgr.writeFile(optId, mode, contents);
            close(optId);
            callback(null);
        }, content);
    }

    /*
    以安全的方式写入文件内容
    @param String filePath 文件路径
    @param String mode  写入方式
    @param String content 写入内容
    @param String key 要写入文件的密码

    */
    function writeSecure(filePath, content, callback, mode, key) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            content = argObj.content;
            mode = argObj.mode;
            key = argObj.key;
            callback = argObj.callback;
        }
        mode = mode || 0;
        if (appcan.isFunction(content)) {
            key = mode;
            mode = callback;
            callback = content;
            content = '';
        }
        openSecure(filePath, 2, key, function (err, data, dataType, optId) {
            if (err) {
                return callback(err);
            }
            uexFileMgr.writeFile(optId, mode, content);
            close(optId);
            callback(null);
        });
    }

    /*
    附近文件到文件的末尾
    @param String filePath 文件路径
    @param String content 内容
    @param Function 回调

    */

    function append(filePath, content, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            content = argObj.content;
            callback = argObj.callback;
        }
        return write(filePath, content, callback, 1);
    }

    /*
    附近文件到文件的末尾
    @param String filePath 文件路径
    @param String content 内容
    @param String key 加密key
    @param Function 回调

    */

    function appendSecure(filePath, content, key, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            content = argObj.content;
            key = argObj.key;
            callback = argObj.callback;
        }
        return writeSecure(filePath, content, callback, 1, key);
    }


    /*
    打开流
    @param String filePath 文件路径
    @param String mode 打开方式

    */
    function open(filePath, mode, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            mode = argObj.mode;
            callback = argObj.callback;
        }
        if (appcan.isFunction(mode)) {
            callback = mode;
            mode = 3;
        }
        mode = mode || 3;
        if (!appcan.isString(filePath)) {
            return callback(new Error('文件路径不正确'));
        }
        //var optId = getOptionId();
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbOpenFile = function (optId, dataType, data) {
                if (dataType != 2) {
                    callback(new Error('open file error'), data, dataType, optId);
                    return;
                }
                callback(null, data, dataType, optId);
            };
        }
        uexFileMgr.openFile(getOptionId(), filePath, mode);
        //close(optId);
    }

    /*
           write调用打开流
    @param String filePath 文件路径
    @param String mode 打开方式

    */
    function writeOpen(filePath, mode, callback, content) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            mode = argObj.mode;
            callback = argObj.callback;
        }
        if (appcan.isFunction(mode)) {
            callback = mode;
            mode = 3;
        }
        mode = mode || 3;
        if (!appcan.isString(filePath)) {
            return callback(new Error('文件路径不正确'));
        }
        var optId = getOptionId();
        writeGlobalQueue.push({ optId: optId, cb: callback, ct: content });
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbOpenFile = function (optId, dataType, data) {
                if (dataType != 2) {
                    //callback(new Error('open file error'),data,dataType,optId,content);
                    processWriteGlobalQueue(new Error('open file error'), data, dataType, optId);
                    return;
                }
                //callback(null,data,dataType,optId,content);
                processWriteGlobalQueue(null, data, dataType, optId);
            };
        }
        uexFileMgr.openFile(optId, filePath, mode);

        //close(optId);
    }

    /*
           write调用打开流
    @param String filePath 文件路径
    @param String mode 打开方式

    */
    function readOpen(filePath, mode, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            mode = argObj.mode;
            callback = argObj.callback;
        }
        if (appcan.isFunction(mode)) {
            callback = mode;
            mode = 3;
        }
        mode = mode || 3;
        if (!appcan.isString(filePath)) {
            return callback(new Error('文件路径不正确'));
        }
        var optId = null;
        $.each(readGlobalQueue, function (i, v) {
            if (v.fPath && v.fPath == filePath) {
                optId = v.readOptId;
            }
        })
        if (!optId) {
            optId = getOptionId();
        }
        if (appcan.isFunction(callback)) {
            readOpenGlobalQueue.push({ optId: optId, cb: callback });
            uexFileMgr.cbOpenFile = function (optId, dataType, data) {
                if (dataType != 2) {
                    //callback(new Error('open file error'),data,dataType,optId,content);
                    processReadOpenGlobalQueue(new Error('open file error'), data, dataType, optId);
                    return;
                }
                //callback(null,data,dataType,optId,content);
                processReadOpenGlobalQueue(null, data, dataType, optId);
            };
        }
        uexFileMgr.openFile(optId, filePath, mode);

        //close(optId);
    }

    /*
    打开加密文件
    @param String filePath 文件路径
    @param String mode 模式
    @param String key 加密字符串
    @param Function callback 打开加密文件成功后的回调
    
    */

    function openSecure(filePath, mode, key, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            mode = argObj.mode;
            key = argObj.key;
            callback = argObj.callback;
        }
        key = key ? key : '';
        mode = mode || 3;
        if (!appcan.isFunction(callback)) {
            callback = null;
        }
        if (!appcan.isString(filePath)) {
            return callback(new Error('文件路径不正确'));
        }
        //var optId = getOptionId();
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbOpenSecure = function (optId, dataType, data) {
                if (dataType != 2) {
                    callback(new Error('open secure file error'), data, dataType, optId);
                    return;
                }
                callback(null, data, dataType, optId);
            };
        }
        uexFileMgr.openSecure(getOptionId(), filePath, mode, key);
        //close(optId);
    }



    /*
    删除文件
    @param String filePath 文件路径
    @param Function callback 删除结果回调函数

    */

    function deleteFile(filePath, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            callback = argObj.callback;
        }
        var optId = getOptionId();
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbDeleteFileByPath = function (optId, dataType, data) {
                if (dataType != 2) {
                    return callback(new Error('delete file error'));
                }
                callback(null, data, dataType, optId);
            };
        }
        uexFileMgr.deleteFileByPath(filePath);
        close(optId);
    }

    /*
    关闭文件流
    @param String optId 操作id

    */
    function close(optId) {
        if (arguments.length === 1 && appcan.isPlainObject(optId)) {
            optId = optId.optId;
        }
        if (!optId) {
            return;
        }
        uexFileMgr.closeFile(optId);
    }



    /*
    创建文件
    @param String filePath 文件路径
    @param Function callback 创建结果回调函数

    */
    function create(filePath, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            callback = argObj.callback;
        }
        var optId = getOptionId();
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbCreateFile = function (optId, dataType, data) {
                if (dataType != 2) {
                    return callback(new Error('create file error'),
                    data, dataType, optId);
                }
                callback(null, data, dataType, optId);
            };
        }
        uexFileMgr.createFile(optId, filePath);
        close(optId);
    }

    /*
    创建文件
    @param String filePath 创建一个加密文件
    @param String key 加密的字符串 
    @param Function callback 创建结果回调函数

    */
    function createSecure(filePath, key, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            key = argObj.key;
            callback = argObj.callback;
        }
        key = key ? key : '';
        var optId = getOptionId();
        if (appcan.isFunction(callback)) {
            uexFileMgr.cbCreateSecure = function (optId, dataType, data) {
                if (dataType != 2) {
                    return callback(new Error('create secure file error'),
                    data, dataType, optId);
                }
                callback(null, data, dataType, optId);
            };
        }
        uexFileMgr.createSecure(optId, filePath, key);
        close(optId);

    }


    //本地文件
    var localPath = 'wgt://data/locFile.txt';

    /*
    删除本地文件
    @param Function callback 删除本地文件

    */

    function deleteLocalFile(callback) {
        if (appcan.isPlainObject(callback)) {
            callback = callback.callback;
        }
        if (!appcan.isFunction(callback)) {
            callback = function () { };
        }
        deleteFile(localPath, callback);
    }

    /*
    写入本地文件
    @param String content 要写入的内容
    @param Function callback 写完后的结果

    */

    function writeLocalFile(content, callback) {
        exists(localPath, function (err, data) {
            if (err) {
                return callback(err);
            }
            if (!data) {
                create(localPath, function (err, res) {
                    if (err) {
                        return callback(err);
                    }
                    if (res == 0) {
                        write(localPath, content, callback);
                    }
                });
            } else {
                write(localPath, content, callback);
            }
        });
    }

    /*
    读本地文件内容
    @param Function callback 结果回调


    */

    function readLocalFile(callback) {
        return read(localPath, callback);
    }

    /*
    获取文件的真实路径
    @param String path 要获取的文件路径
    @param Function callback 获取成功后的回调    
    
    */
    function getRealPath(filePath, callback) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(filePath)) {
            argObj = filePath;
            filePath = argObj.filePath;
            callback = argObj.callback;
        }
        uexFileMgr.cbGetFileRealPath = function (optId, dataType, data) {
            if (dataType != 0) {
                callback(new Error('get file path error'), data, dataType, optId);
                return;
            }
            callback(null, data, dataType, optId);
        };

        uexFileMgr.getFileRealPath(filePath);
    }




    module.exports = {
        wgtPath: 'wgt://',
        resPath: 'res://',
        wgtRootPath: 'wgtroot://',
        open: open,
        close: close,
        read: read,
        readJSON: readJSON,
        write: write,
        create: create,
        remove: deleteFile,
        append: append,
        exists: exists,
        stat: stat,
        deleteLocalFile: deleteLocalFile,
        writeLocalFile: writeLocalFile,
        readLocalFile: readLocalFile,
        getRealPath: getRealPath,
        createSecure: createSecure,
        openSecure: openSecure,
        readSecure: readSecure,
        writeSecure: writeSecure,
        appendSecure: appendSecure
    };

});;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展zepto到appcandom 对象上
    created:2014,08.20
    update:


*/
/*global appcan,window*/
window.appcan && appcan.define('Model', function ($, exports, module) {
    var Backbone = appcan.require('Backbone');
    var Model = Backbone.Model.extend({
        setToken: function () {

        }
    });

    module.exports = Model;
});
;/*

    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan request模块
    create:2014.08.18
    update:______/___author___


*/
/*global window,appcan*/
appcan && appcan.define('request', function ($, exports, module) {
    var jsonpID = 0,
      document = window.document,
      key,
      name,
      rscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi,
      scriptTypeRE = /^(?:text|application)\/javascript/i,
      xmlTypeRE = /^(?:text|application)\/xml/i,
      jsonType = 'application/json',
      htmlType = 'text/html',
      blankRE = /^\s*$/;

    var getXmlHttpId = appcan.getOptionId;

    // trigger a custom event and return false if it was cancelled
    function triggerAndReturn(context, eventName, data) {
        appcan.trigger(eventName, context, data);
    }

    // trigger an Ajax "global" event
    function triggerGlobal(settings, context, eventName, data) {
        if (settings.global) {
            return triggerAndReturn(context || appcan, eventName, data);
        }
    }

    // Number of active Ajax requests
    var active = 0;

    function ajaxStart(settings) {
        if (settings.global && active++ === 0) {
            triggerGlobal(settings, null, 'ajaxStart');
        }
    }
    function ajaxStop(settings) {
        if (settings.global && !(--active)) {
            triggerGlobal(settings, null, 'ajaxStop');
        }
    }

    // triggers an extra global event "ajaxBeforeSend" that's like "ajaxSend" but cancelable
    function ajaxBeforeSend(xhr, settings) {
        var context = settings.context;
        if (settings.beforeSend.call(context, xhr, settings) === false ||
            triggerGlobal(settings, context, 'ajaxBeforeSend', [xhr, settings]) === false) {
            return false;
        }

        triggerGlobal(settings, context, 'ajaxSend', [xhr, settings]);
    }
    function ajaxSuccess(data, requestCode, response, xhr, settings, deferred) {
        var context = settings.context, status = 'success';

        settings.success.call(context, data, status, requestCode, response, xhr);
        if (deferred) {
            deferred.resolveWith(context, [data, status, requestCode, response, xhr]);
        }
        triggerGlobal(settings, context, 'ajaxSuccess', [xhr, settings, data, status, requestCode, response]);
        ajaxComplete(status, xhr, settings);
    }
    // type: "timeout", "error", "abort", "parsererror"
    function ajaxError(error, type, msg, xhr, settings, deferred) {
        var context = settings.context;
        settings.error.call(context, xhr, type, error, msg);
        if (deferred) {
            deferred.rejectWith(context, [xhr, type, error, msg]);
        }
        triggerGlobal(settings, context, 'ajaxError',
          [xhr, settings, error || type, msg]);
        ajaxComplete(type, xhr, settings);
    }
    // status: "success", "notmodified", "error", "timeout", "abort", "parsererror"
    function ajaxComplete(status, xhr, settings) {
        var context = settings.context;
        settings.complete.call(context, xhr, status);
        triggerGlobal(settings, context, 'ajaxComplete', [xhr, settings]);
        ajaxStop(settings);
    }

    // progress: 当前上传进度
    function ajaxProgress(progress, xhr, settings) {
        var context = settings.context;
        settings.progress.call(context, progress, xhr, status);
        triggerGlobal(settings, context, 'ajaxProgress', [progress, xhr, settings]);
    }

    // Empty function, used as default callback
    function empty() { }

    var ajaxSettings = {
        // Default type of request
        type: 'GET',
        // Callback that is executed before request
        beforeSend: empty,
        // Callback that is executed if the request succeeds
        success: empty,
        // Callback that is executed the the server drops error
        error: empty,
        // Callback that is executed on request complete (both: error and success)
        complete: empty,
        //add progress 
        progress: empty,
        // The context for the callbacks
        context: null,
        // Whether to trigger "global" Ajax events
        global: true,
        //证书信息
        certificate: null,
        //添加app认证信息
        appVerify: true,
        //模拟Http
        emulateHTTP: false,
        // Transport
        xhr: function () {
            return window.uexXmlHttpMgr || XMLHttpRequest;
        },
        // MIME types mapping
        // IIS returns Javascript as "application/x-javascript"
        accepts: {
            script: 'text/javascript, application/javascript, application/x-javascript',
            json: jsonType,
            xml: 'application/xml, text/xml',
            html: htmlType,
            text: 'text/plain'
        },
        // Whether the request is to another domain
        crossDomain: false,
        // Default timeout
        timeout: 0,
        //默认不设置content type
        contentType: false,
        // Whether data should be serialized to string
        processData: false,
        // Whether the browser should be allowed to cache GET responses
        cache: true
    };

    function mimeToDataType(mime) {
        if (mime) {
            mime = mime.split(';', 2)[0];
        }
        return mime && (mime == htmlType ? 'html' :
          mime == jsonType ? 'json' :
          scriptTypeRE.test(mime) ? 'script' :
          xmlTypeRE.test(mime) && 'xml') || 'text';
    }

    function appendQuery(url, query) {
        if (query == '') {
            return url;
        }
        return (url + '&' + query).replace(/[&?]{1,2}/, '?');
    }

    function processCompleteResult(xhr, opcode, status, result, requestCode, response, deferred) {
        var settings = xhr['settings_' + opcode];
        var dataType = settings.dataType;
        if (status < 0) {
            if (result == null || result == "") {
                result = response;
            }

            ajaxError(null, 'request error', result, xhr, settings, deferred);
            return;
        }
        if (status == 1) {
            if (!requestCode || requestCode == 200 || (requestCode > 200 && requestCode < 300) || requestCode == 304) {
                //todo release 
                xhr['settings_' + opcode] = null;
                //clearTimeout(abortTimeout);
                var error = false;
                result = result || '';
                try {
                    // http://perfectionkills.com/global-eval-what-are-the-options/
                    if (dataType == 'script') {
                        (1, eval)(result);
                    } else if (dataType == 'xml') {
                        result = result;
                    } else if (dataType == 'json') {
                        result = blankRE.test(result) ? null : $.parseJSON(result);
                    }
                } catch (e) {
                    error = e;
                }
                if (error) {
                    ajaxError(error, 'parsererror', result, xhr, settings, deferred);
                }
                else {
                    ajaxSuccess(result, requestCode, response, xhr, settings, deferred);
                }
            } else {
                if (result == null || result == "") {
                    result = response;
                }
                ajaxError(null, 'request error', result, xhr, settings, deferred);
            }

        } else {
            ajaxError(null, 'error', result, xhr, settings, deferred);
        }
        xhr.close(opcode);
    }

    function processProgress(progress, xhr, optId) {
        var settings = xhr['settings_' + optId];
        ajaxProgress(progress, xhr, settings);
    }

    // serialize payload and append it to the URL for GET requests
    function serializeData(options) {
        if (options.processData && options.data && !appcan.isString(options.data)) {
            options.data = $.param(options.data, options.traditional);
        }
        if (options.data && (!options.type || options.type.toUpperCase() == 'GET')) {
            options.data = $.param(options.data, options.traditional);
            options.url = appendQuery(options.url, options.data);
            options.data = undefined;
        }
    }


    function ajax(options) {
        var httpId = getXmlHttpId();
        var settings = $.extend({}, options || {}),
            deferred = $.Deferred && $.Deferred();
        for (key in ajaxSettings) {
            if (settings[key] === undefined) {
                settings[key] = ajaxSettings[key];
            }
        }
        ajaxStart(settings);
        if (!settings.crossDomain) {
            settings.crossDomain = /^([\w-]+:)?\/\/([^\/]+)/.test(settings.url) &&
                RegExp.$2 != window.location.host;
        }

        if (!settings.url) {
            settings.url = window.location.toString();
        }
        serializeData(settings);

        var dataType = settings.dataType;
        var hasPlaceholder = /\?.+=\?/.test(settings.url);
        if (hasPlaceholder) {
            dataType = 'jsonp';
        }

        if (settings.cache === false || (
            (!options || options.cache !== true) &&
            ('script' == dataType || 'jsonp' == dataType)
        )) {
            settings.url = appendQuery(settings.url, '_=' + Date.now());
        }

        if ('jsonp' == dataType) {
            if (!hasPlaceholder) {
                settings.url = appendQuery(settings.url,
                settings.jsonp ? (settings.jsonp + '=?') : settings.jsonp === false ? '' : 'callback=?');
            }
            return $.ajaxJSONP(settings, deferred);
        }

        var mime = settings.accepts[dataType],
            headers = {},
            setHeader = function (name, value) {
                headers[name.toLowerCase()] = [name, value];
            },
            protocol = /^([\w-]+:)\/\//.test(settings.url) ? RegExp.$1 : window.location.protocol,
            xhr = settings.xhr(),
            nativeSetHeader = function (xhr, headers) {
                var toHeader = {};
                var fromHeader = null;
                for (var key in headers) {
                    fromHeader = headers[key];
                    toHeader[fromHeader[0]] = fromHeader[1];
                }
                xhr.setHeaders(httpId, JSON.stringify(toHeader));
            },
            addAppVerify = function (settings) {
                if (settings.appVerify === true) {
                    //添加app 认证头 修复模拟器不支持setAppvVerify 方法
                    xhr.setAppVerify && xhr.setAppVerify(httpId, 1);
                }
                if (settings.appVerify === false) {
                    //添加app 认证头 修复模拟器不支持setAppvVerify 方法
                    xhr.setAppVerify && xhr.setAppVerify(httpId, 0);
                }
            },
            //更新证书信息
            updateCertificate = function (settings) {
                var certi = settings.certificate;
                if (!certi) {
                    return;
                }
                xhr.setCertificate && xhr.setCertificate(httpId, certi.password || '', certi.path);
            },
            abortTimeout;
        //绑定相应的配置
        xhr['settings_' + httpId] = settings;

        if (deferred) {
            deferred.promise(xhr);
        }
        //发出的ajax请求
        if (!settings.crossDomain) {
            setHeader('X-Requested-With', 'XMLHttpRequest');
        }
        setHeader('Accept', mime || '*/*');
        if (mime = settings.mimeType || mime) {
            if (mime.indexOf(',') > -1) {
                mime = mime.split(',', 2)[0];
            }
            xhr.overrideMimeType && xhr.overrideMimeType(mime);
        }

        if (settings.emulateHTTP && (settings.type === 'PUT' || settings.type === 'DELETE' || settings.type === 'PATCH')) {
            setHeader('X-HTTP-Method-Override', settings.type);
            settings.type = 'POST';
        }

        if (settings.contentType ||
            (settings.contentType !== false &&
            settings.data && settings.type.toUpperCase() != 'GET')) {
            setHeader('Content-Type', settings.contentType || 'application/x-www-form-urlencoded');
        }

        if (settings.headers) {
            for (var name in settings.headers) {
                setHeader(name, settings.headers[name]);
            }
        }

        xhr.setRequestHeader = setHeader;
        //添加progress 回调
        xhr.onPostProgress = function (optId, progress) {
            var resArg = [progress];
            resArg.push(xhr);
            resArg.push(optId);
            processProgress.apply(null, resArg);
        };
        xhr.onData = function () {
            var resArg = [xhr];
            for (var i = 0, len = arguments.length; i < len; i++) {
                resArg.push(arguments[i]);
            }
            resArg.push(deferred);
            processCompleteResult.apply(null, resArg);
        };

        if (ajaxBeforeSend(xhr, settings) === false) {
            xhr.close(httpId);
            ajaxError(null, 'abort', null, xhr, settings, deferred);
            return xhr;
        }

        if (settings.xhrFields) {
            for (name in settings.xhrFields) {
                xhr[name] = settings.xhrFields[name];
            }
        }

        var async = 'async' in settings ? settings.async : true;
        //xhr.open(settings.type, settings.url, async, settings.username, settings.password)
        xhr.open(httpId, settings.type, settings.url, settings.timeout);
        //添加http header
        nativeSetHeader(xhr, headers);
        //设置证书信息
        updateCertificate(settings);
        //添加app认证信息
        addAppVerify(settings);

        if (settings.data && settings.contentType === false) {
            for (name in settings.data) {
                //fixed Number 类型bug
                if (appcan.isPlainObject(settings.data[name])) {
                    if (settings.data[name].path) {
                        //上传文件数据
                        xhr.setPostData(httpId, "1", name, settings.data[name].path);
                    } else {
                        xhr.setPostData(httpId, "0", name, JSON.stringify(settings.data[name]));
                    }
                } else {
                    //添加普通数据
                    xhr.setPostData(httpId, "0", name, settings.data[name]);
                }
            }
        } else {
            if (settings.contentType === 'application/json') {
                if (appcan.isPlainObject(settings.data)) {
                    settings.data = JSON.stringify(settings.data);
                }
            }
            //fixed ios bug 如果调用setBody就是当作post请求发送出来
            if (settings.data) {
                xhr.setBody(httpId, settings.data ? settings.data : null);
            }
        }
        xhr.send(httpId);
        return xhr;
    }

    // handle optional data/success arguments
    function parseArguments(url, data, success, dataType) {
        if (appcan.isFunction(data)) {
            dataType = success;
            success = data;
            data = undefined;
        }
        if (!appcan.isFunction(success)) {
            dataType = success;
            success = undefined;
        }
        return {
            url: url,
            data: data,
            success: success,
            dataType: dataType
        };
    }

    function get(/* url, data, success, dataType */) {
        return ajax(parseArguments.apply(null, arguments));
    }

    function post(/* url, data, success, dataType */) {
        var options = parseArguments.apply(null, arguments);
        options.type = 'POST';
        return ajax(options);
    }

    function getJSON(/* url, data, success */) {
        var options = parseArguments.apply(null, arguments);
        options.dataType = 'json';
        return ajax(options);
    }

    var escape = encodeURIComponent;

    function serialize(params, obj, traditional, scope) {
        var type, array = $.isArray(obj), hash = $.isPlainObject(obj);
        $.each(obj, function (key, value) {
            type = $.type(value);
            if (scope) {
                key = traditional ? scope :
                scope + '[' + (hash || type == 'object' || type == 'array' ? key : '') + ']';
            }
            // handle data in serializeArray() format
            if (!scope && array) {
                params.add(value.name, value.value);
            }
                // recurse into nested objects
            else if (type == 'array' || (!traditional && type == 'object')) {
                serialize(params, value, traditional, key);
            }
            else {
                params.add(key, value);
            }
        });
    }

    function param(obj, traditional) {
        var params = [];
        params.add = function (k, v) {
            this.push(escape(k) + '=' + escape(v));
        };
        serialize(params, obj, traditional);
        return params.join('&').replace(/%20/g, '+');
    }

    //添加post form提交表单 todo:属于扩展对象
    function postForm(form, success, error) {
        if (!form) {
            return;
        }
        form = $(form);
        var submitInputs = [];
        var inputTypes = {
            'color': 1,
            'date': 1,
            'datetime': 1,
            'datetime-local': 1,
            'email': 1,
            'hidden': 1,
            'month': 1,
            'number': 1,
            'password': 1,
            'radio': 1,
            'range': 1,
            'search': 1,
            'tel': 1,
            'text': 1,
            'time': 1,
            'url': 1,
            'week': 1
        };
        var fileType = ['file'];
        var checkTypes = ['checkbox', 'radio'];
        var todoSupport = ['keygen'];
        var eleList = ['input', 'select', 'textarea'];
        var formData = {};

        success = success || function () { };
        error = error || function () { };

        function getFormData() {
            form.find(eleList.join(',')).each(function (i, v) {
                if (v.tagName === 'INPUT') {
                    var ele = $(v);
                    var type = ele.attr('type');
                    if (type in inputTypes) {
                        if (ele.attr('data-ispath')) {
                            formData[ele.attr('name')] = {
                                path: ele.val()
                            }
                        } else {
                            formData[ele.attr('name')] = ele.val();
                        }
                    }
                } else {

                }
            });
        }

        var method = form.attr('method');
        var action = form.attr('action') || location.href;
        method = (method || 'POST').toUpperCase();
        getFormData();
        ajax({
            url: action,
            type: method,
            data: formData,
            success: success,
            error: error
        });
    }

    var offlineClearQueue = [];
    /*
        处理删除离线数据文件回调
        @param string err err对象如果为空则表示 没有错误，否则表示操作出错了
        @param int data表示返回的操作结果，0：处理成功
        @param int dataType操作结果的数据类型，默认正常为2
        @param int optId该操作id
    */
    function processOfflineClearQueue(err, data, dataType, optId) {
        if (offlineClearQueue.length > 0) {
            $.each(offlineClearQueue, function (i, v) {
                if (v && appcan.isFunction(v)) {
                    v(err, data, dataType, optId);
                }
            });
            offlineClearQueue = [];
        }
        return;
    }

    /* 
      清除localStorage中url对应离线缓存数据及离线文件
        url:需要被清除离线数据的url地址
    */

    function clearOffline(url, callback, data) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(url)) {
            argObj = url;
            url = argObj['url'];
            data = argObj['data'];
            callback = argObj['callback'];
        }
        if (!appcan.isFunction(callback)) {
            callback = function () { };
        }
        offlineClearQueue.push(callback);
        var offlineKey = 'offlinedata';
        var offlinedata = appcan.locStorage.val(offlineKey);
        var offlineUrl;
        if (data) {
            var paramsInfo = JSON.stringify(data);
            var fullUrl = url + paramsInfo;
            offlineUrl = appcan.crypto.md5(fullUrl);
        } else {
            offlineUrl = appcan.crypto.md5(url);
        }

        if (offlinedata != null) {
            dataObj = JSON.parse(offlinedata);
            if (dataObj[offlineUrl]) {
                if (dataObj[offlineUrl]['data']) {
                    var localFilePath = dataObj[offlineUrl]['data'];
                    appcan.file.remove({
                        filePath: localFilePath,
                        callback: function (err, data, dataType, optId) {
                            delete dataObj[offlineUrl];
                            appcan.locStorage.val(offlineKey, JSON.stringify(dataObj));
                            processOfflineClearQueue(err, data, dataType, optId);
                        }
                    });
                } else {
                    delete dataObj[offlineUrl];
                    appcan.locStorage.val(offlineKey, JSON.stringify(dataObj));
                    processOfflineClearQueue(null, 0, 2, 0);
                }
            } else {
                processOfflineClearQueue(null, 0, 2, 0);
            }
        } else {
            offlineClearQueue = [];
        }
    }


    module.exports = {
        ajax: function () {
            if (window.uexXmlHttpMgr) {
                ajax.apply(null, arguments);
            } else {
                jQuery.ajax.apply(null, arguments);
            }
        },
        get: function () {
            if (window.uexXmlHttpMgr) {
                get.apply(null, arguments);
            } else {
                jQuery.get.apply(null, arguments);
            }
        },
        post: function () {
            if (window.uexXmlHttpMgr) {
                post.apply(null, arguments);
            } else {
                jQuery.post.apply(null, arguments);
            }
        },
        getJSON: getJSON,
        param: param,
        postForm: postForm,
        clearOffline: clearOffline
    };
});
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展storage 到 appcan 上
    created:2014,08.25
    update:


*/
/*global appcan,window,unescape*/
appcan && appcan.define('locStorage', function ($, exports, module) {

    var storage = window.localStorage,
            i = 0,
            len = 0;
    /*
    从本地存储获取值
    @param String key 设置localstorage的key
    @param String value 设置localstorage的val

    */
    function setValue(key, val) {
        try {
            if (storage) {
                if (!appcan.isString(val)) {
                    val = JSON.stringify(val);
                }
                storage.setItem(key, val);
            } else {

            }
        } catch (e) {

        }
    }
    /*
        批量设置localstorage

    */
    function setValues(key) {
        if (appcan.isPlainObject(key)) {
            for (var k in key) {
                if (key.hasOwnPropery(k)) {
                    setValue(k, key[k]);
                }
            }
        } else if (appcan.isArray(key)) {
            for (i = 0, len = key.length; i < len; i++) {
                if (key[i]) {
                    setValue.apply(this, key[i]);
                }
            }
        } else {
            setValue.apply(this, arguments);
        }
    }

    /*
    从localStorage获取对应的值
    @param String key 获取值的key

    */
    function getValue(key) {
        if (!key) {
            return;
        }
        try {
            if (storage) {
                return storage.getItem(key);
            }
        } catch (e) {

        }
    }
    /*
    从localStorage获取所有的keys

    */
    function getKeys() {
        var res = [];
        var key = '';
        for (var i = 0, len = storage.length; i < len; i++) {
            key = storage.key(i);
            if (key) {
                res.push(key);
            }
        }
        return res;
    }

    /*
    青春对应的key
    @param String key


    */
    function clear(key) {
        try {
            if (key && appcan.isString(key)) {
                storage.removeItem(key);
            } else {
                storage.clear();
            }
        } catch (e) {

        }
    }

    /*
    localStorage剩余空间大小

    */
    function leaveSpace() {
        var space = 1024 * 1024 * 5 - unescape(encodeURIComponent(JSON.stringify(storage))).length;
        return space;
    }

    /*
        获取或者设置localStorage的值
        @param String key
        @param String val
        
    */
    function val(key, value) {
        if (arguments.length === 1) {
            return getValue(key);
        }
        setValue(key, value);
    }

    module.exports = {
        getVal: getValue,
        setVal: setValues,
        leaveSpace: leaveSpace,
        remove: clear,
        keys: getKeys,
        val: val
    };

});
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展user 到appcan对象上
    created:2014,08.21
    update:


*/
/*global window,appcan*/
window.appcan && appcan.extend(function (ac, exports, module) {
    /*
    字符组去除前后空格
    @param String str 要去空格的字符串


    */
    var trim = function (str) {
        if (!str) {
            return '';
        }
        if (String.prototype.trim) {
            return String.prototype.trim.call(str);
        }
        return str.replace(/^\s+|\s+$/ig, '');
    };
    /*
    字符组去除前面空格
    @param String str 要去空格的字符串


    */
    var trimLeft = function (str) {
        if (!str) {
            return '';
        }
        if (String.prototype.trimLeft) {
            return String.prototype.trimLeft.call(str);
        }
        return str.replace(/^\s+/ig, '');
    };

    /*
    字符组去除后面空格
    @param String str 要去空格的字符串


    */
    var trimRight = function (str) {
        if (!str) {
            return '';
        }
        if (String.prototype.trimRight) {
            return String.prototype.trimRight.call(str);
        }
        return str.replace(/\s+$/ig, '');
    };
    /*
    获取字符串的字节长度
    @param String str

    */
    var byteLength = function (str) {
        if (!str) {
            return 0;
        }
        var totalLength = 0;
        var i;
        var charCode;
        for (i = 0; i < str.length; i++) {
            charCode = str.charCodeAt(i);
            if (charCode < 0x007f) {
                totalLength = totalLength + 1;
            } else if ((0x0080 <= charCode) && (charCode <= 0x07ff)) {
                totalLength += 2;
            } else if ((0x0800 <= charCode) && (charCode <= 0xffff)) {
                totalLength += 3;
            }
        }
        return totalLength;
    };

    module.exports = {
        trim: trim,
        trimLeft: trimLeft,
        trimRight: trimRight,
        byteLength: byteLength
    };

});
;/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:扩展user 到appcan对象上
    created:2014,08.21
    update:


*/
/*global appcan*/
appcan && appcan.define('User', function ($, exports, module) {
    var Backbone = appcan.require('Backbone');
    var db = appcan.require('database');
    var User = Backbone.Model.extend({
        login: function () {

        },
        signup: function () {

        },
        logout: function () {


        },
        changePassword: function () {

        }
    });
    module.exports = User;
});
;/*

    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan view模块
    create:2014.08.19
    update:______/___author___


*/
/*global window,appcan*/
window.appcan && appcan.define('view', function ($, exports, module) {
    var _ = appcan.require('underscore');
    var settings = {};
    /*
        配置模版参数
        @param Object newSettings 新的配置信息

    */
    var config = function (newSettings) {
        settings = _.defaults({}, settings, newSettings);
    };
    /*
        替换内容到制定的元素中
        @param String selector 选择器
        @param String template 模板
        @param Object dataSource 数据源
        @param Object options 参数

    */
    var renderTemp = function (selector, template, dataSource, options) {
        options = _.defaults({}, settings, options);
        var render = _.template(template, options);
        var dataRes = render(dataSource);
        $(selector).html(dataRes);
        return dataRes;
    };
    /*
        附加内容到指定的元素中
        @param String selector 选择器
        @param String template 模板
        @param Object dataSource 数据源
        @param Object options 参数

    */
    var apRenderTemp = function (selector, template, dataSource, options) {
        options = _.defaults({}, settings, options);
        var render = _.template(template, options);
        var dataRes = render(dataSource);
        $(selector).append(dataRes);
        return dataRes;
    };
    module.exports = {
        template: _.template,
        render: renderTemp,
        appendRender: apRenderTemp,
        config: config
    };
});
;/*

    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan window模块
    create:2014.08.18
    update:______/___author___


*/
/*global window,appcan,uexWindow*/

window.appcan && appcan.define('window', function ($, exports, module) {

    var subscribeGlobslQueue = [];//订阅队列
    var bounceCallQueue = [];//
    var multiPopoverQueue = {};
    var currentOS = '';
    var keyFuncMapper = {};//映射

    /*
        捕获android实体键
        @param String id 实体键的id
        @param Function callback 当点击时出发的回调函数
    
    
    */
    function monitorKey(id, callback) {
        keyFuncMapper[id] = callback;
        uexWindow.setReportKey(id);
        uexWindow.onKeyPressed = function (keyCode) {
            keyFuncMapper[keyCode] && keyFuncMapper[keyCode](keyCode);
        }
    }

    /*
    打开一个新窗口
    @param String name 新窗口的名字 如果该window已经存在则直接打开
    @param String dataType 数据类型：0：url 1：html 数据 2：html and url
    @param String data 载入的数据
    @param Int aniId 动画id：
        0：无动画
        1:从左向右推入
        2:从右向左推入
        3:从上向下推入
        4:从下向上推入
        5:淡入淡出
        6:左翻页
        7:右翻页
        8:水波纹
        9:由左向右切入
        10:由右向左切入
        11:由上先下切入
        12:由下向上切入

        13:由左向右切出
        14:由右向左切出
        15:由上向下切出
        16:由下向上切出
    @param int width 窗口宽度
    @param int height 窗口的高度
    @param int tpye 窗口的类型
        0:普通窗口
        1:OAuth 窗口
        2:加密页面窗口
        4:强制刷新
        8:url用系统浏览器打开
        16:view不透明
        32:隐藏的winwdow
        64:等待popOver加载完毕后显示
        128:支持手势
        256:标记opn的window上一个window不隐藏
        512:标记呗open的浮动窗口用友打开wabapp
    @param animDuration 动画时长


    */
    function open(name, data, aniId, type, dataType, width, height, animDuration, extraInfo) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            argObj = name;
            name = argObj['name'];
            dataType = argObj['dataType'] || 0;
            aniId = argObj['aniId'] || 0;
            width = argObj['width'];
            height = argObj['height'];
            type = argObj['type'] || 0;
            animDuration = argObj['animDuration'];
            extraInfo = argObj['extraInfo'];
            data = argObj['data'];
        }
        dataType = dataType || 0;
        aniId = aniId || 0;
        type = type || 0;
        animDuration = animDuration || 300;

        try {
            extraInfo = appcan.isString(extraInfo) ? extraInfo : JSON.stringify(extraInfo);
            extraInfo = JSON.parse(extraInfo);
            if (!extraInfo.extraInfo) {
                extraInfo = { extraInfo: extraInfo };
            }
            extraInfo = JSON.stringify(extraInfo);
        } catch (e) {
            extraInfo = extraInfo || '';
        }

        //打开新窗口
        uexWindow.open(name, dataType, data, aniId, width, height, type, animDuration, extraInfo);
    }

    /*
    关闭窗口
    @param String animateId 窗口关闭动画
    @param Int animDuration 动画持续时间

    */
    function close(animId, animDuration) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(animId)) {
            argObj = animId;
            animId = argObj['animId'];
            animDuration = argObj['animDuration'];
        }
        if (animId) {
            animId = parseInt(animId, 10);
            if (isNaN(animId) || animId > 16 || animId < 0) {
                animId = -1;
            }
        }
        if (animDuration) {
            animDuration = parseInt(animDuration, 10);
            animDuration = isNaN(animDuration) ? '' : animDuration;
        }
        animDuration = animDuration || 300;
        uexWindow.close(animId, animDuration);
    }

    /*
    在指定的窗口执行js脚本

    @param string name 窗口的名字
    @param string type 窗口类型
    @param string inscript 窗口内容

    */
    function evaluateScript(name, scriptContent, type) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            argObj = name;
            name = argObj['name'];
            type = argObj['type'] || 0;
            scriptContent = argObj['scriptContent'];
        }
        type = type || 0;
        uexWindow.evaluateScript(name, type, scriptContent);
    }
    /*
    在指定的浮动窗口中执行脚本
    @param String name 执行的窗口名字
    @param String popName 浮动窗口名
    @param String scriptContent 脚本内容

    */
    function evaluatePopoverScript(name, popName, scriptContent) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            argObj = name;
            name = argObj['name'];
            popName = argObj['popName'] || 0;
            scriptContent = argObj['scriptContent'];
        }
        name = name || '';
        if (!appcan.isString(popName) || !popName) {
            return;
        }
        uexWindow.evaluatePopoverScript(name, popName, scriptContent);
    }

    /*
    设置窗口的上拉，下拉效果
    @param String bounceType 弹动效果类型
        0:无任何效果
        1:颜色弹动效果
        2:设置图片弹动
    @param Function downEndCall 下拉到底的回调
    @param Function upEndCall 上拉到底的回调
    @param String color 设置下拉视图的颜色
    @param String imgSettings 设置显示内容
    
    todo:该方法需要重写，

    */

    function setBounce(bounceType, startPullCall, downEndCall, upEndCall, color, imgSettings) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(bounceType)) {
            argObj = bounceType;
            bounceType = argObj['bounceType'] || 1;
            startPullCall = argObj['startPullCall'];
            downEndCall = argObj['downEndCall'];
            upEndCall = argObj['upEndCall'];
            color = argObj['color'] || 'rgba(255,255,255,0)';
            imgSettings = argObj['imgSettings'] || '{"imagePath":"res://reload.png",' +
            '"textColor":"#530606","pullToReloadText":"拖动刷新",' +
            '"releaseToReloadText":"释放刷新",' +
            '"loadingText":"加载中，请稍等"}';

        }
        color = color || 'rgba(255,255,255,0)';
        imgSettings = imgSettings || '{"imagePath":"res://reload.png",' +
        '"textColor":"#530606","pullToReloadText":"拖动刷新",' +
        '"releaseToReloadText":"释放刷新",' +
        '"loadingText":"加载中，请稍等"}';

        // if(!bounceType){
        // return;
        // }
        var startBounce = 1;
        //绑定回弹通知函数
        uexWindow.onBounceStateChange = function (type, status) {
            if (status == 0) {
                startPullCall && startPullCall(type);
            }
            if (status == 1) {
                downEndCall && downEndCall(type);
            }
            if (status == 2) {
                upEndCall && upEndCall(type);
            }
        };
        uexWindow.setBounce(startBounce);
        //设置颜色
        /*if(bounceType == 1){
            uexWindow.showBounceView('0',color,'1');
        }
        if(bounceType == 2){
            uexWindow.setBounceParams('0',imgSettings);
            uexWindow.showBounceView('0',color,1);
        }*/
        //绑定下拉回调
        if (startPullCall || downEndCall || upEndCall) {
            if (!appcan.isArray(bounceType)) {
                bounceType = [bounceType];
            }
            for (var i = 0; i < bounceType.length; i++) {
                uexWindow.notifyBounceEvent(bounceType[i], '1');

                setBounceParams(bounceType[i], imgSettings);
                uexWindow.showBounceView(bounceType[i], color, '1');


            }

        }
    }


    var bounceStateQueue = [];
    /*
处理回调获取弹动状态
@param string msg 传递过来的消息

*/
    function processGetBounceStateQueue(data, dataType, opId) {
        if (bounceStateQueue.length > 0) {
            $.each(bounceStateQueue, function (i, v) {
                if (v && appcan.isFunction(v)) {
                    if (dataType == 2) {
                        v(data, dataType, opId);
                    }

                }
            });
        }
        bounceStateQueue = [];
        return;
    }

    /*
        获取当前的弹动状态
        
        
    */
    function getBounceStatus(callback) {
        if (arguments.length === 1 && appcan.isPlainObject(callback)) {
            callback = callback['callback'];
        }
        if (!appcan.isFunction(callback)) {
            return;
        }
        bounceStateQueue.push(callback);
        uexWindow.cbBounceState = function (opId, dataType, data) {
            processGetBounceStateQueue(data, dataType, opId);
        };

        uexWindow.getBounce();
    }

    /*
        开启上下滑动回弹
        
        
    */
    function enableBounce() {
        //1:开启回弹效果
        uexWindow.setBounce(1);
    }

    /*
        关闭回弹效果
    
    */
    function disableBounce() {
        //0:禁用回弹效果
        uexWindow.setBounce(0);
    }

    /*
        设置回弹类
        @param String type 设置回弹的类型 
        @param String color 设置回弹显示的颜色 
        @param Int flag 设置是否添加回弹回调 
        @param Function callback 回弹的回调函数 
        
    
    */
    function setBounceType(type, color, flag, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(type)) {
            flag = type.flag;
            color = type.color;
            callback = type.callback;
            type = type.type;
        }
        flag = (flag === void 0 ? 1 : flag);
        flag = parseInt(flag, 10);
        color = color || 'rgba(0,0,0,0)';
        type = (type === void 0 ? 0 : type);
        callback = callback || function () { };
        //强制开启页面弹动效果
        enableBounce();

        uexWindow.showBounceView(type, color, flag);
        if (flag) {
            bounceCallQueue.push({ type: type, callback: callback });
            uexWindow.onBounceStateChange || (uexWindow.onBounceStateChange = function (backType, status) {
                var currCallObj = null;
                for (var i = 0, len = bounceCallQueue.length; i < len; i++) {
                    currCallObj = bounceCallQueue[i];
                    if (currCallObj) {
                        if (backType === currCallObj.type) {
                            if (appcan.isFunction(currCallObj.callback)) {
                                currCallObj.callback(status, type);
                            }
                        }
                    }
                }
            });
            //1:接收回调函数
            uexWindow.notifyBounceEvent(type, 1);
        }
    }

    /*
        给上下回弹添加参数
        @param String position 设置回弹的类型
        @param Object data 要设置回弹显示出来内容的json参数
        {
            imagePath:'回弹显示的图片路径',
            textColor:'设置回弹内容的字体颜色',
            levelText:'设置文字的级别',
            pullToReloadText:'下拉超过边界显示出的内容',
            releaseToReloadText:'拖动超过刷新边界后显示的内容',
            loadingText:'拖动超过刷新临界线并且释放，并且拖动'
        }
    
    */
    function setBounceParams(position, data) {
        if (arguments.length === 1 && appcan.isPlainObject(position)) {
            data = position.data;
            position = position.position;
        }
        if (appcan.isPlainObject(data)) {
            data = JSON.stringify(data);
        }
        uexWindow.setBounceParams(position, data);
    }


    /*
    展示弹动结束后显示的网页
    @param String position 0为顶端恢复弹动，1为底部恢复弹动

    */

    function resetBounceView(position) {
        if (appcan.isPlainObject(position)) {
            position = position['position'];
        }
        position = parseInt(position, 10);
        if (isNaN(position)) {
            position = 0;
        } else {
            position = position;
        }
        position = position || 0;
        uexWindow.resetBounceView(position);
    }

    /*
    弹出一个非模态的提示框
    @param String type 消息提示框显示的模式
        0:没有进度条
        1:有进度条
    @param String position 提示在手机上的位置
        1:left_top
        2:top
        3:right_top
        4:left
        5:middle
        6:right
        7:bottom_left
        8:bottom
        9:right_bottom
    @param String msg 提示内容
    @param String duration 提示框存在时间，小于0不会自动关闭


    */


    function openToast(msg, duration, position, type) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(msg)) {
            argObj = msg;
            msg = argObj['msg'];
            duration = argObj['duration'];
            position = argObj['position'] || 5;
            type = argObj['type'];
        }
        type = type || (duration ? 0 : 1);
        duration = duration || 0;
        position = position || 5;
        //执行跳转
        uexWindow.toast(type, position, msg, duration);
    }

    /*
    关闭提示框

    */
    function closeToast() {
        uexWindow.closeToast();
    }

    /*
    移动浮动窗口位置动画
    @param String left 距离左边界的位置
    @param String top 距离上边界的位置
    @param Function callback 动画完成的回调函数
    @param Int duration 动画的移动时间

    */

    function moveAnim(left, top, callback, duration) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(left)) {
            argObj = left;
            left = argObj['left'] || 0;
            top = argObj['top'] || 0;
            callback = argObj['callback'];
            duration = argObj['duration'] || 250;
        }
        left = left || 0;
        top = top || 0;
        duration = duration || 250;
        uexWindow.beginAnimition();
        uexWindow.setAnimitionDuration(duration);
        uexWindow.setAnimitionRepeatCount('0');
        uexWindow.setAnimitionAutoReverse('0');
        uexWindow.makeTranslation(left, top, '0');
        uexWindow.commitAnimition();
        if (appcan.isFunction(callback)) {
            uexWindow.onAnimationFinish = callback;
        }
    }

    /*
        
    
    */

    function setWindowFrame(dx, dy, duration, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(dx)) {
            argObj = dx;
            dx = argObj['dx'] || 0;
            dy = argObj['dy'] || 0;
            duration = argObj['duration'] || 250;
            callback = argObj['callback'] || function () { };
        }
        uexWindow.onSetWindowFrameFinish = callback;
        uexWindow.setWindowFrame(dx, dy, duration);
    }


    /*
    依指定的样式弹出一个提示框
    @param String selector css选择器
    @param String url 加载的数据内容
    @param String left 居左距离
    @param String top 居上距离
    @param String name 弹窗名称

    */

    function popoverElement(id, url, left, top, name, extraInfo) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(id)) {
            argObj = id;
            id = argObj['id'] || 0;
            url = argObj['url'];
            top = argObj['top'];
            left = argObj['left'];
            extraInfo = argObj['extraInfo'];
            name = argObj['name'];
        }
        top = top || 0;
        left = left || 0;
        var ele = $('#' + id);
        var width = ele.width();
        var height = ele.height();
        var fontSize = ele.css('font-size');
        top = parseInt(top, 10);
        top = isNaN(top) ? ele.offset().top : top;//默认使用元素本身的top
        left = parseInt(left, 10);
        left = isNaN(left) ? ele.offset().left : left;//默认使用元素本身的left
        name = name ? name : id;

        extraInfo = extraInfo || '';

        //fixed xiaomi 2s bug
        fontSize = parseInt(fontSize, 10);
        fontSize = isNaN(fontSize) ? 0 : fontSize;

        openPopover(name, 0, url, '', left, top, width, height, fontSize, 0, 0, extraInfo);
    }

    /*
    打开一个浮动窗口
    @param String name 浮动窗口名
    @param String dataType 数据类型0:url 1:html 2:url html
    @param String url  url地址
    @param String data 数据
    @param Int left 居左距离
    @param Int top 居上距离
    @param Int width 宽
    @param Int height 高
    @param Int fontSize 默认字体
    @param Int tpye 窗口的类型
        0:普通窗口
        1:OAuth 窗口
        2:加密页面窗口
        4:强制刷新
        8:url用系统浏览器打开
        16:view不透明
        32:隐藏的winwdow
        64:等待popOver加载完毕后显示
        128:支持手势
        256:标记opn的window上一个window不隐藏
        512:标记呗open的浮动窗口用友打开wabapp
    
    @param Int bottomMargin 浮动窗口相对父窗口底部的距离。为空或0时，默认为0。当值不等于0时，inHeight参数无效


    */
    function openPopover(name, dataType, url, data, left, top, width, height, fontSize, type, bottomMargin, extraInfo) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            argObj = name;
            name = argObj['name'];
            dataType = argObj['dataType'];
            url = argObj['url'];
            data = argObj['data'];
            left = argObj['left'];
            top = argObj['top'];
            width = argObj['width'];
            height = argObj['height'];
            fontSize = argObj['fontSize'];
            type = argObj['type'];
            bottomMargin = argObj['bottomMargin'];
            extraInfo = argObj['extraInfo'];
        }
        dataType = dataType || 0;
        left = left || 0;
        top = top || 0;
        height = height || 0;
        width = width || 0;
        type = type || 0;
        bottomMargin = bottomMargin || 0;
        fontSize = fontSize || 0;
        data = data || '';
        //fixed xiaomi 2s bug
        fontSize = parseInt(fontSize, 10);
        fontSize = isNaN(fontSize) ? 0 : fontSize;

        try {
            extraInfo = appcan.isString(extraInfo) ? extraInfo : JSON.stringify(extraInfo);
            extraInfo = JSON.parse(extraInfo);
            if (!extraInfo.extraInfo) {
                extraInfo = { extraInfo: extraInfo };
            }
            extraInfo = JSON.stringify(extraInfo);
        } catch (e) {
            extraInfo = extraInfo || '';
        }

        //fixed ios bug
        if (uexWidgetOne.platformName && uexWidgetOne.platformName.toLowerCase().indexOf('ios') > -1) {
            var args = ['"' + name + '"', dataType, '"' + url + '"', '"' + data + '"', left, top, width, height, fontSize, type, bottomMargin, "'" + extraInfo + "'"];
            var scriptContent = 'uexWindow.openPopover(' + args.join(',') + ')';
            evaluateScript('', scriptContent);
            return;
        }
        uexWindow.openPopover(name, dataType, url, data, left, top, width, height, fontSize, type, bottomMargin, extraInfo);
    }

    /*
    关闭浮动按钮
    @param String name 浮动窗口的名字

    */

    function closePopover(name) {
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            name = name['name'];
        }
        uexWindow.closePopover(name);
    }


    /*
    根据制定元素重置提示框的位置大小
    @param String id 元素id
    @param String left 距左边距离
    @param String top 距上边的距离
    @param String name 名称，默认为id
    */

    function resizePopoverByEle(id, left, top, name) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(id)) {
            argObj = id;
            id = argObj['id'];
            left = argObj['left'];
            top = argObj['top'];
            name = argObj['name'];
        }
        left = left || 0;
        top = top || 0;
        var ele = $('#' + id);
        var width = ele.width();
        var height = ele.height();
        left = parseInt(left, 10);
        left = isNaN(left) ? 0 : left;
        top = parseInt(top, 10);
        top = isNaN(top) ? 0 : top;
        name = name ? name : id;
        uexWindow.setPopoverFrame(name, left, top, width, height);
    }

    /*
    重置提示框的位置大小
    @param String name popover名
    @param String left 距左边距离
    @param String top 距上边的距离
    @param String width 窗口的宽
    @param String height 窗口的高


    */

    function resizePopover(name, left, top, width, height) {
        var argObj = null;
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            argObj = name;
            name = argObj['name'];
            left = argObj['left'];
            top = argObj['top'];
            width = argObj['width'];
            height = argObj['height'];
        }
        left = left || 0;
        top = top || 0;
        width = width || 0;
        height = height || 0;

        left = parseInt(left, 10);
        left = isNaN(left) ? 0 : left;

        top = parseInt(top, 10);
        top = isNaN(top) ? 0 : top;

        width = parseInt(width, 10);
        width = isNaN(width) ? 0 : width;

        height = parseInt(height, 10);
        height = isNaN(height) ? 0 : height;

        uexWindow.setPopoverFrame(name, left, top, width, height);
    }


    /*
    弹出一个确认框
    @param String title 对话框的标题
    @param String content 对话框的内容
    @param Array buttons 按钮文字


    */
    function windowConfirm(title, content, buttons, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(title)) {
            callback = title['callback'];
            buttons = title['buttons'];
            content = title['content'];
            title = title['title'];
        }
        title = title || '提示';
        buttons = buttons || ['确定'];
        buttons = appcan.isArray(buttons) ? buttons : [buttons];
        popConfirm(title, content, buttons, callback);
    }

    /*
    弹出一个警告框
    @param String title 对话框的标题
    @param String content 对话框的内容
    @param Array buttons 按钮文字


    */
    function popAlert(title, content, buttons) {
        if (arguments.length === 1 && appcan.isPlainObject(title)) {
            buttons = title['buttons'];
            content = title['content'];
            title = title['title'];
        }
        buttons = appcan.isArray(buttons) ? buttons : [buttons];
        uexWindow.alert(title, content, buttons[0]);
    }

    /*
        弹出一个提示框
        @param String title 对话框的标题
        @param String content 对话框的内容
        @param Array buttons 按钮文字
        
        
        
    */
    function popConfirm(title, content, buttons, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(title)) {
            callback = title['callback'];
            buttons = title['buttons'];
            content = title['content'];
            title = title['title'];
        }
        buttons = appcan.isArray(buttons) ? buttons : [buttons];
        if (appcan.isFunction(callback)) {
            uexWindow.cbConfirm = function (optId, dataType, data) {
                if (dataType != 2) {
                    return callback(new Error('confirm error'));
                }
                callback(null, data, dataType, optId);
            };
        }

        uexWindow.confirm(title, content, buttons);
    }

    /*
        弹出一个可提示用户输入的对话框
        @param String title 对话框的标题
        @param String content 对话框显示的内容
        @param String defaultValue 输入框默认文字
        @param Array  buttons 显示在按钮上的文字集合
        @param Function callback  对话框关闭的回调 
        
        
    */
    function popPrompt(title, content, defaultValue, buttons, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(title)) {
            callback = title['callback'];
            buttons = title['buttons'];
            content = title['content'];
            defaultValue = title['defaultValue'];
            title = title['title'];
        }
        buttons = appcan.isArray(buttons) ? buttons : [buttons];
        if (appcan.isFunction(callback)) {
            uexWindow.cbPrompt = function (optId, dataType, data) {
                try {
                    var data = JSON.parse(data);
                    callback(null, data, dataType, optId);
                }
                catch (e) {
                    callback(e);
                }
            };
        }

        uexWindow.prompt(title, content, defaultValue, buttons);
    }

    /*
    把指定的浮动窗口排在所有浮动窗口最上面
    @param String name 浮动窗口的名字

    */

    function bringPopoverToFront(name) {
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            name = name['name'];
        }
        uexWindow.bringPopoverToFront(name);
    }

    /*
    把指定的浮动窗口排在所有浮动窗口最下面
    @param String name 浮动窗口的名字

    */

    function sendPopoverToBack(name) {
        if (arguments.length === 1 && appcan.isPlainObject(name)) {
            name = name['name'];
        }
        uexWindow.sendPopoverToBack(name);
    }

    /*
        订阅一个频道消息,当有消息发布的时候该该回调将会调用该回调
        @param Int channelId 频道id
        @param Function callback回调函数
        
    */
    function subscribe(channelId, callback) {
        if (arguments.length === 1 && appcan.isPlainObject(channelId)) {
            callback = channelId['callback'];
            channelId = channelId['channelId'];
        }
        if (!appcan.isFunction(callback)) {
            return;
        }
        var funName = 'notify_callback_' + appcan.getUID();
        uexWindow[funName] = callback;
        uexWindow.subscribeChannelNotification(channelId, funName);
    }

    /*
        发布一个消息
        @param Int channelId :频道id
        @param String msg : 要发布的消息
    
    */
    function publish(channelId, msg) {
        if (arguments.length === 1 && appcan.isPlainObject(channelId)) {
            msg = channelId['msg'];
            channelId = channelId['channelId'];
        }
        if (appcan.isPlainObject(msg)) {
            msg = JSON.stringify(msg);
        }
        uexWindow.publishChannelNotification(channelId, msg);
    }

    /*
        向全局的公共频道发送消息
        @param String msg 向全局频道发送消息
    
    */

    function publishGlobal(msg) {
        if (arguments.length === 1 && appcan.isPlainObject(msg)) {
            msg = msg['msg'];
        }
        uexWindow.postGlobalNotification(msg);
    }

    /*
        处理全局回调订阅消息
        @param string msg 传递过来的消息
    
    */
    function processSubscribeGolbalQueue(msg) {
        if (subscribeGlobslQueue.length > 0) {
            $.each(subscribeGlobslQueue, function (i, v) {
                if (v && appcan.isFunction(v)) {
                    v(msg);
                }
            });
        }
        return
    }

    /*
        订阅全局的频道
        @param Function callback 订阅的回调
    
    */
    function subscribeGlobal(callback) {
        if (arguments.length === 1 && appcan.isPlainObject(callback)) {
            callback = callback['callback'];
        }
        if (!appcan.isFunction(callback)) {
            return;
        }
        subscribeGlobslQueue.push(callback);
        uexWindow.onGlobalNotification = function (msg) {
            processSubscribeGolbalQueue(msg);
        };
    }

    /*
        移除全局订阅事件
        @param Function callback：要移除的回调的引用
    
    */
    function removeGlobalSubscribe(callback) {
        if (arguments.length === 1 && appcan.isPlainObject(callback)) {
            callback = callback['callback'];
        }
        if (!appcan.isFunction(callback)) {
            return;
        }
        for (var i = 0, len = subscribeGlobslQueue.length; i < len; i++) {
            if (subscribeGlobslQueue[i] === callback) {
                subscribeGlobslQueue.splice(i, 1);
                return;
            }
        }
    }

    /*
        处理多窗口滑动回调事件
        
    */
    function processMultiPopover(err, res) {
        if (err) {
            //todo call error
        } else {
            if (appcan.isString(res)) {
                res = JSON.parse(res);
            }
            if (!res.multiPopName) {
                return;
            }
            var multiCalls = multiPopoverQueue[res.multiPopName];
            $.each(multiCalls, function (i, fun) {
                if (appcan.isFunction(fun)) {
                    fun(null, res);
                }
            });
        }
    }

    /*
        弹出多页面浮动窗口
        @param String popName:弹出窗口的名称 
        @param String content:传入的数据 
        @param String dataType:传入数据的类型 0：url方式载入；1：html内容 方式载入；2：既有url方式，又有html内容方式 
        @param Int left:距离左边的距离 
        @param Int top:距离上边界的距离 
        @param Int width:弹出窗口的宽
        @param Int height:弹出窗口的高 
        @param Int fontSize:字体大小 
        @param Int flag:弹出类型的标识
        @param Int indexSelected:默认选中的窗口 
        
    
    */
    function openMultiPopover(popName, content, dataType, left, top, width, height, change, fontSize, flag, indexSelected, extraInfo) {
        if (arguments.length === 1 && appcan.isPlainObject(popName)) {
            indexSelected = popName['indexSelected'];
            flag = popName['flag'];
            fontSize = popName['fontSize'];
            change = popName['change'];
            height = popName['height'];
            width = popName['width'];
            top = popName['top'];
            left = popName['left'];
            dataType = popName['dataType'];
            content = popName['content'];
            extraInfo = popName['extraInfo']
            popName = popName['popName'];
        }
        dataType = dataType || 0;
        flag = flag || 0;
        indexSelected = parseInt(indexSelected, 10);
        indexSelected = isNaN(indexSelected) ? 0 : indexSelected;
        width = width || 0;
        height = height || 0;
        change = change || function () { };

        try {
            extraInfo = appcan.isString(extraInfo) ? extraInfo : JSON.stringify(extraInfo);
            extraInfo = JSON.parse(extraInfo);
            if (!extraInfo.extraInfo) {
                extraInfo = { extraInfo: extraInfo };
            }
            extraInfo = JSON.stringify(extraInfo);
        } catch (e) {
            extraInfo = extraInfo || '';
        }

        //fixed android 如果少任何一个key就会crash bug
        if (!appcan.isString(content)) {
            if (!content.content) {
                content = {
                    content: content
                };
            }
        } else {
            content = JSON.parse(conent);
            if (!content.content) {
                content = {
                    content: content
                };
            }
        }
        //check all key
        var mustKey = ['inPageName', 'inUrl', 'inData'];
        var realContent = content.content;
        $.each(realContent, function (i, v) {
            $.each(mustKey, function (i1, v1) {
                if (!(v1 in v)) {
                    v[v1] = '';
                }
            });
        });
        //content
        content = JSON.stringify(content);
        if (multiPopoverQueue[popName]) {
            multiPopoverQueue[popName].push(change);
        } else {
            multiPopoverQueue[popName] = [change];
        }
        uexWindow.openMultiPopover(content, popName, dataType, left, top, width, height, fontSize, flag, indexSelected, extraInfo);
        uexWindow.cbOpenMultiPopover = function (optId, dataType, res) {
            if (optId == 0) {
                if (dataType != 1) {
                    processMultiPopover(new Error('multi popover error'));
                } else {
                    processMultiPopover(null, res);
                }
            }
        };
        //fixed ios indexed bug
        setSelectedPopOverInMultiWindow(popName, indexSelected);
    }

    /*
        关闭多页面浮动窗口
        @param String popName:多页面弹出窗口
        
    
    */
    function closeMultiPopover(popName) {
        if (arguments.length === 1 && appcan.isPlainObject(popName)) {
            popName = popName['popName'];
        }
        if (!popName) {
            return;
        }

        uexWindow.closeMultiPopover(popName)

    }

    /*
        设置多页面浮动窗口跳转到的子页面窗口的索引
        @param String popName:多窗口弹出框的名称
        @param String index:页面的索引 
        
        
    */
    function setSelectedPopOverInMultiWindow(popName, index) {
        if (arguments.length === 1 && appcan.isPlainObject(popName)) {
            index = popName['index'];
            popName = popName['popName'];
        }
        if (!popName) {
            return;
        }
        index = parseInt(index, 10);
        index = isNaN(index) ? 0 : index;
        //fixed 模拟器不支持MultiPopOver bug
        uexWindow.setSelectedPopOverInMultiWindow && uexWindow.setSelectedPopOverInMultiWindow(popName, index);

    }



    var windowStatusCallList = [];

    /*
        
        处理窗口回调事件
        @param Function state:当前的状态
        
        
    */
    function processWindowStateChange(state) {
        $.each(windowStatusCallList, function (i, v) {
            if (appcan.isFunction(v)) {
                v(state);
            }
        })
    }


    /*
        当前窗口的状态改变
        @param Function callback:窗口事件改变后的回调函数
        
    
    */
    function onStateChange(callback) {
        if (!appcan.isFunction(callback)) {
            return;
        }
        //兼容老用法

        windowStatusCallList.push(callback);

        uexWindow.onStateChange = processWindowStateChange;
    }

    /*
        默认状态改变事件
        
    
    */

    function defaultStatusChange(state) {
        var tmpResumeCall = null;
        var tmpPauseCall = null;
        if (appcan.window.onResume) {
            tmpResumeCall = appcan.window.onResume;
        }
        if (appcan.window.onPause) {
            tmpPauseCall = appcan.window.onPause;
        }

        if (state === 0) {
            appcanWindow.emit('resume');
            tmpResumeCall && tmpResumeCall();
        }

        if (state === 1) {
            appcanWindow.emit('pause');
            tmpPauseCall && tmpPauseCall();
        }

    }


    /*
    
        swipe回调列表
        
        
    */
    var swipeCallbackList = {
        left: [],
        right: []
    };

    function processSwipeLeft() {

        $.each(swipeCallbackList.left, function (i, v) {
            if (appcan.isFunction(v)) {
                v();
            }
        })

    }

    function processSwipeRight() {

        $.each(swipeCallbackList.right, function (i, v) {
            if (appcan.isFunction(v)) {
                v();
            }
        })
    }

    /*
        当页面滑动的时候，执行的回调方法
        
    
    */
    function onSwipe(direction, callback) {

        if (direction === 'left') {
            swipeCallbackList[direction].push(callback);

            uexWindow.onSwipeLeft = processSwipeLeft;
            return;
        }
        if (direction === 'right') {
            swipeCallbackList[direction].push(callback);

            uexWindow.onSwipeRight = processSwipeRight;
            return;
        }
    }

    function onSwipeLeft(callback) {
        if (!appcan.isFunction(callback)) {
            return;
        }
        onSwipe('left', callback);
    }

    function onSwipeRight(callback) {
        if (!appcan.isFunction(callback)) {
            return;
        }
        onSwipe('right', callback);
    }

    /*
        
        兼容原始appcan.frame.onSwipeLeft 和 appcan.window.onSwipeLeft 方法
        
    
    */
    function defaultSwipeLeft() {
        var tmpSwipeLeftCall = null;
        var tmpFrameSLCall = null;

        if (appcan.window.onSwipeLeft) {
            tmpSwipeLeftCall = appcan.window.onSwipeLeft;
        }

        if (appcan.frame.onSwipeLeft) {
            tmpFrameSLCall = appcan.frame.onSwipeLeft;
        }

        appcanWindow.emit('swipeLeft');
        appcan.frame && appcan.frame.emit && appcan.frame.emit('swipeLeft');
        tmpSwipeLeftCall && tmpSwipeLeftCall();
        tmpFrameSLCall && tmpFrameSLCall();

    }


    /*
       
       兼容原始appcan.frame.onSwipeRight 和 appcan.window.onSwipeRight 方法
       
   
   */
    function defaultSwipeRight() {
        var tmpSwipeRightCall = null;
        var tmpFrameSRCall = null;

        if (appcan.window.onSwipeRight) {
            tmpSwipeRightCall = appcan.window.onSwipeRight;
        }

        if (appcan.frame.onSwipeRight) {
            tmpFrameSRCall = appcan.frame.onSwipeRight;
        }

        appcanWindow.emit('swipeRight');
        appcan.frame && appcan.frame.emit && appcan.frame.emit('swipeRight');
        tmpSwipeRightCall && tmpSwipeRightCall();
        tmpFrameSRCall && tmpFrameSRCall();
    }
    /*
        
        控制父组件是否拦截子组件的事件 
        @param Int enable 设置父组件是否拦截子组件的事件,参数不为1时设置默认拦截；0:可以拦截，子组件不可以正常响应事件 ；1:不拦截，子组件可以正常响应事件 
    */
    function setMultilPopoverFlippingEnbaled(enable) {
        var enable = parseInt(enable, 10);
        enable = isNaN(enable) ? 0 : enable;
        enable = enable != 1 ? 0 : enable;
        uexWindow.setMultilPopoverFlippingEnbaled(enable);
    }


    //默认绑定状态
    appcan.ready(function () {
        //绑定默认状态改变事件
        onStateChange(defaultStatusChange);
        //绑定默认swipe事件
        onSwipeLeft(defaultSwipeLeft);
        //绑定默认swipe事件
        onSwipeRight(defaultSwipeRight);
    });

    //导出接口
    var appcanWindow = module.exports = {
        open: open,
        close: close,
        evaluateScript: evaluateScript,
        evaluatePopoverScript: evaluatePopoverScript,
        setBounce: setBounce,
        setBounceParams: setBounceParams,
        enableBounce: enableBounce,
        disableBounce: disableBounce,
        setBounceType: setBounceType,
        resetBounceView: resetBounceView,
        openToast: openToast,
        closeToast: closeToast,
        moveAnim: moveAnim,
        popoverElement: popoverElement,
        openPopover: openPopover,
        closePopover: closePopover,
        resizePopover: resizePopover,
        resizePopoverByEle: resizePopoverByEle,
        alert: windowConfirm,
        //popAlert:popAlert, 隐藏该接口，因为confirm
        confirm: popConfirm,
        prompt: popPrompt,
        bringPopoverToFront: bringPopoverToFront,
        sendPopoverToBack: sendPopoverToBack,
        publish: publish,
        subscribe: subscribe,
        //publishGlobal:publishGlobal,
        //subscribeGlobal:subscribeGlobal,
        selectMultiPopover: setSelectedPopOverInMultiWindow,
        openMultiPopover: openMultiPopover,
        closeMultiPopover: closeMultiPopover,
        setWindowFrame: setWindowFrame,
        monitorKey: monitorKey,
        stateChange: onStateChange,
        swipeLeft: onSwipeLeft,
        swipeRight: onSwipeRight,
        getBounceStatus: getBounceStatus,
        setMultilPopoverFlippingEnbaled: setMultilPopoverFlippingEnbaled

    };

    appcan.extend(appcanWindow, appcan.eventEmitter);

});

/*
    author:dushaobin
    email:shaobin.du@3g2win.com
    description:构建appcan window模块
    create:2014.08.18
    update:______/___author___

*/
window.appcan && appcan.define('frame', function ($, exports, module) {
    var appWin = appcan.require('window');

    var appcanFrame = module.exports = {
        //open:appWin.openPopover,
        close: appWin.closePopover,
        //resize:appWin.resizePopover,
        bringToFront: appWin.bringPopoverToFront,
        sendToBack: appWin.sendPopoverToBack,
        evaluateScript: appWin.evaluatePopoverScript,
        publish: appWin.publish,
        subscribe: appWin.subscribe,
        //publishGlobal:appWin.publishGlobal,
        //subscribeGlobal:appWin.subscribeGlobal,
        selectMulti: appWin.selectMultiPopover,
        openMulti: appWin.openMultiPopover,
        closeMulti: appWin.closeMultiPopover,
        setBounce: appWin.setBounce,
        getBounceStatus: appWin.getBounceStatus,
        resetBounce: appWin.resetBounceView,
        open: function (id, url, left, top, name, index, change, extraInfo) {
            var argObj = null;
            if (arguments.length === 1 && appcan.isPlainObject(id)) {
                argObj = id;
                id = argObj['id'] || 0;
                url = argObj['url'];
                top = argObj['top'];
                left = argObj['left'];
                name = argObj['name'];
                index = argObj['index'];
                change = argObj['change'];
            }
            if (appcan.isArray(url)) {
                var ele = $('#' + id);
                var width = ele.width();
                var height = ele.height();
                var fontSize = ele.css('font-size');
                top = parseInt(top, 10);
                top = isNaN(top) ? ele.offset().top : top;//默认使用元素本身的top
                left = parseInt(left, 10);
                left = isNaN(left) ? ele.offset().left : left;//默认使用元素本身的left
                name = name ? name : id;
                //fixed xiaomi 2s bug
                fontSize = parseInt(fontSize, 10);
                fontSize = isNaN(fontSize) ? 0 : fontSize;
                appWin.openMultiPopover(name || id,
                    url, 0, left, top, width, height, change || function () { }, fontSize, 0, index, extraInfo);
            }
            else {
                appWin.popoverElement(id, url, left, top, name, extraInfo);
            }
        },
        resize: appWin.resizePopoverByEle,
        swipeLeft: appWin.swipeLeft,
        swipeRight: appWin.swipeRight
    };

    appcan.extend(appcanFrame, appcan.eventEmitter);


});;/*

    author:jiaobingqian
    email:bingqian.jiao@3g2win.com
    description:封装ajax方法的offline离线缓存
    create:2015.08.03
    update:______/___author___

*/
; (function () {
    var requestAjax = appcan.request.ajax;
    //默认缓存文件路径
    var baseFilePath = 'wgt://offlinedata/';
    //默认缓存到LocalStorage数据信息的key
    var offlineKey = 'offlinedata';

    var readFile = appcan.file.read;
    var readSecureFile = appcan.file.readSecure;
    var writeFile = appcan.file.write;
    var writeSecureFile = appcan.file.writeSecure;

    /*
        offline缓存数据主函数
        @param Object opts 离线缓存的ajax请求的参数对象
    */
    function ajax(opts) {
        if (arguments.length === 1 && appcan.isPlainObject(opts)) {
            var url;
            var expires;
            if (opts.data) {
                var paramsInfo = JSON.stringify(opts.data);
                var fullUrl = opts.url + paramsInfo;
                url = appcan.crypto.md5(fullUrl);
            } else {
                url = appcan.crypto.md5(opts.url);
            }
            if (opts.expires && typeof (opts.expires) == 'number') {
                expires = parseInt(opts.expires) + parseInt(new Date().getTime());
            } else if (opts.expires && typeof (opts.expires) == 'string') {
                var result = setISO8601(opts.expires);
                expires = result;
            } else {
                expires = 0;
            }
            if (opts.offlineDataPath != undefined && typeof (opts.offlineDataPath) == 'string') {
                baseFilePath = opts.offlineDataPath;
            }
            //如果设置加密，未设置password,给默认password
            if (opts.crypto && !opts.password) {
                opts.password = '123qwe';
            }
            if (opts.offline != undefined) {
                var isOffline = opts.offline;

                if (isOffline === true) {
                    var offlinedata = appcan.locStorage.val(offlineKey);
                    var dataObj = null;
                    if (offlinedata != null) {
                        dataObj = JSON.parse(offlinedata);
                        if (dataObj[url]) {
                            var urlData = dataObj[url];
                            var localFilePath = urlData.data ? urlData.data : '';
                            var readFileParams = {
                                filePath: localFilePath,
                                length: -1,
                                callback: function (err, data, dataType, optId) {
                                    if (err == null) {
                                        var tempSucc = opts.success;
                                        if (typeof (tempSucc) == 'function') {
                                            if (typeof (data) == 'string' && opts.dataType.toLowerCase() == 'json') {
                                                data = JSON.parse(data);
                                            }
                                            opts.success(data, "success", 200, null, null);
                                        }
                                    } else {
                                        var tempSucc = opts.success;
                                        var tempError = opts.error;
                                        opts.success = function (res) {
                                            tempSucc.apply(this, arguments);
                                            setLocalStorage(url, res, expires, opts);
                                        };
                                        opts.error = function (res) {
                                            tempError.apply(this, arguments);
                                        };

                                        requestAjax(opts);
                                    }
                                }
                            };
                            if (urlData.timeout && urlData.now && urlData.data) {
                                var timeout = parseInt(urlData.now) + parseInt(urlData.timeout);
                                var now = new Date();
                                if (urlData.expires && (urlData.expires > now.getTime())) {
                                    if (opts.crypto) {
                                        readFileParams.key = opts.password;
                                        readSecureFile(readFileParams);
                                    } else {
                                        readFile(readFileParams);
                                    }
                                }
                                else if (timeout > now.getTime()) {
                                    if (opts.crypto) {
                                        readFileParams.key = opts.password;
                                        readSecureFile(readFileParams);
                                    } else {
                                        readFile(readFileParams);
                                    }
                                } else {
                                    var tempSucc = opts.success;
                                    var tempError = opts.error;
                                    opts.success = function (res) {
                                        tempSucc.apply(this, arguments);
                                        setLocalStorage(url, res, expires, opts);
                                    };
                                    opts.error = function (res) {
                                        tempError.apply(this, arguments);
                                    };
                                    requestAjax(opts);
                                }
                            } else if (urlData.data) {
                                if (urlData.expires) {
                                    var now = new Date();
                                    if (urlData.expires > now.getTime()) {
                                        if (opts.crypto) {
                                            readFileParams.key = opts.password;
                                            readSecureFile(readFileParams);
                                        } else {
                                            readFile(readFileParams);
                                        }
                                    } else {
                                        var tempSucc = opts.success;
                                        var tempError = opts.error;
                                        opts.success = function (res) {
                                            tempSucc.apply(this, arguments);
                                            setLocalStorage(url, res, expires, opts);
                                        };
                                        opts.error = function (res) {
                                            tempError.apply(this, arguments);
                                        };
                                        requestAjax(opts);
                                    }
                                } else {
                                    if (opts.crypto) {
                                        readFileParams.key = opts.password;
                                        readSecureFile(readFileParams);
                                    } else {
                                        readFile(readFileParams);
                                    }
                                }
                            } else {
                                var tempSucc = opts.success;
                                var tempError = opts.error;
                                opts.success = function (res) {
                                    tempSucc.apply(this, arguments);
                                    setLocalStorage(url, res, expires, opts);
                                };
                                opts.error = function (res) {
                                    tempError.apply(this, arguments);
                                };

                                requestAjax(opts);
                            }
                        } else {
                            var tempSucc = opts.success;
                            var tempError = opts.error;
                            opts.success = function (res) {
                                tempSucc.apply(this, arguments);
                                setLocalStorage(url, res, expires, opts);
                            };
                            opts.error = function (res) {
                                tempError.apply(this, arguments);
                            };
                            requestAjax(opts);
                        }
                    } else {
                        var tempSucc = opts.success;
                        var tempError = opts.error;
                        opts.success = function (res) {
                            tempSucc.apply(this, arguments);
                            setLocalStorage(url, res, expires, opts);
                        };
                        opts.error = function (res) {
                            tempError.apply(this, arguments);
                        };
                        requestAjax(opts);
                    }
                } else {
                    var tempSucc = opts.success;
                    var tempError = opts.error;
                    opts.success = function (res) {
                        tempSucc.apply(this, arguments);
                        setLocalStorage(url, res, expires, opts);
                    };
                    opts.error = function (res) {
                        tempError.apply(this, arguments);
                    };
                    requestAjax(opts);
                }
            } else {
                var tempSucc = opts.success;
                var tempError = opts.error;
                opts.success = function (res) {
                    tempSucc.apply(this, arguments);
                };
                opts.error = function (res) {
                    tempError.apply(this, arguments);
                };
                requestAjax(opts);
            }
        }
    }
    /*
        缓存ajax请求到的数据并写入文件
        @param String fileUrl 缓存的文件名
        @param String fileData 缓存的JSON格式字符串数据
        @param Number exp 缓存过期时间
        @param Object  opts 缓存ajax请求的参数对象
    */

    function setLocalStorage(fileUrl, fileData, exp, opts) {
        try {
            var filename = fileUrl;
            var localFilePath = baseFilePath + filename + '.txt';
            var saveData = {};
            if ((typeof (fileData) == "object") && (Object.prototype.toString.call(fileData).toLowerCase() == "[object object]") && !fileData.length) {
                fileData = JSON.stringify(fileData);
            }
            var now = new Date().getTime();
            var data = fileData;
            writeFileParams = {
                filePath: localFilePath,
                content: fileData,
                callback: function (err) {
                    if (err == null) {
                        saveData['now'] = now;
                        saveData['data'] = localFilePath;
                        if (data.timeout) {
                            saveData.timeout = data.timeout;
                        } else if (typeof data == "string") {
                            try {
                                var parseData = JSON.parse(data);
                                if (parseData.timeout) {
                                    saveData.timeout = parseData.timeout;
                                }
                            } catch (e) {
                                //console.log(e);
                            }
                        }
                        if (exp > 0) {
                            saveData['expires'] = exp;
                        }
                        var offdata = appcan.locStorage.val(offlineKey) || '{}';
                        var offdataObj = JSON.parse(offdata);
                        offdataObj[filename] = saveData;
                        appcan.locStorage.val(offlineKey, JSON.stringify(offdataObj));
                    }
                }
            }
            if (opts.crypto) {
                writeFileParams.key = opts.password;
                writeSecureFile(writeFileParams);
            } else {
                writeFile(writeFileParams);
            }

        } catch (e) {
            throw e;
        }
    }
    /*
    将符合IOS8601标准的日期格式转成对应毫秒
    @param String string 需要转换成对应毫秒的IOS8601格式的字符串
    */
    function setISO8601(string) {
        var regexp = "([0-9]{4})(-([0-9]{2})(-([0-9]{2})" + "(T([0-9]{2}):([0-9]{2})(:([0-9]{2})(\.([0-9]+))?)?" + "(Z|(([-+])([0-9]{2}):([0-9]{2})))?)?)?)?";
        if (string) {
            try {
                var d = string.match(new RegExp(regexp));
                var offset = 0;
                var date = new Date(d[1], 0, 1);

                if (d[3]) {
                    date.setMonth(d[3] - 1);
                }
                if (d[5]) {
                    date.setDate(d[5]);
                }
                if (d[7]) {
                    date.setHours(d[7]);
                }
                if (d[8]) {
                    date.setMinutes(d[8]);
                }
                if (d[10]) {
                    date.setSeconds(d[10]);
                }
                if (d[12]) {
                    date.setMilliseconds(Number("0." + d[12]) * 1000);
                }
                if (d[14]) {
                    offset = (Number(d[16]) * 60) + Number(d[17]);
                    offset *= ((d[15] == '-') ? 1 : -1);
                }
                offset -= date.getTimezoneOffset();
                time = (Number(date) + (offset * 60 * 1000));
                return Number(time);
            } catch (e) {
                return 0;
            }
            //this.setTime(Number(time));
        } else {
            return 0;
        }
    }
    /**
    *将日期转换成ISO8601格式字符串
    *@param Date d 需要被转换成IOS8601格式字符串的日期参数
    */
    function ISODateString(d) {
        function pad(n) {
            return n < 10 ? '0' + n : n
        }
        return d.getUTCFullYear() + '-' + pad(d.getUTCMonth() + 1) + '-' + pad(d.getUTCDate()) + 'T' + pad(d.getUTCHours()) + ':' + pad(d.getUTCMinutes()) + ':' + pad(d.getUTCSeconds()) + 'Z'
    }

    appcan.extend(appcan.request, {
        ajax: ajax
    });
})();

appcan.define('ajax', function ($, exports, module) {
    module.exports = appcan.request.ajax;
});
;/**
 *
 */
appcan.define("icache", function ($, exports, module) {
    var opid = 1000;
    var CACHE_PATH = "box://icache/";
    function iCache(option) {
        var self = this;
        appcan.extend(this, appcan.eventEmitter);
        self.waiting = [];
        self.running = [];
        self.option = $.extend({
            maxtask: 3
        }, option, true);
        uexFileMgr.cbGetFileRealPath = function (opCode, dataType, path) {
            self.realpath = path;
            self.emit("NEXT_SESSION");
        }
        uexFileMgr.cbIsFileExistByPath = function (opId, dataType, data) {
            self.emit("FS" + opId, self, data);
        }
        uexDownloaderMgr.cbCreateDownloader = function (opCode, dataType, data) {

            self.emit("CDL" + opCode, self, data);
        }
        uexDownloaderMgr.onStatus = function (opCode, fileSize, percent, status) {

            self.emit("DLS" + opCode, self, {
                fileSize: fileSize,
                percent: percent,
                status: status
            });
        }
        uexFileMgr.getFileRealPath("box://");
        self.on("NEXT_SESSION", self._next);

    }


    iCache.prototype = {
        _progress: function (data, session) {
            if (session.progress) {
                session.progress(data, session);
            }
        },
        _success: function (fpath, session) {
            var self = this;
            self.off("DLS" + session.id);
            uexDownloaderMgr.closeDownloader(session.id);
            if (session.success) {
                session.success(fpath, session);
            } else if (session.dom && session.dom.length) {
                switch (session.dom[0].tagName.toLowerCase()) {
                    case "img":
                        session.dom.attr("src", fpath);
                        break;
                    default:
                        session.dom.css("background-image", "url(file://" + fpath + ") !important");
                        break;
                }
            }
            var index = self.running.valueOf(session);
            index != undefined && self.running.splice(index, 1);
            self.emit("NEXT_SESSION");
        },
        _fail: function (session) {
            var self = this;
            self.off("DLS" + session.id);
            uexDownloaderMgr.closeDownloader(session.id);
            var index = self.running.valueOf(session);
            index != undefined && self.running.splice(index, 1);
            if (session.fail) {
                session.fail(session);
            }
            self.emit("NEXT_SESSION");
        },
        _next: function () {
            var self = this;
            if (!self.realpath)
                return;
            if (self.running.length >= self.option.maxtask)
                return;
            var session = self.waiting.shift();
            if (!session)
                return;
            self.running.push(session);
            self._download(session);

        },
        _download: function (session) {
            var self = this;
            var fid = appcan.crypto.md5(session.url);
            var fpath = self.realpath + "/icache/" + fid;
            self.once("FS" + session.id, function (data) {
                if (data) {
                    self._success(fpath, session);
                } else {
                    uexDownloaderMgr.createDownloader(session.id);
                }
            })
            self.once("CDL" + session.id, function (data) {
                if (!data) {
                    (uexDownloaderMgr.setHeaders && session.header) && uexDownloaderMgr.setHeaders(session.id, session.header);
                    uexDownloaderMgr.download(session.id, session.url, fpath, '0');
                } else
                    self._fail(session);
            })
            self.on("DLS" + session.id, function (data) {
                switch (parseInt(data.status)) {
                    case 0:
                        self._progress(data, session)
                        break;
                    case 1:
                        self._success(fpath, session);
                        break;
                    default:
                        self._fail(session);
                        break;
                }
            })
            uexFileMgr.isFileExistByPath(session.id, fpath);
        },
        run: function (option) {
            var self = this;
            var session = $.extend({
                id: ("" + (opid++)),
                status: 0
            }, option, true);
            self.waiting.push(session);
            self.emit("NEXT_SESSION");
        },
        clearcache: function () {
            uexFileMgr.deleteFileByPath(CACHE_PATH);
        }
    }

    module.exports = function (option) {
        return new iCache(option);
    };
})
