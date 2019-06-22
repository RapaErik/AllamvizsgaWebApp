var temperatureInputArray = [];
var humidityInputArray = [];
var heaterInputArray = [];
google.charts.load('current', { 'packages': ['corechart', 'line'] });
google.charts.load('current', { 'packages': ['gauge'] });

function sortingArrayByFirstParameterAsDateASC(array) {
    if (typeof array !== 'undefined' && array.length > 0) {
        array.sort(function (aa, bb) {
            return new Date(aa[0]) - new Date(bb[0]);
        });
        return array;
    } else return [];
}

function normalizeArraySize(array) {
    if (typeof array !== 'undefined' && array.length > 0) {
        /*while (array.length > 50) {
            array.shift();
        }*/
        return array;
    } else return [];
}

function DrawCurveTemperature() {


    var data = new google.visualization.DataTable();
    data.addColumn('date', 'Time of Day');
    data.addColumn('number', 'Temperature');
    data.addRows(normalizeArraySize(sortingArrayByFirstParameterAsDateASC(temperatureInputArray)));


    var options = {

        hAxis: {
            title: 'Time'
        },
        vAxis: {
            title: 'Temperature'
        },
        series: {
            1: { curveType: 'function' }
        },
        colors: ['red', 'blue', 'black', 'green', 'yellow', 'gray']
    };

    var chart = new google.visualization.LineChart(document.getElementById('temperature-line-chart'));
    chart.draw(data, options);
}

function DrawCurveHumidity() {


    var data = new google.visualization.DataTable();
    data.addColumn('date', 'Time of Day');
    data.addColumn('number', 'Humidity');
    data.addRows(normalizeArraySize(sortingArrayByFirstParameterAsDateASC(humidityInputArray)));

    var options = {
        hAxis: {
            title: 'Time'
        },
        vAxis: {
            title: 'Humidity'
        },
        series: {
            1: { curveType: 'function' }
        },
        colors: ['blue', 'black', 'green', 'yellow', 'gray']
    };

    var chart = new google.visualization.LineChart(document.getElementById('humidity-line-chart'));
    chart.draw(data, options);
}

function DrawCurveHeater() {
    var data = new google.visualization.DataTable();
    data.addColumn('date', 'Time of Day');
    data.addColumn('number', 'Heating Speed');
    data.addRows(normalizeArraySize(sortingArrayByFirstParameterAsDateASC(heaterInputArray)));

    var options = {
        hAxis: {
            title: 'Time'
        },
        vAxis: {
            title: 'Heating Speed'
        },
        series: {
            1: { curveType: 'function' }
        },
        colors: ['blue', 'black', 'green', 'yellow', 'gray']
    };

    var chart = new google.visualization.LineChart(document.getElementById('heating-line-chart'));
    chart.draw(data, options);
}

function DrawGaugeChart(value) {
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Gas', value]

    ]);

    var options = {
        width: 600,
        redFrom: 90,
        redTo: 100,
        yellowFrom: 75,
        yellowTo: 90,
        greenFrom: 50,
        greenTo: 75,
        minorTicks: 5,
        min: 0,
        max: 100

    };

    var chart = new google.visualization.Gauge(document.getElementById('heating-gauge-chart'));

    chart.draw(data, options);


}

function InitTemperatureDatas(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            console.log(obj[i]);
            temperatureInputArray.push([new Date(obj[i].TimeStamp), obj[i].Data]);
        }
    } else {
        temperatureInputArray.push([new Date(obj.TimeStamp), obj.Data]);
    }
}

function InitHumidityDatas(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            console.log(obj[i]);
            humidityInputArray.push([new Date(obj[i].TimeStamp), obj[i].Humidity]);
        }
    } else {
        humidityInputArray.push([new Date(obj.TimeStamp), obj.Humidity]);
    }
}

function InsertTemp(date, temp) {
    temperatureInputArray.push([new Date(date), temp]);
    DrawCurveTemperature();
}

function InsertHumidity(date, humi) {
    humidityInputArray.push([new Date(date), humi]);
    DrawCurveHumidity();
}


function InserHeater(date, heatspeed) {
    heaterInputArray.push([new Date(date), heatspeed]);
    DrawCurveHeater();
    DrawGaugeChart(heatspeed);
}

