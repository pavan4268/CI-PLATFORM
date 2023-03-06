$(document).ready(function () {
    alert('ok');
    GetCountry();
    //$('#CountryList').change(function () {
    //    var id = $(this).val();
    //    $('#City')
    //});

});


function GetCountry(){
    $.ajax({
        url: '/Mission/Country',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#CountryList').append('<Option value=' + data.countryId + '>' + data.name + '</Option>');
            });
        }
    });
}