// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//var arr = [];
//var i = 0;
//$(":checkbox").change(function () {
//    if (this.checked) {
//        arr[i] = $(this).val();
//        i++;
//    } else {
//        var val = $(this).val();
//        var index = arr.indexOf(val);
//        arr.splice(index, 1);
//        i--;
//    }
//    console.log(arr);
//});
//const answer = document.getElementById('CorrectAnswer');
//const chek = document.querySelectorAll('.checkbox');
//for (let i = 0; i < chek.length;i++) {
//    c[i].addEventListener('checked', () => {
//        answer.textContent += '|' + document.getElementById(`${i}`).textContent;
//    });
//};
//function Check(id) {
//    const element = document.getElementById(id);
//    const text = document.getElementById('CorrectAnswer');
//    text.textContent += element.textContent;
//}


//inptutCreate.addEventListener('change', () => {
//    spanCreate.textContent = inptutCreate.textContent;
//    console.log('iiiii');
//})




document.querySelector('#plus').addEventListener('click', () => {
    const plus = document.querySelector('#plus');
    const min = document.querySelector('#minus');
    const option = document.querySelectorAll('.aa.none');
    if (option.length === 1) plus.classList.add('none');
  
    option[0].classList.remove('none');
    if (min.classList.contains('none'))  min.classList.remove('none');
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
            option[i].children[0].children[0].checked=false;
            break;
        }
    }
    console.log(None.length)
    if (None.length === 2) {
        min.classList.add('none');
        console.log("opopop");
     
    } 
     
      
    
    

})


function Text() {
    const inptutCreate = document.getElementById('input-Cat');
    const spanCreate = document.getElementById('NameCate');
    spanCreate.textContent = inptutCreate.value;
   
}

const label = document.querySelector('.input__file-button-text');
const img = document.querySelector('#imgCate');
const inputCat = document.querySelector('.input__file');
inputCat.addEventListener('change', () => {
    console.log("ooooooooo")
    label.textContent = 'Файл выбран';
    img.src = URL.createObjectURL(inputCat.files[0]);
});



//const no = document.querySelector('#create-Nooptions');
//const yes = document.querySelector('#create-options');
//console.log('ghfhgfh')
//const rodio21 = document.querySelector('#Nooptions').addEventListener('click', () => {

//    if (this.checked) {
//        no.classList.add('none');
//        console.log('jujjjjjj');
//        yes.classList.remove('none');
//        /*   yes.classList.add('flex');*/
    
//    }
//    //else {
//    //    console.log('ju31');
//    //    no.classList.remove('none');
//    //    /*   yes.classList.remove('flex');*/
//    //    yes.classList.add('none');
//    //}
//})


//const rodio2 = document.querySelector('#options').addEventListener('click', () => {
//    if (this.checked) {
//        console.log('ju3');
//        no.classList.remove('none');
//        /*   yes.classList.remove('flex');*/
//        yes.classList.add('none');

//    }
//    //else {
//    //    no.classList.add('none');
//    //    console.log('jujjjjjj2');
//    //    yes.classList.remove('none');
//    //    /*   yes.classList.add('flex');*/
//    //}
//})


const no = document.querySelector('#create-Nooptions');
console.log('jujjjjjj');

const yes = document.querySelector('#create-options');
const rodio1 = document.querySelector('#options');
const rodio2 = document.querySelector('#Nooptions');
rodio1.addEventListener('change', () => {
    if (rodio1.checked) {
        no.classList.add('none');
        console.log('jujjjjjj');
        yes.classList.remove('none');
        /*   yes.classList.add('flex');*/
    }
});
rodio2.addEventListener('change', () => {
    if (rodio2.checked) {
        yes.classList.add('none');
        console.log('jujjjjjj');
        no.classList.remove('none');
    }
});



// const pitch = document.querySelector("#pitch");
// const pitchValue = document.querySelector(".pitch-value");
// const rate = document.querySelector("#rate");
// const rateValue = document.querySelector(".rate-value");



