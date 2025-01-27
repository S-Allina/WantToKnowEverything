


const tasksListElement = document.querySelector(`.tasks__list`);
const taskElements = tasksListElement.querySelectorAll(`.tasks__item`);

for (const task of taskElements) {
    task.draggable = true;
}

tasksListElement.addEventListener(`dragstart`, (evt) => {
    evt.target.classList.add(`selected`);
});

tasksListElement.addEventListener(`dragend`, (evt) => {
    evt.target.classList.remove(`selected`);
});

const getNextElement = (cursorPosition, currentElement) => {
    const currentElementCoord = currentElement.getBoundingClientRect();
    const currentElementCenter = currentElementCoord.y + currentElementCoord.height / 2;

    const nextElement = (cursorPosition < currentElementCenter) ?
        currentElement :
        currentElement.nextElementSibling;

    return nextElement;
};

tasksListElement.addEventListener(`dragover`, (evt) => {
    evt.preventDefault();

    const activeElement = tasksListElement.querySelector(`.selected`);
    const currentElement = evt.target;
    const isMoveable = activeElement !== currentElement &&
        currentElement.classList.contains(`tasks__item`);

    if (!isMoveable) {
        return;
    }

    const nextElement = getNextElement(evt.clientY, currentElement);

    if (
        nextElement &&
        activeElement === nextElement.previousElementSibling ||
        activeElement === nextElement
    ) {
        return;
    }

    tasksListElement.insertBefore(activeElement, nextElement);
});
function Proverka() {
    const task = document.querySelectorAll('.tasks__item');
    const curr1 = document.getElementById('current1').textContent.split('^');
    const g = document.getElementById('Got');
    g.classList.add('none');
    const d = document.getElementById('Dall');
    d.classList.remove('none');
    console.log(task[0].textContent);
    for (let i = 0; i < task.length; i++) {
        if (task[i].textContent === curr1[i]) {
            task[i].classList.add('True');
            console.log(curr1[i]);
        }
        else {
            task[i].classList.add('False');
            console.log(curr1[i]);

        }
    }

}
