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
});