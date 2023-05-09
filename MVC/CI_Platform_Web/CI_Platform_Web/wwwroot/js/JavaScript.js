// for dag and drop





$(document).ready(function () {
    ImgUpload();
    $("#country-dropdown").change();

    //Title();
    //$('#mission-select').on('change', function () {
    //    Title();
    //})

    //$('#timetable').DataTable({
    //    "paging": false,
    //    "info": false,
    //    "searching": false,
    //    "columns": [
    //        null,
    //        null,
    //        null,
    //        null,
    //        null
    //    ],
    //    "columnDefs": [
    //        { "orderable": true, "targets": [1, 2] },
    //        { "orderable": false, "targets": [0, 3, 4] }
    //    ],
    //    "order": [],
    //    "scrollY": "300px",
    //    "scrollCollapse": false,
    //});


    //$('#goaltable').DataTable({
    //    "paging": false,
    //    "info": false,
    //    "searching": false,
    //    "columns": [
    //        null,
    //        null,
    //        null,
    //        null
    //    ],
    //    "columnDefs": [
    //        { "orderable": true, "targets": [1, 2] },
    //        { "orderable": false, "targets": [0, 3] }
    //    ],
    //    "order": [],
    //    "scrollY": "300px",
    //    "scrollCollapse": false,
    //});
  

});











//profile picture change on frontend part

$(document).ready(function () {
    var readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#user-profile').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $(".file-upload").on('change', function () {
        readURL(this);
    });
    $(".upload-button").on('click', function () {
        $(".file-upload").click();
    })
});


//profile picture change on frontend part


function getcities(cityid) {
    
    var countryid = $("#country-dropdown").find(":selected").val();
    $('#city-dropdown').empty();
    //$('#city-dropdown').append('<Option>Enter Your City</Option>');
    console.log(countryid);
    $.ajax({
        url: '/User/GetCities',
        data: { countryid: countryid },
        success: function (result) {
            console.log(result)
            $.each(result, function (i, data) {
                var selected = (data.cityId == cityid) ? 'selected' : '';
                $('#city-dropdown').append('<Option value ="' + data.cityId +'" ' + selected + '>' + data.name + '</Option>');
            });
            
        }
    })
}
//$("#city-dropdown").val('');
//password modal js

//password validations

const passwordInput = document.getElementById("newpass");
const passwordError = document.getElementById("password-validation");
const passerror = document.getElementById("passerror");

//passwordInput.addEventListener('input', () =>
//{
//    const password = passwordInput.value;

//    //minimum length
//    if (password.length < 6) {
//        passwordError.textContent = 'Password must be atleast 6 characters long';
//        return;
//    }
//    //minimum length


//    //character validations
//    const uppercaseRegex = /[A-Z]/;
//    const lowercaseRegex = /[a-z]/;
//    const numberRegex = /[0-9]/;
//    const specialRegex = /[$@$!%*?&]/;

//    if (!uppercaseRegex.test(password) || !lowercaseRegex.test(password) || !numberRegex.test(password) || !specialRegex.test(password)) {
//        passwordError.textContent = 'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character';
//        return;
//    }
//    //character validations
//    passwordError.textContent = ''; // Clear error message if validation passes

//});

//password validations


