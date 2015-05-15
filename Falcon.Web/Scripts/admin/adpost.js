/*!
 * Ad Post 1.0
 * http://hangtot.com/
 *
 * Copyright 2012, DKT Technology
 *
 * Date: 2012/04/24
 */
function togglePost(pId){
        $("#" + pId).toggle("slow", function () {
            if ($(this).is(':visible') == false) {
                $("#c_"+pId).removeClass("icon_up").addClass("icon_down");
            } else {
                    $("#c_"+pId).removeClass("icon_down").addClass("icon_up");
            }
        });
}
setCapcha();
$('#reset_capcha').click(function(){
        setCapcha();
});
function setCapcha()
{
    $('#capcha_img').attr("src","/Ajax/Capcha/ShowCaptchaImage?a="+ Math.random());
}
var now = new Date();
$("#ExpireDate").datepicker({
    changeMonth: true, 
    changeYear: true,
    dateFormat: 'dd/mm/yy',
    //maxDate: new Date(now.getFullYear(), now.getMonth()+1, now.getDate()),
    minDate: new Date(now.getFullYear(), now.getMonth(), now.getDate()) 
});

$('.tab_upload ul.upload_tabs li').click(function(){
      
    if($(this).attr("class") == 'upload_img')
    {
        $('#upload_img').show();
        $('#library_img').hide();
    }
    if($(this).attr("class") == 'library_img')
    {
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
    $("#divGallery").dialog({
        bgiframe: false,
        resizable: false,
        height: 560,
        width: 630,
        autoOpen: false,
        modal: true,
        overlay: {
            backgroundColor: '#000',
            opacity: 0.5
        }            
    });

    $(".deleteImage").bind("click", function(){
        currentImageId = $(this).attr("imageId");
        $("#img_thumb_" + currentImageId).attr("src", "/Content/Default.gif");
        $("#assetId_" + currentImageId).val("");
        $("#assetFilePath_" + currentImageId).val("");
        $("#asset_desc_" + currentImageId).val("");            
    });

    $(".uploadImage").bind("click", function(){            
        currentImageId = $(this).attr("imageId");            
        $("#divUpload").dialog('open');            
        return false;
    });

    $(".galleryImage").bind("click", function(){            
        currentImageId = $(this).attr("imageId");            
        $("#divGallery").dialog('open');            
        initGallery();
        return false;
    });

    $(".embedImage").bind("click", function(){            
        currentImageId = $(this).attr("imageId"); 
        var src = $("#img_thumb_" + currentImageId).attr("src");
        var desc = $("#asset_desc_" + currentImageId).val();
        if (desc == 'Ghi chú ảnh (tối đa 64 ký tự)') desc = '';
        if (src == "/Content/Default.gif"){
            alert("Bạn hãy chọn ảnh trước khi chèn vào bài viết");
        }else{
            content = '<img src="' + src.replace("/Small/", "/Large/") + '" style="margin:2px" title="' + desc + '" />';
            tinyMCE.get("Content").execCommand("mceInsertContent", false, content);                       
        }
        return false;
    });

    $("#upload_content textarea[name='AssetDescription']").bind({
        focus : function(){                
            if ($(this).val() == 'Ghi chú ảnh (tối đa 64 ký tự)') {
                $(this).val('');
                $(this).css("color", "black");
            }
        },
        blur : function (){
            if ($(this).val() == '') {
                $(this).val('Ghi chú ảnh (tối đa 64 ký tự)');
                $(this).css("color", "gray");
            }else{
                $(this).css("color", "black");
            }
        }        
    });
                          
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
        theme_advanced_toolbar_location : "top",
        theme_advanced_toolbar_align : "left",
        content_css : THEME_PATH + "Styles/main.css",
        relative_urls: false,
        entity_encoding: "raw",
        plugin_preview_width: "100%"
    });
    $('#Sumit').click( function(){
        tinyMCE.triggerSave(true,true);
    });

    $('#imageUpload').uploadify({
        'uploader': '/Content/Uploadify/uploadify.swf',
        'script': '/Ajax/FileUpload/Images',
        'folder': '/Upload',         
        'queueID': 'imageUploadQueue',   
        'width': 100,
        'scriptAccess':'always',
        'multi': false,
        'auto' : true,    
        'scriptData': { token:token},          
        'displayData': 'speed',
        'queueSizeLimit': 1,
        'fileDesc'  : '*.jpg;*.jpeg;*.gif;*.png',
        'fileExt'  : '*.jpg;*.jpeg;*.gif;*.png',
        'sizeLimit'  : (500 * 1024),
        'cancelImg': '/Content/Uploadify/cancel.png',
        'buttonText' : 'Upload Image',
        'onError': function(event, queueID, fileObj, errorObj) {
                    if (errorObj.type == 'File Size'){
                        alert("File upload vượt quá kích thước quy định 500Kb");
                    }else{
                        alert("Lỗi!!! Loại: [" + errorObj.type + "] Thông tin: [" + errorObj.info + "]");
                    }
                },
        'onQueueFull': function (evt, queueSizeLimit){
                alert('Bạn chỉ được Upload tối đa ' + queueSizeLimit + ' file đồng thời.');
                return false;
                },
        'onProgress': function(event,ID,fileObj,data) {
                $('#ajaxLoading').show();
                },
        'onComplete': function (evt, queueID, fileObj, response, data){
                if(response != null){
                    if(checkUploadResult(response) == 0) return;
                    arrTemp = response.split(",");                            
                    if(arrTemp.length == 2){  
                        if (currentImageId > 0){
                            $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
                            $("#assetId_" + currentImageId).val(arrTemp[0]);   
                            $("#assetFilePath_" + currentImageId).val(arrTemp[1]); 
                        }else{
                            $("#imagePath").val(arrTemp[1]);
                            $("#imagePath_thumb").attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
                        }                            
                    }
                }
                $('#ajaxLoading').hide();
                $("#divUpload").dialog('close');
                forceReloadGallery = true;
            }
    });

    $("#imageUploadSubmit").click(function(){
        var url = $("#ImageUploadUrl").val();
        if (isUrl(url)){
            $("#imageUploadDisplay").attr("src", url);
            $("#imageUploadDisplay").css("width", "70px");
            $.ajax({
                url: "/Ajax/FileUpload/ImageUrl",
                type: "POST",
                data: {"token" : token, "url" : url},
                dataType: "html",
                success: function(result) {
                    if (result.substr(0, 6) == "error:") {
                        alert(result.replace("error:", ""));
                    }
                    else {
                        arrTemp = result.split(",");
                        if(arrTemp.length == 2){                 
                            if (currentImageId > 0){
                                $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
                                $("#assetId_" + currentImageId).val(arrTemp[0]);    
                                $("#assetFilePath_" + currentImageId).val(arrTemp[1]); 
                            }else{
                                $("#imagePath").val(arrTemp[1]);
                                $("#imagePath_thumb").attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
                            }                                  
                        }
                    }

//                    if(checkUploadResult(result) == 0){
//                        alert(result);
//                    }else{
//                        arrTemp = result.split(",");
//                        if(arrTemp.length == 2){                 
//                            if (currentImageId > 0){
//                                $("#img_thumb_" + currentImageId).attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
//                                $("#assetId_" + currentImageId).val(arrTemp[0]);    
//                                $("#assetFilePath_" + currentImageId).val(arrTemp[1]); 
//                            }else{
//                                $("#imagePath").val(arrTemp[1]);
//                                $("#imagePath_thumb").attr("src", MEDIA_URL +  "/Thumbnail/Small/" + arrTemp[1]);
//                            }                                  
//                        }
//                    }
                    $("#divUpload").dialog('close');
                    forceReloadGallery = true;
                },
                error: function(error) { alert(error); }
            });
        }else{
            alert("Địa chỉ nhập vào không chính xác");
        }
    });
});

