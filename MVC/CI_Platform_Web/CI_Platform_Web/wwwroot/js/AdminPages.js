jQuery(document).ready(function () {

    var oTable = $('#Admin-table').DataTable({
        "info": false,
        "bLengthChange": false,
        "ordering": false,
        /*dom: '<"top"l<"custom-item">f>t<"bottom"ip>',*/
        "search": {
            search: $('#my-search').val()
        },
        
        "pagingType": 'full_numbers',
        "pageLength": 8,
        language: {
            oPaginate: {
                sNext: '<i class="bi bi-chevron-right"></i>',
                sPrevious: '<i class="bi bi-chevron-left"></i>',
                sFirst: '<i class="bi bi-chevron-double-left"></i>',
                sLast: '<i class="bi bi-chevron-double-right"></i>'
            },
            //'search': '',
            //searchPlaceholder: "Search"
        }
    });
    $('#my-search').keyup(function () {
        oTable.search($(this).val()).draw();
    })




    var listItems = $('ul.nav-list li.nav-item');
    var activeIndex = localStorage.getItem('activeIndex');
    if (activeIndex !== null) {
        listItems.removeClass('nav-active');
        listItems.eq(activeIndex).addClass('nav-active');
        $('span.item-name').removeClass('text-active');
        $('span.item-name', listItems.eq(activeIndex)).addClass('text-active');
        $('i.inav').attr('id', 'item-icon');
        $('i.inav', listItems.eq(activeIndex)).attr('id', 'item-icon-active');
    }
    listItems.on('click', function () {
        var index = $(this).index();
        localStorage.setItem('activeIndex', index);
        listItems.removeClass('nav-active');
        $(this).addClass('nav-active');
        $('span.item-name').removeClass('text-active');
        $('span.item-name', this).addClass('text-active');
        $('i.inav').attr('id', 'item-icon');
        $('i.inav', this).attr('id', 'item-icon-active');
        var link = $('a#obj-link', this);
        if (link.length > 0) {
            link.get(0).click();
        }
    });




    

});





//// Get all list items
//var listItems = $('ul.nav-list li.nav-item');

//// Add click event listener to each list item
//listItems.on('click', function () {
//    // Remove nav-active class from all list items
//    listItems.removeClass('nav-active');

//    // Add nav-active class to clicked list item
//    $(this).addClass('nav-active');

//    // Remove text-active class from all spans
//    $('span.item-name').removeClass('text-active');

//    // Add text-active class to span inside clicked list item
//    $('span.item-name', this).addClass('text-active');

//    // Change icon of all list items to item-icon
//    $('i.bi').attr('id', 'item-icon');

//    // Change icon of clicked list item to item-icon-active
//	$('i.bi', this).attr('id', 'item-icon-active');

//	//to click the hidden button
//	//const hiddenButton = $(this).find('.obj-link');
//	//hiddenButton.trigger('click');
//	/*$('#obj-link', this).click();*/
//	var link = $('a#obj-link', this);
//	if (link.length > 0) {
//		link.get(0).click();
//	}
//});

function displayDateTime() {
	var currentDate = new Date();
	var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
	var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
	var day = days[currentDate.getDay()];
	var month = months[currentDate.getMonth()];
	var date = currentDate.getDate();
	var year = currentDate.getFullYear();
	var hours = currentDate.getHours();
	var minutes = currentDate.getMinutes();
	var ampm = hours >= 12 ? 'PM' : 'AM';
	hours = hours % 12;
	hours = hours ? hours : 12; // the hour '0' should be '12'
	minutes = minutes < 10 ? '0' + minutes : minutes;
	var time = hours + ':' + minutes + ' ' + ampm;
	var dateTimeString = day + ', ' + month + ' ' + date + ', ' + year + ', ' + time;
	document.getElementById("current-date").innerHTML = dateTimeString;
}



//<-------------------------------------------------------------------------User Js-------------------------------------------------------------------------->

//profile picture change on frontend part

$(document).ready(function () {
    $("#country-dropdown").change();
    var readURL = function (input) {
        if (input.files && input.files[0]) {

            // check if the selected file is an image
            if (!input.files[0].type.startsWith('image/')) {
                alert('Please select an image file.');
                //console.log(input.files[0]);
                //input.files[0].val('');
                $("#profileimg").val(null);
                console.log(input.files[0]);
                return;
            }


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

//get cities in dropdown

function getcities(cityid) {
    var countryid = $("#country-dropdown").find(":selected").val();
    $('#city-dropdown').empty();
    //$('#city-dropdown').append('<Option>Enter Your City</Option>');
    console.log(countryid);
    $.ajax({
        url: '/Admin/GetCities',
        data: { countryid: countryid },
        success: function (result) {
            $.each(result, function (i, data) {
                var selected = (data.cityId == cityid) ? 'selected' : '';
                $('#city-dropdown').append('<Option value ="' + data.cityId + '" ' + selected + '>' + data.name + '</Option>');
                //$('#city-dropdown').append('<Option value =' + data.cityId +    '>' + data.name + '</Option>');
            });

        }
    })
}

//get cities in dropdown