function copyInputValue(id, idCh) {

    const text2Input = document.getElementById('CorrectAnswer');
    const checkbox = document.getElementById(idCh);
    const textInput = document.getElementById(id);
    if (checkbox.checked) {


        text2Input.value += '|';
        if (textInput.value === undefined) {
            text2Input.value += textInput.textContent;
        }
        else { text2Input.value += textInput.value; }
        text2Input.textContent += textInput.textContent;
    } else {
        if (textInput.value === undefined) {
            const text = '|' + textInput.textContent;
            const text2 = text2Input.value;
            text2Input.value = text2.replace(text, '');
        } else {
            const text = '|' + textInput.value;
            const text2 = text2Input.value;
            text2Input.value = text2.replace(text, '');
        }


        //var input = document.getElementById("text");
        //var str = input.value;
        //var newStr = str.replace(t, "");
        //input.value = newStr;
    }

}

document.querySelector('#plus').addEventListener('click', () => {
    const plus = document.querySelector('#plus');
    const min = document.querySelector('#minus');
    const option = document.querySelectorAll('.aa.none');
    if (option.length === 1) plus.classList.add('none');

    option[0].classList.remove('none');
    if (min.classList.contains('none')) min.classList.remove('none');
});

document.querySelector('#minus').addEventListener('click', () => {
    const min = document.querySelector('#minus');
    const plus = document.querySelector('#plus');
    if (plus.classList.contains('none')) plus.classList.remove('none');
    const option = document.querySelectorAll('.aa');
    const None = document.querySelectorAll('.aa.none');
    for (let i = option.length - 1; i > 1; i--) {
        if (option[i].classList.contains('none') === false) {
            console.log("-------");
            option[i].classList.add('none');
            option[i].children[1].children[0].value = "";
            option[i].children[0].children[0].checked = false;
            break;
        }
    }
    console.log(None.length)
    if (None.length === 2) {
        min.classList.add('none');

    }





})


const no = document.querySelector('#create-Nooptions');

const yes = document.querySelector('#create-options');
const rodio1 = document.querySelector('#options');
const rodio2 = document.querySelector('#Nooptions');
rodio1.addEventListener('change', () => {
    if (rodio1.checked) {
        no.classList.add('none');
        yes.classList.remove('none');
        /*   yes.classList.add('flex');*/
    }
});
rodio2.addEventListener('change', () => {
    if (rodio2.checked) {
        yes.classList.add('none');
        no.classList.remove('none');
    }
});