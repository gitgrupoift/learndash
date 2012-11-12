function showLoadingOverlay() {
    var container = $("#main-container");
    $("#loadingOverlay").css({
        opacity: 0.6,
        top: container.offset().top,
        left: container.offset().left,
        width: container.outerWidth(),
        height: container.outerHeight()
    });

    $("#imgLoad").css({
        top: (container.height() / 2),
        left: (container.width() / 2)
    });
    $("#loadingOverlay").fadeIn();
}

function hideLoadingOverlay(func) {
    if (func) {
        $("#loadingOverlay").fadeOut(200, func);
    }
    else {
        $("#loadingOverlay").fadeOut(200);
    }
}

function generateNoty(type, message) {
    noty({
        text: message,
        type: type,
        timeout: 1000,
        dismissQueue: true,
        theme: 'default'
    });
}