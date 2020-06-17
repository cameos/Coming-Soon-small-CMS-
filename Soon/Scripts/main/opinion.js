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

    $(document).on("submit", "#add_article", function (e) {
        e.preventDefault();

        var form = new FormData(document.getElementById("add_article"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44363/articles/new",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal == 'string' || modal instanceof String) {
                    if (modal.indexOf("error") !== -1) {
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal);
                        $("#remove_error").removeClass("background_error_hidden").addClass("background_error_show");
                    }
                    else if (modal.indexOf("login") !== -1) {
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal);
                        $("#opinion-login-modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });

        
    });

    $(document).on("click", "#login_cancel", function (e) {
        e.preventDefault();
        $("#opinion-login-modal").modal('hide');

    });
    $(document).on("click", "#register_cancel", function (e) {
        e.preventDefault();
        $("#opinion-register-modal").modal('hide');
    });
    $(document).on("submit", "#opinion_login_form", function (e) {
        e.preventDefault();

        var form = new FormData(document.getElementById("opinion_login_form"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44363/articles/login",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal == 'string' || modal instanceof String) {
                    if (modal.indexOf("error") !== -1) {
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal);
                        $("#remove_error").removeClass("background_error_hidden").addClass("background_error_show");
                    }
                    else if (modal.indexOf("register") !== -1) {
                        var target = $("#login_text_r");
                        target.empty().html();
                        target.append(modal);
                        $("#opinion-login-modal").modal('hide');
                        $("#opinion-register-modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });
    });
    $(document).on("submit", "#opinion_register_form", function (e) {
        e.preventDefault();

        var form = new FormData(document.getElementById("opinion_register_form"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44363/articles/login",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal == 'string' || modal instanceof String) {
                    if (modal.indexOf("error") !== -1) {
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal);
                        $("#remove_error").removeClass("background_error_hidden").addClass("background_error_show");
                    }
                    else if (modal.indexOf("login") !== -1) {
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal);
                        
                        $("#opinion_register_form").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });
    });



});