var upload = {

    getBase64Image: function (img, width) {
        var canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        if (img.width > width) {
            var v = img.height / img.width;
            canvas.width = width;
            canvas.height = v * width;
        }
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
        return canvas.toDataURL("image/jpeg", 0.92);
    },
    request: function (src, ajaxresult, index, dtd, listLength, type) {
        var self = this;
        var img = document.createElement('img');
        img.src = src;
        img.onload = function () {
            if (type == "touxiang") {
                var width = 200;
            }
            else if (type == "tiyanshi") {
                var width = 640;
            } else {
                var width = 640;
            }
            var data = self.getBase64Image(img, width);

            appcan.request.ajax({
                url: url + "/upload/upload",
                dataType: "json",
                type: "Post",
                data: { base64: data, fileID: index, type: type },
                success: function (data) {
                    window.ajaxresult.push(data);
                    if (window.ajaxresult.length == listLength) {
                        dtd.resolve();
                    }
                },
                error: function () {
                    dtd.reject();
                }
            })
        }
    },
    upload: function (list, ajaxresult, type) {
        var self = this;
        var dtd = $.Deferred();
        if (list.length == 0) {
            return dtd.resolve();
        }
        _.each(list, function (item, index) {
            var src = $(item).attr("data-img");
            type = type || "tiezi";
            self.request(src, ajaxresult, index, dtd, list.length, type);
        });
        return dtd.promise();
    },

    uploadSrc: function (list, ajaxresult, type) {
        var self = this;
        var dtd = $.Deferred();
        if (list.length == 0) {
            return dtd.resolve();
        }
        _.each(list, function (item, index) {
            var src = item;
            type = type || "tiezi";
            self.request(src, ajaxresult, index, dtd, list.length, type);
        });
        return dtd.promise();
    }
}





