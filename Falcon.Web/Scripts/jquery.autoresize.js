﻿// Autosize 1.12 - jQuery plugin for textareas
// (c) 2012 Jack Moore - jacklmoore.com
// license: www.opensource.org/licenses/mit-license.php
(function (a) { var b = "hidden", c = "border-box", d = "lineHeight", e = '<textarea tabindex="-1" style="position:absolute; top:-9999px; left:-9999px; right:auto; bottom:auto; -moz-box-sizing:content-box; -webkit-box-sizing:content-box; box-sizing:content-box; word-wrap:break-word; height:0 !important; min-height:0 !important; overflow:hidden;"/>', f = ["fontFamily", "fontSize", "fontWeight", "fontStyle", "letterSpacing", "textTransform", "wordSpacing", "textIndent"], g = "oninput", h = "onpropertychange", i = a(e)[0]; i.setAttribute(g, "return"); if (a.isFunction(i[g]) || h in i) { a(i).css(d, "99px"); if (a(i).css(d) === "99px") { f.push(d) } a.fn.autosize = function (d) { return this.each(function () { function s() { var a, c; if (!n) { n = true; k.value = i.value; k.style.overflowY = i.style.overflowY; k.style.width = j.css("width"); k.scrollTop = 0; k.scrollTop = 9e4; a = k.scrollTop; c = b; if (a > m) { a = m; c = "scroll" } else if (a < l) { a = l } i.style.overflowY = c; i.style.height = a + q + "px"; setTimeout(function () { n = false }, 1) } } var i = this, j = a(i), k, l = j.height(), m = parseInt(j.css("maxHeight"), 10), n, o = f.length, p, q = 0, r = i.value; if (j.css("box-sizing") === c || j.css("-moz-box-sizing") === c || j.css("-webkit-box-sizing") === c) { q = j.outerHeight() - j.height() } if (j.data("mirror") || j.data("ismirror")) { return } else { k = a(e).data("ismirror", true).addClass(d || "autosizejs")[0]; p = j.css("resize") === "none" ? "none" : "horizontal"; j.data("mirror", a(k)).css({ overflow: b, overflowY: b, wordWrap: "break-word", resize: p }) } m = m && m > 0 ? m : 9e4; while (o--) { k.style[f[o]] = j.css(f[o]) } a("body").append(k); if (h in i) { if (g in i) { i[g] = i.onkeyup = s } else { i[h] = s } } else { i[g] = s } a(window).resize(s); j.bind("autosize", s); i.value = ""; i.value = r; s() }) } } else { a.fn.autosize = function () { return this } } })(jQuery)