$(document).ready(function () {
    ImgUpload();
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

                //if (!f.type.match('image.*')) {
                //    return;
                //}

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


//for time and goal div hide and display 

const missionTypeSelect = document.querySelector('#mission-type');

const timediv = document.querySelector('.time-div');
const goaldiv = document.querySelector('.goal-div');


missionTypeSelect.addEventListener('change', function () {
    if (this.value == "Time") {
        timediv.style.display = 'block';
        goaldiv.style.display = 'none';
    }
    if (this.value == "Goal") {
        timediv.style.display = 'none';
        goaldiv.style.display = 'block';
    }
});
//for time and goal div hida and display

$("#img-delete").click(function () {
    var src = $(this).data("source");
    var ext = "img";
    var parent = $(this).parent();
    var missionId = $(this).data("missionid");

    $.ajax({
        type: 'post',
        url: "/Admin/ImageDelete",
        data: {
            id: missionId,
            source: src,
            extension: ext,
        },
        success: function (response) {
            console.log(response);
            if (response) {
                parent.addClass('d-none');
            }
        },
        error: function (xhr, error) {
            console.log(error);
        }
    })
});


//$("#doc-delete").click(function () {
//    var src = $(this).data("source");
//    var ext = "doc";
//    var parent = $(this).parent();
//    var missionId = $(this).data("missionid");

//    $.ajax({
//        type: 'post',
//        url: "/Admin/DocumentDelete",
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

function deleteDoc(docsrc, missionid) {
    //var src = $(this).data("source");
    var ext = "doc";
    //var parent = $(this).parent();
    //var missionId = $(this).data("missionid");

    $.ajax({
        type: 'post',
        url: "/Admin/DocumentDelete",
        data: {
            id: missionid,
            source: docsrc,
            extension: ext,
        },
        success: function (response) {
            console.log(response);
            if (response) {
                parent.addClass('d-none');
            }
        },
        error: function (xhr, error) {
            console.log(error);
        }
    })
}