(function ($) {
    $.fn.falconTabs = function (options) {
        $this = $(this);
        var tabs = $('#' + $this.attr('id') + ' li a.tab-item-link');
        var activeTab = null;
        var displayFirst = null;

        for (var i = 0; i < tabs.length; i++) {
            if (tabs[i].id == options.activeTabId) {
                displayFirst = tabs[i];
                break;
            }
        }

        $.fn.falconTabs.hideAllTabsContent(tabs);

        for (var i = 0; i < tabs.length; i++) {
            $(tabs[i]).bind('click', function () {
                $.fn.falconTabs.showTabContent(tabs, this, activeTab);
                return false;
            });
        }

        if (displayFirst != null) {
            $.fn.falconTabs.showTabContent(tabs, displayFirst, activeTab);
        }
        
        return this;
    }

    $.fn.falconTabs.hideAllTabsContent = function (tabs) {
        for (var i = 0; i < tabs.length; i++) {
            $.fn.falconTabs.hideTabContent(tabs[i]);
        }
    }

    $.fn.falconTabs.hideTabContent = function (tab) {
        var tabContentElement = $.fn.falconTabs.getTabContentElementId(tab);
        if (tabContentElement) {
            $("#" + tabContentElement).hide();
            $("#" + tab.id).removeClass('active');
        }
    }

    $.fn.falconTabs.getTabContentElementId = function (tab) {
        if (tab) {
            return tab.id + '_content';
        }
        return false;
    }

    $.fn.falconTabs.showTabContent = function (tabs, tab, activeTab) {
        $.fn.falconTabs.hideAllTabsContent(tabs);
        var tabContentElement = $.fn.falconTabs.getTabContentElementId(tab);
        if (tabContentElement) {
            $("#" + tabContentElement).show();
            $("#" + tab.id).addClass('active');
        }
    }

})(jQuery);