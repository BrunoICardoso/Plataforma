function DesenharGraficoBarras(selector, dataset, width, heigth) {

    var margin = {top: 20, right: 20, bottom: 70, left: 40},
       width = width - margin.left - margin.right,
       height = heigth - margin.top - margin.bottom;

    // Parse the date / time
    var	parseDate = d3.time.format("%Y-%m-%d").parse;

    var x = d3.scale.ordinal().rangeRoundBands([0, width], .05);

    var y = d3.scale.linear().range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom")
        .ticks(6)
        .tickFormat(d3.time.format("%d/%m"))
    

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(10);

    var svg = d3.select(selector).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", 
              "translate(" + margin.left + "," + margin.top + ")");

    dataset.forEach(function (d) {
            d.date = parseDate(d.date);
            d.valor = +d.valor;
        });
	
    x.domain(dataset.map(function (d) { return d.date; }));
    y.domain([0, d3.max(dataset, function (d) { return d.valor; })]);

        svg.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis)
          .selectAll("text")
            .style("text-anchor", "end")
            .attr("dx", "-.8em")
            .attr("dy", "-.55em")
            .attr("transform", "rotate(-60)" );

        svg.append("g")
            .attr("class", "y axis")
            .call(yAxis)
          .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", ".71em")
            .style("text-anchor", "end")

        svg.selectAll("bar")
            .data(dataset)
          .enter().append("rect")
            .attr("class","barra")
            .attr("x", function(d) { return x(d.date); })
            .attr("width", x.rangeBand() - 10)
            .attr("y", function (d) { return y(d.valor); })
            .attr("height", function (d) { return height - y(d.valor); });
 
}