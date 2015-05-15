$(window).on("load", function () {
    $(document)
        .ajaxStart(function () {
            $("#ajax-loading").show();
        })
        .ajaxStop(function () {
            $("#ajax-loading").hide();
            if ($("img.lazyload").length > 0) {
                $("img.lazyload").show().lazyload({ threshold: 200, failure_limit: 50 }).removeClass("lazyload");
            }
            $(window).resize();
        });
    if ($("img.lazyload").length > 0) {
        $("img.lazyload").show().lazyload({ threshold: 200, failure_limit: 50 }).removeClass("lazyload");
    }
});
/*END Startup function*/

String.prototype.trim = function () { return this.replace(/^\s\s*/, '').replace(/\s\s*$/, ''); };
String.prototype.ltrim = function () { return this.replace(/^\s+/, ''); }
String.prototype.rtrim = function () { return this.replace(/\s+$/, ''); }
String.prototype.fulltrim = function () { return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' '); }

if (!Array.prototype.forEach) {
    Array.prototype.forEach = function (fn, scope) {
        for (var i = 0, len = this.length; i < len; ++i) {
            fn.call(scope, this[i], i, this);
        }
    }
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

function Get_Cookie(name) {
    var start = document.cookie.indexOf(name + "=");
    var len = start + name.length + 1;
    if ((!start) && (name != document.cookie.substring(0, name.length))) {
        return null;
    }
    if (start == -1) return null;
    var end = document.cookie.indexOf(";", len);
    if (end == -1) end = document.cookie.length;
    return unescape(document.cookie.substring(len, end));
}

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : ";path=/; expires=" + exdate.toUTCString() + ";domain=.hangtot.com");
    document.cookie = c_name + "=" + c_value;
}

function changeCity(cId) {
    setCookie("City", cId, 365);
    reloadCity(cId);
}
function changePrivateCity(cId) {
    setCookie("City", cId, 365);
    reloadCity(cId);
}

function reloadCity(cId) {
    var hash = location.href.indexOf("?") > -1 ? querystring(location.href.substr(location.href.indexOf("?") + 1)) : {};
    if (cId == 0) {
        delete hash["cityId"];
    } else {
        hash["cityId"] = cId;
    }
    var params = decodeURIComponent($.param(hash));
    if (params == "") {
        window.location.reload();
    }
    else {
        window.location.href = window.location.href.split("?")[0] + "?" + params;
    }
}

/*BEGIN Product Details*/
function displayYM(name, im, msg) {
    document.write('<a title="' + name + '" style="text-decoration:none; display: block; padding: 4px;" href="ymsgr:sendim?' + im + '&amp;m=' + msg + '"><img alt="Yahoo" src="http://opi.yahoo.com/online?u=' + im + '&amp;m=g&amp;t=1&amp;l=us"></a>');
}

function displaySkype(name, nick) {
    document.write('<a title="' + name + '" href="skype:' + nick + '?chat" style="text-decoration:none"><img src="http://mystatus.skype.com/smallclassic/' + nick + '" style="border: none;" width="114" height="20" alt="Skype" /></a>');
}
/*END Product Details*/

