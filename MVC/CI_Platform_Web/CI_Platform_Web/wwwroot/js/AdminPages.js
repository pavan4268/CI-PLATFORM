jQuery(document).ready(function () {





});





// Get all list items
var listItems = $('ul.nav-list li.nav-item');

// Add click event listener to each list item
listItems.on('click', function () {
    // Remove nav-active class from all list items
    listItems.removeClass('nav-active');

    // Add nav-active class to clicked list item
    $(this).addClass('nav-active');

    // Remove text-active class from all spans
    $('span.item-name').removeClass('text-active');

    // Add text-active class to span inside clicked list item
    $('span.item-name', this).addClass('text-active');

    // Change icon of all list items to item-icon
    $('i.bi').attr('id', 'item-icon');

    // Change icon of clicked list item to item-icon-active
	$('i.bi', this).attr('id', 'item-icon-active');

	//to click the hidden button
	//const hiddenButton = $(this).find('.obj-link');
	//hiddenButton.trigger('click');
	/*$('#obj-link', this).click();*/
	var link = $('a#obj-link', this);
	if (link.length > 0) {
		link.get(0).click();
	}
});

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