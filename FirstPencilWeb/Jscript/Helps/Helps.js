//字符串长度限制 
//属性中添加：class="displayPart" displayLength="（要限制的长度）"
$.fn.extend({
    displayPart: function () {
        var displayLength = 100;
        displayLength = this.attr("displayLength") || displayLength;
        var text = this.text();
        if (!text) return "";

        var result = "";
        var count = 0;
        for (var i = 0; i < displayLength; i++) {
            var _char = text.charAt(i);
            if (count >= displayLength) break;
            if (/[^x00-xff]/.test(_char)) count++;  //双字节字符，//[u4e00-u9fa5]中文

            result += _char;
            count++;
        }
        if (result.length < text.length) {
            result += "...";
        }
        this.text(result);
    }
});

$(function () {
    $(".displayPart").displayPart();
})


//字符串长度限制 
window.onload = function () {
    jQuery.fn.limit = function () {
        var self = $(".limit");
        self.each(function () {
            var objString = $(this).text();
            var objLength = $(this).text().length;
            var num = $(this).attr("limit");
            if (objLength > num) {
                $(this).attr("title", objString);
                objString = $(this).text(objString.substring(0, num) + "...");
            }
        })
    }
    $(function () {
        $(".limit").limit();
    })
}
