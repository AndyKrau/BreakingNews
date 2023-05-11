function myFunction() {

    const elem = document.getElementById('btnOpenModal');

    let _id = elem.getAttribute('data-id');
    let _name = elem.getAttribute('data-name');
    let _password = elem.getAttribute('data-password');
    let _email = elem.getAttribute('data-email');
    let _phone = elem.getAttribute('data-phone');
    let _role = elem.getAttribute('data-role');
    let _country = elem.getAttribute('data-country');
    let _postalCode = elem.getAttribute('data-postalCode');

    console.log("password " + _password);
    console.log("email " + _email);
    console.log("phone " + _phone);
    console.log("role " + _role);
    console.log("country " + _country);
    console.log("postalCode " + _postalCode);

    document.getElementById("id").innerText = _id;
    document.getElementById("name").innerText = _name;
    document.getElementById("password").innerText = _password;
    document.getElementById("email").innerText = _email;
    document.getElementById("phone").innerText = _phone;
    document.getElementById("role").innerText = _role;
    document.getElementById("country").innerText = _country;
    document.getElementById("postalCode").innerText = _postalCode;

}


//----------------------------Pagination-check/uncheck-------------------------------------//
//document.addEventListener("click", function (isit) {

//    pageNumber = isit.target.className;
//    if (pageNumber == "pagination__btn") {
//        alert("pagination__btn!");
//        isit.target.classList.add("active");
//    }

//});

//document.addEventListener("click", function(isit) {
//    the_class = isit. target.className;
//    the_id = isit.target.id;

//    if (the_class == "pagination__btn") {
//        alert("pagination__btn!");
//    }

//    });

//function uncheck(){
// var uncheck = document.getElementsByClassName('custom-checkbox');
////  console.log(uncheck);
//    for(var i=0; i<7; i++){
//        if(uncheck[i].type=='radio') {
//        uncheck[i].checked=false;
//        }
//    }
//};


    //const myFirstEvent = document.querySelector('a');

    // обращаемс€ к свойству напр€мую
    //myFirstEvent.onclick = function() {
        //    alert('ƒа будет так!');
        //}

        // используем метод addEventListener
        //myFirstEvent.addEventListener('click', function () {
        //    alert('ƒорогу осилит идущий');
        //})


