$(document).ready(function () {
    loadToolbarPartial('/Home/Home/Collection', '#dvColletion');
    loadToolbarPartial('/Home/Home/Feature', '#dvFeature');
});

var loadToolbarPartial = function (sUrl, sId) {
    $.ajax({
        url: sUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'Get',
        dataType: 'html'
    })
        .success(function (result) {
            $(sId).html(result);
            loadBootstrapSelect();
        })
        .error(function (xhr, status) {
            console.log(status);
        });
};