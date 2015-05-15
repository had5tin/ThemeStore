$(document).ready(function () {
    loadTabs();
});

var loadTabs = function () {
    var txtUrl = $(location).attr('pathname');
    txtUrl = txtUrl.replace("/admin/", "");
    var k = txtUrl.indexOf('/');
    if (k > -1) {
        txtUrl = txtUrl.slice(0, k);
    }
    console.log(txtUrl);
    var li1 = '#li';
    var li3 = '#li-hethong > ul > li > ul';
    switch (txtUrl) {
        case 'dashboard':
            li1 = li1 + '';
            break;
        case 'pages':
            li1 = li1 + '-noidung';
            break;
        case 'systemsetting':
            li1 = li1 + '-hethong';
            break;
        case 'theme':
            li1 = li1 + '-hethong';
            break;
        case 'elmahlog':
            li1 = li1 + '-hethong';
            break;
        case 'user':
            li1 = li1 + '-hethong';
            $(li3).css("display", "block");
            $('#li-hethong-phanquyen').addClass('active open');
            break;
        case 'roles':
            li1 = li1 + '-hethong';
            $(li3).css("display", "block");
            $('#li-hethong-phanquyen').addClass('active open');
            break;
        default:
            break;
    }

    var li2 = li1 + '-' + txtUrl;
    console.log(li1);
    $(li1).addClass('active open');
    $(li1 + ' > ul').css("display", "block");
    $(li2).addClass('active');
};