﻿google.charts.load('current', { packages: ['corechart', 'line'] });
google.charts.load('current', { 'packages': ['gauge'] });

google.charts.setOnLoadCallback(DrawCurveTemperature);
//google.charts.setOnLoadCallback(drawCurveTypes1);
google.charts.setOnLoadCallback(DrawCurveHumidity);

var temperatureInputArray = [];
var humidityInputArray = [];

function sortingArrayByFirstParameterAsDateASC(array) {
    if (typeof array !== 'undefined' && array.length > 0) {
        array.sort(function (aa, bb) {
            return new Date(aa[0]) - new Date(bb[0]);
        });
        return array;
    }
    else return [];
}
function normalizeArraySize(array) {
    if (typeof array !== 'undefined' && array.length > 0) {
        while (array.length > 50) {
            array.shift();
        }
        return array;
    }
    else return [];
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

    var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
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

    var chart = new google.visualization.LineChart(document.getElementById('chart_div1'));
    chart.draw(data, options);
}
function drawGaugeChart() {
    var data = google.visualization.arrayToDataTable([
        ['Label', 'Value'],
        ['Gas', 80]

    ]);

    var options = {
        width: 600,
        redFrom: 90, redTo: 100,
        yellowFrom: 75, yellowTo: 90,
        greenFrom: 50, greenTo: 75,
        minorTicks: 5, min: 0, max: 100

    };

    var chart = new google.visualization.Gauge(document.getElementById('gas_div'));

    chart.draw(data, options);

    //ez itt egy idozito amit majd szedj ki most maradhat
    setInterval(function () {
        data.setValue(0, 1, 40);
        chart.draw(data, options);
    }, 500);

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

function DeserealizeAndControl(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            switch (obj[i].Sensor.Type) {
                case "temperature":
                    InsertTemp(obj[i].TimeStamp, obj[i].Data);
                    break;
                case "humidity":
                    InsertHumidity(obj[i].TimeStamp, obj[i].Data);
                    break;
            }

        }
    } else {
        switch (obj.Sensor.Type) {
            case "temperature":
                InsertTemp(obj.TimeStamp, obj.Data);
                break;
            case "humidity":
                InsertHumidity(obj.TimeStamp, obj.Data);
                break;
        }
    }
}




window.onload = function (e) {
    DeserealizeAndControl(document.getElementById("json").textContent);
}