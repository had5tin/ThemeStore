/*
  *	Andrew Tetlaw - 2006/02
 *	A port of the FCKEditor file browser as a TinyMCE plugin.
 *	http://tetlaw.id.au/view/blog/fckeditor-file-browser-plugin-for-tinymce-editor/
 */


/* Import plugin specific language pack */
//tinyMCE.importPluginLanguagePack('simplebrowser', 'en');

/**
 * Information about the plugin.
 */
var SimpleBrowser = {};
var SimpleBrowserParams = {};

var _browseimageurl = '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?Type=Image&connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx';
var _browseflashurl = '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?Type=Flash&connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx';
var _browselinkurl = '/Scripts/tiny_mce_v2/plugins/simplebrowser/browser.htm?connector=/Scripts/tiny_mce_v2/plugins/simplebrowser/connectors/aspx/connector.aspx';

function TinyMCE_simplebrowser_getInfo() {
	return {
		longname : 'Simple Browser plugin',
		author : 'Andrew Tetlaw',
		authorurl : 'http://tetlaw.id.au',
		infourl : 'http://tetlaw.id.au/view/blog/fckeditor-file-browser-plugin-for-tinymce-editor/',
		version : "1.0"
	};
};

/**
 * Gets executed when a editor instance is initialized
 */
function TinyMCE_simplebrowser_initInstance(inst) {
	SimpleBrowser = {
		browselinkurl: tinyMCE.getParam("plugin_simplebrowser_browselinkurl", false),
		browseimageurl: tinyMCE.getParam("plugin_simplebrowser_browseimageurl", false),
		browseflashurl: tinyMCE.getParam("plugin_simplebrowser_browseflashurl", false)
    }
}

function TinyMCE_simplebrowser_browse(field_name, current_url, type, win) {
	switch(type.toLowerCase()) {
		case 'image':
			if(_browseimageurl) {
			    TinyMCE_simplebrowser_openServerBrowser(field_name, current_url, type, win, _browseimageurl);
			    
			} else {
				alert("Image browser URL not set [].");
            }
			break;
		case 'flash':
			if(_browseflashurl) {
				TinyMCE_simplebrowser_openServerBrowser(field_name, current_url, type, win, _browseflashurl);
			} else {
			    alert("Flash browser URL not set [].");
			}
			break;
		default:
			if(_browselinkurl) {
				TinyMCE_simplebrowser_openServerBrowser(field_name, current_url, type, win, _browselinkurl);
			} else {
			    alert("Link browser URL not set [].");
			}
	}
}

function TinyMCE_simplebrowser_openServerBrowser(field_name, current_url, link_type, win, browse_url)
{
	SimpleBrowserParams = {
		field : field_name,
		curl : current_url,
		type : link_type,
		target : win
	}

	var sOptions = "toolbar=no,status=no,resizable=yes,dependent=yes";
	sOptions += ",width=800";// + width;
	sOptions += ",height=600";// + height;

	if (tinyMCE.isMSIE)	{
		// The following change has been made otherwise IE will open the file 
		// browser on a different server session (on some cases):
		// http://support.microsoft.com/default.aspx?scid=kb;en-us;831678
		// by Simone Chiaretta.
		var oWindow = oEditor.window.open(browse_url, "TinyMCESimpleBrowserWindow", sOptions ) ;
		oWindow.opener = window ;
    } else {
		window.open(browse_url, "TinyMCESimpleBrowserWindow", sOptions );
	}
}

function TinyMCE_simplebrowser_browserCallback(returnValue) {
	if(!returnValue) return;
	SimpleBrowserParams['target'].document.forms[0].elements[SimpleBrowserParams['field']].value = returnValue;
}