/*Base64 encode, decode from stringencoders.googlecode.com*/
base64 = {}; base64.PADCHAR = "="; base64.ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"; base64.getbyte64 = function (a, b) { var c = base64.ALPHA.indexOf(a.charAt(b)); if (c == -1) { throw "Cannot decode base64" } return c }; base64.decode = function (a) { a = "" + a; var b = base64.getbyte64; var c, d, e; var f = a.length; if (f == 0) { return a } if (f % 4 != 0) { throw "Cannot decode base64" } c = 0; if (a.charAt(f - 1) == base64.PADCHAR) { c = 1; if (a.charAt(f - 2) == base64.PADCHAR) { c = 2 } f -= 4 } var g = []; for (d = 0; d < f; d += 4) { e = b(a, d) << 18 | b(a, d + 1) << 12 | b(a, d + 2) << 6 | b(a, d + 3); g.push(String.fromCharCode(e >> 16, e >> 8 & 255, e & 255)) } switch (c) { case 1: e = b(a, d) << 18 | b(a, d + 1) << 12 | b(a, d + 2) << 6; g.push(String.fromCharCode(e >> 16, e >> 8 & 255)); break; case 2: e = b(a, d) << 18 | b(a, d + 1) << 12; g.push(String.fromCharCode(e >> 16)); break } return g.join("") }; base64.getbyte = function (a, b) { var c = a.charCodeAt(b); if (c > 255) { throw "INVALID_CHARACTER_ERR: DOM Exception 5" } return c }; base64.encode = function (a) { if (arguments.length != 1) { throw "SyntaxError: Not enough arguments" } var b = base64.PADCHAR; var c = base64.ALPHA; var d = base64.getbyte; var e, f; var g = []; a = "" + a; var h = a.length - a.length % 3; if (a.length == 0) { return a } for (e = 0; e < h; e += 3) { f = d(a, e) << 16 | d(a, e + 1) << 8 | d(a, e + 2); g.push(c.charAt(f >> 18)); g.push(c.charAt(f >> 12 & 63)); g.push(c.charAt(f >> 6 & 63)); g.push(c.charAt(f & 63)) } switch (a.length - h) { case 1: f = d(a, e) << 16; g.push(c.charAt(f >> 18) + c.charAt(f >> 12 & 63) + b + b); break; case 2: f = d(a, e) << 16 | d(a, e + 1) << 8; g.push(c.charAt(f >> 18) + c.charAt(f >> 12 & 63) + c.charAt(f >> 6 & 63) + b); break } return g.join("") }

function formatMoney(money, separator) {
    //su dung jquery number-formatter
    if (separator == null || separator == '')
        separator = '.'
    return $.formatNumber(money, { format: "#" + separator + "###", locale: "vn" });

}

//SearchSuggest
var timer;

var prevAjaxRequest = null;
var cacheSuggest = {};
var clearCacheSuggestTimeout = 0;
var isProductSearch = 1;

$(window).on("load", function () {
    isProductSearch = $(".frm-quick-search").attr("action") === HANGTOT_DOMAIN + '/tim-kiem.html' ? 1 : 0;
    $(".search-option").click(function () {
        var selected = $(this);
        $(".current-search-area").text(selected.text());
        if (selected.attr("switch-to") === 'ad') {
            $(".frm-quick-search").attr("action", "/tim-kiem.html");
            $(".SuggestResult").html("");
            isProductSearch = 0;
        } else if (selected.attr("switch-to") === 'advisory') {
            $(".frm-quick-search").attr("action", REVIEWS_DOMAIN + "/tim-kiem.html");
            $(".SuggestResult").html("");
            isProductSearch = 0;
        }
        else {
            $(".frm-quick-search").attr("action", HANGTOT_DOMAIN + "/tim-kiem.html");
            isProductSearch = 1;
        }
        $(".search-name").children(".sub-lv1-search").css({
            opacity: 0,
            left: "-9999em",
            top: "-9999em"
        });
        setTimeout(function () {
            $(".search-name").children(".sub-lv1-search").attr("style", "");
        }, 100);
    });

    $('.txtSearchKey').bind(jQuery.browser.opera ? "keypress" : "keydown", function (event) {
        var mainSearch = $(this).parents(".search-main");
        if (isProductSearch == 1) {
            var charcode = event.which;
            if (charcode == 13) {
                return true;
            }
            if (charcode == 38) {
                //UP key press
                event.preventDefault();
                setCurrentSearchSuggestSelect(mainSearch, 1);
                return false;
            } else if (charcode == 40) {
                //DOWN key press
                event.preventDefault();
                setCurrentSearchSuggestSelect(mainSearch, -1);
                return false;
            }

            var $this = $(this);
            clearTimeout(timer);
            setTimeout(function () {
                getSuggestSearch(mainSearch, $this.val(), $this.hasClass("main-text"));
            }, 150);
        }
    });

    $('.txtSearchKey').click(function (event) {
        var mainSearch = $(this).parents(".search-main");
        mainSearch.find(".SuggestResult").css("width", mainSearch.find(".frm-quick-search").width() + 2);
        mainSearch.find(".SuggestResult").show();
        event.preventDefault();
        event.stopPropagation();
    });

    $(".SuggestResult").click(function (e) {
        if (e.target.nodeName.toLowerCase() != "a" && e.target.nodeName.toLowerCase() != "span" && e.target.nodeName.toLowerCase() != "strong") {
            e.preventDefault();
            e.stopPropagation();
        }
        else {
            return true;
        }
    });
    $("#header .search-main .txtSearchKey").bind(jQuery.browser.opera ? "keypress" : "keyup", function (event) {
        $("#menu-top .txtSearchKey").val($(this).val());
    });

    $("#menu-top .txtSearchKey").bind(jQuery.browser.opera ? "keypress" : "keyup", function (event) {
        $("#header .search-main .txtSearchKey").val($(this).val());
    });

    $(document).click(function (e) {
        $(".SuggestResult").hide();
    });
});

