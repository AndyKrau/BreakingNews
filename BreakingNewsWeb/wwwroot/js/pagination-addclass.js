window.onload = function () {
    // прочеряем наличие переменной в хранилище
    if ('currentPage') {

        // получаем её значение в переменную
        let page = localStorage.getItem('currentPage')

        // формирую класс текущей страницы
        let findClass = ".pages-" + page;

        $(findClass).addClass('active');

        //localStorage.removeItem(page);
        //console.log("Holla!" + page +" was deleted from LocalStorage");
    }
    else {
        let findClass = ".pages-0";

        $(findClass).addClass('active');
    }
}

$(".page-item").click(function myfunc() {
    $(this).addClass("active");

    let strClass = $(this).attr('class');
    console.log(strClass);

    let pattern = /\d/g;
    let x = strClass.match(pattern);
    let y = x[0];

    page = strClass.slice(16, 17);
    console.log(page);

    localStorage.setItem('currentPage', page);

    return y;
});