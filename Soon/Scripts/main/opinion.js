var $ = jQuery.noConflict();
$(document).ready(function () {
    $(document).on("click", "#user_sign", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
    });

    $('[data-toggle="popover"]').popover({
        animation: true,
        placement: 'bottom',
        container: 'body',
        trigger: 'focus',
        title: 'Information',
        content: '<div class="text-left"><a href="#" id="sigout" style="text-decoration:none; cursor:pointer; font-weight:bold;" >Your articles</a></div><div class="text-left"><a href="https://localhost:44363/articles/signout" id="sigot" style="text-decoration:none; cursor:pointer; font-weight:bold;" >Sign out</a></div>',
        html: true,
        delay: { show: 0, hide: 500 }
    });   

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

   

    $(document).on("click", "#login_cancel", function (e) {
        e.preventDefault();
        $("#opinion-login-modal").modal('hide');

    });
    $(document).on("click", "#error_cancel", function (e) {
        e.preventDefault();
        $("#opinion_article_error_modal").modal('hide');

    });
    $(document).on("click", "#register_cancel", function (e) {
        e.preventDefault();
        $("#opinion-register-modal").modal('hide');
    });
    $(document).on("click", "#article_add_cancel", function (e) {

        e.preventDefault();
        e.stopImmediatePropagation();
        window.location.href = "https://localhost:44363/soon/opinions";

    });
    $(document).on("click", "#sigout", function (e) {
        $.ajax({
            method: "GET",
            url: "https://localhost:44363/articles/signout",
            cache: false,
            success: function (modal) {
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else if (modal.modal.indexOf("verify") !== -1) {
                        var target = $("#login_text_v");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-register-modal").modal('hide');
                        $("#opinion-verify-modal").modal({ keyboard: false, backdrop: 'static' });
                    } else if (modal.modal.indexOf("add") !== -1) {
                        var target = $("#title-back-t");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-verify-modal").modal('hide');
                        $("#user_block").removeClass("user_block_hidden").addClass("user_block_show");
                        $("#opinion_article_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });
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
                console.log(modal);
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else if (modal.modal.indexOf("login") !== -1) {
                       
                        var target = $("#login_text");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-login-modal").modal({ keyboard: false, backdrop: 'static' });
                    } else if (modal.modal.indexOf("add") !== -1) {
                        var target = $("#title-back-t");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#user_block").removeClass("user_block_hidden").addClass("user_block_show");
                        $("#opinion_article_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });


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
                console.log("this is modal" + modal);
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-login-modal").modal('hide');
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else if (modal.modal.indexOf("register") !== -1) {
                        var target = $("#login_text_r");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-login-modal").modal('hide');
                        $("#opinion-register-modal").modal({ keyboard: false, backdrop: 'static' });
                    } else if (modal.modal.indexOf("add") !== -1) {
                        var target = $("#title-back-t");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-login-modal").modal('hide');
                        $("#user_block").removeClass("user_block_hidden").addClass("user_block_show");
                        $("#opinion_article_modal").modal({ keyboard: false, backdrop: 'static' });
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
            url: "https://localhost:44363/articles/register",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal);
                        $("#opinion-register-modal").modal('hide');
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else if (modal.modal.indexOf("verify") !== -1) {
                        var target = $("#login_text_v");
                        target.empty().html();
                        target.append(modal);
                        $("#opinion-register-modal").modal('hide');
                        $("#opinion-verify-modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });
    });
    $(document).on("submit", "#opinion_verify_form", function (e) {
        e.preventDefault();

        var form = new FormData(document.getElementById("opinion_verify_form"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44363/articles/verify",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else if (modal.modal.indexOf("verify") !== -1) {
                        var target = $("#login_text_v");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-register-modal").modal('hide');
                        $("#opinion-verify-modal").modal({ keyboard: false, backdrop: 'static' });
                    } else if (modal.modal.indexOf("add") !== -1) {
                        var target = $("#title-back-t");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion-verify-modal").modal('hide');
                        $("#user_block").removeClass("user_block_hidden").addClass("user_block_show");
                        $("#opinion_article_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                } 
            }
        });
    });
    $(document).on("submit", "#add_article_form", function (e) {

        e.preventDefault();
        tinymce.triggerSave();
        var ed = tinymce.get('opinion_type_article');


        var data = ed.getContent();
        console.log("this is data:" + $("#textarea_type_article").val());


        var opinion_type_article = encodeHTML($("#opinion_type_article").val());


        var formData = new FormData();       
        formData.append('opinion_title', $('#opinion_title').val());
        formData.append('opinion_minutes', $('#opinion_minutes').val());
        formData.append('opinion_image', $('input[type=file]')[0].files[0]);
        formData.append('opinion_type_article', opinion_type_article);


        $.ajax({
            method: "POST",
            url: "https://localhost:44363/articles/article",
            dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            cache: false,
            success: function (modal) {
                if (typeof modal.modal == 'string' || modal.modal instanceof String) {
                    if (modal.modal.indexOf("error") !== -1) {
                        var target = $("#back_error");
                        target.empty().html();
                        target.append(modal.modal);
                        $("#opinion_article_modal").modal('hide');
                        $("#opinion_article_error_modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                    else {
                        window.location.href = "https://localhost:44363" + modal.modal;
                    }
                }
            }
        });

    });

});