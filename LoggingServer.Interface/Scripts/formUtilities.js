//Partner js for HtmlHelperExtensions.CheckBoxesForFlagsEnum 
$(document).ready(function () {
    $('form').one('submit', function (e) {
        e.preventDefault();
        //Grab all flag enum checkbox names
        var names = jQuery.map($('[flaggedenum="true"]'), function (checkbox) {
            return $(checkbox).attr("name");
        });
        //Get a distinct list of checkbox group names and set the associated hidden field to the checked values
        $(jQuery.unique(names)).each(function () {
            var multiChecked = jQuery.map($('[name="' + this.toString() + '"]:checked'), function (multiCheck) {
                return multiCheck.value;
            }).join(',');
            var hiddenName = this.toString().replace("Not.", "");
            $('[name="' + hiddenName + '"]').val(multiChecked);
        });
        //Continue submitting form
        $(this).submit();
    });
});