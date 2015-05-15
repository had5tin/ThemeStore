setCapcha();

function setCapcha() {
    $('#capcha_img').attr("src", "/Ajax/Capcha/ShowCaptchaImage?a=" + Math.random());
}

function deleteImage(e) {
    var divImage = $(e).parents(".upload-image-div");
    divImage.find("img").attr("src", "/Content/noimage.png");
    divImage.find("img").css("padding", "15px");
    divImage.find("input").val("");
}
var listIdGalleryImages = [];
var listThumbGalleryImages = [];
var listPathGalleryImages = [];
function setAsset(id) {
    var checkbox = $(".asset-checkbox[value='" + id + "']");
    var index = listIdGalleryImages.indexOf(id);
    if (checkbox.is(":checked")) {
        checkbox.removeAttr("checked");
        if (index > -1) {
            listIdGalleryImages.splice(index, 1);
            listThumbGalleryImages.splice(index, 1);
            listPathGalleryImages.splice(index, 1);
        }
    }
    else {
        checkbox.attr("checked", "checked");
        if (index == -1) {
            listIdGalleryImages.push(id);
            listThumbGalleryImages.push($("#asset_thumb_" + id).attr("src"));
            listPathGalleryImages.push(checkbox.attr("path"));
        }
    }
}

function changeAsset(e) {
    var checkbox = $(e);
    var id = $(e).val();
    var index = listIdGalleryImages.indexOf(id);
    if (checkbox.is(":checked")) {
        if (index == -1) {
            listIdGalleryImages.push(id);
            listThumbGalleryImages.push($("#asset_thumb_" + id).attr("src"));
            listPathGalleryImages.push(checkbox.attr("path"));
        }
    }
    else {
        if (index > -1) {
            listIdGalleryImages.splice(index, 1);
            listThumbGalleryImages.splice(index, 1);
            listPathGalleryImages.splice(index, 1);
        }
    }
}

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
    var now = new Date();
    $('#reset_capcha').click(function () {
        setCapcha();
    });

    $("#ExpireDate").datepicker({
        changeMonth: true,
        changeYear: true,
        dateFormat: 'dd/mm/yy',
        minDate: new Date(now.getFullYear(), now.getMonth(), now.getDate())
    });

    $('#Content').tinymce({
        script_url: '/Scripts/tiny_mce/tiny_mce.js',
        theme: "advanced",
        plugins: "paste,inlinepopups",
        theme_advanced_toolbar_location: "top",
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,separator,justifyleft,justifycenter,justifyright,justifyfull,bullist,numlist,fontselect,fontsizeselect,forecolor,backcolor,separator,link,unlink,pasteword,removeformat",
        theme_advanced_buttons2: "",
        theme_advanced_buttons3: "",
        theme_advanced_toolbar_align: "left",
        fullscreen_new_window: true,
        fullscreen_settings: {
            theme_advanced_path_location: "top"
        },
        content_css: THEME_PATH + "Styles/main.css",
        entity_encoding: "raw",
        relative_urls: false,
        plugin_preview_width: "100%"
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