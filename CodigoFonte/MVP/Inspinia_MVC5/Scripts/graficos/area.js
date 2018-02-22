function DesenharGraficoArea(selector, dataset, width, heigth) {

    var margin = { top: 20, right: 0, bottom: 30, left: 50 },
        width = width - margin.left - margin.right,
        height = heigth - margin.top - margin.bottom;

    var parseDate = d3.time.format("%Y-%m-%d").parse;

    var x = d3.time.scale()
        .range([0, width]);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .ticks(6)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .ticks(6)
        .orient("left");

    var area = d3.svg.area()
        .x(function (d) { return x(d.date); })
        .y0(height)
        .y1(function (d) { return y(d.valor); });

    var svg = d3.select(selector).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


    dataset.forEach(function (d) {
        d.date = parseDate(d.date);
        d.valor = +d.valor;
    });

    var maxValue = d3.max(dataset, function (d) { return d.valor; });
    var minValue = d3.min(dataset, function (d) { return d.valor; });

    minValue = minValue * 0.95;

    x.domain(d3.extent(dataset, function (d) { return d.date; }));
    y.domain([minValue,maxValue ]);

    svg.append("path")
        .datum(dataset)
        .attr("class", "area")
        .attr("d", area);

    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis);

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
      .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", ".71em")
        .style("text-anchor", "end")
        .text("Rank");
}