function changepass() {
    
    document.getElementById("passerror").textContent = "";
    document.getElementById("password-validation").textContent = "";
    document.getElementById("pass-match").textContent = "";
    var cpass = document.getElementById("cpass").value;
    var newpass = document.getElementById("newpass").value;
    var cnfpass = document.getElementById("cnfpass").value;
    const password = newpass;
    const uppercaseRegex = /[A-Z]/;
    const lowercaseRegex = /[a-z]/;
    const numberRegex = /[0-9]/;
    const specialRegex = /[$@$!%*?&]/;

    if (cpass == "") {
        passerror.textContent = "Please Enter the Current Password";
        return;
    }
    if (password.length < 6) {
        passwordError.textContent = 'Password must be atleast 6 characters long';
        return;
    }
    if (!uppercaseRegex.test(password) || !lowercaseRegex.test(password) || !numberRegex.test(password) || !specialRegex.test(password)) {
        passwordError.textContent = 'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character';
        return;
    }
    if (newpass != cnfpass) {
        $("#pass-match").textContent="New Password and Confirm Password do not Match.";
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/User/ChangePassword',
            data: {
                cpass: cpass,
                newpass : newpass
            },
            success: function (response) {
                if (response == true) {
                    $("#change-pass-cancel").click();
                    alert("Password updated sucessfully.");
                }
                else {
                    $("pass-error")
                    $("pass-error").textContent = "Current Password is Wrong";
                    //document.getElementById("passerror").style.display = "block";
                }
            }
        });
    }
}
//password modal js

function addskill() {
    var skilltoadd = document.getElementById("allskills");
    var addedskill = document.getElementById("userskills");
    for (var i = 0; i < skilltoadd.options.length; i++) {
        if (skilltoadd.options[i].selected) {
            var exists = false;
            for (var j = 0; j < addedskill.options.length; j++) {
                if (addedskill.options[j].value == skilltoadd.options[i].value) {
                    exists = true;
                    break;
                }
            }
            if (!exists) {
                $("#userskills").append('<option value ="' + skilltoadd.options[i].value + '">' + skilltoadd.options[i].text + '</option>');
            }
        }
    }
}


function removeskill() {
    var remove = document.getElementById("userskills");
    for (var i = remove.options.length - 1; i >= 0; i--) {
        if (remove.options[i].selected) {
            remove.options[i].remove();
        }
    }
}

function saveskills() {
    var skillslist = [];
    var selected = document.getElementById("userskills");
    for (var i = 0; i < selected.options.length;i++) {
        
            skillslist.push(selected.options[i].value);
        
        
    }
    console.log(skillslist);
    $.ajax({
        type: 'post',
        url: '/User/SaveUserSkills',
        data: { skillid: skillslist },
        success: function (result) {
            console.log(result);
            $("#closemodal").click();
            $("#uzer-skills").html(result);
        }
    })
}


function getuserskills() {
    $("#userskills").empty();
    $("#allskills option:selected").prop("selected", false);
    $.ajax({
        url: '/User/GetUserSkills',
        success: function (result) {
            console.log(result);
            $.each(result, function (i, data) {
                $("#userskills").append('<Option value =' + data.skillId + '>' + data.skillName + '</Option>');
            });
        }
    });
}


//function getcities() {
//    var countryid = $("#country-dropdown").find(":selected").val();
//    $('#city-dropdown').empty();
//    //$('#city-dropdown').append('<Option>Enter Your City</Option>');
//    console.log(countryid);
//    $.ajax({
//        url: '/User/GetCities',
//        data: { countryid: countryid },
//        success: function (result) {
//            $.each(result, function (i, data) {

//                $('#city-dropdown').append('<Option value =' + data.cityId + '>' + data.name + '</Option>');
//            });
//        }
//    })
//}

//linkedin url pattern validation function

const linkedinUrlRegex = /^(https?:\/\/)?([\w]+\.)?linkedin\.com\/.*$/i;

function validateLinkedinUrl(input) {
    return linkedinUrlRegex.test(input);
}

//linkedin url pattern validation function


//onsubmit validations

const submitButton = document.getElementById("form-submit");
$("#form-submit").on('click', (event) => {
    var linkedInUrlInput = document.getElementById("linkedin");
    var linkedInUrl = linkedInUrlInput.value.trim();
    if (!validateLinkedinUrl(linkedInUrl)) {
        event.preventDefault();
        alert('Please Enter a valid LinkedIn Url.');
        linkedInUrlInput.focus();
    }
})
//submitButton.addEventListener('click', (event) => {
//    var linkedInUrlInput = document.getElementById("linkedin");
//    var linkedInUrl = linkedInUrlInput.value.trim();
//    if (!validateLinkedinUrl(linkedInUrl)) {
//        event.preventDefault();
//        alert('Please Enter a valid LinkedIn Url.');
//        linkedInUrlInput.focus();
//    }
//});

