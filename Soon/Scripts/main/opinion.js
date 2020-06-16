var $ = jQuery.noConflict();
$(document).ready(function () {
    tinymce.init({
        selector: '#opinion_type_article',
        editor_selector: "mceEditor",
        setup: function (editor) {
            editor.on('change', function () {
                tinymce.triggerSave();
            });
        }
    });

    //encode and decode for the html
    function encodeHTML(str) {
        return str.replace(/[\u00A0-\u9999<>&](?!#)/gim, function (i) {
            return '&#' + i.charCodeAt(0) + ';';
        });
    }

    function decodeHTML(str) {
        return str.replace(/&#([0-9]{1,3});/gi, function (match, num) {
            return String.fromCharCode(parseInt(num));
        });
    }

    $(document).on("change", ".change_image_article", function (e) {


        for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {

            var file = e.originalEvent.srcElement.files[i];

            var reader = new FileReader();
            reader.onloadend = function () {
                // $('#img-inside-small').attr('src', reader.result);
                $('.image_preview_article').html('<img src="' + reader.result + '" id="img-inside-small" class="news-back-imge"/>');
            }
            reader.readAsDataURL(file);
        }
    });


    $(document).on("change", ".opinion_image", function (e) {


        for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {

            var file = e.originalEvent.srcElement.files[i];

            var reader = new FileReader();
            reader.onloadend = function () {
                // $('#img-inside-small').attr('src', reader.result);
                $('.image_preview_article').html('<img src="' + reader.result + '" id="img-inside-small" class="news-back-imge"/>');
            }
            reader.readAsDataURL(file);
        }
    });




});