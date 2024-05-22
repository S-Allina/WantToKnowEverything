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