function g(x) {
    return document.getElementById(x);
}
function CheckSearch(e) {
    var textbox = $(e).find(".txtSearchKey");
    var value = textbox.val();
    console.log(value);
    if (Trim(value) == '' || value == 'Từ khóa tìm kiếm') {
        alert("Mời bạn nhập từ khóa cần tìm kiếm");
        textbox.focus();
        return false;
    }
    return true;
}

function Trim(iStr) {
    while (iStr.charCodeAt(0) <= 32) {
        iStr = iStr.substr(1);
    }
    while (iStr.charCodeAt(iStr.length - 1) <= 32) {
        iStr = iStr.substr(0, iStr.length - 1);
    }
    return iStr;
}

//clear Cache after 3 minutes     
function clearCacheSuggest() {
    clearTimeout(clearCacheSuggestTimeout);
    clearCacheSuggestTimeout = setTimeout(function () {
        cacheSuggest = {};
        clearCacheSuggest();
    }, 180000);
}

clearCacheSuggest();

function getSuggestSearch(mainSearch, keyword, isMain) {
    if (prevAjaxRequest) prevAjaxRequest.abort();

    keyword = $.trim(keyword);
    if (keyword === "") {
        $(".SuggestResult").html("");
        $(".home-search-ajax-loading").hide();
    } else {
        var cacheData = cacheSuggest[keyword];
        if (cacheData == null) {
            mainSearch.find(".home-search-ajax-loading").css("display", "block");
            prevAjaxRequest = $.ajax({
                url: HANGTOT_DOMAIN + "/Ajax/SearchSuggest",
                type: "GET",
                data: {
                    'keyword': keyword,
                    'rand': Math.random()
                },
                dataType: "html",
                success: function (result) {
                    mainSearch.find(".home-search-ajax-loading").css("display", "none");
                    mainSearch.find(".SuggestResult").html(result).css("display", "block");
                    cacheSuggest[keyword] = result;
                    if (isMain) {
                        $("#menu-top .SuggestResult").html($("#header .SuggestResult").html());
                    }
                    else {
                        $("#header .SuggestResult").html($("#menu-top .SuggestResult").html());
                    }
                },
            });
        } else {
            mainSearch.find(".SuggestResult").html(cacheData).css("display", "block");
        }
    }
}

