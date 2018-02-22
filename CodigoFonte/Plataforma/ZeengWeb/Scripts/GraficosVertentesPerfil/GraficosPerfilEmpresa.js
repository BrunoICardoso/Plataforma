var GraficosVertentes = {

    GraficoRedesSociais: function (idEmpresa, qtdeMeses) {
        //$(".infoRedesSocias").hide();
        $(".loadingRedesSocias").show();

        var url = "/api/EmpresaAPI/GetGraficoInteracoesRedesSociais?idEmpresa=" + idEmpresa + "&QtdeMeses=" + qtdeMeses;
        $.getJSON(url, function (data) {

            var totalPosts = 0;
            $.each(data, function (i, v) {
                totalPosts += v.valor;
            });

            if (totalPosts > 0) {

                $(".infoRedesSocias").show();
                $(".loadingRedesSocias").hide();

                $('.ibox-content.redes-sociais > .graf-caption').find('span').html(totalPosts);

                var coresCategoria = [];
                coresCategoria["twitter"] = "#E37686";
                coresCategoria["instagram"] = "#D2D087";
                coresCategoria["facebook"] = "#6AC6B6";
                coresCategoria["youtube"] = "#1C97B0";

                larguraGraf = $("#graf-redesSociais").width();
                alturaGraf = $("#graf-redesSociais").height() - 40;
                DesenharGraficoLinhas("#graf-redesSociais", data, larguraGraf, alturaGraf, coresCategoria, "%b/%y");
            } else {

                //$("#graf-redesSociais").remove();
                $(".legenda_social").remove();
                //$("#legendaRedesSociais").remove();
                //$("#vermaisRedesSocias").remove();
                $(".loadingRedesSocias").hide();
                //$(".redes-sociais").append(nenhumResultadoEncontrado());
                $("#graf-redesSociais").append(nenhumResultadoEncontrado());

            }


        });


    },

    GraficoNoticias: function (idEmpresa, qtdeMeses) {

        $(".graf-noticias").empty();
        $(".graf-noticias").append(Loading());

        var url = "/api/EmpresaAPI/GetNoticiasUltimosMeses?idEmpresa=" + idEmpresa + "&QtdeMeses=" + qtdeMeses;
        $.getJSON(url, function (data) {

            larguraGraf = $(".graf-noticias").width();
            alturaGraf = $(".graf-noticias").height();

            if (data) {
                var Total = 0;
                $.each(data, function (i, v) {
                    Total += v.valor;
                });

                $(".infoNoticias").show();
                $(".graf-noticias").find(".loadingPerfilEmpresa").hide();

                DesenharGraficoBarras(".graf-noticias", data, larguraGraf, alturaGraf, "%b/%y");
                $('.ibox-content.noticias').find('.graf-legenda').html('* notícias dos últimos 12 meses');
                $('#totalNoticiasEmpresa').html(Total);

            } else {
                //$(".graf-noticias").remove();
                //$("#legendaNoticias").remove();
                //$("#vermaisnoticias").remove();
                //$(".box-noticias").append(nenhumResultadoEncontrado());
                $(".graf-noticias").append(nenhumResultadoEncontrado());
                $(".graf-noticias").find(".loadingPerfilEmpresa").hide();
            }
        });

    },

    GraficoPresencaOnline: function (idEmpresa, qtdeMeses) {

        //$(".infoPresencaOnline").hide();
        $(".graf-presencaonline").empty();
        $(".graf-presencaonline").append(Loading());

        var url = "/api/EmpresaAPI/GetRankBrasil?idEmpresa=" + idEmpresa + '&QtdeMeses=' + qtdeMeses;
        $.getJSON(url, function (data) {

            var totalPosts = 0;
            if (data != null) {
                $.each(data, function (i, v) {
                    totalPosts += v.valor;
                });
            }

            if (totalPosts) {
                var larguraGraf = $(".graf-presencaonline").width();
                var alturaGraf = $(".graf-presencaonline").height() - 32;

                $(".infoPresencaOnline").show();
                $(".graf-presencaonline").find(".loadingPerfilEmpresa").hide();

                $('.graf-caption.presencaonline > span').html(formataNumero(data[data.length - 1].valor));
               
                DesenharGraficoArea(".graf-presencaonline", data, larguraGraf, alturaGraf, "%b/%y");

            } else {
                //$(".graf-presencaonline").remove();
                //$("#graflegendaPresencaOnline").empty();
                //$("#vermaisPresencaOnline").empty();
                //$(".presenca-online-box").append(nenhumResultadoEncontrado());
                $(".graf-presencaonline").html(nenhumResultadoEncontrado());

                //$(".graf-presencaonline").html('<div style="float: left; top:50%; left:50%; margin-left: 25%; margin-top: 25%;" ><img src="'+DiretorioImagens+'/icon_chateado.png">&nbsp; Não há dados para este gráfico <div>');
            }
        });

    },

    GraficoPromocoes: function (idEmpresa) {
        $(".graf-promocoes").empty();
        $(".graf-promocoes").append(Loading());

        var url = "/api/EmpresaAPI/RetornaGraficoModalidadePerfilEmpresa?idEmpresa=" + idEmpresa;

        var dataGrafico = {
            valores: []
        };

        var totalPromocoesCapturadas = 0;

        $.ajax({
            url: url,
            type: "GET",
            async: false,
            success: function (data) {

                var totalPromocoes = 0;
                $.each(data, function (i, v) {
                    totalPromocoes += v.valor;
                });

                if (totalPromocoes > 0) {

                    $(".infoPromocoes").show();
                    $(".graf-promocoes").find(".loadingPerfilEmpresa").hide();

                    $.each(data, function (i, item) {
                        dataGrafico.valores.push(parseInt(item.valor));
                        totalPromocoesCapturadas += item.valor;
                        $(".graf-promocoes-legenda").append(legendaGraficoDonut(item, RetornaCorGraficoDonut(i)));

                    });

                    $("#totalPromocoesEmpresa").text(totalPromocoesCapturadas);

                    larguraGraf = $(".graf-promocoes").width();
                    alturaGraf = $(".graf-promocoes").height();
                    var radius = 120;

                    DesenharGraficoDonut(".graf-promocoes", dataGrafico, larguraGraf, alturaGraf, radius);


                } else {
                    //$(".graf-promocoes").remove();
                    //$(".graf-legenda-promocoes").empty();
                    //$(".graf-promocoes-legenda").remove();
                    //$("#vermaisPromocoes").empty();
                    //$(".box-promocoes").append(nenhumResultadoEncontrado());
                    $(".graf-promocoes").find(".loadingPerfilEmpresa").hide();
                    $(".graf-promocoes").append(nenhumResultadoEncontrado());
                    $(".graf-promocoes-legenda").css('margin-top', '62px');
                }
            }
        });
    },

    GraficoLancamentoProdutos: function (idEmpresa, qtdeMeses) {
      
        //$(".infoLancamento").hide();
        $(".graf-lancamento").empty();
        $(".graf-lancamento").append(Loading());

        var url = "/api/EmpresaAPI/GetGraficoLancamentoProdutos?idEmpresa=" + idEmpresa + '&QtdeMeses=' + qtdeMeses;

        $.getJSON(url, function (data) {

            //if(data == null)
            //    CarregaGraficoLancamentoProdutos(idEmpresa,12);

            var totalProdutos = 0;
            $.each(data, function (i, v) {
                totalProdutos += v.valor;
            });

            if (totalProdutos > 0) {
                $(".infoLancamento").show();

                $('#qtdProdutosEmpresa').find('span').html(totalProdutos);

                $('.ibox-content.lancamento-produtos').find('.graf-legenda').html('* registros nos últimos 12 meses');

                var coresCategoria = [];
                coresCategoria["MAPA"] = "#E37686";
                coresCategoria["instagram"] = "#D2D087";
                coresCategoria["facebook"] = "#6AC6B6";
                coresCategoria["youtube"] = "#1C97B0";

                larguraGraf = $("#graf-lancamento").width();
                alturaGraf = $("#graf-lancamento").height() - 40;

                $(".graf-lancamento").find(".loadingPerfilEmpresa").hide();

                DesenharGraficoLinhas("#graf-lancamento", data, larguraGraf, alturaGraf, coresCategoria, "%b/%y");

            } else {

                //$("#graf-lancamento").remove();
                //$("#legendaLancamentoProdutos").empty();
                //$("#vermaisLancamentoProdutos").empty();
                $(".graf-lancamento").find(".loadingPerfilEmpresa").hide();
                //$(".lancamento-produtos").append(nenhumResultadoEncontrado());
                $("#graf-lancamento").append(nenhumResultadoEncontrado());
            }

        });

    }

};

function legendaGraficoDonut(item, cor) {

    return "<i class='fa fa-square' aria-hidden='true' style='color:" + cor + "''><span>" + item.categoria + "</span></i>";

}

function Loading() {

    return '<div class="loadingPerfilEmpresa">' +
                    '<div class="loadingPerfilEmpresaInner sk-spinner sk-spinner-three-bounce">' +
                        '<div class="sk-bounce1"></div>' +
                        '<div class="sk-bounce2"></div>' +
                        '<div class="sk-bounce3"></div>' +
                    '</div>' +
          '</div>';

}

function nenhumResultadoEncontrado() {

    return '<div class="nenhumResultado">' +
                    '<div>' +
                        '<img class="imgNenhumResultado" src="' + DiretorioImagens + '/icon_chateado.png" /> <br>' +
                       '<span>Nenhum Resultado Encontrado.</span>' +
                    '</div>' +
                '</div>';

}