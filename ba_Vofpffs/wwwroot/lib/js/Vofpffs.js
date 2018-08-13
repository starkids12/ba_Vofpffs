var selectedDbSet = "api/GetA";

// control the Visualisation
var selectedVisualiation = "";
var selectedProperty = "";

// html Elements
var visualSelector;
var propertySelector;

// html Elements for displaying FileEntry data
var idp;
var filenamep;
var sizep;
var ipp;
var datep;
var headerl;
var selectedDbSetp;

// 
var parse = d3.utcParse("%Y-%m-%dT%H:%M:%S.%f");
var formatTime = d3.utcFormat("%B %d, %Y");
var color = d3.scaleOrdinal(d3.schemeCategory10);


function initMap() {
    //todo: Setting Map Center dynamically
};

// GEtting the HTML Elements
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
    visualSelector = document.getElementById("visualSelector");
    propertySelector = document.getElementById("propertySelector");

    renderChart();
});

// New Window Size requires re rendering of the Visualization to fit the screen
$(window).resize(function () {

    renderChart();
});

// new Visualisation has been selected
function onVisualChange(select) {

    selectedVisualiation = select;
    renderChart();
    resetInspector();
};

// new Property to display has been selected
function onPropertyChange(select) {
    selectedProperty = select;
    renderChart();
    resetInspector();
};

// hide/show the property selector
function setPropertySelector(display) {
    propertySelector.style.display = display;
}

// render the Visualisation
function renderChart() {

    d3.json(selectedDbSet, function (error, classes) {
        if (error)
            throw error;

        if (selectedVisualiation == "") {

            selectedVisualiation = "treeMap";
            selectedProperty = "id";
            setSelectorFields(Object.keys(classes[0]));
            setPropertySelector("block");
        }

        if (selectedVisualiation == "treeMap") {
            drawSvgContent(classes, selectedProperty);
            setPropertySelector("block");
        }
        else if (selectedVisualiation == "googleMap") {
            drawGoogleMap(classes);
            setPropertySelector("none");
        }
        else if (selectedVisualiation == "timeLine") {
            drawTimeLine(classes);
            setPropertySelector("none");
        }
    });
};

// Render the TimeLine Visualisation
function drawTimeLine(classes) {

    var date = classes[0].dateTime.slice(0, -1);

    var canvasElement = document.getElementById("FileEntryChart");
    while (canvasElement.firstChild) {
        canvasElement.removeChild(canvasElement.firstChild);
    }
    var svg = d3.select(canvasElement).append("svg");

    var width = canvasElement.clientWidth;
    var height = canvasElement.clientHeight;

    svg.attr("width", width).attr("height", height);

    var nested = d3.nest()
        .key(function (d) { return d.dateTime; })
        .entries(classes);

    var x = d3.scaleTime()
        .domain([parse(classes[0].dateTime.slice(0, -1)),
        parse(classes[classes.length - 1].dateTime.slice(0, -1))])
        .range([0, width - 200]);

    var y = d3.scaleLinear()
        .rangeRound([0, height-200])
        .domain([0, d3.max(nested, function (d) { return d.values.length; })]);

    var yscale = d3.scaleLinear()
        .rangeRound([height-200, 0])
        .domain([0, d3.max(nested, function (d) { return d.values.length; })]);

    //console.log(d3.max(nested, function (d) { return d.values.length; }) );

    var axisX = d3.axisBottom(x);//.ticks(d3.timeMinute.every(2));
    var axisY = d3.axisLeft(yscale).tickArguments([3, "s"]);

    svg.append("g")
        .attr("transform", "translate(100," + height / 1.15 + ")")
        .call(axisX);

    svg.append("g")
        .attr("transform", "translate(95," + (height - (height / 1.15) - ((100/1.15) - 115)) + ")")
        .call(axisY);

    svg.selectAll(".bar")
        .data(nested)
        .enter()
        .append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(parse(d.key.slice(0, -1))) + 98; })
        .attr("y", function (d) { return (height / 1.15) - y(d.values.length); })
        .attr("width", 4)
        .attr("height", function (d) { return y(d.values.length); })
        .attr("fill", "blue");

    svg.selectAll(".bar")
        .append("title")
        .text(function (d) {

            var t1 = "File Count : " + d.values.length;

            d.values.forEach(function (element) {

                t1 += "\n" + " - - - - -" + "\n";
                t1 += "Filename: " + element.filename + "\n" +
                    "Filesize: " + element.size + " Byte \n" +
                    "IpAddress: " + element.ipAddress + "\n" +
                    "Datetime:" + parse(element.dateTime.slice(0, -1)) + "\n";

                
            });
            return t1;

        });
}

// Render The google Map Visualisation
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

// render Treemap
function drawSvgContent(classes, property) {

    // ugly Code
    var canvasElement = document.getElementById("FileEntryChart");
    while (canvasElement.firstChild) {
        canvasElement.removeChild(canvasElement.firstChild);
    }
    var svg = d3.select(canvasElement).append("svg");

    var width = canvasElement.clientWidth;
    var height = canvasElement.clientHeight;

    // transition time eine s
    var transitionDuration = 1000;

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

// set the propertys to select from
function setSelectorFields(keys) {

    // remove the filepath 
    var index = keys.indexOf("filepath");
    if (index > -1) {
        keys.splice(index, 1);
    }

    while (visualSelector.firstChild) {
        visualSelector.removeChild(visualSelector.firstChild);
    }

    while (propertySelector.firstChild) {
        propertySelector.removeChild(propertySelector.firstChild);
    }

    keys.forEach(function (element) {
        var optK = document.createElement("option")
        optK.value = element;
        optK.innerHTML = element;

        propertySelector.appendChild(optK);
    });

    var opt1 = document.createElement("option");
    opt1.value = "treeMap";
    opt1.innerHTML = "Tree Map";

    var opt2 = document.createElement("option");
    opt2.value = "googleMap";
    opt2.innerHTML = "IP Google Map";

    var opt3 = document.createElement("option");
    opt3.value = "timeLine";
    opt3.innerHTML = "Time Line";

    visualSelector.appendChild(opt1);
    visualSelector.appendChild(opt2);
    visualSelector.appendChild(opt3);
}

// get Textwidth of an Element
function getTextWidth(text, font) {
    // if given, use cached canvas for better performance
    // else, create new canvas
    var canvas = getTextWidth.canvas || (getTextWidth.canvas = document.createElement("canvas"));
    var context = canvas.getContext("2d");
    context.font = font;
    var metrics = context.measureText(text);
    return metrics.width;
};

// reset the HeaderList of reuse
function resetHeaderList() {

    while (headerl.firstChild) {
        headerl.removeChild(headerl.firstChild);
    }
};

// Set Data to display for a selected FileEntry
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

// Set the displayed DBSet
function setDbSet(id) {

    if (id == "buttonA") {
        selectedDbSetp.innerText = "Selected Set: A";
        selectedDbSet = "api/GetA";
    }
    else if (id == "buttonB") {
        selectedDbSetp.innerText = "Selected Set: B";
        selectedDbSet = "api/GetB";
    }

    resetInspector()
    renderChart();
};

// reset the Inspector for reuse
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

// get Lat Lon for grouping
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