function initImageDescription(){
    $("#upload_content textarea[name='ImageDescription']").each(function(){
        if ($(this).val() == 'Ghi chú ảnh (tối đa 64 ký tự)') {                    
            $(this).css("color", "gray");
        }else{
            $(this).css("color", "black");
        }
    });
}

function initGallery(){
    if ($("#listImages").html() == '' || forceReloadGallery){
        getAssets(1);
    }
}

function chooseAsset(assetId){
    var src = $("#asset_thumb_" + assetId).attr("src");
    var src2;
    if (src.indexOf(MEDIA_URL) == 0){
        src2 = src.substring(MEDIA_URL.length + 16);
    }else{
        src2 = src.substring(16);    
    }

    if (currentImageId > 0){
        $("#img_thumb_" + currentImageId).attr("src", src);
        $("#assetId_" + currentImageId).val(assetId);     
        $("#assetFilePath_" + currentImageId).val(src2);          
    }else{        
        $("#imagePath_thumb").attr("src", src);
        $("#imagePath").val(src2);
    }   
    $("#divGallery").dialog('close'); 
}

function getAssets(index){        
    $.ajax({
            url: "/Ajax/Account/GetAssetThumbnails",
            type: "GET",
            data: { 'thumbSize': 'Small', 'pageIndex' : index },
            dataType: "html",
            success: function(result) {  
                $("#listImages").html(result);                                                
            },
            error: function(error) { alert(error); }
        });
}

