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

var currentImageId = 0;
var forceReloadGallery = false;

$(function () {
    initImageDescription();

    $("#StoreProduct").dialog({
        bgiframe: false,
        resizable: false,
        height: 450,
        width: 750,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        }
    });

    $('#Title').bind('input keyup', function () {
        var $this = $(this);
        var delay = 500;
        clearTimeout($this.data('timer'));
        if ($this.val().trim() != '') {
            $this.data('timer', setTimeout(function () {
                $this.removeData('timer');
                getSuggestProducts($this.val());
            }, delay));
        }
    });

    if ($('#Title').val() != '') {
        getSuggestProducts($('#Title').val());
    }

    $('#txtCaptcha').keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#Submit').click();
            return false;
        }
    });

    //validate setting
    $(this.form).each(function () {
        var validator = this.data('validator');
        validator.settings.ignore = "";
    });

    //$("#divUpload").dialog({
    //    bgiframe: false,
    //    resizable: false,
    //    height: 250,
    //    width: 550,
    //    autoOpen: false,
    //    modal: true,
    //    overlay: {
    //        backgroundColor: '#000',
    //        opacity: 0.5
    //    }
    //});
    //$("#divGallery").dialog({
    //    bgiframe: false,
    //    resizable: false,
    //    height: 590,
    //    width: 630,
    //    autoOpen: false,
    //    modal: true,
    //    overlay: {
    //        backgroundColor: '#000',
    //        opacity: 0.5
    //    }
    //});

    $(".deleteImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");

        if ($("#assetId_" + currentImageId).val() != '') {
            $("#div_asset_deleted").append("<input type='hidden' name='DeletedAssetIds' value='" + $("#assetId_" + currentImageId).val() + "'/>");
        }

        $("#img_thumb_" + currentImageId).attr("src", "/Content/Default.gif");
        $("#assetId_" + currentImageId).val("");
        $("#assetFilePath_" + currentImageId).val("");
        $("#asset_desc_" + currentImageId).val("");
    });

    $(".uploadImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");
        $("#ImageUploadUrl").val('');
        $("#imageUploadDisplay").attr("src", "/Content/default.gif");
        //$("#divUpload").dialog('open');
        $("#divUpload").modal('show');
        $("img").load(function () {


        }).error(function () {


        });

        return false;
    });

    $(".galleryImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");
        //$("#divGallery").dialog('open');
        $("#divGallery").modal('show');
        initGallery();
        console.log('a');
        
        return false;
    });

    $(".embedImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");
        var divImageParent = $(this).parents(".upload-image-div");
        var src = $("#img_thumb_" + currentImageId).attr("src");
        var desc = $("#asset_desc_" + currentImageId).val();
        if (desc == 'Ghi chú ảnh (tối đa 64 ký tự)') desc = '';
        var asset = divImageParent.find("input[type='hidden'][name='AssetId']").val();
        if (asset == "") {
            alert("Bạn hãy chọn ảnh trước khi chèn vào bài viết");
        } else {
            content = '<img src="' + src.replace("/Small/", "/Large/") + '" style="margin:2px" title="' + desc + '" />';
            tinyMCE.get("Content").execCommand("mceInsertContent", false, content);
        }
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

    $('#Content').tinymce({
        script_url: '/Scripts/tiny_mce/tiny_mce.js',
        theme: "advanced",
        plugins: "autolink,lists,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,contextmenu,paste,directionality,noneditable,visualchars,nonbreaking,xhtmlxtras,template,wordcount,advlist,",
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,bullist,numlist,|,outdent,indent,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,forecolor,backcolor",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        content_css: THEME_PATH + "Styles/main.css",
        relative_urls: false
    });
    $('#Sumit').click(function () {
        tinyMCE.triggerSave(true, true);
    });



    $('#imageUpload').uploadify({
        'uploader': '/Content/Uploadify/uploadify.swf',
        'script': '/Ajax/FileUpload/Images',
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

                //                if (checkUploadResult(response) == 0) return;
                //                arrTemp = response.split(",");
                //                if (arrTemp.length == 2) {
                //                    if (currentImageId > 0) {
                //                        $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                //                        $("#assetId_" + currentImageId).val(arrTemp[0]);
                //                        $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                //                    } else {
                //                        $("#imagePath").val(arrTemp[1]);
                //                        $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                //                    }
                //                }
            }
            $('#ajaxLoading').hide();

            //$("#divUpload").dialog('close');
            $("#divUpload").modal('hide');
            forceReloadGallery = true;
        }
    });

    $("#imageUploadSubmit").click(function () {
        var url = $("#ImageUploadUrl").val();
        var categoryId = $("#CategoryId").val();
        if (isUrl(url)) {
            $("#imageUploadDisplay").attr("src", url);
            $.ajax({
                url: "/Ajax/FileUpload/ImageUrl",
                type: "POST",
                data: { "token": token, "url": url, "categoryId": categoryId },
                dataType: "html",
                success: function (result) {
                    if (result.substr(0, 6) == "error:") {
                        alert(result.replace("error:", ""));
                    }
                    else {
                        arrTemp = result.split(",");
                        if (arrTemp.length == 2) {
                            if (currentImageId > 0) {
                                $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                                $("#assetId_" + currentImageId).val(arrTemp[0]);
                                $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                            } else {
                                $("#imagePath").val(arrTemp[1]);
                                $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                            }
                        }
                    }

                    //                    if (checkUploadResult(result) == 0) {
                    //                        alert(result);
                    //                    } else {
                    //                        arrTemp = result.split(",");
                    //                        if (arrTemp.length == 2) {
                    //                            if (currentImageId > 0) {
                    //                                $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                    //                                $("#assetId_" + currentImageId).val(arrTemp[0]);
                    //                                $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                    //                            } else {
                    //                                $("#imagePath").val(arrTemp[1]);
                    //                                $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                    //                            }
                    //                        }
                    //                    }

                    $("#ImageUploadUrl").val('');
                    //$("#divUpload").dialog('close');
                    $("#divUpload").modal('hide');
                    forceReloadGallery = true;
                },
                error: function (error) { alert(error); }
            });
        } else {
            alert("Địa chỉ nhập vào không chính xác");
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

function initGallery() {
    if ($("#listImages").html() == '' || forceReloadGallery) {
        getAssets(1);
    }
}

function chooseAsset(assetId) {
    var src = $("#asset_thumb_" + assetId).attr("src");
    var src2;
    if (src.indexOf(MEDIA_URL) == 0) {
        src2 = src.substring(MEDIA_URL.length + 16);
    } else {
        src2 = src.substring(16);
    }

    if (currentImageId > 0) {
        $("#img_thumb_" + currentImageId).attr("src", src);
        $("#assetId_" + currentImageId).val(assetId);
        $("#assetFilePath_" + currentImageId).val(src2);
    } else {
        $("#imagePath_thumb").attr("src", src);
        $("#imagePath").val(src2);
    }
    //$("#divGallery").dialog('close');
    $("#divGallery").modal('hide');
}

function getAssets(index) {
    $.ajax({
        url: "/Ajax/Account/GetAssetThumbnails",
        type: "GET",
        data: { 'thumbSize': 'Small', 'pageIndex': index },
        dataType: "html",
        success: function (result) {
            $("#listImages").html(result);
            $("#divGallery img").load(function () {
                var modal = $("#divGallery");
                modal.css('margin-top', (modal.outerHeight() / 2) * -1)
                                .css('margin-left', (modal.outerWidth() / 2) * -1);

            }).error(function () {
                var modal = $("#divGallery");
                modal.css('margin-top', (modal.outerHeight() / 2) * -1)
                                .css('margin-left', (modal.outerWidth() / 2) * -1);
            });
        },
        error: function (error) { alert(error); }
    });
}

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