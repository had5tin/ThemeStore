/*!
* Product Post 1.0
* http://hangtot.com/
*
* Copyright 2012, DKT Technology
*
* Date: 2012/07/02
*/
function togglePost(pId) {
    $("#" + pId).toggle("slow", function () {
        if ($(this).is(':visible') == false) {
            $("#c_" + pId).removeClass("icon_up").addClass("icon_down");
        } else {
            $("#c_" + pId).removeClass("icon_down").addClass("icon_up");
        }
    });
}
var now = new Date();
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

var currentImageId = 0;
var forceReloadGallery = false;

$(function () {
    initImageDescription();

    $("#divUpload").dialog({
        bgiframe: false,
        resizable: false,
        height: 250,
        width: 550,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        }
    });
    $(".deleteImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");

        if ($("#assetId_" + currentImageId).val() != '') {
            $("#div_asset_deleted").append("<input type='hidden' name='DeletedAssetIds' value='" + $("#assetId_" + currentImageId).val() + "'/>");
        }

        $("#img_thumb_" + currentImageId).attr("src", "/Content/Default.gif");
        $("#assetId_" + currentImageId).val("");
        $("#assetFilePath_" + currentImageId).val("");
        $("#assetDescription_" + currentImageId).val("");
    });

    $(".uploadImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");
        $("#divUpload").dialog('open');
        return false;
    });

    $("#upload_content textarea[name='AssetDescription']").bind({
        focus: function () {
            if ($(this).val() == 'Ghi chú ảnh (tối đa 64 ký tự)') {
                $(this).val('');
                $(this).css("color", "black");
            }
        },
        blur: function () {
            if ($(this).val() == '') {
                $(this).val('Ghi chú ảnh (tối đa 64 ký tự)');
                $(this).css("color", "gray");
            } else {
                $(this).css("color", "black");
            }
        }
    });

    $('#imageUpload').uploadify({
        'uploader': '/Content/Uploadify/uploadify.swf',
        'script': '/Ajax/FileUpload/ImagesAdmin',
        'folder': '/Upload',
        'queueID': 'imageUploadQueue',
        'width': 100,
        'scriptAccess': 'always',
        'multi': false,
        'auto': true,
        'scriptData': { token: token, categoryId: categoryId },
        'displayData': 'speed',
        'queueSizeLimit': 1,
        'fileDesc': '*.jpg;*.jpeg;*.gif;*.png',
        'fileExt': '*.jpg;*.jpeg;*.gif;*.png',
        'sizeLimit': (500 * 1024),
        'cancelImg': '/Content/Uploadify/cancel.png',
        'buttonText': 'Upload Image',
        'onError': function (event, queueID, fileObj, errorObj) {
            if (errorObj.type == 'File Size') {
                alert("File upload vượt quá kích thước quy định 500Kb");
            } else {
                alert("Lỗi!!! Loại: [" + errorObj.type + "] Thông tin: [" + errorObj.info + "]");
            }
        },
        'onQueueFull': function (evt, queueSizeLimit) {
            alert('Bạn chỉ được Upload tối đa ' + queueSizeLimit + ' file đồng thời.');
            return false;
        },
        'onProgress': function (event, ID, fileObj, data) {
            $('#ajaxLoading').show();
        },
        'onComplete': function (evt, queueID, fileObj, response, data) {
            if (response != null) {
                if (response.substr(0, 6) == "error:") {
                    alert(response.replace("error:", ""));
                }
                else {
                    arrTemp = response.split(",");
                    if (currentImageId > 0) {
                        $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                        $("#assetId_" + currentImageId).val(arrTemp[0]);
                        $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                    }
                    else {
                        $("#imagePath").val(arrTemp[1]);
                        $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                    }
                }
            }
            $('#ajaxLoading').hide();
            $("#divUpload").dialog('close');
            forceReloadGallery = true;
        }
    });
});

function initImageDescription() {
    $("#upload_content textarea[name='ImageDescription']").each(function () {
        if ($(this).val() == 'Ghi chú ảnh (tối đa 64 ký tự)') {
            $(this).css("color", "gray");
        } else {
            $(this).css("color", "black");
        }
    });
}

$('#Content').tinymce({
    script_url: '/Scripts/tiny_mce_v2/tiny_mce.js',
    theme: "advanced",
    plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template,simplebrowser",
    file_browser_callback: "TinyMCE_simplebrowser_browse",
    plugin_simplebrowser_browselinkurl: '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx',
    plugin_simplebrowser_browseimageurl: '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?Type=Image&connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx',
    plugin_simplebrowser_browseflashurl: '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?Type=Flash&connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx',

    // Theme options
    theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
    theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
    theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
    theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,spellchecker,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,blockquote,pagebreak,|,insertfile,insertimage",
    theme_advanced_toolbar_location: "top",
    theme_advanced_toolbar_align: "left",
    content_css: THEME_PATH + "Styles/main.css",
    relative_urls: false,
    entity_encoding: "raw",
    plugin_preview_width: "100%"
});

function checkUploadResult(result) {
    if (result.substr(0, 6) == "error:") {
        alert(result.replace("error:", ""));
    }
    return true;
}

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