function checkUploadResult(result){
    if (result.substr(0,6) == "error:"){
        alert(result.replace("error:", ""));
    }
    return true;
}

function showTooltip(ob){
        
    ob.tooltip({
        bodyHandler: function(){                
            img= ob.find("img:last");                
            content = '<div class="picture"><img src="' + img.attr("src").replace("/Small/", "/Medium/") + '" /></div>';
            return content;
        },
        track: true,
        showURL: false,
        extraClass: "tooltip_picture"
    });
}
    
$("#CityId").change(function() {    
    if ($("#CityId > option:selected").attr("value") != "" && $("#DistrictId").length > 0){
            $.ajax({
            url: "/Ajax/GetDistricts/" + $("#CityId > option:selected").attr("value"),
            type: "POST",
            data: { 'id': $(this).val() },
            dataType: "json",
            success: function(result) { 
                var items = "<option value=''>---Chọn quận huyện---</option>";  
                $.each(result, function(i, state) {  
                    items += "<option value=\"" + state.Value + "\">" + state.Text + "</option>";  
                });  
                $("#DistrictId").html(items);  
            },
            error: function(error) { alert(error); }
        })
    }        
});  

function isNumeric(e){
    var key =(window.event) ? window.event.keyCode:e.charCode;
    var nkey = String.fromCharCode(key);
    if(key !=0 && nkey.match(/[^0-9]/)){
        KeypressThrow(e);
    }
}
function KeypressThrow(e){
    if(window.event)
            window.event.returnValue = null;
    else
        if(e.preventDefault)
			e.preventDefault();
			e.returnValue = false;
    return false;    
}
function isUrl(s) {
	var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@@!\-\/]))?/
	return regexp.test(s);
}

/*Google Maps*/
// Variables to hold 
// INITIALIZATION PART ENDS
var map;
var clickmarker;
var infoWindow;
var searchInfoWindow;
var searchClickMarker;
var marker_placed = false;
var old_id = 0;
var city_location = new Array();
var geocoder;
var map;

window.onload = loadGoogleMapScript;

function geocode(address) 
{
    geocoder.geocode({'address':address}, onGeocodeResult);
}	

function onGeocodeResult(result, status) 
{
    if (status == google.maps.GeocoderStatus.OK) { 
        //CancelSearch();
	    // Get the lattitude and langgitude of the first result (note: real app should check for no result being returned)
	    var myLatitudeAndLangitude=new google.maps.LatLng(
		    result[0]['geometry']['location']['lat'](),
		    result[0]['geometry']['location']['lng']());

	    // Show a marker in the first result.
	    // http://code.google.com/apis/maps/documentation/javascript/overlays.html		
//	    searchClickMarker = new google.maps.Marker({
//		    map:map,
//		    draggable:true,
//		    animation:google.maps.Animation.BOUNCE,
//		    position:myLatitudeAndLangitude
//	    });
	    map.panTo(myLatitudeAndLangitude);
	
	    var myOptions = {
		    zoom: 16,
	    }	
	    map.setOptions(myOptions);
	
//	    searchInfoWindow = new google.maps.InfoWindow({
//		    content: result[0]['formatted_address']
//	    });
//	    searchInfoWindow.open(map, searchClickMarker);	
    }else{
        alert('Không tìm thấy địa điểm nào thỏa mãn');
    }
}

function loadGoogleMapScript() {
  var script = document.createElement("script");
  script.type = "text/javascript";
  script.src = "http://maps.google.com/maps/api/js?sensor=true&callback=initialize";
  document.body.appendChild(script);
}
  
