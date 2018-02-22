var BubbleChart, root,
  bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; };

BubbleChart = (function () {
    function BubbleChart(data, width, height) {

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
        this.get_type_from_key_name = bind(this.get_type_from_key_name, this);
        this.get_color_map_lookup_set = bind(this.get_color_map_lookup_set, this);
        this.get_color_map_grade = bind(this.get_color_map_grade, this);
        this.get_color_map_achievement = bind(this.get_color_map_achievement, this);
        this.move_towards_center = bind(this.move_towards_center, this);
        this.display_group_all = bind(this.display_group_all, this);
        this.start = bind(this.start, this);
        this.create_vis = bind(this.create_vis, this);
        this.create_nodes = bind(this.create_nodes, this);
        this.data = data;
        this.width = width;
        this.height = height;
        this.default_radius = 15;
        this.tooltip = CustomTooltip("my_tooltip", 400);
        this.center = {
            x: this.width / 2,
            y: this.height / 2
        };
        this.layout_gravity = -0.01;
        this.damper = 0.5;
        this.vis = null;
        this.force = null;
        this.circles = null;
        this.nodes = [];
        this.create_nodes();
        this.create_vis();
    }

    BubbleChart.prototype.create_nodes = function () {
        return this.data.forEach((function (_this) {
            return function (d, i) {
                var node;
                node = {
                    id: i,
                    original: d,
                    radius: _this.default_radius,
                    value: 99,
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
                return '#cfcfcf';
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

    BubbleChart.prototype.get_color_map_achievement = function (allValuesArray) {
        var color_map, j, len, value;
        color_map = {};
        for (j = 0, len = allValuesArray.length; j < len; j++) {
            value = allValuesArray[j];
            if (value <= -2) {
                color_map[value] = '#ff0000';
            } else if (value <= -1) {
                color_map[value] = '#ff9900';
            } else if (value <= 0) {
                color_map[value] = '#ffff00';
            } else if (value < 2) {
                color_map[value] = '#00FF00';
            } else {
                color_map[value] = '#FF00CC';
            }
        }
        return color_map;
    };

    BubbleChart.prototype.get_color_map_grade = function (allValuesArray) {
        var color_map, j, len, value;
        color_map = {};
        for (j = 0, len = allValuesArray.length; j < len; j++) {
            value = allValuesArray[j];
            switch (value) {
                case "*":
                    color_map[value] = '#FF00CC';
                    break;
                case "Z":
                    color_map[value] = '#FF00CC';
                    break;
                case "A":
                    color_map[value] = '#00FF00';
                    break;
                case "B":
                    color_map[value] = '#00FF00';
                    break;
                case "C":
                    color_map[value] = '#FFFF00';
                    break;
                case "D":
                    color_map[value] = '#FF0000';
                    break;
                case "E":
                    color_map[value] = '#FF0000';
                    break;
                case "F":
                    color_map[value] = '#FF0000';
                    break;
                case "G":
                    color_map[value] = '#FF0000';
                    break;
                case "U":
                    color_map[value] = '#7F0000';
                    break;
                default:
                    color_map[value] = '#4F4F4F';
            }
        }
        return color_map;
    };

    BubbleChart.prototype.get_color_map_lookup_set = function (allValuesArray) {
        var baseArray, color_map, index, j, len, value;
        baseArray = ["#3D89A1", "#BD6567", "#A3A13A", "#0195AE", "#51C2C6", "#C7AEAC", "#9FCFD3", "#BEBFC4", "#31B59B", "#BA9653", "#FAD79F", "#0195AE", "#5EB0D8", "#B6B345", "#87C5E1", "#D2D087", "#DFA660", "#B8B1B8", "#FAC14D", "#D7C6C4", "#E6BB88", "#D0D1D5", "#6BC6B5"];
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
        position = 1;
        total_slots = allValuesArray.length + 2;

        allValuesArray.forEach((function (_this) {

            return function (i) {

                //console.log(_this.width + ' * ' + position + ' / ' + total_slots);
                                
                var x_position;
                x_position = _this.width * position / total_slots;

                _this.group_centers[i] = {
                    x: x_position,
                    y: _this.height / 2
                };

                _this.group_labels[i] = x_position;
                //_this.group_labels[i] = x_position - 50;
                return position = position + 1;
            };
        })(this));
        this.hide_labels();
        this.force.gravity(this.layout_gravity).charge(this.charge).friction(0.8).on("tick", (function (_this) {
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

        // console.log(label_data); quantos labels vem
        //console.log(labels);
        var objetolabel;

        //var qtdeBubbles = 19;
        //var cont=0;

        objetolabel = labels.enter()
                        .append("g")
                        .attr("transform", (function (_this) {
                            
                            //ainda precisa pegar o valor de object -> x e setar no transforma do label.
                            //acho que pra pegar o valor de x é algo assim     //   console.log(_this.nodes[1]["x"]);
                            
                            //console.log(_this.vis[0][0]);
                            //console.log(_this.vis[0][0].lastChild.g()); // nome marca
                            //console.log(_this.vis[0])

                            _this.nodes.forEach(function (item) {
                                //console.log(item["original"]["nomeMarca"]);

                                //console.log(_this);
                                
                                //_this.group_labels["la marca1"] = 715;

                                //var nomeMarca = item["original"]["nomeMarca"];

                                //labels[0].forEach(function (item, index) {

                                //    console.log(item["__data__"]);

                                //    if (nomeMarca == item["__data__"]) {
                                //        console.log(item1["x"]);
                                //    }
                                //});
                            });

                            return function (d) {                                
                                return "translate(" + _this.group_labels[d] + ", 10)";
                            };

                        })(this))                
                        .append("text")
                        .attr("class", "top_labels")
                        //.attr("x", (function (_this) {
                        //    return function (d) {
                        //        return _this.group_labels[d];
                        //    };
                        //})(this))
                        //.attr("y", 40)
                        .attr("text-anchor", "start")
                        .attr("style","font-weight:bold")
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

        content += "<span class=\"name\">Marca:</span><span class=\"value\"> " + ref.nomeMarca + "</span><br/>";
        content += "<span class=\"name\">Produto:</span><span class=\"value\"> " + ref.nomeProduto + "</span><br/>";
        content += "<span class=\"name\">UF:</span><span class=\"value\"> " + ref.nomeEstado + "</span><br/>";
        content += "<span class=\"name\">Área:</span><span class=\"value\"> " + ref.nomeArea + "</span><br/>";
        content += "<span class=\"name\">Espécie:</span><span class=\"value\"> " + ref.nomeEspecie + "</span><br/>";
        content += "<span class=\"name\">Subespécie:</span><span class=\"value\"> " + ref.nomeSubEspecie + "</span><br/>";
        content += "<span class=\"name\">Base:</span><span class=\"value\"> " + ref.nomeBase + "</span><br/>";
        content += "<span class=\"name\">Característica:</span><span class=\"value\"> " + ref.nomeCaracteristica + "</span><br/>";
        content += "<span class=\"name\">Atríbuto:</span><span class=\"value\"> " + ref.nomeAtributo + "</span><br/>";
        content += "<span class=\"name\">Complemento:</span><span class=\"value\"> " + ref.nomeComplemento + "</span><br/>";
        content += "<span class=\"name\">Data da concessão:</span><span class=\"value\"> " + formataDataJson(ref.dataconcessao) + "</span><br/>";
        content += "<span class=\"name\">Registro:</span><span class=\"value\"> " + ref.numregistro + "</span><br/>";
        content += "<span class=\"name\">Origem:</span><span class=\"value\"> " + ref.nomeOrigem + "</span><br/>";
        content += "<span class=\"name\">Status:</span><span class=\"value\"> " + ref.Status + "</span><br/>";

        return this.tooltip.showTooltip(content, d3.event);
    };

    BubbleChart.prototype.hide_details = function (data, i, element) {
        d3.select(element).attr("stroke", "#404040");
        return this.tooltip.hideTooltip();
    };

    BubbleChart.prototype.use_filters = function (filters) {
        return this.nodes.forEach((function (_this) {
            return function (d) {
                d.radius = _this.default_radius;
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
        
    
    //return d3.csv("/data/mapa.csv", render_vis);
});

// ---
// generated by coffee-script 1.9.2