function setLocation(url) {
    window.location.href = url;
}

function deleteConfirm(confirmMsg, postUrl) {
    if (confirm(confirmMsg)) {
        $.post(postUrl, function (data) {
            //reload after delete            
            window.location.href = window.location.href;
        });
    }
    return false;
}

function closeFancyPopup() {
    $.fancybox.close(true);
    return false;
}

jQuery.fn.sortItems = (function () {
    var sort = [].sort;
    return function (comparator, getSortable) {
        getSortable = getSortable || function () { return this; };
        var placements = this.map(function () {
            var sortElement = getSortable.call(this),
                parentNode = sortElement.parentNode,

                nextSibling = parentNode.insertBefore(
                    document.createTextNode(''),
                    sortElement.nextSibling
                );

            return function () {

                if (parentNode === this) {
                    throw new Error(
                        "You can't sort elements if any one is a descendant of another."
                    );
                }
                parentNode.insertBefore(this, nextSibling);
                parentNode.removeChild(nextSibling);

            };

        });
        return sort.call(this, comparator).each(function (i) {
            placements[i].call(getSortable.call(this));
        });
    };
})();

