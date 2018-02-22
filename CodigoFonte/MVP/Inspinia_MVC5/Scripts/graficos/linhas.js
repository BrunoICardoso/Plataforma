function DesenharGraficoLinhas(selector, dataset, width, heigth, coresCategoria) {


    // Set the dimensions of the canvas / graph
    var margin = { top: 10, right: 0, bottom: 30, left: 50 },
        width = width - margin.left - margin.right,
        height = heigth - margin.top - margin.bottom;

    // Parse the date / time
    var parseDate = d3.time.format("%Y-%m-%d").parse;

    var color = d3.scale.category10();

    // Set the ranges
    var x = d3.time.scale().range([0, width]);
    var y = d3.scale.linear().range([height, 0]);

    // Define the axes
    var xAxis = d3.svg.axis().scale(x)
        .orient("bottom").ticks(8).tickFormat(d3.time.format("%d/%m"));

    var yAxis = d3.svg.axis().scale(y)
        .orient("left").ticks(5);

    // Define the line
    var priceline = d3.svg.line()
        .x(function (d) { return x(d.date); })
        .y(function (d) { return y(d.valor); });

    // Adds the svg canvas
    var svg = d3.select(selector)
        .append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
        .append("g")
            .attr("transform",
                  "translate(" + margin.left + "," + margin.top + ")");

    // Get the data

    dataset.forEach(function (d) {
        d.date = parseDate(d.date);
        d.valor = +d.valor;
    });

    // Scale the range of the data
    x.domain(d3.extent(dataset, function (d) { return d.date; }));
    y.domain([0, d3.max(dataset, function (d) { return d.valor; })]);

    // Nest the entries by symbol
    var dataNest = d3.nest()
        .key(function (d) { return d.categoria; })
        .entries(dataset);

    // Loop through each symbol / key
    dataNest.forEach(function (d) {

        svg.append("path")
            .attr("class", "line")
            .style("stroke", function () {
                return coresCategoria[d.key];
            })
            .attr("d", priceline(d.values));

    });

    // Add the X Axis
    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis);

    // Add the Y Axis
    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis);

}
