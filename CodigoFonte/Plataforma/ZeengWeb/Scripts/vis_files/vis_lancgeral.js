var BubbleChart, root,
  bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

BubbleChart = (function () {
    function BubbleChart(data) {
        this.do_filter = bind(this.do_filter, this);
        this.use_filters = bind(this.use_filters, this);
        this.hide_details = bind(this.hide_details, this);
        this.show_details = bind(this.show_details, this);
        this.hide_labels = bind(this.hide_labels, this);
        this.display_labels = bind(this.display_labels, this);
        this.move_towards_group = bind(this.move_towards_group, this);
        this.display_by_group = bind(this.display_by_group, this);
        this.move_towards_group_center = bind(this.move_towards_group_center, this);
        this.group_by = bind(this.group_by, this);
        this.get_distinct_values = bind(this.get_distinct_values, this);
        this.color_by = bind(this.color_by, this);
        this.remove_colors = bind(this.remove_colors, this);
        this.sort = bind(this.sort, this);
        this.get_color_map = bind(this.get_color_map, this);
        this.get_color = bind(this.get_color, this);
        this.get_type_from_key_name = bind(this.get_type_from_key_name, this);
        this.get_color_map_lookup_set = bind(this.get_color_map_lookup_set, this);
        this.get_color_map_grade = bind(this.get_color_map_grade, this);
        this.get_color_map_achievement = bind(this.get_color_map_achievement, this);
        this.get_map_max_values = bind(this.get_map_max_values, this);
        this.max_values_map = this.get_map_max_values(data);


        this.move_towards_center = bind(this.move_towards_center, this);
        this.display_group_all = bind(this.display_group_all, this);
        this.start = bind(this.start, this);
        this.create_vis = bind(this.create_vis, this);
        this.create_nodes = bind(this.create_nodes, this);
        this.data = data;
        this.width = 1000;
        this.height = 600;
        this.default_radius = 15;
        //this.max_value = d3.max(data, function (d) { if (d.Categoria == 'Empresa') return d.Valor });
        //this.max_value_empresa = d3.max(data, function (d) { if(d.Categoria == 'Empresa') return d.Valor })

        this.max_radius = 50;


        this.tooltip = CustomTooltip("my_tooltip", 240);
        this.center = {
            x: this.width / 2,
            y: this.height / 2
        };
        this.layout_gravity = -0.01;
        this.damper = 0.6;
        this.vis = null;
        this.force = null;
        this.circles = null;
        this.nodes = [];
        this.create_nodes();
        this.create_vis();




    }

    BubbleChart.prototype.get_map_max_values = function (data) {
        var max_values_map = {};

        max_values_map["Empresa"] = d3.max(data, function (d) {
            if (d.Categoria == 'Empresa') {
                return parseFloat(d.Valor)
            } 
        });

        max_values_map["Marca"] = d3.max(data, function (d) {
            if (d.Categoria == 'Marca') {
                return parseFloat(d.Valor)
            } 
        });
        max_values_map["MesAno"] = d3.max(data, function (d) {
            if (d.Categoria == 'MesAno') {
                return parseFloat(d.Valor)
            } 
        })
        max_values_map["Estado"] = d3.max(data, function (d) {
            if (d.Categoria == 'Estado') {
                return parseFloat(d.Valor)
            } 
        })
        max_values_map["Origem"] = d3.max(data, function (d) {
            if (d.Categoria == 'Origem') {
                return parseFloat(d.Valor)
            }
        })

        return max_values_map;
    }

    BubbleChart.prototype.create_nodes = function () {
        return this.data.forEach((function (_this) {
            return function (d, i) {

                var radiusNode = parseInt(parseFloat(d.Valor) * _this.max_radius / _this.max_values_map[d.Categoria]);
                if (radiusNode < 5) {
                    radiusNode = 5;
                }
                var node;
                node = {
                    id: i,
                    original: d,
                    radius: radiusNode,
                    value: 99,
                    color: _this.get_color(d.Categoria),
                    x: Math.random() * _this.width,
                    y: Math.random() * _this.height
                };
                return _this.nodes.push(node);
            };
        })(this));
    };

    BubbleChart.prototype.create_vis = function () {
        var that;
        this.vis = d3.select("#vis").append("svg").attr("width", this.width).attr("height", this.height).attr("id", "svg_vis");
        this.circles = this.vis.selectAll("circle").data(this.nodes, function (d) {
            return d.id;
        });
        that = this;

        this.circles.enter().append("circle").attr("r", 100).style("fill", (function (_this) {
            return function (d) {
                return d.color;
                //return this.get_color(d.Categoria);
            };
        })(this)).attr("stroke-width", 0).attr("stroke", (function (_this) {
            return function (d) {
                return '#404040';
            };
        })(this)).attr("id", function (d) {
            return "bubble_" + d.id;
        }).attr("opacity", 0).on("mouseover", function (d, i) {
            return that.show_details(d, i, this);
        }).on("mouseout", function (d, i) {
            return that.hide_details(d, i, this);
        });
        return this.circles.transition().duration(1500).attr("opacity", 1).attr("r", function (d) {
            return d.radius;
        });
    };

    BubbleChart.prototype.charge = function (d) {
        if (d.radius === 0) {
            return 0;
        }
        return -Math.pow(d.radius, 2);
    };

    BubbleChart.prototype.start = function () {
        this.force = d3.layout.force().nodes(this.nodes).size([this.width, this.height]);
        return this.circles.call(this.force.drag);
    };

    BubbleChart.prototype.display_group_all = function () {
        this.hide_labels();
        this.force.gravity(this.layout_gravity).charge(this.charge).friction(0.9).on("tick", (function (_this) {
            return function (e) {
                return _this.circles.each(_this.move_towards_center(e.alpha)).attr("cx", function (d) {
                    return d.x;
                }).attr("cy", function (d) {
                    return d.y;
                });
            };
        })(this));
        return this.force.start();
    };

    BubbleChart.prototype.move_towards_center = function (alpha) {
        return (function (_this) {
            return function (d) {
                d.x = d.x + (_this.center.x - d.x) * (_this.damper + 0.02) * alpha;
                return d.y = d.y + (_this.center.y - d.y) * (_this.damper + 0.02) * alpha;
            };
        })(this);
    };

    

    BubbleChart.prototype.get_color = function (categoria) {
        switch (categoria) {
            case "Marca":
                return '#C7AEAC';
                break;
            case "Empresa":
                return '#6AC6B5';
                break;
            case "Estado":
                return '#87C4E0';
                break;
            case "MesAno":
                return '#B6B345';
                break;
            case "Origem":
                return '#0796AF';
                break;
            default:
                return '#2f4050';
        }

    };

    BubbleChart.prototype.get_color_map_lookup_set = function (allValuesArray) {
        var baseArray, color_map, index, j, len, value;
        baseArray = ["#6AC6B5", "#C7AEAC", "#87C4E0", "#FFCC66", "#66CC33", "#33FFCC", "#00A0AA", "#FFCCFF", "#FF9933", "#99FF99", "#00BB00", "#CCFFCC", "#333333", "#CCCCCC", "#99CCCC", "#FF0000"];
        index = 0;
        color_map = {};
        for (j = 0, len = allValuesArray.length; j < len; j++) {
            value = allValuesArray[j];
            color_map[value] = baseArray[index];
            index = index + 1;
            if (index >= baseArray.length) {
                index = 0;
            }
        }
        return color_map;
    };

    BubbleChart.prototype.get_type_from_key_name = function (keyName) {
        if (/^Achievement/.test(keyName)) {
            return "Achievement";
        }
        if (/^Grade/.test(keyName)) {
            return "Grade";
        }
        return "Other";
    };

    BubbleChart.prototype.get_color_map = function (keyName, allValuesArray) {
        var key_type;
        key_type = this.get_type_from_key_name(keyName);
        switch (key_type) {
            case "Achievement":
                return this.get_color_map_achievement(allValuesArray);
            case "Grade":
                return this.get_color_map_grade(allValuesArray);
            default:
                return this.get_color_map_lookup_set(allValuesArray);
        }
    };

    BubbleChart.prototype.sort = function (keyName, allValuesArray) {
        var key_type;
        key_type = this.get_type_from_key_name(keyName);
        switch (key_type) {
            case "Achievement":
                return allValuesArray.sort((function (_this) {
                    return function (a, b) {
                        return Number(a) - Number(b);
                    };
                })(this));
            default:
                return allValuesArray.sort();
        }
    };

    BubbleChart.prototype.remove_colors = function () {
        this.circles.transition().duration(2000).style("fill", "#cfcfcf");
        return hide_color_chart();
    };

    BubbleChart.prototype.color_by = function (what_to_color_by) {
        var allValuesArray, color_mapper;
        this.what_to_color_by = what_to_color_by;
        allValuesArray = this.get_distinct_values(what_to_color_by);
        color_mapper = this.get_color_map(what_to_color_by, allValuesArray);
        show_color_chart(what_to_color_by, color_mapper);
        return this.circles.transition().duration(1000).style("fill", (function (_this) {
            return function (d) {
                return color_mapper[d.original[what_to_color_by]];
            };
        })(this));
    };

    BubbleChart.prototype.get_distinct_values = function (what) {
        var allValues, allValuesArray, key, value;
        allValues = {};
        this.nodes.forEach((function (_this) {
            return function (d) {
                var value;
                value = d.original[what];
                return allValues[value] = true;
            };
        })(this));
        allValuesArray = [];
        for (key in allValues) {
            value = allValues[key];
            allValuesArray.push(key);
        }
        this.sort(what, allValuesArray);
        return allValuesArray;
    };

    BubbleChart.prototype.group_by = function (what_to_group_by) {
        var allValuesArray, numCenters, position, total_slots;
        this.what_to_group_by = what_to_group_by;
        allValuesArray = this.get_distinct_values(what_to_group_by);
        numCenters = allValuesArray.length;
        this.group_centers = {};
        this.group_labels = {};
        position = 2;
        total_slots = allValuesArray.length + 4;
        allValuesArray.forEach((function (_this) {
            return function (i) {
                var x_position;
                x_position = _this.width * position / total_slots;
                _this.group_centers[i] = {
                    x: x_position,
                    y: _this.height / 2
                };
                _this.group_labels[i] = x_position;
                return position = position + 1;
            };
        })(this));
        this.hide_labels();
        this.force.gravity(this.layout_gravity).charge(this.charge).friction(0.9).on("tick", (function (_this) {
            return function (e) {
                return _this.circles.each(_this.move_towards_group_center(e.alpha)).attr("cx", function (d) {
                    return d.x;
                }).attr("cy", function (d) {
                    return d.y;
                });
            };
        })(this));
        this.force.start();
        return this.display_labels();
    };

    BubbleChart.prototype.move_towards_group_center = function (alpha) {
        return (function (_this) {
            return function (d) {
                var target, value;
                value = d.original[_this.what_to_group_by];
                target = _this.group_centers[value];
                d.x = d.x + (target.x - d.x) * (_this.damper + 0.02) * alpha * 1.1;
                return d.y = d.y + (target.y - d.y) * (_this.damper + 0.02) * alpha * 1.1;
            };
        })(this);
    };

    BubbleChart.prototype.display_by_group = function () {
        this.force.gravity(this.layout_gravity).charge(this.charge).friction(0.9).on("tick", (function (_this) {
            return function (e) {
                return _this.circles.each(_this.move_towards_group(e.alpha)).attr("cx", function (d) {
                    return d.x;
                }).attr("cy", function (d) {
                    return d.y;
                });
            };
        })(this));
        this.force.start();
        return this.display_years();
    };

    BubbleChart.prototype.move_towards_group = function (alpha) {
        return (function (_this) {
            return function (d) {
                var target;
                target = _this.group_centers[d.group];
                d.x = d.x + (target.x - d.x) * (_this.damper + 0.02) * alpha * 1.1;
                return d.y = d.y + (target.y - d.y) * (_this.damper + 0.02) * alpha * 1.1;
            };
        })(this);
    };

    BubbleChart.prototype.display_labels = function () {
        var label_data, labels;
        label_data = d3.keys(this.group_labels);
        labels = this.vis.selectAll(".top_labels").data(label_data);
        return labels.enter()
                .append("g")
                .append("text")
                .attr("class", "top_labels")
                .attr("x", (function (_this) {
                    return function (d) {
                        return _this.group_labels[d];
                    };
                })(this))
                .attr("y", 40)
                .attr("text-anchor", "start")
                .text(function (d) {
                    return d;
                }
        );
    };

    BubbleChart.prototype.hide_labels = function () {
        var labels;
        return labels = this.vis.selectAll(".top_labels").remove();
    };

    BubbleChart.prototype.show_details = function (data, i, element) {
        var content, key, ref, title, value;
        d3.select(element).attr("stroke", "black");
        content = "";
        ref = data.original;
        for (key in ref) {
            value = ref[key];
            title = key.substring(key.indexOf(":") + 1);
            content += "<span class=\"name\">" + title + ":</span><span class=\"value\"> " + value + "</span><br/>";
        }
        return this.tooltip.showTooltip(content, d3.event);
    };

    BubbleChart.prototype.hide_details = function (data, i, element) {
        d3.select(element).attr("stroke", "#404040");
        return this.tooltip.hideTooltip();
    };

    BubbleChart.prototype.use_filters = function (filters) {
        return this.nodes.forEach((function (_this) {
            return function (d) {
                d.radius = parseInt(parseFloat(d.original.Valor) * _this.max_radius / _this.max_values_map[d.original.Categoria]);
                if (d.radius < 5) {
                    d.radius = 5;
                }

                filters.discrete.forEach(function (filter) {
                    var value;
                    value = d.original[filter.target];
                    if (filter.removeValues[value] != null) {
                        return d.radius = 0;
                    }
                });
                return _this.do_filter();
            };
        })(this));
    };

    BubbleChart.prototype.do_filter = function () {
        this.force.start();
        return this.circles.transition().duration(2000).attr("r", function (d) {
            return d.radius;
        });
    };

    return BubbleChart;

})();

root = typeof exports !== "undefined" && exports !== null ? exports : this;

$(function () {
    var chart, render_chart, render_vis;
    chart = null;
    render_vis = function (csv) {
        render_filters_colors_and_groups(csv);
        return render_chart(csv);
    };
    render_chart = function (csv) {
        chart = new BubbleChart(csv);
        chart.start();
        return root.display_all();
    };
    root.display_all = (function (_this) {
        return function () {
            return chart.display_group_all();
        };
    })(this);
    root.group_by = (function (_this) {
        return function (groupBy) {
            if (groupBy === '') {
                return chart.display_group_all();
            } else {
                return chart.group_by(groupBy);
            }
        };
    })(this);
    root.color_by = (function (_this) {
        return function (colorBy) {
            if (colorBy === '') {
                return chart.remove_colors();
            } else {
                return chart.color_by(colorBy);
            }
        };
    })(this);
    root.use_filters = (function (_this) {
        return function (filters) {
            return chart.use_filters(filters);
        };
    })(this);
    return d3.csv("/data/mapa_geral.csv", render_vis);
});

// ---
// generated by coffee-script 1.9.2