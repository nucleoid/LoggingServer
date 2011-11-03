$(document).ready(function () {
    //Partner js for HtmlHelperExtensions.CheckBoxesForFlagsEnum 
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

    //Grid row highlighter
    $("tr:not(.no-url)").click(function () {
        window.location = $(this).attr("url");
    });
    $("tr:not(.no-url)").not(':first').hover(
          function () {
              $(this).css("background", "yellow");
              $(this).css("cursor", "pointer");
          },
          function () {
              $(this).css("background", "");
          }
        );
});