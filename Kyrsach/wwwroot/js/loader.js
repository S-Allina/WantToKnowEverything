
// Получаем ссылку на элемент индикатора загрузки
var loader = document.getElementById("loader");

// Функция для показа индикатора загрузки
function showLoader() {
    loader.style.display = "block";
    document.body.style.overflowY = "hidden";
}

// Функция для скрытия индикатора загрузки
function hideLoader() {
    loader.style.display = "none";
    //document.body.style.overflowY = "scroll";
}

// Перехватываем клик по ссылке и показываем индикатор загрузки перед переходом
// Получаем все элементы с классом "navbar"
var navbarElements = document.querySelectorAll('a');


// Навешиваем событие на каждый элемент с классом "navbar"
for (var i = 0; i < navbarElements.length; i++) {
    navbarElements[i].addEventListener('click', showLoader);
}

//document.addEventListener("ajaxStart", showLoader, false)
// Перехватываем окончание AJAX-запроса и скрываем индикатор загрузки
document.addEventListener("ajaxStop", hideLoader, false);