function setCurrentSearchSuggestSelect(searchMain, upOrDown) {
    var currentSelect = searchMain.find(".SuggestResult ul li.list_current");
    var currentIndex = -1;
    if ($(currentSelect).length > 0) {
        currentIndex = searchMain.find(".SuggestResult ul li").index(currentSelect);
    }
    var totalResult = searchMain.find(".SuggestResult ul li").length;

    if (upOrDown == 1) {
        //UP
        if (currentIndex > 0) {
            currentIndex--;
        }
        else {
            currentIndex = totalResult - 1;
        }
    } else {
        //DOWN
        if (currentIndex < totalResult - 1) {
            currentIndex++;
        } else {
            currentIndex = 0;
        }
    }
    currentSelect = searchMain.find(".SuggestResult ul li:eq(" + currentIndex + ")");
    searchMain.find(".SuggestResult ul li").removeClass("list_current");
    currentSelect.addClass("list_current");
    var link = currentSelect.find('a').first();
    if ($(link).attr("fullName")) {
        searchMain.find(".txtSearchKey").val($(link).attr("fullName"));
    } else {
        searchMain.find(".txtSearchKey").val(Trim(link.text()));
    }
}
function scrollTo(id, time) {
    if (time <= 0) {
        time = 500;
    }
    if ($("#" + id).length > 0) {
        $('html, body').animate({
            scrollTop: $("#" + id).offset().top
        }, time);
    }
    return false;
}
function goToTabs(index) {
    $(".tabs").tabs({ active: index });
    return false;
}
/*BEGIN Top header functions*/
function writeSupportContent() {
    $('#header-popup-support').html('<div class="menu-top-content sub-lv1"><span class="menu-top-content-icon menu-top-sub-icon"></span><div class="sub-lv1-content" style="padding-left: 7px; padding-right: 0px; padding-bottom: 53px;"><div class="menu-top-support-tittle" style="line-height: 24px; width: 545px;"><span> Chào mừng bạn đến với Hangtot.com! </span></div><div class="menu-top-support-content" style="padding-top: 12px;"><span> Mọi ý kiến đóng góp và thắc mắc, quý khách vui lòng liên hệ với chúng tôi: </span><div class="detail-support"><div class="support-yahoo"><a href="ymsgr:sendIM?hotro_hangtot">Hỗ trợ khách hàng</a></div><div class="support-skype"><a href="skype:hotro_hangtot?chat">Hỗ trợ khách hàng</a></div><div class="support-phone"> <strong>(04) 6650 5818</strong> </div><div class="support-mail"> <a href="mailto:support@hangtot.com">support@hangtot.com</a> </div></div><div class="clear"></div><span> Thời gian làm việc: </span><b> 8h-12h và 13h30 -17h30 </b><i> từ thứ 2 đến tứ 6. Thứ 7 </i><span> từ </span> <b> 8h -12h </b><br /><span> Ngoài giờ hành chính xin gửi về địa chỉ email </span><a href="mailto:support@hangtot.com" style="display: inline;"> support@hangtot.com </a><div style="text-align: center"><i>Kết nối người bán và người mua <br> Giúp cho việc bán hàng trở nên dễ dàng hơn và việc mua hàng trở thành niềm vui</i></div></div><img src="/Themes/Portal/Default2/Styles/Images/popup_support_icon.png" style="position: absolute; right: 0; top: 30px;"/><div class="menu-top-support-bottom" style="position: absolute; left: 1px; top: 254px; width: 562px; padding-top: 6px;"><span>HANGTOT.COM KHÔNG BÁN HÀNG TRỰC TIẾP</span><span>Quý khách mua hàng xin vui lòng liên hệ với người bán hoặc gọi số điện thoại hỗ trợ</span></div></div></div>');
}

