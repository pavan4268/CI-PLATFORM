
var input = document.querySelector('#myInput');/*to find input*/
var cards = document.querySelectorAll('.card');/*return all cards*/
var cardTitle = document.querySelectorAll('#missiontitle');/*to fetch mision titles*/
input.addEventListener('keyup', search);

function search() {
    // Declare variables
    
    
    /*input = document.querySelector('#myInput');*/
    /*console.log(input);*/
    
    filter = input.value.toUpperCase();/*value inside input*/
   
    

    // Loop through all list items, and hide those who don't match the search query
    for (var i = 0; i < cards.length; i++) {
        if (cardTitle[i].innerHTML.toUpperCase().includes(filter)) {/*match card title with input value*/
            cards[i].classList.remove('d-none');/*show card*/
        }
        else {
            cards[i].classList.add('d-none');/*hide card*/
        }
        //var a = cards[i].getElementsById("missiontitle")[0];
        //txtValue = a.textContent || a.innerText;
        //if (txtValue.toUpperCase().indexOf(filter) > -1) {
        //    cards[i].style.display = "";
        //} else {
        //    cards[i].style.display = "none";
        //}
    }
}




