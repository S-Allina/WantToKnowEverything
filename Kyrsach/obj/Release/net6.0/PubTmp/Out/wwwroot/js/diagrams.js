function GenerateDiagramm1(data) {
    console.log(data)
    console.log(Math.max.apply(null,data.averageScore));
    console.log(data.nameTest)
    console.log(data.averageScore)
    document.querySelector("#titleDiagramm1").textContent = "Оценки по тестам из категории";
    //console.log(Math.max.apply(null, data.averageScore));
    var salesData = {
        labels: data.nameTest,
        datasets: [
            {
                label: "Front",
                fillColor: "rgba(195, 40, 96, 0.1)",
                strokeColor: "rgba(195, 40, 96, 1)",
                pointColor: "rgba(195, 40, 96, 1)",
                pointStrokeColor: "#202b33",
                pointHighlightStroke: "rgba(225,225,225,0.9)",
                data: data.averageScore
            },
            //{
            //    label: "Middle",
            //    fillColor: "rgba(255, 172, 100, 0.1)",
            //    strokeColor: "rgba(255, 172, 100, 1)",
            //    pointColor: "rgba(255, 172, 100, 1)",
            //    pointStrokeColor: "#202b33",
            //    pointHighlightStroke: "rgba(225,225,225,0.9)",
            //    data: [0, 0, 0]
            //},
            //{
            //    label: "Back",
            //    fillColor: "rgba(19, 71, 34, 0.3)",
            //    strokeColor: "rgba(88, 188, 116, 1)",
            //    pointColor: "rgba(88, 188, 116, 1)",
            //    pointStrokeColor: "#202b33",
            //    pointHighlightStroke: "rgba(225,225,225,0.9)",
            //    data: [0, 0, 0]
            //}
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
        scaleSteps: Math.max.apply(null,data.averageScore) + 1,
        scaleStepWidth: 1,
        scaleStartValue: 0,

        responsive: true

    });
}
function GenerateDiagramm2(data) {
    document.querySelector("#titleDiagramm2").textContent = "Средние оценки по всем категориям";
    var data = {
        labels: data.nameCategory,
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: data.averageScore
            }
        ]
    };

    var ctx = document.getElementById("myChart").getContext("2d");
    window.myBarChart = new Chart(ctx).Bar(data, {
        //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
        scaleBeginAtZero: true,

        //Boolean - Whether grid lines are shown across the chart
        scaleShowGridLines: true,

        //String - Colour of the grid lines
        scaleGridLineColor: "rgba(0, 0, 0, .05)",

        //Number - Width of the grid lines
        scaleGridLineWidth: 1,

        //Boolean - Whether to show horizontal lines (except X axis)
        scaleShowHorizontalLines: true,

        //Boolean - Whether to show vertical lines (except Y axis)
        scaleShowVerticalLines: true,

        //Boolean - If there is a stroke on each bar
        barShowStroke: true,

        //Number - Pixel width of the bar stroke
        barStrokeWidth: 2,

        //Number - Spacing between each of the X value sets
        barValueSpacing: 5,

        //Number - Spacing between data sets within X values
        barDatasetSpacing: 1,

        //String - A legend template
        legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

    });
}

function GenerateDiagramm3(data) {
    console.log("dig3")
    console.log(data)
    var chart = am4core.create("chartdiv", am4charts.PieChart);

    // Add data
    chart.data = [{
        "test": "От 0 до 3",
        "value": data[0]
    }, {
        "test": "От 3 до 10",
        "value": data[1]
    }, {
        "test": "От 7 до 10",
        "value": data[2]
    }];

    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "value";
    pieSeries.dataFields.category = "test";
    pieSeries.labels.template.disabled = true;
    pieSeries.ticks.template.disabled = true;

    chart.legend = new am4charts.Legend();
    chart.legend.position = "right";

    chart.innerRadius = am4core.percent(60);
    
    var label = pieSeries.createChild(am4core.Label);
    label.text = "{values.value.sum}";
    label.horizontalCenter = "middle";
    label.verticalCenter = "middle";
    label.fontSize = 40;
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
        idCategory: parseInt(selectedTest.value),
        startDate: new Date(startDate.value),
        endDate: new Date(endDate.value)
    };
    console.log(data)
    // Send a POST request to the controller and retrieve the JSON data
    fetch('/Diagrams/GetResultTest', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            // Process the returned JSON data as needed
            console.log(data);
            GenerateDiagramm1(data)
            selectedUser.toggleAttribute("disabled");
            selectedTest.toggleAttribute("disabled");
            startDate.toggleAttribute("disabled");
            endDate.toggleAttribute("disabled");
        })
        .catch(error => {
            console.error('Error:', error);
        });
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/Diagrams/GetResultCategory?idUser=" + data.idUser, true);
    xhr.onreadystatechange = function () {
        console.log(xhr.status)
        if (xhr.readyState === 4 && xhr.status === 200) {
            var options = JSON.parse(xhr.responseText);
            console.log(xhr.responseText)
            GenerateDiagramm2(options);
        }
    };
    xhr.send();



    fetch('/Diagrams/GetPerformanceTest', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            // Process the returned JSON data as needed
            console.log(data);
            GenerateDiagramm3(data);
            //GenerateDiagramm1(data)
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
        xhr.open("GET", "/Diagrams/GetTests?idUser=" + type, true);
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
    function fillOptionsSelect(options) {
        console.log(options)
        options.forEach(function (option) {
            var optionElement = document.createElement("option");
            optionElement.text = option.nameCategory;
            optionElement.value = option.idCategory;
            optionsSelect.add(optionElement);
        });
    }
};