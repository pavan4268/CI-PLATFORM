$(document).ready(function () {
    alert('ok');
    GetCountry();
    $('#CountryList').change(function () {
        var id = $(this).val();
        $('#CityList').empty();
        $('#CityList').append('<Option>City</Option>');
        $.ajax({
            url: '/Mission/City?id=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#CityList').append('<Option value =' + data.cityId + '>' + data.name + '</Option>');
                });
            }
        });
    });

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