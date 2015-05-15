Simple Browser Plugin
A port of the FCKEditor file browser as a TinyMCE plugin.
http://tetlaw.id.au/view/blog/fckeditor-file-browser-plugin-for-tinymce-editor/

FCKEditor connectors should all work.

Configuration:

tinyMCE.init({
	theme : "advanced",
	plugins : "simplebrowser",
	file_browser_callback : "TinyMCE_simplebrowser_browse", // This is required
	plugin_simplebrowser_browselinkurl : 'tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.html?Connector=PATH/TO/YOUR/CONNECTOR',
	plugin_simplebrowser_browseimageurl : 'tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.html?Type=Image&Connector=PATH/TO/YOUR/CONNECTOR',
	plugin_simplebrowser_browseflashurl : 'tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.html?Type=Flash&Connector=PATH/TO/YOUR/CONNECTOR',
});
