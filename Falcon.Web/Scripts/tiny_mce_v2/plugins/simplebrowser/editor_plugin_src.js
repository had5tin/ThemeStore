/*
 *	Andrew Tetlaw - 2006/02 - for TinyMCE 2.0.3 and above
 *	A port of the FCKEditor file browser as a TinyMCE plugin.
 *	http://tetlaw.id.au/view/blog/fckeditor-file-browser-plugin-for-tinymce-editor/
 */

//tinyMCE.importPluginLanguagePack('simplebrowser', 'en');

var _browseimageurl = '/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.htm?Type=Image&connector=/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/connectors/aspx/connector.aspx';
var _browseflashurl = '/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.htm?Type=Flash&connector=/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/connectors/aspx/connector.aspx';
var _browselinkurl = '/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/browser.htm?connector=/Extensions/tinymce/jscripts/tiny_mce/plugins/simplebrowser/connectors/aspx/connector.aspx';

var TinyMCE_SimpleBrowserPlugin = {
	options : {},
	getInfo : function() {
		return {
			longname : 'Simple Browser plugin',
			author : 'Andrew Tetlaw',
			authorurl : 'http://tetlaw.id.au',
			infourl : 'http://tetlaw.id.au/view/blog/fckeditor-file-browser-plugin-for-tinymce-editor/',
			version : "2.1"
		};
	},

	initInstance : function(inst) {
		// You can take out plugin specific parameters
		//alert("Initialization parameter:" + tinyMCE.getParam("template_someparam", false));
		tinyMCE.settings['file_browser_callback'] = "TinyMCE_SimpleBrowserPlugin_browse";
		TinyMCE_SimpleBrowserPlugin.options = {
			width : tinyMCE.getParam("plugin_simplebrowser_width", '800'),
			height : tinyMCE.getParam("plugin_simplebrowser_height", '600'),
			browseimageurl : tinyMCE.getParam("plugin_simplebrowser_browseimageurl", false),
			browselinkurl : tinyMCE.getParam("plugin_simplebrowser_browselinkurl", false),
			browseflashurl : tinyMCE.getParam("plugin_simplebrowser_browseflashurl", false)
		}
	},

	browse : function(field_name, current_url, type, win) {
		switch(type.toLowerCase()) {
			case 'image':
				if(_browseimageurl) {
					TinyMCE_SimpleBrowserPlugin.openServerBrowser(field_name, current_url, type, win, _browseimageurl);
				} else {
					alert("Image browser URL not set [src].");
				}
				break;
			case 'flash':
				if(_browseflashurl) {
					TinyMCE_SimpleBrowserPlugin.openServerBrowser(field_name, current_url, type, win, _browseflashurl);
				} else {
				    alert("Flash browser URL not set [src].");
				}
				break;
			default:
				if(_browselinkurl) {
					TinyMCE_SimpleBrowserPlugin.openServerBrowser(field_name, current_url, type, win, _browselinkurl);
				} else {
				    alert("Link browser URL not set [src].s");
				}
		}
	},

	openServerBrowser : function(field_name, current_url, link_type, win, browse_url)
	{
			TinyMCE_SimpleBrowserPlugin.options['field'] = field_name;
			TinyMCE_SimpleBrowserPlugin.options['curl'] = current_url;
			TinyMCE_SimpleBrowserPlugin.options['type'] = link_type;
			TinyMCE_SimpleBrowserPlugin.options['target'] = win;

		var sOptions = "toolbar=no,status=no,resizable=yes,dependent=yes";
		sOptions += ",width=" + TinyMCE_SimpleBrowserPlugin.options['width'];
		sOptions += ",height=" + TinyMCE_SimpleBrowserPlugin.options['height'];

		if (tinyMCE.isMSIE)	{
			// The following change has been made otherwise IE will open the file 
			// browser on a different server session (on some cases):
			// http://support.microsoft.com/default.aspx?scid=kb;en-us;831678
			// by Simone Chiaretta.
			var oWindow = window.open(browse_url, "TinyMCESimpleBrowserWindow", sOptions ) ;
			oWindow.opener = window;
		} else {
			window.open(browse_url, "TinyMCESimpleBrowserWindow", sOptions );
		}
	},

	browserCallback : function(returnValue) {
		if(!returnValue) return;
		TinyMCE_SimpleBrowserPlugin.options['target'].document.forms[0].elements[TinyMCE_SimpleBrowserPlugin.options['field']].value = returnValue;
	}
};

function TinyMCE_SimpleBrowserPlugin_browse(field_name, current_url, type, win) {
	TinyMCE_SimpleBrowserPlugin.browse(field_name, current_url, type, win)
};

tinyMCE.addPlugin("simplebrowser", TinyMCE_SimpleBrowserPlugin);