var checkLoadPCateChild = false;
var checkLoadPcateParent = false;
var checkLoadAdCateChild = false;
var checkLoadAdcateParent = false;
$(window).on("load", function () {
    var productCategoryByParentAjax = null;
    $(".view-pCate-top-menu").mouseenter(function () {
        if (productCategoryByParentAjax) productCategoryByParentAjax.abort();
        pTimer = setTimeout(function () {
            //load parrent menu
            if (!checkLoadPcateParent) {
                productCategoryByParentAjax = $.ajax({
                    url: HANGTOT_DOMAIN + "/ajax/GetProductCategoryByParentId/0",
                    type: "POST",
                    dataType: "JSon",
                    success: function (result) {
                        checkLoadPcateParent = true;
                        var htmlWrite = "";
                        htmlWrite += "<ul style='overflow: hidden;'>";
                        for (var i = 0; i < result.length; i++) {
                            htmlWrite += '<li idata="' + result[i].Id + '" class="cate_item_' + result[i].Id + '">';
                            htmlWrite += '<a href="' + HANGTOT_DOMAIN + '/' + result[i].Id + '/' + result[i].Alias + '.html">';
                            htmlWrite += result[i].Name;
                            htmlWrite += "</a>";
                            htmlWrite += "</li>";
                        }
                        htmlWrite += "</ul>";

                        $('#main-categories').html(htmlWrite);

                        //load child menu
                        if (!checkLoadPCateChild) {
                            $.getScript(HANGTOT_DOMAIN + "/Content/left_menu.js")
                            .done(function (script, textStatus) {
                                $("body").append($("<script />", {
                                    html: script
                                }));
                                checkLoadPCateChild = true;
                                initLeftMenuDrop();
                            })
                            .fail(function (jqxhr, settings, exception) {
                                $('.view-pCate-top-menu').find(".sub-lv1-pcate").css("display", "none");
                                checkLoadPCateChild = false;
                            });
                        } else {
                            initLeftMenuDrop();
                        }
                        $(".view-pCate-top-menu").addClass("view-pCate-top-menu-control");
                    },
                    complete: function () {
                        if (checkLoadPcateParent == false || typeof (checkLoadPcateParent) == "undefined") {
                            $('.view-pCate-top-menu').find(".sub-lv1-pcate").css("display", "none");
                        }
                    }
                });
            } else {
                $(".view-pCate-top-menu").addClass("view-pCate-top-menu-control");
            }
        }, 200)
    }).mouseleave(function () {
        if ($(".view-pCate-top-menu").hasClass("view-pCate-top-menu-control")) {
            $(".view-pCate-top-menu").removeClass("view-pCate-top-menu-control");
        }
        clearTimeout(pTimer);
    });

    $(".border-top-menu ul li").click(function (e) {
        e.stopPropagation();
    });

    var getAdCateByParrentAjax = null;
    $(".view-adCate-top-menu").mouseenter(function () {
        if (getAdCateByParrentAjax) getAdCateByParrentAjax.abort();
        adTimer = setTimeout(function () {
            if (!checkLoadAdcateParent) {
                getAdCateByParrentAjax = $.ajax({
                    url: "/Categories/GetAdCateByParrent/0",
                    type: "POST",
                    dataType: "JSon",
                    success: function (result) {
                        checkLoadAdcateParent = true;
                        var cityId = Get_Cookie('Ad_City');
                        var cityName = Get_Cookie('Ad_City_Name');
                        if (cityId == null) {
                            cityId = 0;
                            cityName = 'Toan-Quoc';
                        }
                        var htmlWrite = "";
                        htmlWrite += "<ul style='overflow: hidden;'>";
                        for (var i = 0; i < result.length; i++) {
                            htmlWrite += '<li idata="' + result[i].Id + '" class="cate_item_' + result[i].Id + '">';
                            htmlWrite += '<a href="/' + cityName + '/' + result[i].Alias + '-c' + result[i].Id + '.html">';
                            htmlWrite += result[i].Name;
                            htmlWrite += "</a>";
                            htmlWrite += "</li>";
                        }
                        htmlWrite += "</ul>";

                        $('#main-ad-categories').html(htmlWrite);
                        if (!checkLoadAdCateChild) {
                            $.getScript("/Content/left_menu_ad.js")
                            .done(function (script, textStatus) {
                                $("body").append($("<script />", {
                                    html: script
                                }));
                                checkLoadAdCateChild = true;
                                initLeftMenuADDrop();
                            })
                            .fail(function (jqxhr, settings, exception) {
                                $('.view-adCate-top-menu').find(".sub-lv1-adcate").css("display", "none");
                                checkLoadAdCateChild = false;
                            });
                        }
                        $(".view-adCate-top-menu").addClass("view-adCate-top-menu-control");
                    },
                    complete: function () {
                        if (checkLoadAdcateParent == false || typeof (checkLoadAdcateParent) == "undefined") {
                            $('.view-adCate-top-menu').find(".sub-lv1-adcate").css("display", "none");
                        }
                    }
                });
            }
            else {
                $(".view-adCate-top-menu").addClass("view-adCate-top-menu-control");
                if (checkLoadAdCateChild) {
                    initLeftMenuADDrop();
                }
            }
        }, 200);
    }).mouseleave(function () {
        if ($(".view-adCate-top-menu").hasClass("view-adCate-top-menu-control")) {
            $(".view-adCate-top-menu").removeClass("view-adCate-top-menu-control");
        }
        clearTimeout(adTimer);
    });

    $(".registration").mouseenter(function () {
        registrationTimer = setTimeout(function () {
            $(".registration").addClass("registration-control");
        }, 200);
    }).mouseleave(function () {
        if ($(".registration").hasClass("registration-control")) {
            $(".registration").removeClass("registration-control");
        }
        clearTimeout(registrationTimer);
    });

    $(".post-something").mouseenter(function () {
        postSomethingTimer = setTimeout(function () {
            $(".post-something").addClass("post-something-control");
        }, 200);
    }).mouseleave(function () {
        if ($(".post-something").hasClass("post-something-control")) {
            $(".post-something").removeClass("post-something-control");
        }
        clearTimeout(postSomethingTimer);
    });
    var menuLoginTimer = 0;
    $(".menu-top-login").mouseenter(function () {
        menuLoginTimer = setTimeout(function () {
            $(".menu-top-login").addClass("menu-top-login-control");
        }, 200);
    }).mouseleave(function () {
        if ($(".menu-top-login").hasClass("menu-top-login-control")) {
            $(".menu-top-login").removeClass("menu-top-login-control");
        }
        clearTimeout(menuLoginTimer);
    });

    $(".menu-top-cart").mouseenter(function () {
        menuCartTimer = setTimeout(function () {
            $(".menu-top-cart").addClass("menu-top-cart-control");
        }, 200);
    }).mouseleave(function () {
        if ($(".menu-top-cart").hasClass("menu-top-cart-control")) {
            $(".menu-top-cart").removeClass("menu-top-cart-control");
        }
        clearTimeout(menuCartTimer);
    });

    $.ajax({
        url: HANGTOT_DOMAIN + "/Accounts/GetAccountInfomation?returnUrl=" + encodeURIComponent(document.URL),
        xhrFields: {
            withCredentials: true
        },
        type: "GET",
        data: {
        },
        dataType: "html",
        success: function (result) {
            $(".menu-top-login").html(result);
        },
    });
});
/*END Top header functions*/
function adHit(adId) {
    $("<img src='/Hit?adId=" + adId + "&rand=" + Math.random() + "'/>").appendTo("body");
}

