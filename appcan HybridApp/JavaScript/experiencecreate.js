appcan.define("experiencecreate", function ($, exports, module) {
    function view(option) {

        var createPost = option.createPost;

        uexImageBrowser.cbPick = uexCamera.cbOpen = function (opCode, dataType, data) {
            var images = data.split(",");
            for (var i in images) {
                createPost.find(".experienceul").append(
             "<li>\
                   <div> <input placeholder=\"您的甜蜜导语\" /><b class=\"iconfont icon-lajixiang\"></b></div>\
                    <img data-img=\"" + images[i] + "\" \
                src=\"" + images[i] + "\" />\
             </li>");
                $("body").scrollTop($("div:first-child").height());
            }

        }
        createPost.find("#paizhao").on("tap", function () {
            uexCamera.open(0, 90)

        })

        createPost.find("#addMark").on("tap", function () {
            uexImageBrowser.pickMulti(3);
        })


        createPost.find(".experienceul").on("tap"," .icon-lajixiang", function () {
           $(this).parents("li").remove();
        })


        createPost.find(".title ,.content").one("tap", function () {
            $(this).removeClass("colorccc");
            $(this).html("");
            $(this).attr("data-edit", "true");


        })



    };

    module.exports = function (option) {
        return new view(option);
    };


})
