function DesenharGraficoLinhas(selector, dataset, width, heigth, coresCategoria, formatoDate, periodoEscalaX) {

    if (!formatoDate || formatoDate == null || formatoDate == "")
        formatoDate = "%d/%m";
    
    var marginLeft = 30;

    var valorMaximo = d3.max(dataset, function (d) { return d.valor; });

    if (valorMaximo > 999) {
        marginLeft = 40;
    }

    var margin = { top: 10, right: 10, bottom: 45, left: marginLeft },
        width = width - margin.left - margin.right,
        height = heigth - margin.top - margin.bottom;

    var parseDate = d3.time.format("%Y-%m-%d").parse;

    var color = d3.scale.category10();

    var x = d3.time.scale().range([0, width]);
    var y = d3.scale.linear().range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .ticks(7)
        //.ticks(d3.time.days, 7)
        .tickFormat(d3.time.format(formatoDate))
        .tickPadding(20)
        .orient("bottom");
     
    // Verificação de periodicidade na escala X do gráfico
    // ===========================================================================
    //if (!periodoEscalaX || periodoEscalaX == null || periodoEscalaX == "")
    //    xAxis.ticks(18);
    //else {
    //    if (periodoEscalaX == 'DIARIO')
    //        xAxis.ticks(18);
    //    else if (periodoEscalaX == 'SEMANA')
    //        xAxis.ticks(d3.time.days, 7);
    //}
    
        var totalTicks = 5;
   
        if (valorMaximo < 5){
            totalTicks = valorMaximo;
        }       

        var yAxis = d3.svg.axis()
        .scale(y)
        .tickFormat(formataNumeroY)
        .orient("left")
        .ticks(totalTicks)
        .tickSubdivide(0);

    var priceline = d3.svg.line()
        .x(function (d) {
            //console.log(d.data);
            return x(d.data);
        })
        .y(function (d) {
            //console.log("valor:" + d.valor + " - y:" + y(d.valor));
            return y(d.valor);
        });

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
        d.data = parseDate(d.data);
        //d.valor = +d.valor;
    });

    // Scale the range of the data
    x.domain(d3.extent(dataset, function (d) { return d.data; }));
    y.domain([0, valorMaximo]);

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

function formataNumeroY(d) {

    if (d >= 1000) {
        return d.toString().slice(0, -3) + 'k';
    }
    else {
        if (Number.isInteger(d))
            return d
        else
            return "";
    }

}
