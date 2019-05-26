function AddNewRoom() {
    var settingslist = document.getElementById("settings-list");
    console.log(settingslist);
    settingslist.appendChild(CreateRoom());
    connection.invoke("AddNewRoom").catch(function (err) {
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
    lerp = function(a, b, u) {
        return (1 - u) * a + u * b;
    };

    fade = function(element, property, start, end, duration) {
        console.log("fade start on" + element.cells[0].innerText)
        var interval = 10;
        var steps = duration / interval;
        var step_u = 1.0 / steps;
        var u = 0.0;
        var theInterval = setInterval(function() {
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