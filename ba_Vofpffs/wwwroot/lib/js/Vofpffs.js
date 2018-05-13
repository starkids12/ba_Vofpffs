var selectedDbSet = "api/uploadA";
var selectedProperty = "";

var idp;
var hashp;
var sizep;
var ipp;
var datep;
var headerl;
var selectedDbSetp;

$(document).ready(function () {

    idp = document.getElementById("idParagraph");
    hashp = document.getElementById("hashParagraph");
    sizep = document.getElementById("sizeParagraph");
    ipp = document.getElementById("ipParagraph");
    datep = document.getElementById("dateParagraph");
    headerl = document.getElementById("HeaderList");

    selectedDbSetp = document.getElementById("selectedDbSetParagraph");

    renderChart();
});

$(window).resize(function () {

    renderChart();
});

function onSelectChange(select) {

    selectedProperty = select;
    renderChart();
    resetInspector();
}

function renderChart() {

    d3.json(selectedDbSet, function (error, classes) {
        if (error)
            throw error;

        if (selectedProperty == "") {

            setSelectorFields(classes);
        }

        // ugly Code
        var svgDiv = document.getElementById("FileEntryChart");
        while (svgDiv.firstChild) {
            svgDiv.removeChild(svgDiv.firstChild);
        }
        var svg = d3.select(svgDiv).append("svg");

        var width = svgDiv.clientWidth;
        var height = svgDiv.clientHeight;

        // transition tim ein ms
        var transitionDuration = 500;

        svg.attr("width", width).attr("height", height);

        // ->
        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var radiusScale = d3.scaleSqrt()
            .domain([d3.min(classes, function (d) { return d.size; }), d3.max(classes, function (d) { return d.size; })])
            .range([10, 100]);


        var nested = d3.nest()
            .key(function (d) { return d[selectedProperty]; })
            .entries(classes);

        // create a root Node for the Tree Structure
        var root = { key: "rootNode", children: [] };

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
                return "Name : " + d.data.hash + "\n" +
                    "Size : " + d.data.size + "Byte" + "\n" +
                    "IPAddres : " + d.data.ipAddress + "\n" +
                    "DateTime : " + d.data.dateTime;
            });

        leafes.append("text")
            .attr("x", function (d) { return (d.x1 - d.x0) / 2; })
            .attr("y", function (d) { return (d.y1 - d.y0) / 2; })
            .attr("class", "label")
            .text(function (d) {
                var t = "Name: " + d.data.hash;

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
    });
};

function setSelectorFields(data) {

    var propertySelector = document.getElementById("propertySelector");

    while (propertySelector.firstChild) {
        propertySelector.removeChild(propertySelector.firstChild);
    }

    var propertys = Object.keys(data[0]);

    propertys.forEach(function (element) {

        var opt = document.createElement("option");
        opt.value = element;
        opt.innerHTML = element;

        propertySelector.appendChild(opt);
    });

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
    hashp.innerText = "Hash : " + data.hash;
    sizep.innerText = "Size : " + data.size + " Byte";
    ipp.innerText = "IpAddress : " + data.ipAddress;
    datep.innerText = "Date : " + data.dateTime;

    var hvPair = data.headers.split("|");
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
    hashp.innerText = "Hash : ";
    sizep.innerText = "Size : ";
    ipp.innerText = "IpAddress : ";
    datep.innerText = "Date : ";
};