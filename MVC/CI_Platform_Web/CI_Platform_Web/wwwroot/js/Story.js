$(document).ready(function () {
    ImgUpload();
    
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




function getSavedData() {
    var missionid = $("#mission-select").find(":selected").val();
    console.log(missionid);
    $.ajax({

        url: "/Story/DraftedData",
        data: { missionid: missionid },
        success: function (response) {
            console.log(response);
            $("#sharestorypv").html(response);




        }
    })
}


//$(".img-delete").click(function () {
//    var src = $(this).data("source");
//    var ext = "img";
//    var parent = $(this).parent();
//    var missionId = $(this).data("missionid");

//    $.ajax({

//        url: "/Admin/ImageDelete",
//        data: {
//            id: missionId,
//            source: src,
//            extension: ext,
//        },
//        success: function (response) {
//            console.log(response);
//            if (response) {
//                parent.addClass('d-none');
//            }
//        },
//        error: function (xhr, error) {
//            console.log(error);
//        }
//    })
//});


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
