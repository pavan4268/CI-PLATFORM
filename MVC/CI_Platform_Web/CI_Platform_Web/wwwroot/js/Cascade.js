


jQuery(document).ready(function () {
    
    GetCountry();
    GetMissionThemes();
    GetMissionSkillList();
    $('#CountryList').change(function () {
        var id = $(this).val();
        $('#citylist').empty();
        /*$('#citylist').append('<Option>City</Option>');*/
        $.ajax({
            url: '/Mission/City?id=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    /*$('#CityList').append('<Option value =' + data.cityId + '>' + data.name + '</Option>');*/
                     $('#citylist').append(`<li class = "dropbtn"><input class="form-check-input me-2" value="${data.name}" type = "checkbox" id = "${data.cityId}city-checkbox" name = "cities"/>${data.name}</li>`);
                });
                $("#filterform input").change((e) => { FilterMission(e); })
            }
        });
    });
    $("#filterform").submit((e) => { FilterMission(e); })
    $("#filterform input").change((e) => { FilterMission(e); })
    $("#filterform select").change((e) => { FilterMission(e); })
   


    //$('#searchButton'.on("click", function () {
    //    var inputvalue = $("#InputField").val();
    //    $.ajax({
    //        url: controller / IDBTransaction
    //        data: { controller_parameter: inputvalue }
    //        success: function (result) {

    //        }
    //    })
    //})

    //})
   
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
                /*$('#MissionThemeList').append('<Option value=' + data.missionThemeId + '>' + data.title + '</Option>');*/
                $("#themelist").append(`<li class = "dropbtn"><input class="form-check-input me-2" value="${data.title}" type = "checkbox" name = "themes" id = "${data.missionThemeId}theme-checkbox"/> ${data.title} </li>`);
            });
            $("#filterform input").change((e) => { FilterMission(e); })
        }
    });
}




function GetMissionSkillList() {
    $.ajax({
        url: '/Mission/Skills',
        success: function (result) {
            $.each(result, function (i, data) {
                /*$('#MissionSkillList').append('<Option value=' + data.skillId + '>' + data.skillName + '</Option>');*/
                $("#skilllist").append(`<li class = "dropbtn"><input class="form-check-input me-2" value="${data.skillId}" type = "checkbox" name = "skills" id = "${data.skillId}skill-checkbox"> ${data.skillName} </li>`);

            });
            $("#filterform input").change((e) => { FilterMission(e); })
        }
    });
}


function FilterMission(e) {

    e.preventDefault();
    var filterFormData = $("#filterform").serialize();

    $.ajax({
        method: 'Post',
        url: '/Mission/FilterMissions',
        data: filterFormData,


        /* data: {countryId:countryId, SearchText:searchtext, sort : sort},*/
        success: function (response) {



            console.log(response);
            $("#maindata").html(response);

            //FilterCityTag();
            //var count = document.querySelector("#modelcount").value;
            //var staticText = document.querySelector(".statictext"); // get the statictext element
            //staticText.textContent = count + " missions";
            //grid = document.querySelector("#gridView");
            //list = document.querySelector("#listView");
            //btngrid = document.querySelector("#btngrid");
            //btnlist = document.querySelector("#btnlist");

            //if ($('#btngrid').hasClass('gridhover')) {
            //    btngrid.click();
            //}
            //else {
            //    btnlist.click();
            //}



            console.log(response);
        },
        error: function (xhr, status, error) {
            console.log("ajax error" + xhr.responseText);
        }
    });




}


//$(document).ready(function () {

//    $('input[type=checkbox][id=countrylist]').change(function () {
//        var selectCountry = $('input[type=checkbox][id=countrylist]:checked').map(function () {
//            return $(this).val();
//        }).get();

//        if (selectCountry.length > 0) {
//            $('.card').hide();
//            $.each(selectCountry, function (index, value) {
//                $('.card:contains("' + value + '")').show();
//            });
//        } else {
//            $('.card').show();
//        }
//    });
//});
$("#apply-btn").on('click', function () {
    var missionid = $(this).attr("data-missionid");
    console.log(missionid);
    $.ajax({
        type: 'post',
        url: '/Mission/ApplyToMission',
        data: { "missionid": missionid },
        success: function (result) {
            location.reload();
        }
    })
})


//recommend to coworker
function recommend(sid) {
    var selecteduser = [];
    $('#recommendtocoworker input:checkbox[id=rectoid]:checked').each(function () {
        selecteduser.push($(this).val());
    })
    console.log(selecteduser);
    if (selecteduser != null) {
        $.ajax({
            method: 'POST',
            url: '/Mission/RecommendToCoWorker',
            data: {
                "missionid": sid,
                "selecteduser": selecteduser

            },
            success: function (response) {
                console.log('mail sent');
            }
        })
    }
}
//recommend to coworker


//ratings

$(".ratingStar").hover(function () {
    $(".ratingStar").addClass("bi-star").removeClass("bi-star-fill");

    $(this).addClass("bi-star-fill").removeClass("bi-star");

    $(this).prevAll(".ratingStar").addClass("bi-star-fill").removeClass("bi-star");

});


$("#ratings_star_div .ratingStar").on('click', function () {
    var starValue = $(this).index() + 1;
    console.log(starValue);
    var missionId = $(this).data("mission-id");

    

    $.ajax({
        type: "Post",
        url: "/Mission/AddRating",
        data: { "missionid": missionId, "ratingvalue": starValue},
        success: function (response) {
            console.log(response);
            location.reload();
        },
        error: function (response) {
            console.log(response);
        }

    });
});

//ratings




// sorting js goes here
document.getElementById("selectsort").addEventListener("change", function () {

    sortFunction(document.getElementById("selectsort").value);

});
function sortFunction(sortvalue) {
    let url = window.location.href;
    let seprator = url.indexOf('?') !== -1 ? '&' : '?';
    if (url.includes("sortby=")) {
        url = url.replace(/sortby=([^&]*)/, 'sortby=' + sortvalue);
    }
    else {
        url += seprator + 'sortby=' + sortvalue;
    }
    window.location.href = url;
}
// sorting js ends here


//search script goes here

//var debounceTimer;

//function debounce(func , delay) {
//    clearTimeout(debounceTimer);
//    debounceTimer = setTimeout(func , delay);
//}

//document.getElementById("myInput").addEventListener("input", function () {

//})

//search script ends here


//add to fav ajax call
/*document.getElementById("fav-div").addEventListener("click", AddToFavourite());*/
function AddToFavourite(userid, missionid) {
    $.ajax
        ({
            method: 'Post',
            url: '/Mission/AddToFavourite',
            data: {
                UserId : userid,
                id : missionid
            },
            success: function (Response) {
                if (Response) {
                    $(".add-fav i").addClass("bi-heart-fill");
                    $(".add-fav i").removeClass("bi-heart");
                }
                else {
                    $(".add-fav i").addClass("bi-heart");
                    $(".add-fav i").removeClass("bi-heart-fill");
                }
            },
            error: function (xhr, status, error) {
                console.log(error);
            }
        });
}


//post comment ajax call
function PostComment(userid, missionid) {
    var comment = document.getElementById("Comment-to-post").value;

    $.ajax
        ({
            method: 'Post',
            url: '/Mission/DisplayComments',
            data: {
                UserId: userid,
                id: missionid,
                CommentText : comment
                
            },
            success: function (result) {
                console.log(result);
                
            }

        });
}

function Share() {
    console.log("share btn clicked");
    $('#Modal').modal('show');
}

function CloseModal() {
    $('#modalshare').modal('hide');
    console.log("clicked");
}