/*Metro click effect*/
(function ($) {
    $.fn.metromousedown = function (e) {
        var el = $(this).children(), originX = 0, originY = 0, ang = 10, orizorvert = 0, duration = 220, anim,
        mouse = { x: e.pageX - el.offset().left, y: e.pageY - el.offset().top };
        var positiontype = -1;
        // for a better antialiasing
        if (el.css('box-shadow') == 'none')
            el.css({ 'box-shadow': '0 0 1px transparent' })

        // needed to define how much links should tilt
        el.parent().css({
            '-webkit-perspective': el.outerWidth() * 5,
            'perspective': el.outerWidth() * 5
        })

        //identify mouse position relatively to the clicked box
        if (mouse.x < el.outerWidth() / 3) {
            orizorvert = 1          //  left
            originX = 100
            originY = 50
            ang = -ang
            positiontype = 0
        } else if (mouse.x > parseInt(el.outerWidth() * 2 / 3)) {
            orizorvert = 1          // right
            originX = 0
            originY = 50
            ang = ang
            positiontype = 1
        } else {
            if (mouse.y < el.outerHeight() / 3) {
                orizorvert = 2  // center-top
                originX = 50
                originY = 100
                ang = 2 * ang
                positiontype = 2
            } else if (mouse.y > parseInt(el.outerHeight() * 2 / 3)) {
                orizorvert = 2  // center-bottom
                originX = 50
                originY = 0
                ang = -2 * ang
                positiontype = 3
            }
        }

        $.each(el, function (i, e) {
            if (orizorvert > 0)
                $(e).css({
                    '-webkit-transform-origin': originX + "%" + " " + originY + "%",
                    'transform-origin': originX + "%" + " " + originY + "%",
                    '-ms-transform-origin': originX + "%" + " " + originY + "%"
                }).animate({ 'z-index': $(e).css('z-index') }, {
                    duration: duration, step: function (now, fx) {
                        anim = ang * Math.sin((fx.pos * Math.PI / 2))
                        $(e).css({
                            '-webkit-transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)',
                            'transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)',
                            '-ms-transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)'
                        })
                    }, queue: false
                }).delay(duration)
            else if (orizorvert == 0 || !$.browser.webkit) {
                $(e).css({
                    '-webkit-transform-origin': '50% 50%',
                    'transform-origin': '50% 50%',
                    '-ms-transform-origin': '50% 50%'
                }).animate({ 'z-index': $(e).css('z-index') }, {
                    duration: duration, step: function (now, fx) {
                        anim = 1 - Math.sin(fx.pos * Math.PI / 2) / 10
                        $(e).css({
                            '-webkit-transform': 'scale(' + anim + ')',
                            '-moz-transform': 'scale(' + anim + ')',
                            '-o-transform': 'scale(' + anim + ')',
                            '-ms-transform': 'scale(' + anim + ')'
                        })
                    }, queue: false
                }).delay(duration);
                positiontype = 4
            }
        })
        return positiontype;
    }

    $.fn.metromouseup = function (e, position) {
        var el = $(this).children(), originX = 0, originY = 0, ang = 0, orizorvert = 0, duration = 220, anim,
mouse = { x: e.pageX - el.offset().left, y: e.pageY - el.offset().top };

        // for a better antialiasing
        if (el.css('box-shadow') == 'none')
            el.css({ 'box-shadow': '0 0 1px transparent' })

        // needed to define how much links should tilt
        el.parent().css({
            '-webkit-perspective': el.outerWidth() * 5,
            'perspective': el.outerWidth() * 5
        })

        if (position == 0) {
            orizorvert = 1          //  left
            originX = 100
            originY = 50
            ang = -ang
        } else if (position == 1) {
            orizorvert = 1          // right
            originX = 0
            originY = 50
        } else if (position == 2) {
            orizorvert = 2  // center-top
            originX = 50
            originY = 100
            ang = 2 * ang
        } else if (position == 3) {
            orizorvert = 2  // center-bottom
            ang = -2 * ang
            originX = 50
            originY = 0
        }


        $.each(el, function (i, e) {
            if (orizorvert > 0)
                $(e).css({
                    '-webkit-transform-origin': originX + "%" + " " + originY + "%",
                    'transform-origin': originX + "%" + " " + originY + "%",
                    '-ms-transform-origin': originX + "%" + " " + originY + "%"
                })
                .animate({ 'z-index': $(e).css('z-index') }, {
                    duration: duration, step: function (now, fx) {
                        anim = ang * Math.sin((fx.pos * Math.PI / 2))
                        //var data = "<div>anim: " + anim + "</div>";
                        //$('body').append(data);
                        $(e).css({
                            '-webkit-transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)',
                            'transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)',
                            '-ms-transform': 'rotate' + (orizorvert == 1 ? 'Y' : 'X') + '(' + anim + 'deg)'
                        })
                    }, queue: false
                }).delay(duration)
            else if (orizorvert == 0)
                $(e).css({
                    '-webkit-transform-origin': '50% 50%',
                    'transform-origin': '50% 50%',
                    '-ms-transform-origin': '50% 50%'
                }).animate({ 'z-index': $(e).css('z-index') }, {
                    duration: duration, step: function (now, fx) {
                        anim = 0.9 + Math.sin(fx.pos * Math.PI / 2) / 10
                        $(e).css({
                            '-webkit-transform': 'scale(' + anim + ')',
                            '-moz-transform': 'scale(' + anim + ')',
                            '-o-transform': 'scale(' + anim + ')',
                            '-ms-transform': 'scale(' + anim + ')'
                        })
                    }, queue: false
                }).delay(duration)
        })
    }
})(jQuery)