function initialize() {     
    if (document.getElementById("map_canvas") != null) 
    {
        var defaultLatLng;
        if (typeof(init_InfoWindow) != "undefined"){
            defaultLatLng = new google.maps.LatLng(init_lat, init_lng);
        }else{
            defaultLatLng = new google.maps.LatLng(21.02777771911, 105.85237033331);
        }

        var myOptions = { 
            zoom: 16,
            center: defaultLatLng,
            scrollwheel: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

        map.setCenter(defaultLatLng);

        geocoder = new google.maps.Geocoder();

        if (typeof(init_InfoWindow) != "undefined"){
            loadMarker(defaultLatLng, init_InfoWindow);    
        }
    
        google.maps.event.addListener(map, 'click', function(event) {
            if (marker_placed) {
                Cancel();            
            }else{
                placeMarker(event.latLng);
                marker_placed = true;
            }
        
        });

        city_location[1] = new google.maps.LatLng(21.027521, 105.852449);
        city_location[2] = new google.maps.LatLng(10.759579, 106.668661);
        city_location[3] = new google.maps.LatLng(20.860221, 106.680783);
        city_location[4] = new google.maps.LatLng(16.050528, 108.213351);
        city_location[5] = new google.maps.LatLng(10.032231, 105.783058);
        city_location[6] = new google.maps.LatLng(20.419280, 106.168141);
        city_location[7] = new google.maps.LatLng(19.807559, 105.776839);
        city_location[8] = new google.maps.LatLng(18.669806, 105.684185);
        city_location[9] = new google.maps.LatLng(12.247155, 109.189210);
        city_location[10] = new google.maps.LatLng(10.033829, 105.780144);
    }
}

function placeMarker(location) {
    clickmarker = new google.maps.Marker({
        position: location,
        clickable: false,
        map: map
    });
    
    var contentString = '<table id="formInput" width="350" style="border-spacing:5px;"><tr><td colspan="2" class="content_title">Nhập địa chỉ</td></tr><tr><td nowrap="nowrap" class="form_name"><font color="red">*</font>Địa chỉ:</td><td><textarea style="width:300px; height:70px" name="Google_Adress" id="gmap_address_0" class="form_control" maxlength="200" ></textarea></td></tr><tr><td colspan="2" align="center"><input type="button" value="Hủy bỏ" id="calcelMap" onclick="CancelMarker();" class="form_button"/></td></tr></table><input type="hidden" id="Google_lat" name="Google_lat" value="' + location.lat() + '" /><input type="hidden" name="Google_lng" id="Google_lng" value="' + location.lng() + '" /><input type="hidden" name="action" value="insert" />';

    infoWindow = new google.maps.InfoWindow({ content: contentString });
    infoWindow.open(map, clickmarker);

    google.maps.event.addListener(infoWindow, 'closeclick', function() { clickmarker.setVisible(false); clickable = true; });
}
  
function Cancel() {
    CancelSearch();
    if (infoWindow) {
        infoWindow.close();
    }    
    clickmarker.setVisible(false); 
    clickable = true; 
    marker_placed = false;    
}

function CancelMarker() {
    CancelSearch();
    if (infoWindow) {
        infoWindow.close();
    }    
    clickmarker.setVisible(false); 
    clickable = true;
    marker_placed = true;    
}

function CancelSearch(){
    if (searchInfoWindow) {
        searchInfoWindow.close();
    }    
    if (searchClickMarker){
        searchClickMarker.setVisible(false); 
        searchClickMarker = true;
    }
}

function loadMarker(myLocation, myInfoWindow) {        
    clickmarker = new google.maps.Marker({
        position: myLocation,
        clickable: false,
        map: map
    });

    infoWindow = new google.maps.InfoWindow({ content: myInfoWindow });
    infoWindow.open(map, clickmarker);

    google.maps.event.addListener(infoWindow, 'closeclick', function() { clickmarker.setVisible(false); clickable = true; });

}

function changeCityMap() {
    map.setCenter(city_location[document.getElementById("city_map").value]);
}

function change_map_size(value) {
    switch (value) {
        case "1":
            document.getElementById("map_canvas").style.width = "600px";
            document.getElementById("map_canvas").style.height = "400px";
            break;
        case "2":
            document.getElementById("map_canvas").style.width = "800px";
            document.getElementById("map_canvas").style.height = "500px";
            break;
        case "3":
            document.getElementById("map_canvas").style.width = "1024px";
            document.getElementById("map_canvas").style.height = "600px";
            break;
    }
}

$(function(){
    $("#address").autocomplete({
        source: function (request, response) {
            if (geocoder == null) {
                geocoder = new google.maps.Geocoder();
            }
            geocoder.geocode({
                'address': request.term
            }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {

                    var searchLoc = results[0].geometry.location;
                    var lat = results[0].geometry.location.lat();
                    var lng = results[0].geometry.location.lng();
                    var latlng = new google.maps.LatLng(lat, lng);
                    var bounds = results[0].geometry.bounds;

                    geocoder.geocode({
                        'latLng': latlng
                    }, function (results1, status1) {
                        if (status1 == google.maps.GeocoderStatus.OK) {
                            if (results1[1]) {
                                response($.map(results1, function (loc) {
                                    return {
                                        label: loc.formatted_address,
                                        value: loc.formatted_address,
                                        bounds: loc.geometry.bounds
                                    }
                                }));
                            }
                        }
                    });
                }
            });     
        },
        select: function (event, ui) {
            var pos = ui.item.position;
            var lct = ui.item.locType;
            var bounds = ui.item.bounds;
            if (bounds) {
                map.fitBounds(bounds);
            }
        }
    });
});