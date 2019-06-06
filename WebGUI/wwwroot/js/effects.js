function AddNewRoom() {
    var settingslist = document.getElementById("settings-list");
    console.log(settingslist);
    settingslist.appendChild(CreateRoom());
    connection.invoke("AddNewRoom").catch(function (err) {
        return console.error(err.toString());
    });

}
function UpdateRoomName() {
    if (this.value == "")
        return;
    var urlArray = document.URL.split("/");
    connection.invoke("UpdateRoomName", urlArray[urlArray.length - 1], this.value).catch(function (err) {
        return console.error(err.toString());
    });
    let h1 = document.getElementsByTagName("h1")[0];
    h1.textContent = this.value;
    let settingBox = document.getElementsByClassName("setting-box")[0];
    let elems = settingBox.getElementsByClassName("float-right");
    elems[0].textContent = this.value;
}
function UpdateRoomDayliSetpoint() {
    if (this.value == "")
        return;
    var urlArray = document.URL.split("/");
    connection.invoke("UpdateRoomDayliSetpoint", urlArray[urlArray.length - 1], this.value).catch(function (err) {
        return console.error(err.toString());
    });
    let settingBox = document.getElementsByClassName("setting-box")[0];
    let elems = settingBox.getElementsByClassName("float-right");
    elems[1].textContent = this.value;
}
function UpdateRoomNightliSetpoint() {
    if (this.value == "")
        return;
    var urlArray = document.URL.split("/");
    connection.invoke("UpdateRoomNightliSetpoint", urlArray[urlArray.length - 1], this.value).catch(function (err) {
        return console.error(err.toString());
    });
    let settingBox = document.getElementsByClassName("setting-box")[0];
    let elems = settingBox.getElementsByClassName("float-right");
    elems[2].textContent = this.value;
}
function numbersonly(myfield, e) {
    var key;
    var keychar;

    if (window.event)
        key = window.event.keyCode;
    else if (e)
        key = e.which;
    else
        return true;

    keychar = String.fromCharCode(key);

    // control keys
    if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27))
        return true;

    // numbers
    else if ((("0123456789").indexOf(keychar) > -1))
        return true;

    // only one decimal point
    else if ((keychar == ".")) {
        if (myfield.value.indexOf(keychar) > -1)
            return false;
    } else
        return false;
}

function AddEspToRoom() {
    let option = this.options[this.selectedIndex];

    var urlArray = document.URL.split("/");
    connection.invoke("AddEspToRoom", urlArray[urlArray.length - 1], option.value).catch (function (err) {
        return console.error(err.toString());
    });
    option.remove();
    document.getElementById("select-esp").selectedIndex = 0;
}
function MakeSelectOptionsWithEsps(json) {
    var select = document.getElementById("select-esp");
    for (var i = 1; i < select.length; i++) {
        select[i].remove();
    }

    var esps = [];


    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (i = 0; i < obj.length; i++) {
            if (obj[i].RoomId == null) {
                esps.push(obj[i].Esp.Id);//majd valami specko lesz
            }       
        }
    }
    uniqueArray = esps.filter(function (item, pos) {
        return esps.indexOf(item) == pos;
    });

    for (i = 0; i < uniqueArray.length; i++) {
        var node = document.createElement("option");
        node.value = uniqueArray[i];
        node.innerHTML = uniqueArray[i];
        select.appendChild(node);
    }

}
function DisplayEspsOfRoom(json) {
    var sensorList = document.getElementById("sensor-listing");
    for (var i = 1; i < sensorList.length; i++) {
        sensorList[i].remove();
    }
    var sensors = [];
    var espSensors = [];

    var obj = JSON.parse(json);
    if (Array.isArray(obj)) {
        for (i = 0; i < obj.length; i++) {
            sensors.push([obj[i].EspId, obj[i].Type]);
            if (!espSensors.includes(obj[i].EspId)) {
                espSensors.push(obj[i].EspId);
            }
        }
    }

    for (i = 0; i < espSensors.length; i++) {
        var p = CreateEspParagraph(espSensors[i]);
        sensorList.appendChild(p);
    }

    for (i = 0; i < sensors.length; i++) {

        var para = FindByAttributeValue("remove-id", sensors[i][0], "p");

        var spanSensor = document.createElement("span");
        spanSensor.classList.add("float-right");
        spanSensor.innerHTML = sensors[i][1];
        para.appendChild(spanSensor);
        para.appendChild(document.createElement("br"));
    }
}
function CreateEspParagraph(name) {
    var p = document.createElement("p");
    p.setAttribute("remove-id", name);
    var spanEsp = document.createElement("span");
    spanEsp.innerHTML = name + ":";

    var spanx = document.createElement("span");
    var ielement = document.createElement("i");
    const attribute = name;

    ielement.classList.add("far");
    ielement.classList.add("fa-times-circle");
    ielement.addEventListener("click", () => DeleteSensorFromRoom(attribute));
    spanx.appendChild(ielement);
    spanx.classList.add("float-left");
    p.appendChild(spanx);
    p.appendChild(spanEsp);

    return p;
}
function DeleteSensorFromRoom(value) {
    var element = FindByAttributeValue("remove-id", value, "p");
    element.remove();
    connection.invoke("RemoveEspFromRoom", value).catch(function (err) {
        return console.error(err.toString());
    });

    GetAllFreeEspsInvoke();
}

