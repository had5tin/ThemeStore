var loadThemePartial = function (sUrl, sId) {
    $.ajax({
        url: sUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'Get',
        dataType: 'html'
    })
        .success(function (result) {
            $(sId).html(result);
            loadBootstrapSelect();
            $('#loadTheme').addClass('hidden');
        })
        .error(function (xhr, status) {
            console.log(status);
        });
};

var loadTheme = function () {

    $('#loadTheme').removeClass('hidden');

    var price = $('#dvPrice a.active').attr('href');
    var collectionId = $("#CollectionId").val();
    var featureId = $("#FeatureId").val();
    var sortId = $("#SortId").val();
    //console.log(sortId);
    var query = $(location).attr('search');

    if (query == "") {
        query = "?";
    }
    if (price != null && price != "") {
        price = price.slice(2);
        query += "&" + price;
    }
    //console.log(price);

    if (featureId != "") {
        query += '&feature=' + featureId;
    }
    if (collectionId != "") {
        query += '&collection=' + collectionId;
    }
    if (sortId != "") {
        query += '&sort=' + sortId;
    }

    //console.log(query);

    var pathname = $(location).attr('pathname');
    if (pathname != "/") {
        window.location = "/" + query;
    }
    $(location).attr('hash', query);
    loadThemePartial('/themestore/' + query, '#divTheme');
};