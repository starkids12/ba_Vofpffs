var selectedDbSet = "api/uploadA";
var selectedProperty = "";

var selectedVisualiation = "";

var idp;
var filenamep;
var sizep;
var ipp;
var datep;
var headerl;
var selectedDbSetp;

function initMap() {

};

$(document).ready(function () {

    idp = document.getElementById("idParagraph");
    filenamep = document.getElementById("filenameParagraph");
    sizep = document.getElementById("sizeParagraph");
    ipp = document.getElementById("ipParagraph");
    datep = document.getElementById("dateParagraph");
    headerl = document.getElementById("HeaderList");
    countryp = document.getElementById("countryParagraph");
    regionNamep = document.getElementById("regionNameParagraph");
    cityp = document.getElementById("cityParagraph");
    latp = document.getElementById("latParagraph");
    lonp = document.getElementById("lonParagraph");
    ispp = document.getElementById("ispParagraph");

    selectedDbSetp = document.getElementById("selectedDbSetParagraph");

    renderChart();
});

$(window).resize(function () {

    renderChart();
});

function onSelectChange(select) {

    selectedVisualiation = select;
    renderChart();
    resetInspector();
}

function renderChart() {

    d3.json(selectedDbSet, function (error, classes) {
        if (error)
            throw error;

        if (selectedVisualiation == "") {

            selectedVisualiation = "treeMap"
            setSelectorFields();
        }

        if (selectedVisualiation == "treeMap") {
            drawSvgContent(classes, "ipAddress");
        }
        else if (selectedVisualiation == "googleMap") {
            drawGoogleMap(classes);
        }
        else if (selectedVisualiation == "headerFingerprint") {
            drawSvgContent(classes, "headerFingerprint");
        }
        else if (selectedVisualiation == "timeLine") {
            drawTimeLine(classes);
        }
    });
};

function drawTimeLine(classes) {
    var parse = d3.utcParse("%Y-%m-%dT%H:%M:%S.%f");
    var formatTime = d3.utcFormat("%B %d, %Y");
    
    console.log(classes);
    var date = classes[0].dateTime.slice(0, -1);
    console.log(date);
    console.log(parse(date));
    console.log(formatTime(parse(date)));

    var canvasElement = document.getElementById("FileEntryChart");
    while (canvasElement.firstChild) {
        canvasElement.removeChild(canvasElement.firstChild);
    }
    var svg = d3.select(canvasElement).append("svg");

    var width = canvasElement.clientWidth;
    var height = canvasElement.clientHeight;

    svg.attr("width", width).attr("height", height);

    var x = d3.scaleTime()
        .rangeRound([0, width]);

    var axis = d3.axisBottom(x);

    svg.append("g")
        .attr("transform", "translate(0," + height / 2 + ")")
        .call(axis);
}

function drawGoogleMap(classes) {

    var canvasElement = document.getElementById("FileEntryChart");
    while (canvasElement.firstChild) {
        canvasElement.removeChild(canvasElement.firstChild);
    }

    // create Map
    var map = new google.maps.Map(canvasElement, {
        zoom: 8,
        center: new google.maps.LatLng(53.574, 9.9747),
        mapTypeId: google.maps.MapTypeId.TERRAIN
    });

    var cords = getAllLatLon(classes);
    console.log(cords);

    cords.forEach(function (element) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(element.lat, element.lon),
            map: map,
            title: "Data Count: " + element.count
        })
    });
}

function drawSvgContent(classes, property) {

    // ugly Code
    var canvasElement = document.getElementById("FileEntryChart");
    while (canvasElement.firstChild) {
        canvasElement.removeChild(canvasElement.firstChild);
    }
    var svg = d3.select(canvasElement).append("svg");

    var width = canvasElement.clientWidth;
    var height = canvasElement.clientHeight;

    // transition tim ein ms
    var transitionDuration = 500;

    svg.attr("width", width).attr("height", height);

    // ->
    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var radiusScale = d3.scaleSqrt()
        .domain([d3.min(classes, function (d) { return d.size; }), d3.max(classes, function (d) { return d.size; })])
        .range([10, 100]);

    console.log(classes);

    var nested = d3.nest()
        .key(function (d) { return d[property]; })
        .entries(classes);

    console.log(nested);

    // create a root Node for the Tree Structure
    var root = { key: property, children: [] };

    // create Parent Elements for each Nested Attribute
    nested.forEach(function (element) {

        root.children.push({ key: element.key, children: element.values });
    });

    // creating the hierarchy structure for the TreeMap
    var test = d3.hierarchy(root)
        .eachBefore(function (d) { d.data.id = d.data.id; })
        .sum(function (d) { return radiusScale(d.size); })
        .sort(function (a, b) { return b.size - a.size; });

    var treemap = d3.treemap()
        .size([width, height])
        .round(true)
        .paddingInner(2)
        .paddingOuter(2)
        .paddingTop(20);

    treemap(test);

    // Appending all Tree Descenmdants to the svg
    var cell = svg.selectAll(".node")
        .data(test.descendants())
        .enter()
        .append("g")
        .attr("transform", function (d) { return "translate(" + d.x0 + "," + d.y0 + ")"; })
        .attr("class", "node")
        .each(function (d) { d.node = this; })

    // Appending rectangles for all Tree Members
    cell.append("rect")
        .attr("id", function (d) { return d.data.id; })
        .attr("width", function (d) { return (d.x1 - d.x0); })
        .attr("height", function (d) { return (d.y1 - d.y0); })
        .attr("fill", function (d) {

            if (d.parent)
                return color(d.parent.data.key);
            else
                return color("root");
        });


    // Differ leafes vs tree in Visualization
    var leafes = cell.filter(function (d) { return !d.children; })
    var tree = cell.filter(function (d) { return d.children; })

    // set OnClick for leafes
    leafes.on("click", function (d) { setIncpector(d.data); })

    // Info for leafes : actuall Files
    leafes.append("title")
        .text(function (d) {
            return "Name : " + d.data.filename + "\n" +
                "Size : " + d.data.size + "Byte" + "\n" +
                "IPAddres : " + d.data.ipAddress + "\n" +
                "DateTime : " + d.data.dateTime + "\n" +
                "Country : " + d.data.country;
        });

    leafes.append("text")
        .attr("x", function (d) { return (d.x1 - d.x0) / 2; })
        .attr("y", function (d) { return (d.y1 - d.y0) / 2; })
        .attr("class", "label")
        .text(function (d) {
            var t = "Name: " + d.data.filename;

            if (getTextWidth(t, "bold 12pt arial") > (d.x1 - d.x0))
                return "X";
            else
                return t;
        });

    leafes.append("text")
        .attr("x", function (d) { return (d.x1 - d.x0) / 2; })
        .attr("y", function (d) { return ((d.y1 - d.y0) / 2) + 16; })
        .attr("class", "label")
        .text(function (d) {
            var t = "Size : " + (d.data.size / 1024).toFixed(2) + " KB";

            if (getTextWidth(t, "bold 12pt arial") > (d.x1 - d.x0))
                return "";
            else
                return t;
        });


    // Info for the Tree : nested Property
    tree.append("text")
        .attr("dx", function (d) { return 0; })
        .attr("dy", function (d) { return 15; })
        .text(function (d) {
            return "Key: " + d.data.key;
        });
}