//onsubmit validations


//contact us details post

function SaveMessage() {
    var subject = document.getElementById("contact-subject").value;
    var message = document.getElementById("contact-message").value;
    $.ajax({
        type: 'post',
        url: '/User/ContactUs',
        data: {
            Subject: subject,
            Message: message
        },
        success: function (response) {
            if (response == "") {
                $("#contact-us-close").click();
                alert('Message Sent Sucessfully');
            }
            else {
                document.getElementById("subjectvalidation").textContent = "";
                document.getElementById("messagevalidation").textContent = "";
                const suberror = document.getElementById("subjectvalidation");
                const messerror = document.getElementById("messagevalidation");
                $("#subjectvalidation").textContent = "";
                $("#messagevalidation").textContent = "";
                if (response == "Please Enter a Subject") {
                    suberror.textContent = response;
                }
                else if (response == "Please Enter a Message") {
                    messerror.textContent = response;
                }
                else {
                    $("#contact-us-close").click();
                    alert(response);
                }
            }
        }
    });
}

//contact us details post


//<------------------------------------------------TImesheet Section Js---------------------------------------------------------------------------------------------->

function gettimemissions(mtype) {
    if (mtype == 0) {
        $("#time-mission-dropdown").empty()
        $("#time-mission-dropdown").append('<Option value =' + null + '>Please Select A Mission</Option>');
    }
    else {
        $("#goal-mission-dropdown").empty()
        $("#goal-mission-dropdown").append('<Option value =' + null + '>Please Select A Mission</Option>');
    }
    
    $.ajax({
        type: 'get',
        url: '/User/GetUserTimeMissions',
        data: { Type: mtype },
        success: function (result) {
            console.log(result);
            if (mtype == 0) {
                $.each(result, function (i, data) {

                    $("#time-mission-dropdown").append('<Option value =' + data.missionId + '>' + data.missionName + '</Option>');
                });
            }
            else {
                $.each(result, function (i, data) {

                    $("#goal-mission-dropdown").append('<Option value =' + data.missionId + '>' + data.missionName + '</Option>');
                });
            }
        }
    });
}


function sendtimebasedsheet() {
    /*if(document.getElementById(""))*/
    //console.log($("#time-mission-dropdown").val());
    //var optionvalue = $("#time-mission-dropdown").val();
    //if (optionvalue == "null") {
    //    alert('please select a mission');
    //}
    
    var formdata = $("#add-time-data").serialize();
    console.log(formdata);
    $.ajax({
        type: 'post',
        url: '/User/AddTimeData',
        data: formdata,
        success: function (response) {
            if (response == "") {
                $("#add-time-error").html("");
                $("#time-add-close").click();
                alert("Data Added Sucessfully");
                location.reload();
            }
            else {
                document.getElementById("add-time-error").style.color = "red";
                document.getElementById("add-time-error").innerHTML = response;
            }
        }
    });
    
}


function sendgoalbasedsheet() {
    var formdata = $("#sendgoalsheet").serialize();
    $.ajax({
        type: 'post',
        url: '/User/AddGoalData',
        data: formdata,
        success: function (response) {
            if (response == "") {
                $("#add-goal-error").html("");
                $("#goal-add-close").click();
                alert("Data Added Successfully");
                location.reload();
            }
            else {
                document.getElementById("add-goal-error").style.color = "red";
                document.getElementById("add-goal-error").innerHTML = response;
            }
        }
    });
}


