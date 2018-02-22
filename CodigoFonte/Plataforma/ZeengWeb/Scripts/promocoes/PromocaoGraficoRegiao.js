
var GraficoRegiao = {

    CarregaGraficoAbrangencia: function () {

        var dados = retornaFiltro();

        $.ajax({
            type: "POST",
            url: "/api/PromocoesAPI/GetGraficoBrasil",
            data: JSON.stringify(dados),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                $('#brazil-map').html('');

                if (data != null && data.Estados != null) {

                    var dados = {};
                    var totalEstados = 0;
                    var arrTotais = [];


                    $.each(data.Estados, function (indice, obj) {

                        var local = obj.uf.toLowerCase();
                        var total = obj.total;

                        dados[local] = obj.total;

                        arrTotais.push(obj.total);
                    })

                    //$('#valorMenor').html("Menor valor: "+ arrTotais[arrTotais.length - 1]);
                    //$('#valorMaior').html("Maior valor: "+ arrTotais[0]);

                    //$('#valorMenor').html( arrTotais[arrTotais.length - 1]);
                    if (arrTotais[0] != null && arrTotais[0] != undefined)
                        $('#valorMaior').html(arrTotais[0]);
                    else
                        $('#valorMaior').html('0');


                    $('#brazil-map').vectorMap({
                        map: 'brazil',
                        backgroundColor: "transparent",
                        regionStyle: {
                            initial: {
                                fill: '#e4e4e4',
                                "fill-opacity": 0.9,
                                stroke: 'none',
                                "stroke-width": 0,
                                "stroke-opacity": 0
                            }

                        },
                        series: {
                            regions: [{
                                values: dados,
                                scale: ["#22d6b1", "#1ab394"],
                                normalizeFunction: 'polynomial'
                            }]
                        },
                        //onRegionOver: function (e, el, code) {
                        //},

                        onRegionTipShow: function (e, el, code) {

                            dados[code] != undefined ? el.html(el.html() + '<br>' + dados[code] + ' promoções').css("text-align", "center")

                                : el.html(el.html() + '<br> 0 promoções').css("text-align", "center");


                        }
                    });

                } else {
                    $('#valorMenor').html('0');
                    $('#valorMaior').html('0');

                    $('#brazil-map').vectorMap({
                        map: 'brazil',
                        backgroundColor: "transparent",
                        regionStyle: {
                            initial: {
                                fill: '#e4e4e4',
                                "fill-opacity": 0.9,
                                stroke: 'none',
                                "stroke-width": 0,
                                "stroke-opacity": 0
                            }
                        },
                        series: {
                            regions: [{
                                values: [],
                                scale: ["#22d6b1", "#1ab394"],
                                normalizeFunction: 'polynomial'
                            }]
                        },
                        //onRegionOver: function (e, el, code) {

                        //},

                        onRegionTipShow: function (e, el, code) {

                            el.html(el.html() + '<br> 0 promoções').css("text-align", "center");


                        }
                    });
                }


            }
        });
    }
}