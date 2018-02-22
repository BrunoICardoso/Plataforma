

Histograma = function () {

    
    var dataset_copy;
    var brush;
    var max;


    var margin = {
        top: 10,
        right: 10,
        bottom: 20,
        left: 40
    };

      
    var width;
    var height;

    this.margin = margin;
    this.width = width;
    this.height = height;
    this.pageWidth = 960;
    this.percentWidth = 0;

    // Refresh data based on a range
    function getData(ds, startDate, endDate) {

        var startDate_time = (new Date(startDate)).getTime();
        var endDate_time = (new Date(endDate)).getTime();
        var parsedDate_date = new Date();
        var parsedDate_time;

        dataset = ds.filter(function (d, startDate, endDate) {
            parsedDate_time = new Date((d.Day)).getTime();
            return (parsedDate_time <= endDate_time) && (parsedDate_time >= startDate_time);
        });


    }

    function render() {

        datasetHist = d3.layout.histogram()
            .frequency(false)
            .range([0, max])
            //.bins(buckets)
        (dataset.map(function (d) {
            return d.Quantity;
        }));


    }

    // Draw axes
    function drawAxis() {


    }



    function renderSnapshot() {

        var brushExtent = brush.extent();
        var rangeExtent = [x(brushExtent[0]), x(brushExtent[1])];
        var rangeWidth = rangeExtent[1] - rangeExtent[0];

        datasetHist = d3.layout.histogram()
            .frequency(false)
        //.bins(x.ticks(buckets))
        .range([0, max])
            //.bins(buckets)
        (dataset.map(function (d) {
            return d.Quantity;
        }));


        // Draw lines around selected range
        context2.append("line")
            .attr("class", "brushSnapshot")
            .attr("x1", rangeExtent[0])
            .attr("x2", rangeExtent[0] + rangeWidth)
            .attr("y1", -6)
            .attr("y2", -6);

        context2.append("line")
            .attr("class", "brushSnapshot")
            .attr("x1", rangeExtent[0])
            .attr("x2", rangeExtent[0] + rangeWidth)
            .attr("y1", height + 1)
            .attr("y2", height + 1);


        context2.append("line")
            .attr("class", "brushSnapshot")
            .attr("x1", rangeExtent[0])
            .attr("x2", rangeExtent[0])
            .attr("y1", -6)
            .attr("y2", height + 1);

        context2.append("line")
            .attr("class", "brushSnapshot")
            .attr("x1", rangeExtent[0] + rangeWidth)
            .attr("x2", rangeExtent[0] + rangeWidth)
            .attr("y1", -6)
            .attr("y2", height + 1);

    }

    function clearSnapshot() {

        context2.selectAll("line").remove();
    }



    this.init = function (selector) {
        

        width = this.pageWidth - margin.left - margin.right;
        height = 63 - margin.top - margin.bottom;

        var parseDate = d3.time.format("%Y-%m-%d").parse;

        var datasetHist;
        var dataset;

        var x = d3.time.scale().range([0, width]);
        var y = d3.scale.linear().range([height, 0]);
        var bar;

        var xAxis, yAxis;

        brush = d3.svg.brush()
            .x(x)
            .on("brush", brushed);

        // A formatter for counts.
        var formatCount = d3.format(",.0f");
        var formatPercent = d3.format("%");

        var snapshotClicked = false;

        var svg = d3.select(selector).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom);


        var context = svg.append("g")
            .attr("class", "context")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        var context2 = svg.append("g")
            .attr("class", "context")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        var area_daily = d3.svg.area()
            .interpolate("step-after")
            .x(function (d) {
                return x(parseDate(d.Day));
            })
            .y0(height)
            .y1(function (d) {
                return y(d.Quantity);
            });

        var raw = d3.select("#csvdata").text();
        dataset_copy = d3.csv.parse(raw);
        var dataset = dataset_copy;

        getData(dataset, parseDate("2016-01-01"), parseDate("2016-12-31"));

        max = d3.max(dataset, function (d) {
            return +d.Quantity;
        });

        x.domain(d3.extent(dataset.map(function (d) {
            return parseDate(d.Day);
        })));

        // Generate a histogram using uniformly-spaced bins
        datasetHist = d3.layout.histogram()
            .frequency(false)

            .range([0, max])
            //.bins(buckets)
        (dataset.map(function (d) {

            return d.Quantity;
        }));

        y.domain([0, d3.max(dataset, function (d) {
            return +d.Quantity;
        })]);


        yAxis = d3.svg.axis()
            .scale(y)
            .ticks(2)
            .orient("left")
            
            //.tickFormat(formatCount);

        x.domain(d3.extent(dataset.map(function (d) {
            return parseDate(d.Day);
        })));

        xAxis = d3.svg.axis().scale(x).orient("bottom").tickFormat(d3.time.format("%d/%m"));

        datasetHist = d3.layout.histogram()
            .frequency(false)
            .range([0, max])
            //.bins(buckets)
        (dataset.map(function (d) {
            return d.Quantity;
        }));


        drawAxis();

        context.append("path")
            .datum(dataset)
            .attr("class", "area daily")
            .attr("d", area_daily);
        context.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(0," + height + ")")
            .call(xAxis);
        context.append("g")
            .attr("class", "x brush")
            .call(brush)
            .selectAll("rect")
            .attr("y", -6)
            .attr("height", height + 7);


        context.append("g")
            .attr("class", "y axis")
            .attr("transform", "translate(0,0)")
            .call(yAxis);
    }

    
    function brushed() {
        getData(dataset_copy, brush.extent()[0], brush.extent()[1]);
        render();

    }

    function type(d) {
        d.Quantity = +d.Quantity;
        return d;
    }

}