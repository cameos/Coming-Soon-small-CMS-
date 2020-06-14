var $ = jQuery.noConflict();
$(document).ready(function () {
    //send  emails with thoughts and sanitize later
    $(document).on("submit", "#social_form", function (e) {
        e.preventDefault();

        var form = new FormData(document.getElementById("social_form"));
        $.ajax({
            method: "POST",
            url: "https://localhost:44363/soon/send",
            dataType: "json",
            data: form,
            contentType: false,
            processData: false,
            cache: false,
            success: function (error_message) {
                if (typeof error_message == 'string' || error_message instanceof String) {
                    if (error_message.indexOf("error") !== -1) {
                        var target = $("#text_error");
                        target.empty().html();
                        target.append(error_message);
                        $("#remove_error").removeClass("background_error_hidden").addClass("background_error_show");
                    }
                    else {
                        var target = $("#body-back");
                        target.empty().html();
                        target.append(error_message);
                        $("#success-contact-modal").modal({ keyboard: false, backdrop: 'static' });
                    }
                }
            }
        });

    });
});