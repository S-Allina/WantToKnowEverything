function Text() {
    const inptutCreate = document.getElementById('input-Cat');
    const spanCreate = document.getElementById('NameCate');
    spanCreate.textContent = inptutCreate.value;

}

const label = document.querySelector('.input__file-button-text');
const img = document.querySelector('#imgCate');
const inputCat = document.querySelector('.input__file');
inputCat.addEventListener('change', () => {

    label.textContent = 'Файл выбран';
    img.src = URL.createObjectURL(inputCat.files[0]);
});
