
function togglePost(pId) {
    $("#" + pId).toggle("slow", function () {
        if ($(this).is(':visible') == false) {
            $("#c_" + pId).removeClass("icon_up").addClass("icon_down");
        } else {
            $("#c_" + pId).removeClass("icon_down").addClass("icon_up");
        }
    });
}
setCapcha();
$('#reset_capcha').click(function () {
    setCapcha();
});
function setCapcha() {
    $('#capcha_img').attr("src", "/Ajax/Capcha/ShowCaptchaImage?a=" + Math.random());
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
    //$('#Title').bind('input keyup', function () {
    //    var $this = $(this);
    //    var delay = 500;
    //    clearTimeout($this.data('timer'));
    //    if ($this.val().trim() != '') {
    //        $this.data('timer', setTimeout(function () {
    //            $this.removeData('timer');
    //            getSuggestProducts($this.val());
    //        }, delay));
    //    }
    //});

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

    $(".deleteImage").live("click", function (e) {
        currentImageId = $(this).attr("imageId");
        if ($("#assetId_" + currentImageId).val() != '') {
            $("#div_asset_deleted").append("<input type='hidden' name='DeletedAssetIds' value='" + $("#assetId_" + currentImageId).val() + "'/>");
        }

        $("#img_thumb_" + currentImageId).attr("src", "/Content/blank.gif");
        $("#assetId_" + currentImageId).val("");
        $("#assetFilePath_" + currentImageId).val("");
        $("#asset_desc_" + currentImageId).val("");
        $("#image-dimensions-" + currentImageId).text("");
        $("#assetDimensions_" + currentImageId).val("").attr("test", "");
    });

    $(".uploadImage").bind("click", function () {
        currentImageId = $(this).attr("imageId");
        $("#divUpload").dialog('open');
        return false;
    });

//    $("#upload_content textarea[name='AssetDescription']").bind({
//        focus: function () {
//            if ($(this).val() == 'Ghi chú ảnh (tối đa 64 ký tự)') {
//                $(this).val('');
//                $(this).css("color", "black");
//            }
//        },
//        blur: function () {
//            if ($(this).val() == '') {
//                $(this).val('Ghi chú ảnh (tối đa 64 ký tự)');
//                $(this).css("color", "gray");
//            } else {
//                $(this).css("color", "black");
//            }
//        }
//    });

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
        'sizeLimit': (2000 * 1024),
        'cancelImg': '/Content/Uploadify/cancel.png',
        'buttonText': 'Upload Image',
        'onError': function (event, queueID, fileObj, errorObj) {
            if (errorObj.type == 'File Size') {
                alert("File upload vượt quá kích thước quy định 2MB");
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
                if (checkUploadResult(response) == 0) return;
                arrTemp = response.split(",");
                if (arrTemp.length == 2) {
                    if (currentImageId > 0) {
                        $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]).attr("onerror", "this.src='/Thumbnail.ashx?i=~" + arrTemp[1] + "&s=s&c=0'");
                        $("#assetId_" + currentImageId).val(arrTemp[0]);
                        $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                        $("#primary-image-" + currentImageId).attr("checked", true);
                        var img = new Image();
                        img.onload = function () {
                            $("#image-dimensions-" + currentImageId).text(this.width + ' x ' + this.height);
                            $("#assetDimensions_" + currentImageId).val("{ Width : " + this.width + ", Height : " + this.height + "}");
                            if (this.width * this.height >= 120000) {
                                $("#image-dimensions-" + currentImageId).css("background-color", "#08bd00").css("color", "#FFFFFF");
                            }
                            else {
                                $("#image-dimensions-" + currentImageId).css("background-color", "#cb0202").css("color", "#FFFFFF");
                            }
                        }
                        img.src = arrTemp[1];
                    } else {
                        //$("#imagePath").val(arrTemp[1]);
                        //if ($("#imagePath_thumb").hasClass('primary-image')) {
                        //    $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Large/" + arrTemp[1]);
                        //} else {
                        //    $("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]);
                        //}
                    }
                }
            }

            $('#ajaxLoading').hide();
            $("#divUpload").dialog('close');
            forceReloadGallery = true;
        }
    });

    $("#imageUploadSubmit").click(function () {
        var url = $("#ImageUploadUrl").val();
        if (isUrl(url)) {
            $("#imageUploadDisplay").attr("src", url);
            $("#divUpload").dialog('close');
            $("#image-dimensions-" + currentImageId).text("Đang upload");
            $("#image-dimensions-" + currentImageId).css("background-color", "#cb0202").css("color", "#FFFFFF");
            $.ajax({
                url: "/Ajax/FileUpload/ImageUrlAdmin",
                type: "POST",
                data: { "token": token, "url": url, "categoryId": categoryId },
                dataType: "html",
                success: function (result) {
                    if (checkUploadResult(result) == 0) {
                        alert(result);
                    } else {
                        arrTemp = result.split(",");
                        if (arrTemp.length == 2) {
                            if (currentImageId > 0) {
                                $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL + "/Thumbnail/Small/" + arrTemp[1]).attr("onerror", "this.src='/Thumbnail.ashx?i=~" + arrTemp[1] + "&s=s&c=0'");
                                $("#assetId_" + currentImageId).val(arrTemp[0]);
                                $("#assetFilePath_" + currentImageId).val(arrTemp[1]);
                                $("#primary-image-" + currentImageId).attr("checked", true);
                                var img = new Image();
                                img.onload = function () {
                                    $("#image-dimensions-" + currentImageId).text(this.width + ' x ' + this.height);
                                    $("#assetDimensions_" + currentImageId).val("{ Width : " + this.width + ", Height : " + this.height + "}");
                                    if (this.width * this.height >= 120000) {
                                        $("#image-dimensions-" + currentImageId).css("background-color", "#08bd00").css("color", "#FFFFFF");
                                    }
                                    else {
                                        $("#image-dimensions-" + currentImageId).css("background-color", "#cb0202").css("color", "#FFFFFF");
                                    }
                                }
                                img.src = url;
                            } else {
                                //$("#imagePath").val(arrTemp[1]);
                                //$("#imagePath_thumb").attr("src", MEDIA_URL + "/Thumbnail/Large/" + arrTemp[1]);
                            }

                            // bind to default
                            $("#ImageUploadUrl").val('');
                            $("#imageUploadDisplay").attr('src', '/Content/logo_340_300.png');
                        }
                    }
                    $("#divUpload").dialog('close');
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
    $("#divGallery").dialog('close');
}

function getAssets(index) {
    $.ajax({
        url: "/Ajax/Account/GetAssetThumbnails",
        type: "GET",
        data: { 'thumbSize': 'Small', 'pageIndex': index },
        dataType: "html",
        success: function (result) {
            $("#listImages").html(result);
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
function isUrl(s) {
    var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@@!\-\/]))?/
    return regexp.test(s);
}

