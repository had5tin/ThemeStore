$(document).ready(function () {
    loadBootstrapSelect();
});
var loadBootstrapSelect = function () {
    $('.selectpicker').selectpicker({
        size: 5,
        selectedTextFormat: 'count>3'
    });
};