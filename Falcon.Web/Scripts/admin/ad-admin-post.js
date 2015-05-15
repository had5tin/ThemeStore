function closeFancyPopup() {
    $.fancybox.close(true);
}

function togglePost(pId) {
    $("#" + pId).toggle("slow", function () {
        if ($(this).is(':visible') == false) {
            $("#c_" + pId).removeClass("icon_up").addClass("icon_down");
        } else {
            $("#c_" + pId).removeClass("icon_down").addClass("icon_up");
        }
    });
}

function deleteImage(e) {
    var divImage = $(e).parents(".upload-image-div");
    divImage.find("img").attr("src", "/Content/noimage.png");
    divImage.find("img").css("width", "40px");
    divImage.find("img").css("padding-left", "15px");
    divImage.find("img").css("padding-top", "15px");
    divImage.find("input").val("");
}

var now = new Date();
$("#ExpireDate").datepicker({
    changeMonth: true,
    changeYear: true,
    dateFormat: 'dd/mm/yy',
    //maxDate: new Date(now.getFullYear(), now.getMonth()+1, now.getDate()),
    minDate: new Date(now.getFullYear(), now.getMonth(), now.getDate())
});
$('.tab_upload ul.upload_tabs li').click(function () {
    if ($(this).attr("class") == 'upload_img') {
        $('#upload_img').show();
        $('#library_img').hide();
    }
    if ($(this).attr("class") == 'library_img') {
        $('#upload_img').hide();
        $('#library_img').show();
    }
    $('.tab_upload ul.upload_tabs li').removeClass("active");
    $(this).addClass("active");
});

function embebImage(e) {
    var imageid = $(e).attr("imageid");
    var divImageParent = $(e).parents(".upload-image-div");
    var src = divImageParent.find("img").attr("src");
    var title = $("#asset_title_" + imageid).val();
    var asset = divImageParent.find("input[type='hidden'][name='AssetId']").val();
    if (asset == "") {
        alert("Bạn hãy chọn ảnh trước khi chèn vào bài viết");
    } else {
        content = '<img src="' + src.replace("/Small/", "/Large/").replace("s=s", "s=l") + '" style="margin:2px" title="' + title + '" />';
        tinyMCE.get("Content").execCommand("mceInsertContent", false, content);
    }
    return false;
}
$(function () {
    $('#Content').tinymce({
        script_url: '/Scripts/tiny_mce/tiny_mce.js',
        theme: "advanced",
        plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,contextmenu,paste,directionality,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist",
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,bullist,numlist,|,outdent,indent,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,forecolor,backcolor",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        relative_urls: false,
        plugin_preview_width: "100%",
        entity_encoding: "raw"
    });
    $('#Sumit').click(function () {
        tinyMCE.triggerSave(true, true);
    });
});

function showTooltip(ob) {

    ob.tooltip({
        bodyHandler: function () {
            img = ob.find("img:last");
            content = '<div class="picture"><img src="' + img.attr("src").replace("/Small/", "/Medium/") + '" /></div>';
            return content;
        },
        track: true,
        showURL: false,
        extraClass: "tooltip_picture"
    });
}

function isNumeric(e) {
    var key = (window.event) ? window.event.keyCode : e.charCode;
    var nkey = String.fromCharCode(key);
    if (key != 0 && nkey.match(/[^0-9]/)) {
        KeypressThrow(e);
    }
}
function KeypressThrow(e) {
    if (window.event)
        window.event.returnValue = null;
    else
        if (e.preventDefault)
            e.preventDefault();
    e.returnValue = false;
    return false;
}
function isUrl(s) {
    var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@@!\-\/]))?/
    return regexp.test(s);
}