function setSelectorFields() {

    var propertySelector = document.getElementById("propertySelector");

    while (propertySelector.firstChild) {
        propertySelector.removeChild(propertySelector.firstChild);
    }

    var opt1 = document.createElement("option");
    opt1.value = "treeMap";
    opt1.innerHTML = "IP Tree Map";

    var opt2 = document.createElement("option");
    opt2.value = "googleMap";
    opt2.innerHTML = "IP Google Map";

    var opt3 = document.createElement("option");
    opt3.value = "headerFingerprint";
    opt3.innerHTML = "HeaderFingerprint";

    var opt4 = document.createElement("option");
    opt4.value = "timeLine";
    opt4.innerHTML = "Time Line";

    propertySelector.appendChild(opt1);
    propertySelector.appendChild(opt2);
    propertySelector.appendChild(opt3);
    propertySelector.appendChild(opt4);
}

function getTextWidth(text, font) {
    // if given, use cached canvas for better performance
    // else, create new canvas
    var canvas = getTextWidth.canvas || (getTextWidth.canvas = document.createElement("canvas"));
    var context = canvas.getContext("2d");
    context.font = font;
    var metrics = context.measureText(text);
    return metrics.width;
};

function resetHeaderList() {

    while (headerl.firstChild) {
        headerl.removeChild(headerl.firstChild);
    }
};

function setIncpector(data) {

    resetHeaderList();

    idp.innerText = "Id : " + data.id;
    filenamep.innerText = "Filename : " + data.filename;
    sizep.innerText = "Size : " + data.size + " Byte";
    ipp.innerText = "IpAddress : " + data.ipAddress;
    datep.innerText = "Date : " + data.dateTime;
    countryp.innerText = "Country : " + data.country;
    regionNamep.innerText = "Region Name : " + data.regionName;
    cityp.innerText = "City : " + data.city;
    latp.innerText = "Lat : " + data.lat;
    lonp.innerText = "Lon : " + data.lon;
    ispp.innerText = "ISP : " + data.isp;

    var hvPair = data.headers.split("|");
    console.log(hvPair);
    hvPair.forEach(function (element) {

        var text = element.replace("=", " = ");

        var entry = document.createElement('li');
        entry.appendChild(document.createTextNode(text));

        headerl.appendChild(entry);
    });
};

function setDbSet(id) {

    if (id == "buttonA") {
        selectedDbSetp.innerText = "DB Set A";
        selectedDbSet = "api/uploadA";
    }
    else if (id == "buttonB") {
        selectedDbSetp.innerText = "DB Set B";
        selectedDbSet = "api/uploadB";
    }

    resetInspector()
    renderChart();
};

function resetInspector() {

    resetHeaderList();

    idp.innerText = "Id : ";
    filenamep.innerText = "Filename : ";
    sizep.innerText = "Size : ";
    ipp.innerText = "IpAddress : ";
    datep.innerText = "Date : ";
    countryp.innerText = "Country : ";
    regionNamep.innerText = "Region Name : ";
    cityp.innerText = "City : ";
    latp.innerText = "Lat : ";
    lonp.innerText = "Lon : ";
    ispp.innerText = "ISP : ";
};

function getAllLatLon(classes) {

    var nested = d3.nest()
        .key(function (d) { return d["ipAddress"]; })
        .entries(classes);

    var ret = [];

    nested.forEach(function (element) {
        ret.push({ lat: element.values[0].lat, lon: element.values[0].lon, count: element.values.length });
    });

    return ret;
}