/*login facebook*/
function signInFb(remember) {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            var form = document.createElement("form");
            form.setAttribute("method", 'post');
            form.setAttribute("action", HANGTOT_DOMAIN + '/Accounts/OpenId');

            var field = document.createElement("input");
            field.setAttribute("type", "hidden");
            field.setAttribute("name", 'AccessToken');
            field.setAttribute("value", response.authResponse.accessToken);
            form.appendChild(field);

            var field2 = document.createElement("input");
            field2.setAttribute("type", "hidden");
            field2.setAttribute("name", 'Type');
            field2.setAttribute("value", 1);
            form.appendChild(field2);

            var field3 = document.createElement("input");
            field3.setAttribute("type", "hidden");
            field3.setAttribute("name", 'Remember');
            field3.setAttribute("value", remember);
            form.appendChild(field3);

            var field4 = document.createElement("input");
            field4.setAttribute("type", "hidden");
            field4.setAttribute("name", 'Redirect');
            field4.setAttribute("value", document.URL);
            form.appendChild(field4);

            document.body.appendChild(form);
            form.submit();
        } else {
            login(remember);
        }
    });
    return false;
}

function login(remember) {
    FB.login(function (response) {
        if (response.authResponse) {
            var form = document.createElement("form");
            form.setAttribute("method", 'post');
            form.setAttribute("action", HANGTOT_DOMAIN + '/Accounts/OpenId');

            var field = document.createElement("input");
            field.setAttribute("type", "hidden");
            field.setAttribute("name", 'AccessToken');
            field.setAttribute("value", response.authResponse.accessToken);
            form.appendChild(field);

            var field2 = document.createElement("input");
            field2.setAttribute("type", "hidden");
            field2.setAttribute("name", 'Type');
            field2.setAttribute("value", 1);
            form.appendChild(field2);

            var field3 = document.createElement("input");
            field3.setAttribute("type", "hidden");
            field3.setAttribute("name", 'Remember');
            field3.setAttribute("value", remember);
            form.appendChild(field3);

            var field3 = document.createElement("input");
            field3.setAttribute("type", "hidden");
            field3.setAttribute("name", 'Redirect');
            field3.setAttribute("value", document.URL);
            form.appendChild(field3);

            document.body.appendChild(form);
            form.submit();
        } else {

        }
    }, { scope: 'email,user_birthday,publish_actions' });
}

