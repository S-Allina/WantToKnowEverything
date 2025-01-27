document.querySelectorAll(".delete").forEach((value) => {
    value.addEventListener('click', (event) => {
        event.preventDefault();
        // Подтверждение удаления
        const confirmation = confirm("Вы точно хотите удалить данную запись?");
        if (confirmation) {
            window.location.href = value.href;
        } else {
            // Скрываем загрузчик

            // Отмена перехода
            event.stopPropagation();
            hideLoader();
        }
    })
})

//var loader = document.getElementById("loader");

//function showLoader() {
//    loader.style.display = "block";
//}

//function hideLoader() {
//    loader.style.display = "none";
//}

//function setupLoader() {
//    document.addEventListener("ajaxStart", showLoader, false);
//    document.addEventListener("ajaxStop", hideLoader, false);
//}

//setupLoader();

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
var navbarElements = document.querySelectorAll('.head__link a');


// Навешиваем событие на каждый элемент с классом "navbar"
for (var i = 0; i < navbarElements.length; i++) {
    navbarElements[i].addEventListener('click', showLoader);
}

//document.addEventListener("ajaxStart", showLoader, false)
// Перехватываем окончание AJAX-запроса и скрываем индикатор загрузки
document.addEventListener("ajaxStop", hideLoader, false);