appcan.define("circleCreatePost", function ($, exports, module) {
    function view(option) {

        var createPost = option.createPost;

        uexImageBrowser.cbPick = uexCamera.cbOpen = function (opCode, dataType, data) {
            var images = data.split(",");
            for (var i in images) {
                createPost.find("#addMark").before(
                        "<li data-img=\"" + images[i] + "\" style=\"background-image:url(" + images[i] + ")\"></li>")
            }

        }
        createPost.find("#paizhao").on("tap", function () {
            uexCamera.open(0, 90)

        })
        createPost.find("#addMark").on("tap", function () {
            uexImageBrowser.pickMulti(3);
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
