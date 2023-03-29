// for dag and drop



jQuery(document).ready(function () {
    ImgUpload();

//    Title();
//    $('#mission-select').on('change', function () {
//        Title();
//    })
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
                $("#storydesc").val(response.title);
                const date = response.createdAt;
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
                $("#editor").html(response.description);
                console.log(response.description);
            }
            
            
        }
    })
}