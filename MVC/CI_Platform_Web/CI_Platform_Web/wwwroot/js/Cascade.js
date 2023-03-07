$(document).ready(function () {
    
    GetCountry();
    GetMissionThemes();
    GetMissionSkillList();
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

function GetMissionThemes() {
    $.ajax({
        url: '/Mission/Theme',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#MissionThemeList').append('<Option value=' + data.missionThemeId + '>' + data.title + '</Option>');
            });
        }
    });
}


function GetMissionSkillList() {
    $.ajax({
        url: '/Mission/Skills',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#MissionSkillList').append('<Option value=' + data.skillId + '>' + data.skillName + '</Option>');
            });
        }
    });
}