/*END Login Facebook*/

/*BEGIN Login Yahoo*/
function signInYahoo(remember) {
    var form = document.createElement("form");
    form.setAttribute("method", 'post');
    form.setAttribute("action", HANGTOT_DOMAIN + '/Accounts/OpenId');

    var field2 = document.createElement("input");
    field2.setAttribute("type", "hidden");
    field2.setAttribute("name", 'Type');
    field2.setAttribute("value", 3);
    form.appendChild(field2);

    var field3 = document.createElement("input");
    field3.setAttribute("type", "hidden");
    field3.setAttribute("name", 'Remember');
    field3.setAttribute("value", remember);
    form.appendChild(field3);

    var field3 = document.createElement("input");
    field3.setAttribute("type", "hidden");
    field3.setAttribute("name", 'Redirect');
    field3.setAttribute("value", document.URL);
    form.appendChild(field3);

    document.body.appendChild(form);
    form.submit();
}

function signInGoogle(remember) {
    var form = document.createElement("form");
    form.setAttribute("method", 'post');
    form.setAttribute("action", HANGTOT_DOMAIN + '/Accounts/OpenId');

    var field2 = document.createElement("input");
    field2.setAttribute("type", "hidden");
    field2.setAttribute("name", 'Type');
    field2.setAttribute("value", 2);
    form.appendChild(field2);

    var field3 = document.createElement("input");
    field3.setAttribute("type", "hidden");
    field3.setAttribute("name", 'Remember');
    field3.setAttribute("value", remember);
    form.appendChild(field3);

    var field3 = document.createElement("input");
    field3.setAttribute("type", "hidden");
    field3.setAttribute("name", 'Redirect');
    field3.setAttribute("value", document.URL);
    form.appendChild(field3);

    document.body.appendChild(form);
    form.submit();
}

function querystring(e) {
    var _queryString = {};
    e.replace(new RegExp("([^?=&]+)(=([^&]*))?", "g"), function ($0, $1, $2, $3) { _queryString[$1] = $3; });
    return _queryString;
}
$(document).ready(function () {
    $("span.span-link[data-href],span.remove-filter-list[data-href],span.multifilter-label[data-href],span.multifilter-label-checked[data-href]").each(function () {
        $(this).replaceWith($('<a ' + ($(this).attr("style") != undefined ? 'style="' + $(this).attr("style") + '"' : '') + ' href="' + base64.decode($(this).attr("data-href")) + '" class="' + $(this).attr("class") + ' replace-link' + '">' + this.innerHTML + '</a>'));
    });
    writeSupportContent();
});