google.charts.load('current', { packages: ['corechart', 'line'] });
google.charts.setOnLoadCallback(drawCurveTypes);
/*google.charts.setOnLoadCallback(drawCurveTypes1);*/
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
function drawCurveTypes() {
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

function drawCurveTypes1() {
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
function InitTemperatureDatas(json) {
    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (var i = 0; i < obj.length; i++) {
            console.log(obj[i]);
            temperatureInputArray.push([new Date(obj[i].TimeStamp), obj[i].Temperature]);
        }
    } else {
        temperatureInputArray.push([new Date(obj.TimeStamp), obj.Temperature]);
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

function f(json) {
    InitTemperatureDatas(json);
    InitHumidityDatas(json);
    drawCurveTypes();
    drawCurveTypes1();
}
window.onload = function (e) {
    f(document.getElementById("hjson").textContent);
}