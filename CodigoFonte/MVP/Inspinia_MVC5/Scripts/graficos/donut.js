function DesenharGraficoDonut(selector, dataset, width, height, radius) {

    var valortotal = d3.sum(dataset.valores, function (d) { return d; });

    var color = d3.scale.ordinal().range(["#E37686", "#D2D087", "#6AC6B6", "#1C97B0"]); //.category20();

    var pie = d3.layout.pie()
        .sort(null);

    var arc = d3.svg.arc()
        .innerRadius(radius - 40)
        .outerRadius(radius);

    var arc2 = d3.svg.arc()
       .innerRadius(radius)
       .outerRadius(radius + 30);

    var svg = d3.select(selector).append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    var path = svg.selectAll("path")
        .data(pie(dataset.valores))
        .enter().append("path")
        .attr("class", "partgraf")
        .attr("fill", function (d, i) { return color(i); })
        .attr("d", arc)
        .attr("centroidx", function (d, i) { return arc.centroid(d)[0] })
        .attr("centroidy", function (d, i) { return arc.centroid(d)[1] });


    var path = svg.selectAll("label")
        .data(pie(dataset.valores))
        .enter().append("path")
        .attr("class", "label")
        .attr("fill", function (d, i) { return "#fff"; })
        .attr("d", arc2)
        .attr("centroidx", function (d, i) { return arc2.centroid(d)[0] })
        .attr("centroidy", function (d, i) { return arc2.centroid(d)[1] })

    var labels = $(selector + " .label");

    $(labels).each(function (i, e) {


        var x = $(e).attr("centroidx");
        var y = $(e).attr("centroidy");
        var dx = 0;
        var dy = 0;


        if (x < 0) {
            dx = getTextWidth(dataset.valores[i], 12);
        }

        if (y > 0) {
            dy = 10
        }


        svg.append("text")
            .attr("x", x - dx)
            .attr("y", parseFloat(y) + dy + 12)
            .attr("class", "label_absolute")
            .attr("fill", "#AAA")
            .text(function (d) { return dataset.valores[i]; });


        var perc = parseFloat(dataset.valores[i]) / valortotal;
        svg.append("text")
            .attr("x", x - dx)
            .attr("y", parseFloat(y) + dy)
            .attr("class", "label_percent")
            .attr("fill", "#AAA")
            .text(function (d) { return Math.round(perc * 100) + "%"; });

    });

}