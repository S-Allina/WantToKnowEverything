﻿var search = document.querySelector("#search");
search.addEventListener('input', function () { // Изменено на 'input'
    document.querySelectorAll(".soughtFor").forEach(u => {
        if (u.textContent.toLowerCase().includes(search.value.toLowerCase()) && search.value != "") { // Приведение к нижнему регистру

            u.closest(".found").style.border = "4px solid white";
            console.log(u.closest(".found"));
            console.log("yes")
        } else {
            u.closest(".found").style.border = "none"; // Сброс границы
        }
    })
})