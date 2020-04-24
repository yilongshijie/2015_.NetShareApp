appcan.define("Huifu", function ($, exports, module) {
    function view(option) {

        var huiFuKuang = option.huiFuKuang;

        uexImageBrowser.cbPick = function (opCode, dataType, data) {
            var images = data.split(",");
            for (var i in images) {
                huiFuKuang.find(".huifuText").append("<img class=\"uploadimg\"  data-img=\"" + images[i] + "\"  src=\"" + images[i] + "\"/>");
            }
            $(".loading").removeClass("uhide");
            huiFuKuang.find(".submit").trigger("tap");
        }



        for (i = 1; i <= 40; i++) {
            huiFuKuang.find(".faceList ul").append("<li> <img src=\"express/" + i + ".png\"/> </li>")
        }
        huiFuKuang.find(".face").on("tap", function () {
            huiFuKuang.find(".faceList ul li").off("tap", "img");
            huiFuKuang.find(".faceList").toggleClass("remove");
            if (huiFuKuang.find(".faceList").hasClass("remove")) {

            }
            else {
                setTimeout(function () {

                    huiFuKuang.find(".faceList ul li").on("tap", "img", function () {
                        huiFuKuang.find(".huifuText").append("<img class=\"phiz\" src=\"" + $(this).attr("src") + "\"/>");
                    });
                }, 1000);

            }
        })
        huiFuKuang.find(" .photo").on("tap", function () {
            if (!window.shangpinpingjia) {
                if (localStorage.getItem("authentication_userLevel") < 3) {
                    appcan.window.openToast("3级以上才解锁发图功能，赶紧升级吧", 2500, 5, 0);
                    return;
                }
            }
            uexImageBrowser.pickMulti(3);

        });



    };

    module.exports = function (option) {
        return new view(option);
    };


})