function FindByAttributeValue(attribute, value, element_type) {
    element_type = element_type || "*";
    var All = document.getElementsByTagName(element_type);
    for (var i = 0; i < All.length; i++) {
        if (All[i].getAttribute(attribute) == value) { return All[i]; }
    }
}

function GetAllFreeEspsInvoke() {
    connection.invoke("GettAllFreeEsps").catch(function (err) {
        return console.error(err.toString());
    });
}
function GetEspsOfRoomInvoke() {
    var urlArray = document.URL.split("/");
    connection.invoke("GetEspsOfRoomInvoke", urlArray[urlArray.length - 1]).catch(function (err) {
        return console.error(err.toString());
    });
}
function counterpp() {
    var counter = document.getElementById("counterpp");
    counter.textContent = parseInt(counter.textContent) + 1;
}

function CreateRoom() {
    var node = document.createElement("div");
    node.classList.add("settings-item");
    var nodeOdNode = document.createElement("a");
    var text = document.createTextNode("Room");
    nodeOdNode.appendChild(text);
    nodeOdNode.href = "#";
    node.appendChild(nodeOdNode);
    return node;
}

function IncomingData() {
    lerp = function (a, b, u) {
        return (1 - u) * a + u * b;
    };

    fade = function (element, property, start, end, duration) {
        console.log("fade start on" + element.cells[0].innerText)
        var interval = 10;
        var steps = duration / interval;
        var step_u = 1.0 / steps;
        var u = 0.0;
        var theInterval = setInterval(function () {
            if (u > 1.0) {
                clearInterval(theInterval);
                element.removeAttribute("style");
                console.log("fade finished" + element.cells[0].innerText)
                return;
            }
            var r = parseInt(lerp(start.r, end.r, u));
            var g = parseInt(lerp(start.g, end.g, u));
            var b = parseInt(lerp(start.b, end.b, u));
            var colorname = 'rgb(' + r + ',' + g + ',' + b + ')';
            element.style.setProperty(property, colorname);
            u += step_u;
        }, interval);

    };

    var elementsList = document.getElementsByClassName("incoming-data");
    for (let index = 0; index < elementsList.length; index++) {
        var elem = elementsList[index];

        var style = getComputedStyle(elem);
        var targetColor = style.backgroundColor;
        var rgb = targetColor.substring(4, targetColor.length - 1)
            .replace(/ /g, '')
            .split(',');
        console.log(rgb);
        var endColor = { r: parseInt(rgb[0]), g: parseInt(rgb[1]), b: parseInt(rgb[2]) };
        var startColor = { r: 255, g: 255, b: 255 };

        elem.classList.remove('incoming-data');
        fade(elem, 'background-color', startColor, endColor, 1000);
    }


}