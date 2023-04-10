// for dag and drop



jQuery(document).ready(function () {
    ImgUpload();
    $("#country-dropdown").change();

//    Title();
//    $('#mission-select').on('change', function () {
//        Title();
//    })
    $('#editor').summernote({
        height: 200, // set the height of the editor
        toolbar: [
            // add formatting options to the toolbar
            ['style', ['bold', 'italic', 'strikethrough', 'subscript', 'superscript', 'underline']]
        ]
    });

});

function ImgUpload() {
    var imgWrap = "";
    var imgArray = [];

    $('.upload__inputfile').each(function () {
        $(this).on('change', function (e) {
            imgWrap = $(this).closest('.upload__box').find('.upload__img-wrap');
            var maxLength = $(this).attr('data-max_length');

            var files = e.target.files;
            var filesArr = Array.prototype.slice.call(files);
            var iterator = 0;
            filesArr.forEach(function (f, index) {

                if (!f.type.match('image.*')) {
                    return;
                }

                if (imgArray.length > maxLength) {
                    return false
                } else {
                    var len = 0;
                    for (var i = 0; i < imgArray.length; i++) {
                        if (imgArray[i] !== undefined) {
                            len++;
                        }
                    }
                    if (len > maxLength) {
                        return false;
                    } else {
                        imgArray.push(f);

                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var html = "<div class='upload__img-box'><div style='background-image: url(" + e.target.result + ")' data-number='" + $(".upload__img-close").length + "' data-file='" + f.name + "' class='img-bg'><div class='upload__img-close'></div></div></div>";
                            imgWrap.append(html);
                            iterator++;
                        }
                        reader.readAsDataURL(f);
                    }
                }
            });
        });
    });

    $('body').on('click', ".upload__img-close", function (e) {
        var file = $(this).parent().data("file");
        for (var i = 0; i < imgArray.length; i++) {
            if (imgArray[i].name === file) {
                imgArray.splice(i, 1);
                break;
            }
        }
        $(this).parent().parent().remove();
    });
}

//function Title() {
//    var selected = $("#mission-select").find(":selected").val();
//    console.log(selected);
//    $.ajax({
//        type: 'Post',
//        url: '/Story/Submit',
//        data: { "missionId": selected },
//        success: function (response) {
//            console.log(response);$(3)

//        }
//    })
//}
//for drag and drop

//function Save() {
//    var missionid = $("#mission-select").find(":selected").val();
//    console.log(missionid);
//    var storytitle = $("#storydesc").val();
//    console.log(storytitle);
//    var date = $("#date-select").val();
//    console.log(date);
//    var text = $(".ck-blurred p").text();
//    console.log(text);
//    var url = $("#video-url").val();
//    console.log(url);
//    var img = $("#storyimg").val();
//    console.log(img);
//    $.ajax({
//        type: 'Post',
//        url: '/Story/Save',
//        data: {
//            "missionid": missionid,
//            "StoryTitle": storytitle,
//            "Date": date,
//            "StoryDescription": text
//        },
//        success: function result() {
//            console.log(result);
//        }
//    })

    //$.ajax({
    //    url: '/Story/SavedData',
    //    data: { "missionid": missionid },
    //    success: function (response) {
    //        console.log(response);
    //        $("#storydesc").val(response.title);
            //const date = response.createdAt;
            //console.log(date);
            //const newdate = new Date(date);
            //const year = newdate.getFullYear();
            //const month = ("0" + (newdate.getMonth() + 1)).slice(-2);
            //const day = ("0" + newdate.getDate()).slice(-2);
            //console.log(year);
            //console.log(month);
            //console.log(newdate);
            //const dateinput = document.getElementById("date-select");
            //const formattedDate = `${year}-${month}-${day}`;
            //dateinput.value = formattedDate;
            
            
    //        console.log(formattedDate);

            
            
            
    //    }
    //})

//}

