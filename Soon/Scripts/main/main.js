var $ = jQuery.noConflict();
$(document).ready(function () {
    var image = new Array('../Content/images/anonymous-black-person-with-large-plaid-bag-standing-by-blue-4177747.jpg', '../Content/images/close-up-of-shoes-and-bag-336372.jpg', '../Content/images/pexels-4599047.jpg', '../Content/images/photo-of-woman-wearing-head-scarf-2951093.jpg', '../Content/images/photo-of-man-standing-beside-doorway-2951458.jpg', '../Content/images/serious-black-woman-standing-with-storage-bag-4177839.jpg', '../Content/images/photo-of-woman-touching-her-jaw-2951092.jpg'); 
    var nextimage = 0;
    doSlideShow();

    function doSlideShow()
    {
        if (nextimage >= image.length)
            nextimage = 0;
        $('.page-wrap')
            .css('background-image', 'url(" ' + image[nextimage++] + ' ")')
            .fadeIn(500,function () {
                setTimeout(doSlideShow, 4000);
            });
    }

});