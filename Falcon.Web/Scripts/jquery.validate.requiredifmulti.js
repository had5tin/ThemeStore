$.validator.addMethod('requiredifmulti',
function (value, element, parameters) {
    // get the target value (as a string, 
    // as that's what actual value will be)
    var targetvalue = parameters['targetvalue'];
    targetvalue = (targetvalue == null ? '' : targetvalue).toString();

    var targetvaluearray = targetvalue.split('|');

    for (var i = 0; i < targetvaluearray.length; i++) {

        // get the actual value of the target control
        // note - this probably needs to cater for more 
        // control types, e.g. radios
        var control = $("[name='" + parameters['dependentproperty'] + "']");
        var controltype = control.attr('type');
        var actualvalue;
        var valueTarget = targetvaluearray[i];

        if (controltype === 'radio') {
            control = control.filter(":checked");
            actualvalue = control.val();
        }
        else if (controltype === 'checkbox') {
            actualvalue = control.is(":checked");
            valueTarget = targetvaluearray[i] == "true";
        }
        else {
            actualvalue = control.val();
        }

        // if the condition is true, reuse the existing 
        // required field validator functionality
        if (valueTarget == actualvalue) {
            return $.validator.methods.required.call(this, value, element, parameters);
        }
    }

    return true;
}
);
$.validator.unobtrusive.adapters.add(
    'requiredifmulti',
    ['dependentproperty', 'targetvalue'],
    function (options) {
        options.rules['requiredifmulti'] = {
            dependentproperty: options.params['dependentproperty'],
            targetvalue: options.params['targetvalue']
        };
        options.messages['requiredifmulti'] = options.message;
    });