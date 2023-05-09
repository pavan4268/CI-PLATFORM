
$(document).ready(function () {
    

    //Title();
    //$('#mission-select').on('change', function () {
    //    Title();
    //})

    $('#timetable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columns": [
            null,
            null,
            null,
            null,
            null
        ],
        "columnDefs": [
            { "orderable": true, "targets": [1, 2] },
            { "orderable": false, "targets": [0, 3, 4] }
        ],
        "order": [],
        "scrollY": "300px",
        "scrollCollapse": false,
    });


    $('#goaltable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "columns": [
            null,
            null,
            null,
            null
        ],
        "columnDefs": [
            { "orderable": true, "targets": [1, 2] },
            { "orderable": false, "targets": [0, 3] }
        ],
        "order": [],
        "scrollY": "300px",
        "scrollCollapse": false,
    });


});


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