function GenerateDiagramm(data) {
    console.log(data)
    console.log(data.time)
    console.log(data.current)
    console.log(Math.max.apply(null, data.current));
    var salesData = {
        labels: data.time,
        datasets: [
            {
                label: "Front",
                fillColor: "rgba(195, 40, 96, 0.1)",
                strokeColor: "rgba(195, 40, 96, 1)",
                pointColor: "rgba(195, 40, 96, 1)",
                pointStrokeColor: "#202b33",
                pointHighlightStroke: "rgba(225,225,225,0.9)",
                data: data.current
            },
            {
                label: "Middle",
                fillColor: "rgba(255, 172, 100, 0.1)",
                strokeColor: "rgba(255, 172, 100, 1)",
                pointColor: "rgba(255, 172, 100, 1)",
                pointStrokeColor: "#202b33",
                pointHighlightStroke: "rgba(225,225,225,0.9)",
                data: [0, 0, 0]
            },
            {
                label: "Back",
                fillColor: "rgba(19, 71, 34, 0.3)",
                strokeColor: "rgba(88, 188, 116, 1)",
                pointColor: "rgba(88, 188, 116, 1)",
                pointStrokeColor: "#202b33",
                pointHighlightStroke: "rgba(225,225,225,0.9)",
                data: [0, 0, 0]
            }
        ]
    };
    var ctx = document.getElementById("salesData").getContext("2d");
    window.myLineChart = new Chart(ctx).Line(salesData, {
        pointDotRadius: 6,
        pointDotStrokeWidth: 2,
        datasetStrokeWidth: 3,
        scaleShowVerticalLines: false,
        scaleGridLineWidth: 2,
        scaleShowGridLines: true,
        scaleGridLineColor: "rgba(225, 255, 255, 0.02)",
        scaleOverride: true,
        scaleSteps: Math.max.apply(null, data.current) + 1,
        scaleStepWidth: 1,
        scaleStartValue: 0,

        responsive: true

    });
}
document.querySelector('form.contener button').addEventListener('click', function (e) {
    e.preventDefault();

    // Get selected values from the form
    var selectedUser = document.querySelector('#users');
    var selectedTest = document.querySelector('#tests');
    var startDate = document.querySelector('#startDate');
    var endDate = document.querySelector('#endDate');



    // Create a data object to send to the controller
    var data = {
        idUser: selectedUser.value,
        idTest: parseInt(selectedTest.value),
        startDate: new Date(startDate.value),
        endDate: new Date(endDate.value)
    };
    // Send a POST request to the controller and retrieve the JSON data
    fetch('/AnswersUsers/GetResult', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            // Process the returned JSON data as needed
            console.log(data);
            GenerateDiagramm(data)
            selectedUser.toggleAttribute("disabled");
            selectedTest.toggleAttribute("disabled");
            startDate.toggleAttribute("disabled");
            endDate.toggleAttribute("disabled");
        })
        .catch(error => {
            console.error('Error:', error);
        });
});


window.onload = function () {
    var usersSelect = document.getElementById("users");
    var optionsSelect = document.getElementById("tests");
    usersSelect.addEventListener("change", function () {
        var selectedUser = usersSelect.value;
        optionsSelect.innerHTML = "";

        loadOptionsFromDatabase(selectedUser)
    });

    function loadOptionsFromDatabase(type) {
        console.log(type)
        var xhr = new XMLHttpRequest();
        xhr.open("GET", "/AnswersUsers/GetTests?type=" + type, true);
        xhr.onreadystatechange = function () {
            console.log(xhr.status)
            if (xhr.readyState === 4 && xhr.status === 200) {
                var options = JSON.parse(xhr.responseText);
                console.log(xhr.responseText)
                fillOptionsSelect(options);
            }
        };
        xhr.send();
    }
    xhr.send();
    function fillOptionsSelect(options) {
        console.log(options)
        options.forEach(function (option) {
            var optionElement = document.createElement("option");
            optionElement.text = option.nameTest;
            optionElement.value = option.idTest;
            optionsSelect.add(optionElement);
        });
    }
};