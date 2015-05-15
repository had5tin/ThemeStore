function getScript(url, success) {
    var script = document.createElement('script');
    script.src = url;
    var head = document.getElementsByTagName('head')[0],
            done = false;
    script.onload = script.onreadystatechange = function () {
        if (!done && (!this.readyState || this.readyState == 'loaded' || this.readyState == 'complete')) {
            done = true;
            success();
            script.onload = script.onreadystatechange = null;
            head.removeChild(script);
        }
    };
    head.appendChild(script);
}

function loadScript(src) {
    var s = document.createElement('script');
    s.type = 'text/javascript';
    s.async = true;
    s.src = src;
    var x = document.getElementsByTagName('script')[0];
    x.parentNode.insertBefore(s, x);
}

function loadAllScripts() {        
        loadScript('/Scripts/jquery-ui-1.8.15.min.js?v=1.3');
        loadScript('/Scripts/jcarousellite_1.0.1.min.js?v=1.3');
        loadScript('/Scripts/jquery.mousewheel.min.js?v=1.3');
        loadScript('/Scripts/jquery.lazyload.js?v=1.3');
        loadScript('/Scripts/jquery.validate.min.js?v=1.3');
        loadScript('/Scripts/jquery.validate.unobtrusive.min.js?v=1.3');
        loadScript('/Scripts/jquery.colorbox.min.js?v=1.3');
        loadScript('/Scripts/jshashtable-2.1.js?v=1.3');
        loadScript('/Scripts/jquery.numberformatter-1.2.3.min.js?v=1.3');
        loadScript('/Scripts/jquery.hover-intent.min.js?v=1.3');
        loadScript('/Scripts/jquery.qtip.min.js?v=1.3');
        loadScript('/Scripts/jquery.fancybox.min.js?v=1.3');
        loadScript('/Scripts/jquery.jqzoom-core.js?v=1.3');
        loadScript('/Scripts/jquery.autoresize.min.js?v=1.3');
        loadScript('/Scripts/ui.datepicker-vi.js?v=1.0');
        loadScript('/Scripts/ZeroClipboard.min.js?v=1.3');
        loadScript('/Scripts/ff.js?v=1.3');
}