function getSavedData() {
    var missionid = $("#mission-select").find(":selected").val();
    console.log(missionid);
    $.ajax({
        url: "/Story/DraftedData",
        data: { "missionid": missionid },
        success: function (response) {
            console.log(response);
            if (response != null) {
                document.getElementById("submit-btn").disabled = false;
                $("#storydesc").val(response.storyTitle);
                const date = response.date;
                console.log(date);
                const newdate = new Date(date);
                const year = newdate.getFullYear();
                const month = ("0" + (newdate.getMonth() + 1)).slice(-2);
                const day = ("0" + newdate.getDate()).slice(-2);
                console.log(year);
                console.log(month);
                console.log(newdate);
                const dateinput = document.getElementById("date-select");
                const formattedDate = `${year}-${month}-${day}`;
                dateinput.value = formattedDate;
                
                tinymce.get('editor').setContent(response.storyDesctiption);
                var videolist = response.videoList;
                console.log(videolist);
                const textarea = document.getElementById('videourl');
                
                for (var i = 0; i < videolist.length; i++) {
                    
                    textarea.value += videolist[i].videoPath + '\n';
                }
            }
            else {
                var inputs = document.querySelectorAll("input");
                
                for (var i = 0; i < inputs.length; i++) {
                    inputs[i].value = null;
                    document.getElementById("submit-btn").disabled = true;
                }
                document.getElementById("videourl").value = '';
                var today = new Date();
                var day = today.getDate();
                var month = today.getMonth() + 1; // Add 1 to month because January is 0
                var year = today.getFullYear();

                // Format the date as YYYY-MM-DD
                var formattedDate = year + '-' + month.toString().padStart(2, '0') + '-' + day.toString().padStart(2, '0');

                // Set the formatted date as the value of the input field
                document.getElementById("date-select").value = formattedDate;
                document.getElementById("editor").value = null;
                
            }

            
            
            
        }
    })
}


function recommend(sid) {
    var selecteduser = [];
    $('#recommendtocoworker input:checkbox[id=rectoid]:checked').each(function () {
        selecteduser.push($(this).val());
    })
    console.log(selecteduser);
    if (selecteduser != null) {
        $.ajax({
            method: 'POST',
            url: '/Story/StoryDetails',
            data: {
                "storyid": sid,
                "selecteduser": selecteduser

            },
            success: function (response) {
                console.log('mail sent');
            }
        })
    }
}


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


function getcities() {
    var countryid = $("#country-dropdown").find(":selected").val();
    $('#city-dropdown').empty();
    //$('#city-dropdown').append('<Option>Enter Your City</Option>');
    console.log(countryid);
    $.ajax({
        url: '/User/GetCities',
        data: { countryid: countryid },
        success: function (result) {
            $.each(result, function (i, data) {

                $('#city-dropdown').append('<Option value =' + data.cityId + '>' + data.name + '</Option>');
            });
            
        }
    })
}
//$("#city-dropdown").val('');
//password modal js

//password validations

const passwordInput = document.getElementById("newpass");
const passwordError = document.getElementById("password-validation");


passwordInput.addEventListener('input', () =>
{
    const password = passwordInput.value;

    //minimum length
    if (password.length < 6) {
        passwordError.textContent = 'Password must be atleast 6 characters long';
        return;
    }
    //minimum length


    //character validations
    const uppercaseRegex = /[A-Z]/;
    const lowercaseRegex = /[a-z]/;
    const numberRegex = /[0-9]/;
    const specialRegex = /[$@$!%*?&]/;

    if (!uppercaseRegex.test(password) || !lowercaseRegex.test(password) || !numberRegex.test(password) || !specialRegex.test(password)) {
        passwordError.textContent = 'Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character';
        return;
    }
    //character validations
    passwordError.textContent = ''; // Clear error message if validation passes

});

//password validations


function changepass() {
    document.getElementById("passerror").style.display = "none";
    var cpass = document.getElementById("cpass").value;
    var newpass = document.getElementById("newpass").value;
    var cnfpass = document.getElementById("cnfpass").value;
    if (newpass != cnfpass) {
        alert("New Password and Confirm Password do not Match.");
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
                    document.getElementById("passerror").style.display = "block";
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
submitButton.addEventListener('click', (event) => {
    var linkedInUrlInput = document.getElementById("linkedin");
    var linkedInUrl = linkedInUrlInput.value.trim();
    if (!validateLinkedinUrl(linkedInUrl)) {
        event.preventDefault();
        alert('Please Enter a valid LinkedIn Url.');
        linkedInUrlInput.focus();
    }
});

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
            if (response == true) {
                $("#contact-us-close").click();
                alert('Message Sent Sucessfully');
            }
            else {
                alert('Please Login once Again');
            }
        }
    });
}

//contact us details post