function timebasededit(timesheetid) {
    $.ajax({
        url: '/User/GetTimeEdit',
        data: { timesheetid: timesheetid },
        success: function (response) {
            $("#timeEditSelect").empty();
            $("#timeEditSelect").append('<Option value =' + response.missionId + '>' + response.missionName + '</Option>');
            
            const date = response.dateVolunteered;
            console.log(response);
            console.log(response.timesheetId);
            const newdate = new Date(date);
            const year = newdate.getFullYear();
            const month = ("0" + (newdate.getMonth() + 1)).slice(-2);
            const day = ("0" + newdate.getDate()).slice(-2);
            const dateinput = document.getElementById("timeEditDate");
            const formattedDate = `${year}-${month}-${day}`;
            dateinput.value = formattedDate;

            var time = response.time;
            var duration = moment.duration(time);
            var hours = duration.hours();
            var mins = duration.minutes();
            document.getElementById("timeEditHours").value = hours;
            document.getElementById("timeEditMins").value = mins;

            document.getElementById("timeEditMessage").value = response.notes;
            document.getElementById("time-edit-hidden").value = response.timesheetId;
            document.getElementById("time-delete-timesheetid").value = response.timesheetId;
        }
    })
}


function timebasededitsave() {
    var editformdata = $("#edit-time-data").serialize();
    $.ajax({
        type: 'post',
        url: '/User/EditTimeData',
        data: editformdata,

        success: function (response) {
            if (response == "") {
                $("#time-edit-error").html("");
                $("#time-edit-close").click();
                alert('Data Saved Successfully');
                location.reload();
            }
            else {
                document.getElementById("time-edit-error").style.color = "red";
                document.getElementById("time-edit-error").innerHTML = response;
            }
        }
    })
}

function goalbasededit(timesheetid) {
    $.ajax({
        url: '/User/GetGoalEdit',
        data: { timesheetid: timesheetid },
        success: function (response) {
            $("#goal-edit-select").empty();
            $("#goal-edit-select").append('<Option value =' + response.missionId + '>' + response.missionName + '</Option>');
            const date = response.dateVolunteered;
            console.log(response);
            
            const newdate = new Date(date);
            const year = newdate.getFullYear();
            const month = ("0" + (newdate.getMonth() + 1)).slice(-2);
            const day = ("0" + newdate.getDate()).slice(-2);
            const dateinput = document.getElementById("goal-edit-date");
            const formattedDate = `${year}-${month}-${day}`;
            dateinput.value = formattedDate;

            document.getElementById("goal-edit-action").value = response.action;
            document.getElementById("goal-edit-message").value = response.notes;
            document.getElementById("goal-edit-timesheetid").value = response.timesheetId;
            
        }
    })
}


function goalbasededitsave() {
    var editgoalform = $("#edit-goal-data").serialize();
    $.ajax({
        type: 'post',
        url: '/User/EditGoalData',
        data: editgoalform,
        success: function (response) {
            if (response == "") {
                $("#goal-edit-error").html("");
                $("#goal-edit-close").click();
                alert('Data Saved Successfully');
                location.reload();
            }
            else {
                document.getElementById("goal-edit-error").style.color = "red";
                document.getElementById("goal-edit-error").innerHTML = response;
            }
        }
    })
}


function timebaseddelete(id) {
    /*var timesheetid = document.getElementById("time-delete-timesheetid").value;*/
    $.ajax({
        type: 'post',
        url: '/User/TimeDelete',
        data: { timesheetid: id },
        success: function (response) {
            alert(response);
            
            location.reload();
        }
    })
}

function confirmdelete(timesheetid) {
    if (confirm("Are You Sure You Want To Delete The Timesheet?")) {
        timebaseddelete(timesheetid);
    }
}


function goalbaseddelete(id) {
    $.ajax({
        type: 'post',
        url: '/User/GoalDelete',
        data: { timesheetid: id },
        success: function (response) {
            alert(response);

            location.reload();
        }
    })
}



function confirmgoaldelete(timesheetid) {
    if (confirm("Are You Sure You Want To Delete The Timesheet?")) {
        goalbaseddelete(timesheetid);
    }
}