function AddNewLog(obj) {
    var table = document.getElementsByClassName("table")[0];
    var tb = table.getElementsByTagName("tbody")[0];
    var row = tb.insertRow(0);
    row.classList.add("incoming-data");
    switch (obj.Device.Type) {
        case "temperature":
            row.classList.add("temp-data");
            break;
        case "humidity":
            row.classList.add("humidity-data");
            break;
        case "heater":
            row.classList.add("temp-event");
            break;
    }




    var cell0 = row.insertCell(0);
    var cell1 = row.insertCell(1);
    var cell2 = row.insertCell(2);
    var cell3 = row.insertCell(3);

    cell0.innerHTML = obj.TimeStamp;
    cell1.innerHTML = obj.Device.CommunicationUnitId;
    cell2.innerHTML = obj.Data;
    cell3.innerHTML = obj.Device.Type;

}


function DeserealizeAndControlForVisualisation(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            switch (obj[i].Device.Type) {
                case "temperature":
                    InsertTemp(obj[i].TimeStamp, obj[i].Data);
                    break;
                case "humidity":
                    InsertHumidity(obj[i].TimeStamp, obj[i].Data);
                    break;
                case "heater":
                    InserHeater(obj[i].TimeStamp, obj[i].Data);
                    break;
            }
        }
    } else {
        switch (obj.Device.Type) {
            case "temperature":
                InsertTemp(obj.TimeStamp, obj.Data);
                break;
            case "humidity":
                InsertHumidity(obj.TimeStamp, obj.Data);
                break;
            case "heater":
                InserHeater(obj.TimeStamp, obj.Data);
                break;
        }
    }
}

function DeserealizeAndControlForLogging(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            AddNewLog(obj[i]);
        }
    } else {
        AddNewLog(obj);
    }
    IncomingData();
}

function LogPagerControl() {
    var first = document.getElementById("first-page-log");
    first.addEventListener("click", () => {
        console.log("a");

    });
    var left = document.getElementById("page-left-log");
    left = addEventListener("click", () => {
        console.log("a");

    });
    var right = document.getElementById("page-right-log");
    right = addEventListener("click", () => {
        console.log("a");

    });
    var last = document.getElementById("last-page-log");
    last = addEventListener("click", () => {
        console.log("a");

    });
    var selector = document.getElementById("page-size-selector");
    selector = addEventListener("change", () => {
        console.log("a");

    });
}

window.onload = function (e) {
    var navbar = document.getElementsByClassName("nbar");
    var activeLink = document.getElementsByClassName("active")[0].textContent;
    switch (activeLink) {
        case "Home":


            google.charts.setOnLoadCallback(DrawCurveHumidity);
            google.charts.setOnLoadCallback(DrawCurveTemperature);


            DeserealizeAndControlForVisualisation(document.getElementById("json").textContent);

            break;
        case "Logs":
            var first = document.getElementById("first-page-log");
            first.addEventListener("click", counterpp);

            var left = document.getElementById("page-left-log");
            left.addEventListener("click", counterpp);

            var right = document.getElementById("page-right-log");
            right.addEventListener("click", counterpp);

            var last = document.getElementById("last-page-log");
            last.addEventListener("click", counterpp);

            var selector = document.getElementById("page-size-selector");
            selector.addEventListener("change", counterpp);

            IncomingData();
            break;
        case "Gas":


            google.charts.setOnLoadCallback(DrawCurveHeater);
            DeserealizeAndControlForVisualisation(document.getElementById("json").textContent);

            break;
        case "Settings":
            if (document.URL.includes("RoomSettings")) {

                GetAllFreeEspsInvoke();
                GetEspsOfRoomInvoke();

                var select = document.getElementById("select-esp");
                select.addEventListener("change", AddEspToRoom);

                var name = document.getElementById("room-name-input");
                name.addEventListener("focusout", UpdateRoomName);
                
                var dayli = document.getElementById("room-dayli-input");
                dayli.addEventListener("focusout", UpdateRoomDayliSetpoint);

                var nightly = document.getElementById("room-nightly-input");
                nightly.addEventListener("focusout", UpdateRoomNightliSetpoint);


                var heat = document.getElementById("toggle-heater");
                var cool = document.getElementById("toggle-cooler");
                heat.addEventListener("click", ToogleRoomHeaterCooler);
                cool.addEventListener("click", ToogleRoomHeaterCooler);
            }
            else {
                var addRoomButton = document.getElementById("add-room-button");
                addRoomButton.addEventListener("click", AddNewRoom);
            }
            break;
    }
}