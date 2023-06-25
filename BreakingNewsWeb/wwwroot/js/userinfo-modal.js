$(".btn-open-modal").click(function () {
    var id = $(this).data('id');
    var name = $(this).data('name');
    var role = $(this).data('role');
    var email = $(this).data('email');
    var phone = $(this).data('phone');
    var password = $(this).data('password');
    var country = $(this).data('country');
    var postalcode = $(this).data('postalcode');


    $("#id").html(id);
    $("#name").html(name);
    $("#role").html(role);
    $("#email").html(email);
    $("#phone").html(phone);
    $("#password").html(password);
    $("#country").html(country);
    $("#postalcode").html(postalcode);
});

