
var dadosRelatorio;

function Relatorios() {

    // SETs
    this.Pesquisar = _Pesquisar;
    this.MensagemExcluir = _mensagemExcluir;
    this.metricasFacebook = _metricasFacebook;
    this.metricasTwitter = _metricasTwitter;
    this.metricasInstagram = _metricasInstagram;
    this.GerarDadosCSV = _gerarDadosCSV;
    this.metricasYoutube = _metricasYoutube;
    this.GerarCSV = _gerarCSV;
    this.CarregaPeriodoRelatorio = _carregaPeriodoRelatorio;
    this.dadosRelatorio = {};
    this.MetricasPresencaNoticia = _metricasPresencaNoticia;

    // Métodos
    function _Pesquisar() {

        filtros.pagina = paginaAtual;

        $('#listagemRelatorios > tbody').html('');
        $('#listagemRelatorios > tbody').html('<tr><td colspan="5">Pesquisando... Aguarde!</td></tr>');

        var html = "";

        $.ajax({
            type: "POST",
            url: "/api/RelatoriosAPI/Pesquisar/",
            data: JSON.stringify(filtros),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.totalRegistros > 0) {

                    $.each(data.relatorios, function (index, obj) {

                        html += _templateGrid(obj);

                        Paginacao.CriaPaginacao(".paginacaoRelatorio", parseInt((data.totalRegistros / qtdeRegistros) + 0.9), data.totalRegistros, paginaAtual);
                        Paginacao.setMudouPagina(_MudouPagina);
                    });


                } else {
                    html = '<tr><td colspan="5">Sem dados para esta consulta!</td></tr>';
                    $('.paginacaoRelatorio').html('');
                }


                $('#listagemRelatorios > tbody').html(html);
            }
        });
    };

    function _MudouPagina(pagina) {
        paginaAtual = pagina;
        _Pesquisar();
    }

    function _templateGrid(obj) {

        //dados = {
        //    idRelatorio: 99,
        //    conteudo: $('#ID_DA_DIV').html()
        //}

        return '<tr>' +
                    '<td>' + formataDataJson(obj.dataInicial) + '</td>' +
                    '<td>' + formataDataJson(obj.dataFinal) + '</td>' +
                    '<td>' + obj.nomeRelatorio + '</td>' +
                    '<td>' + obj.nomeUsuario + '</td>' +
                    '<td style="width:1%; white-space: nowrap;">' +
                        '<a href="/Relatorios/Visualizar/?idRelatorio=' + obj.idClienteRelatorio + '" class="btn btn-white btn-sm" title="Visualizar" ><i class="fa fa-desktop"></i></a>&nbsp;&nbsp;' +
                        '<a href="/Relatorios/Editar/?idRelatorio=' + obj.idClienteRelatorio + '" class="btn btn-white btn-sm" title="Editar"><i class="fa fa-edit"></i></a>&nbsp;&nbsp;' +
                        '<a onclick="objRelatios.MensagemExcluir(' + obj.idClienteRelatorio + ')" class="btn btn-white btn-sm" title="Excluir"><i class="fa fa-trash-o"></i></a>&nbsp;&nbsp;' +
                        //'<a href="/Relatorios/GerarPDF/?idRelatorio=' + obj.idClienteRelatorio + '&conteudo=$(\'#ID_DA_DIV\').html()" target="_blank" class="btn btn-white btn-sm" title="GERAR PDF"><i class="fa fa-plus"></i></a>&nbsp;&nbsp;' +
                    '</td>' +
               '</tr>'
    }

    function _mensagemExcluir(idRelatorio) {
        swal({
            title: "Excluir Relatório?",
            text: "Você tem certeza que deseja excluir este relatório?",
            type: "warning",
            showCancelButton: true,
            cancelButtonText: "Cancelar",
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Sim",
            closeOnConfirm: false
        },
            function () {
                _excluir(idRelatorio);
                swal("Excluido!", "", "success");
            });
    }

    function _excluir(idRelatorio) {

        $.getJSON("/api/RelatoriosAPI/Excluir/?idRelatorio=" + idRelatorio, function (data) {
            if (data == false) {
                _erroExclusao();
            } else {
                _Pesquisar();
            }
        });
    }

    function _erroExclusao() {
        swal({
            title: "Excluir Relatório?",
            text: "Erro ao excluir relatório!",
            type: "warning",
            showCancelButton: true,
            cancelButtonText: "OK",
            closeOnConfirm: false
        });
    }


    var _medias = []

    function _gerarCSV(idRelatorio, obj, redeSocial) {

        var conteudo = $(document).find('#dados-' + redeSocial).html();
        //conteudo = conteudo.replace('%3B', ';');
        //conteudo = decodeURIComponent(conteudo);

        $(obj).attr('href', '/Relatorios/GerarCSV/?idRelatorio=' + idRelatorio + '&nomeRedeSocial=' + redeSocial + '&conteudo=' + conteudo);
        $(obj).attr('target', '_blank');
        $(obj).trigger('click');

    }

    function _gerarDadosCSV() {

        //================================================================================================
        // Facebook
        //================================================================================================
        var tabelaCompletaFACE = "";

        // Cabeçalhos
        var arrNomeColunaFACE = [];
        var strNomeColunasFACE = "";

        $('.titulos-metricas-facebook > th').each(function (index) {
            arrNomeColunaFACE.push($(this).text());
        });

        strNomeColunasFACE = arrNomeColunaFACE.join(';');
        strNomeColunasFACE += '\n';
        tabelaCompletaFACE += strNomeColunasFACE;

        // Colunas e valores
        var arrEmpresasValoresFACE = [];
        var strEmpresasValoresFACE = "";

        $('.tabela-metricas-facebook > tr').each(function (index) {

            arrEmpresasValoresFACE = [];

            $(this).find('td').each(function (index) {
                arrEmpresasValoresFACE.push($(this).text());
            });

            strEmpresasValoresFACE += arrEmpresasValoresFACE.join(';');
            strEmpresasValoresFACE += '\n';
        });

        tabelaCompletaFACE += strEmpresasValoresFACE;

        // Média
        var arrEmpresasValoresMediaFACE = [];
        var strEmpresasValoresMediaFACE = "";

        $('.tabela-media-geral-facebook > tr > td').each(function (index) {
            arrEmpresasValoresMediaFACE.push($(this).text());
        });

        strEmpresasValoresMediaFACE += arrEmpresasValoresMediaFACE.join(';');
        strEmpresasValoresMediaFACE += '\n';
        tabelaCompletaFACE += strEmpresasValoresMediaFACE;

        //console.log(tabelaCompletaFACE);

        $(document).find('#dados-facebook').html(encodeURIComponent(tabelaCompletaFACE));

        // ================================================================================================
        // Twitter
        // ================================================================================================

        var tabelaCompletaTW = "";

        // Cabeçalhos
        var arrNomeColunaTW = [];
        var strNomeColunasTW = "";

        $('.titulos-metricas-twitter > th').each(function (index) {
            arrNomeColunaTW.push($(this).text());
        });

        strNomeColunasTW = arrNomeColunaTW.join(';');
        strNomeColunasTW += '\n';
        tabelaCompletaTW += strNomeColunasTW;

        // Colunas e valores
        var arrEmpresasValoresTW = [];
        var strEmpresasValoresTW = "";

        $('.tabela-metricas-twitter > tr').each(function (index) {

            arrEmpresasValoresTW = [];

            $(this).find('td').each(function (index) {
                arrEmpresasValoresTW.push($(this).text());
            });

            strEmpresasValoresTW += arrEmpresasValoresTW.join(';');
            strEmpresasValoresTW += '\n';
        });

        tabelaCompletaTW += strEmpresasValoresTW;

        // Média
        var arrEmpresasValoresMediaTW = [];
        var strEmpresasValoresMediaTW = "";

        $('.tabela-media-geral-twitter > tr > td').each(function (index) {
            arrEmpresasValoresMediaTW.push($(this).text());
        });

        strEmpresasValoresMediaTW += arrEmpresasValoresMediaTW.join(';');
        strEmpresasValoresMediaTW += '\n';
        tabelaCompletaTW += strEmpresasValoresMediaTW;

        $(document).find('#dados-twitter').html(encodeURIComponent(tabelaCompletaTW));

        // ================================================================================================
        // Instagram
        // ================================================================================================
        var tabelaCompletaINSTA = "";

        // Cabeçalhos
        var arrNomeColunaINSTA = [];
        var strNomeColunasINSTA = "";

        $('.titulos-metricas-instagram > th').each(function (index) {
            arrNomeColunaINSTA.push($(this).text());
        });

        strNomeColunasINSTA = arrNomeColunaINSTA.join(';');
        strNomeColunasINSTA += '\n';
        tabelaCompletaINSTA += strNomeColunasINSTA;

        // Colunas e valores
        var arrEmpresasValoresINSTA = [];
        var strEmpresasValoresINSTA = "";

        $('.tabela-metricas-instagram > tr').each(function (index) {

            arrEmpresasValoresINSTA = [];

            $(this).find('td').each(function (index) {
                arrEmpresasValoresINSTA.push($(this).text());
            });

            strEmpresasValoresINSTA += arrEmpresasValoresINSTA.join(';');
            strEmpresasValoresINSTA += '\n';
        });

        tabelaCompletaINSTA += strEmpresasValoresINSTA;

        // Média
        var arrEmpresasValoresMediaINSTA = [];
        var strEmpresasValoresMediaINSTA = "";

        $('.tabela-media-geral-instagram > tr > td').each(function (index) {
            arrEmpresasValoresMediaINSTA.push($(this).text());
        });

        strEmpresasValoresMediaINSTA += arrEmpresasValoresMediaINSTA.join(';');
        strEmpresasValoresMediaINSTA += '\n';
        tabelaCompletaINSTA += strEmpresasValoresMediaINSTA;

        $(document).find('#dados-instagram').html(encodeURIComponent(tabelaCompletaINSTA));

        // ================================================================================================
        // Youtube
        // ================================================================================================
        var tabelaCompletaYT = "";

        // Cabeçalhos
        var arrNomeColunaYT = [];
        var strNomeColunasYT = "";

        $('.titulos-metricas-youtube > th').each(function (index) {
            arrNomeColunaYT.push($(this).text());
        });

        strNomeColunasYT = arrNomeColunaYT.join(';');
        strNomeColunasYT += '\n';
        tabelaCompletaYT += strNomeColunasYT;

        // Colunas e valores
        var arrEmpresasValoresYT = [];
        var strEmpresasValoresYT = "";

        $('.tabela-metricas-youtube > tr').each(function (index) {

            arrEmpresasValoresYT = [];

            $(this).find('td').each(function (index) {
                arrEmpresasValoresYT.push($(this).text());
            });

            strEmpresasValoresYT += arrEmpresasValoresYT.join(';');
            strEmpresasValoresYT += '\n';
        });

        tabelaCompletaYT += strEmpresasValoresYT;

        // Média
        var arrEmpresasValoresMediaYT = [];
        var strEmpresasValoresMediaYT = "";

        $('.tabela-media-geral-youtube > tr > td').each(function (index) {
            arrEmpresasValoresMediaYT.push($(this).text());
        });

        strEmpresasValoresMediaYT += arrEmpresasValoresMediaYT.join(';');
        strEmpresasValoresMediaYT += '\n';
        tabelaCompletaYT += strEmpresasValoresMediaYT;

        $(document).find('#dados-youtube').html(encodeURIComponent(tabelaCompletaYT));

    }

    function _carregaDadosRelatorio(idRelatorio) {
        $.getJSON("/api/RelatoriosAPI/RetornaRelatorio/?idRelatorio=" + idRelatorio, function (data) {

            $('#periodoCompara').html(formataDataJson(data.dataInicial) + ' à ' + formataDataJson(data.dataFinal))
            $('#nomeRelatorio').html(data.nomeRelatorio);

        });
    }

    function _carregaPeriodoRelatorio(idRelatorio) {
        _carregaDadosRelatorio(idRelatorio);
    }

    /*
        Presença Online e Noticias    
    */
    function _metricasPresencaNoticia(idRelatorio) {
        $.ajax({
            type: "GET",
            url: "/api/RelatoriosAPI/RetornaMetricasPrersencaOnlineNoticia?idRelatorio=" + idRelatorio,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.metricasPresencaNoticia.length > 0 && data.metricasRelatorioPresencaNoticia.length > 0) {
                    $(".ibox-presenca-noticia").show();
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_rankbrasil") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Rank Brasil", "presenca_rankbrasil"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_rankglobal") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Rank lobal", "presenca_rankglobal"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_cres_rankbrasil") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Cresc. rank Brasil", "presenca_cres_rankbrasil"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_cres_rankglobal") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Cresc. rank global", "presenca_cres_rankglobal"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_visitaspesquisa") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Visitas pesquisa", "presenca_visitaspesquisa"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_taxarejeicao") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Taxa rejeição", "presenca_taxarejeicao"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_paginasvisitadas") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Páginas visitadas", "presenca_paginasvisitadas"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("presenca_tempovisitas") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Tempo visitas", "presenca_tempovisitas"));
                }
                if (data.metricasRelatorioPresencaNoticia.indexOf("noticias_total") > -1) {
                    $(".titulos-metricas-presenca-noticia").append(_elementoTrPresencaNoticia("Total notícias", "noticias_total"));
                }

                _medias["presenca_rankbrasil"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_rankglobal"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_cres_rankbrasil"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_cres_rankglobal"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_visitaspesquisa"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_taxarejeicao"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_paginasvisitadas"] = { "soma": 0, "linhas": 0 };
                _medias["presenca_tempovisitas"] = { "soma": 0, "linhas": 0 };
                _medias["noticias_total"] = { "soma": 0, "linhas": 0 };

                $.each(data.metricasPresencaNoticia, function (key, element) {

                    var linha = "<tr>";
                    linha += _elementoTdPresencaNoticia(element.nomeEmpresa);

                    $('th[data-presenca]').each(function () {

                        if ($(this).attr('data-presenca') == "presenca_rankbrasil") {
                            if (element.rankbrasilAtual == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_rankbrasil");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.rankbrasilAtual), "presenca_rankbrasil");

                                _medias["presenca_rankbrasil"].soma += element.rankbrasilAtual;
                                _medias["presenca_rankbrasil"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_rankglobal") {
                            if (element.rankglobalAtual == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_rankglobal");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.rankglobalAtual), "presenca_rankglobal");

                                _medias["presenca_rankglobal"].soma += element.rankglobalAtual;
                                _medias["presenca_rankglobal"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_cres_rankbrasil") {
                            if (element.crescimentoRankbrasil == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_cres_rankbrasil");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.crescimentoRankbrasil), "presenca_cres_rankbrasil");

                                _medias["presenca_cres_rankbrasil"].soma += element.crescimentoRankbrasil;
                                _medias["presenca_cres_rankbrasil"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_cres_rankglobal") {
                            if (element.crescimentoRankglobal == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_cres_rankglobal");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.crescimentoRankglobal), "presenca_cres_rankglobal");

                                _medias["presenca_cres_rankglobal"].soma += element.crescimentoRankglobal;
                                _medias["presenca_cres_rankglobal"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_visitaspesquisa") {
                            if (element.visitasPesquisa == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_visitaspesquisa");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.visitasPesquisa), "presenca_visitaspesquisa");

                                _medias["presenca_visitaspesquisa"].soma += element.visitasPesquisa;
                                _medias["presenca_visitaspesquisa"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_taxarejeicao") {
                            if (element.taxaRejeicao == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_taxarejeicao");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.taxaRejeicao), "presenca_taxarejeicao");

                                _medias["presenca_taxarejeicao"].soma += element.taxaRejeicao;
                                _medias["presenca_taxarejeicao"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_paginasvisitadas") {
                            if (element.paginasVisitadas == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_paginasvisitadas");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.paginasVisitadas), "presenca_paginasvisitadas");

                                _medias["presenca_paginasvisitadas"].soma += element.paginasVisitadas;
                                _medias["presenca_paginasvisitadas"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "presenca_tempovisitas") {
                            if (element.tempoVisitas == null) {
                                linha += _elementoTdPresencaNoticia("-", "presenca_tempovisitas");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.tempoVisitas), "presenca_tempovisitas");

                                _medias["presenca_tempovisitas"].soma += element.tempoVisitas;
                                _medias["presenca_tempovisitas"].linhas += 1;
                            }
                        }

                        if ($(this).attr('data-presenca') == "noticias_total") {
                            if (element.totalNoticias == null) {
                                linha += _elementoTdPresencaNoticia("-", "noticias_total");
                            }
                            else {
                                linha += _elementoTdPresencaNoticia(separadorMilhares(element.totalNoticias), "noticias_total");

                                _medias["noticias_total"].soma += element.totalNoticias;
                                _medias["noticias_total"].linhas += 1;
                            }
                        }

                    });
                    linha += "</tr>";
                    $(".tabela-metricas-presenca-noticia").append(linha);

                });

                _mediaGeralMetricasPresencaNoticia();
            }
        });
    }

    function _elementoTrPresencaNoticia(item, valor) {
        return "<th data-presenca='" + valor + "' data-toggle='true' class='footable-visible footable-sortable footable-first-column'>" + item + "<span class='footable-sort-indicator'></span></th>"
    }

    function _elementoTdPresencaNoticia(item, valor) {
        return "<td data-metrica-presenca='" + valor + "'>" + item + "</td>"
    }

    function _mediaGeralMetricasPresencaNoticia() {

        var linha = "<tr>";
        linha += _elementoTdPresencaNoticia("Média geral");

        $('th[data-presenca]').each(function () {

            if ($(this).attr('data-presenca') == "presenca_rankbrasil") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_rankbrasil");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_rankbrasil");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_rankglobal") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_rankglobal");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_rankglobal");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_cres_rankbrasil") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_cres_rankbrasil");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_cres_rankbrasil");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_cres_rankglobal") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_cres_rankglobal");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_cres_rankglobal");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_visitaspesquisa") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_visitaspesquisa");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_visitaspesquisa");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_taxarejeicao") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_taxarejeicao");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_taxarejeicao");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_paginasvisitadas") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "presenca_paginasvisitadas");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_paginasvisitadas");
                }
            }

            if ($(this).attr('data-presenca') == "presenca_tempovisitas") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    
                    var arrTotais = [];
                    $('.tabela-metricas-presenca-noticia').find('[data-metrica-presenca="presenca_tempovisitas"]').each(function () {                        
                        arrTotais.push($(this).text());
                    });

                    linha += _elementoTdPresencaNoticia(MediasHoras(arrTotais), "presenca_tempovisitas");

                } else {
                    linha += _elementoTdPresencaNoticia("-", "presenca_tempovisitas");
                }
            }

            if ($(this).attr('data-presenca') == "noticias_total") {

                if (!!_medias[$(this).attr('data-presenca')].linhas) {
                    var media = _medias[$(this).attr('data-presenca')].soma / _medias[$(this).attr('data-presenca')].linhas;

                    linha += _elementoTdPresencaNoticia(separadorMilhares(media.toFixed(1).toString()), "noticias_total");
                } else {
                    linha += _elementoTdPresencaNoticia("-", "noticias_total");
                }
            }

        });

        linha += "</tr>";
        $(".tabela-media-geral-presenca-noticia").append(linha);
    }

    /*
        Presença Online e Noticias
    */


    /*
       Facebook    
   */

    function _metricasFacebook(idRelatorio) {

        $.ajax({
            type: "GET",
            url: "/api/RelatoriosAPI/RetornaMetricasFacebook?idRelatorio=" + idRelatorio,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.metricasFacebook.length > 0 && data.metricasRelatorioFacebook.length > 0) {
                    $(".ibox-facebook").show();
                }

                //$.each(data.metricasRelatorioFacebook, function (key, element) {
                //    if (element == "facebook_total_fas") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Total de fãs","facebook_total_fas"));
                //    }
                //    if (element == "facebook_crescimento_fas") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Crescimento de fãs", "facebook_crescimento_fas"));
                //    }
                //    if (element == "facebook_qtd_postagens") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Quantidade de postagens", "facebook_qtd_postagens"));
                //    }
                //    if (element == "facebook_curtidas_posts") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Total de reações", "facebook_curtidas_posts"));
                //    }
                //    if (element == "facebook_comentarios") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Comentários", "facebook_comentarios"));
                //    }
                //    if (element == "facebook_compartilhamentos") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Compartilhamentos", "facebook_compartilhamentos"));
                //    }
                //    if (element == "facebook_total_interacoes") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Total de interações","facebook_total_interacoes"));
                //    }
                //    if (element == "facebook_engajamento_medio") {
                //        $(".titulos-metricas-facebook").append(_elementoTrFacebook("Engajamento médio","facebook_engajamento_medio"));
                //    }
                //});

                //$.each(data.metricasRelatorioFacebook, function (key, element) {
                if (data.metricasRelatorioFacebook.indexOf("facebook_total_fas") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Fãs", "facebook_total_fas"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_crescimento_fas") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Crescimento de fãs", "facebook_crescimento_fas"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_qtd_postagens") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Postagens", "facebook_qtd_postagens"));
                }

                if (data.metricasRelatorioFacebook.indexOf("facebook_curtidas_posts") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Reações", "facebook_curtidas_posts"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_comentarios") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Comentários", "facebook_comentarios"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_compartilhamentos") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Compartilhamentos", "facebook_compartilhamentos"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_total_interacoes") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Interações", "facebook_total_interacoes"));
                }
                if (data.metricasRelatorioFacebook.indexOf("facebook_engajamento_medio") > -1) {
                    $(".titulos-metricas-facebook").append(_elementoTrFacebook("Engajamento médio", "facebook_engajamento_medio"));
                }
                //});

                _medias["facebook_total_fas"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_total_interacoes"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_crescimento_fas"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_comentarios"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_qtd_postagens"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_curtidas_posts"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_compartilhamentos"] = { "soma": 0, "linhas": 0 };
                _medias["facebook_engajamento_medio"] = { "soma": 0, "linhas": 0 };

                $.each(data.metricasFacebook, function (key, element) {

                    var linha = "<tr>";
                    linha += _elementoTdFacebook(element.nome);

                    $('th[data-face]').each(function () {

                        if ($(this).attr('data-face') == "facebook_total_fas") {
                            if (element.qtdLikesAtual == null) {
                                linha += _elementoTdFacebook("-", "facebook_total_fas");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.qtdLikesAtual), "facebook_total_fas");

                                _medias["facebook_total_fas"].soma += element.qtdLikesAtual;
                                _medias["facebook_total_fas"].linhas += 1;
                            }
                        }
                        if ($(this).attr('data-face') == "facebook_crescimento_fas") {
                            if (element.crescimentoFas == null) {
                                linha += _elementoTdFacebook("-", "facebook_crescimento_fas");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.crescimentoFas), "facebook_crescimento_fas");

                                _medias["facebook_crescimento_fas"].soma += element.crescimentoFas;
                                _medias["facebook_crescimento_fas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-face') == "facebook_total_interacoes") {
                            if (element.interacoes == null) {
                                linha += _elementoTdFacebook("-", "facebook_total_interacoes");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(((element.interacoes)).toString()), "facebook_total_interacoes");

                                _medias["facebook_total_interacoes"].soma += element.interacoes;
                                _medias["facebook_total_interacoes"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-face') == "facebook_comentarios") {
                            if (element.comentarios == null) {
                                linha += _elementoTdFacebook("-", "facebook_comentarios");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.comentarios), "facebook_comentarios");

                                _medias["facebook_comentarios"].soma += element.comentarios;
                                _medias["facebook_comentarios"].linhas += 1;
                            }
                        }
                        if ($(this).attr('data-face') == "facebook_qtd_postagens") {
                            if (element.posts == null) {
                                linha += _elementoTdFacebook("-", "facebook_qtd_postagens");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.posts), "facebook_qtd_postagens");

                                _medias["facebook_qtd_postagens"].soma += element.posts;
                                _medias["facebook_qtd_postagens"].linhas += 1;
                            }
                        }
                        if ($(this).attr('data-face') == "facebook_curtidas_posts") {
                            if (element.reacoes == null) {
                                linha += _elementoTdFacebook("-", "facebook_curtidas_posts");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.reacoes), "facebook_curtidas_posts");

                                _medias["facebook_curtidas_posts"].soma += element.reacoes;
                                _medias["facebook_curtidas_posts"].linhas += 1;
                            }
                        }
                        if ($(this).attr('data-face') == "facebook_compartilhamentos") {
                            if (element.compartilhamentos == null) {
                                linha += _elementoTdFacebook("-", "facebook_compartilhamentos");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.compartilhamentos), "facebook_compartilhamentos");

                                _medias["facebook_compartilhamentos"].soma += element.compartilhamentos;
                                _medias["facebook_compartilhamentos"].linhas += 1;
                            }
                        }
                        if ($(this).attr('data-face') == "facebook_engajamento_medio") {
                            if (element.engajamento == null) {
                                linha += _elementoTdFacebook("-", "facebook_engajamento_medio");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(((element.engajamento).toFixed(2)).toString()) + "%", "facebook_engajamento_medio");

                                _medias["facebook_engajamento_medio"].soma += element.engajamento;
                                _medias["facebook_engajamento_medio"].linhas += 1;
                            }

                        }

                    });
                    linha += "</tr>";
                    $(".tabela-metricas-facebook").append(linha);

                });

                _mediaGeralMetricasFacebook();



                // Verifica em cada coluna se o valor está menor ou maior que a média
                setTimeout(function () {
                    if (data.metricasRelatorioFacebook.indexOf("facebook_total_fas") > -1) {
                        var media = $('.tabela-media-geral-facebook > tr > td[data-metrica-face="facebook_total_fas"]').text();
                        //media = separadorMilhares(media);

                        console.log(media + '<<<<<<');

                        $('.tabela-metricas-facebook > tr > td[data-metrica-face="facebook_total_fas"]').each(function (key, element) {
                            var valorTD = $(this).text();

                            console.log($(this).text() + '============');

                            if (valorTD.toString() > media.toString()) {
                                $(this).addClass('valorMaiorMedia');
                            } else {
                                $(this).addClass('valorMenorMedia');
                            }
                        });
                    }
                }, 500);
               
            }
        });


    }

    function _elementoTrFacebook(item, valor) {
        return "<th data-face='" + valor + "' data-toggle='true' class='footable-visible footable-sortable footable-first-column'>" + item + "<span class='footable-sort-indicator'></span></th>"
    }

    function _elementoTdFacebook(item, valor) {
        return "<td data-metrica-face='" + valor + "'>" + item + "</td>"
    }

    function _mediaGeralMetricasFacebook() {

        var linha = "<tr>";
        linha += _elementoTdFacebook("Média geral");

        $('th[data-face]').each(function () {

            if ($(this).attr('data-face') == "facebook_total_fas") {

                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_total_fas");
                    //linha += _elementoTdFacebook(separadorMilhares(media), "facebook_total_fas");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_total_fas");
                }

            }

            if ($(this).attr('data-face') == "facebook_total_interacoes") {

                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_total_interacoes");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_total_interacoes");
                }

            }

            if ($(this).attr('data-face') == "facebook_crescimento_fas") {

                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_crescimento_fas");
                }

            }

            if ($(this).attr('data-face') == "facebook_comentarios") {
                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_comentarios");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_comentarios");
                }
            }

            if ($(this).attr('data-face') == "facebook_qtd_postagens") {
                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_qtd_postagens");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_qtd_postagens");
                }

            }

            if ($(this).attr('data-face') == "facebook_curtidas_posts") {
                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_curtidas_posts");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_curtidas_posts");
                }

            }

            if ($(this).attr('data-face') == "facebook_compartilhamentos") {

                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(1).toString()), "facebook_compartilhamentos");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_compartilhamentos");
                }

            }

            if ($(this).attr('data-face') == "facebook_engajamento_medio") {

                if (!!_medias[$(this).attr('data-face')].linhas) {
                    var media = _medias[$(this).attr('data-face')].soma / _medias[$(this).attr('data-face')].linhas;

                    linha += _elementoTdFacebook(separadorMilhares(media.toFixed(2).toString()) + "%", "facebook_engajamento_medio");
                } else {
                    linha += _elementoTdFacebook("-", "facebook_engajamento_medio");
                }

            }

        });

        linha += "</tr>";
        $(".tabela-media-geral-facebook").append(linha);
    }

    /*
      end  Facebook    
    */


    /*
         Twitter    
    */

    function _metricasTwitter(idRelatorio) {

        $.ajax({
            type: "GET",
            url: "/api/RelatoriosAPI/RetornaMetricasTwitter?idRelatorio=" + idRelatorio,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.metricasTwitter.length > 0 && data.metricasRelatorioTwitter.length > 0) {
                    $(".ibox-twitter").show();
                }

                //$.each(data.metricasRelatorioTwitter, function (key, element) {
                if (data.metricasRelatorioTwitter.indexOf("twitter_total_fas") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Seguidores", "twitter_total_fas"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_crescimento_fas") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Crescimento de seguidores", "twitter_crescimento_fas"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_posts") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Quantidade de tweets", "twitter_posts"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_curtidas") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Curtidas", "twitter_curtidas"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_retweets") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Retweets", "twitter_retweets"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_interacoes") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Interações", "twitter_interacoes"));
                }
                if (data.metricasRelatorioTwitter.indexOf("twitter_engajamento") > -1) {
                    $(".titulos-metricas-twitter").append(_elementoTrTwitter("Engajamento médio", "twitter_engajamento"));
                }

                //});

                _medias["twitter_total_fas"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_crescimento_fas"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_retweets"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_curtidas"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_interacoes"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_engajamento"] = { "soma": 0, "linhas": 0 };
                _medias["twitter_posts"] = { "soma": 0, "linhas": 0 };


                $.each(data.metricasTwitter, function (key, element) {

                    var linha = "<tr>";
                    linha += _elementoTdTwitter(element.nome);

                    $('th[data-tw]').each(function () {
                        if ($(this).attr('data-tw') == "twitter_total_fas") {

                            if (element.qtdLikesAtual == null) {
                                linha += _elementoTdFacebook("-", "twitter_total_fas");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.qtdLikesAtual), "twitter_total_fas");

                                _medias["twitter_total_fas"].soma += element.qtdLikesAtual;
                                _medias["twitter_total_fas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-tw') == "twitter_crescimento_fas") {

                            if (element.crescimentoFas == null) {
                                linha += _elementoTdFacebook("-", "twitter_crescimento_fas");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.crescimentoFas), "twitter_crescimento_fas");

                                _medias["twitter_crescimento_fas"].soma += element.crescimentoFas;
                                _medias["twitter_crescimento_fas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-tw') == "twitter_retweets") {

                            if (element.retweets == null) {
                                linha += _elementoTdFacebook("-", "twitter_retweets");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.retweets), "twitter_retweets");

                                _medias["twitter_retweets"].soma += element.retweets;
                                _medias["twitter_retweets"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-tw') == "twitter_curtidas") {

                            if (element.curtidas == null) {
                                linha += _elementoTdFacebook("-", "twitter_curtidas");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.curtidas), "twitter_curtidas");

                                _medias["twitter_curtidas"].soma += element.curtidas;
                                _medias["twitter_curtidas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-tw') == "twitter_interacoes") {

                            if (element.interacoes == null) {
                                linha += _elementoTdFacebook("-", "twitter_interacoes");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.interacoes), "twitter_interacoes");

                                _medias["twitter_interacoes"].soma += element.interacoes;
                                _medias["twitter_interacoes"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-tw') == "twitter_posts") {

                            if (element.posts == null) {
                                linha += _elementoTdFacebook("-", "twitter_posts");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(element.posts), "twitter_posts");

                                _medias["twitter_posts"].soma += element.posts;
                                _medias["twitter_posts"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-tw') == "twitter_engajamento") {

                            if (element.engajamento == null) {
                                linha += _elementoTdFacebook("-", "twitter_engajamento");
                            }
                            else {
                                linha += _elementoTdFacebook(separadorMilhares(((element.engajamento).toFixed(2)).toString()) + "%", "twitter_engajamento");

                                _medias["twitter_engajamento"].soma += element.engajamento;
                                _medias["twitter_engajamento"].linhas += 1;
                            }
                            //linha += _elementoTdTwitter((!!element.engajamento ? (separadorMilhares(((element.engajamento).toFixed(2)).toString())) : 0) + "%", "twitter_engajamento");
                        }
                    });
                    linha += "</tr>";
                    $(".tabela-metricas-twitter").append(linha);

                });

                _mediaGeralMetricasTwitter();

            }
        });
    }

    function _elementoTrTwitter(item, valor) {
        return "<th data-tw='" + valor + "' data-toggle='true' class='footable-visible footable-sortable footable-first-column'>" + item + "<span class='footable-sort-indicator'></span></th>"
    }

    function _elementoTdTwitter(item, valor) {
        return "<td data-metrica-tw='" + valor + "'>" + item + "</td>"
    }

    function _mediaGeralMetricasTwitter() {

        var linha = "<tr>";
        linha += _elementoTdTwitter("Média geral");

        $('th[data-tw]').each(function () {


            if ($(this).attr('data-tw') == "twitter_total_fas") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_total_fas");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_total_fas");
                }

            }

            if ($(this).attr('data-tw') == "twitter_crescimento_fas") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_crescimento_fas");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_crescimento_fas");
                }

            }

            if ($(this).attr('data-tw') == "twitter_retweets") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_retweets");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_retweets");
                }

            }

            if ($(this).attr('data-tw') == "twitter_curtidas") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_curtidas");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_curtidas");
                }

            }

            if ($(this).attr('data-tw') == "twitter_interacoes") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_interacoes");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_interacoes");
                }

            }

            if ($(this).attr('data-tw') == "twitter_engajamento") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(2).toString()) + "%", "twitter_engajamento");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_engajamento");
                }

            }

            if ($(this).attr('data-tw') == "twitter_posts") {

                if (!!_medias[$(this).attr('data-tw')].linhas) {
                    var media = _medias[$(this).attr('data-tw')].soma / _medias[$(this).attr('data-tw')].linhas;

                    linha += _elementoTdTwitter(separadorMilhares(media.toFixed(1).toString()), "twitter_posts");
                } else {
                    linha += _elementoTdTwitter("-", "twitter_posts");
                }

            }

        });

        linha += "</tr>";
        $(".tabela-media-geral-twitter").append(linha);


    }

    /*
      end  Twitter    
    */


    /*
         Instagram    
    */

    function _metricasInstagram(idRelatorio) {

        $.ajax({
            type: "GET",
            url: "/api/RelatoriosAPI/RetornaMetricasInstagram?idRelatorio=" + idRelatorio,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.metricasInstagram.length > 0 && data.metricasRelatorioInstagram.length > 0) {
                    $(".ibox-instagram").show();
                }

                //$.each(data.metricasRelatorioInstagram, function (key, element) {
                if (data.metricasRelatorioInstagram.indexOf("instagram_total_fas") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Seguidores", "instagram_total_fas"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_crescimento_fas") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Crescimento de seguidores", "instagram_crescimento_fas"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_qtd_posts") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Posts", "instagram_qtd_posts"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_curtidas_posts") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Curtidas", "instagram_curtidas_posts"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_comentarios") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Comentários", "instagram_comentarios"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_total_interacoes") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Interações", "instagram_total_interacoes"));
                }
                if (data.metricasRelatorioInstagram.indexOf("instagram_engajamento_medio") > -1) {
                    $(".titulos-metricas-instagram").append(_elementoTrInstagram("Engajamento médio", "instagram_engajamento_medio"));
                }
                //});

                _medias["instagram_total_fas"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_crescimento_fas"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_curtidas_posts"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_comentarios"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_qtd_posts"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_total_interacoes"] = { "soma": 0, "linhas": 0 };
                _medias["instagram_engajamento_medio"] = { "soma": 0, "linhas": 0 };


                $.each(data.metricasInstagram, function (key, element) {

                    var linha = "<tr>";
                    linha += _elementoTdInstagram(element.nome);

                    $('th[data-insta]').each(function () {
                        if ($(this).attr('data-insta') == "instagram_total_fas") {

                            if (element.qtdSeguidoresAtual == null) {
                                linha += _elementoTdInstagram("-", "instagram_total_fas");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.qtdSeguidoresAtual), "instagram_total_fas");

                                _medias["instagram_total_fas"].soma += element.qtdSeguidoresAtual;
                                _medias["instagram_total_fas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_crescimento_fas") {

                            if (element.crescimentoFas == null) {
                                linha += _elementoTdInstagram("-", "instagram_crescimento_fas");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.crescimentoFas), "instagram_crescimento_fas");

                                _medias["instagram_crescimento_fas"].soma += element.crescimentoFas;
                                _medias["instagram_crescimento_fas"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_curtidas_posts") {

                            if (element.curtidas == null) {
                                linha += _elementoTdInstagram("-", "instagram_curtidas_posts");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.curtidas), "instagram_curtidas_posts");

                                _medias["instagram_curtidas_posts"].soma += element.curtidas;
                                _medias["instagram_curtidas_posts"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_comentarios") {

                            if (element.comentarios == null) {
                                linha += _elementoTdInstagram("-", "instagram_comentarios");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.comentarios), "instagram_comentarios");

                                _medias["instagram_comentarios"].soma += element.comentarios;
                                _medias["instagram_comentarios"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_qtd_posts") {

                            if (element.posts == null) {
                                linha += _elementoTdInstagram("-", "instagram_qtd_posts");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.posts), "instagram_qtd_posts");

                                _medias["instagram_qtd_posts"].soma += element.posts;
                                _medias["instagram_qtd_posts"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_total_interacoes") {

                            if (element.interacoes == null) {
                                linha += _elementoTdInstagram("-", "instagram_total_interacoes");
                            }
                            else {
                                linha += _elementoTdInstagram(separadorMilhares(element.interacoes), "instagram_total_interacoes");

                                _medias["instagram_total_interacoes"].soma += element.interacoes;
                                _medias["instagram_total_interacoes"].linhas += 1;
                            }

                        }
                        if ($(this).attr('data-insta') == "instagram_engajamento_medio") {

                            if (element.engajamento == null) {
                                linha += _elementoTdInstagram("-", "instagram_engajamento_medio");
                            }
                            else {
                                //linha += _elementoTdInstagram(separadorMilhares(element.engajamento) + "%", "instagram_engajamento_medio");
                                linha += _elementoTdInstagram(separadorMilhares(((element.engajamento).toFixed(2)).toString()) + "%", "instagram_engajamento_medio");

                                _medias["instagram_engajamento_medio"].soma += element.engajamento;
                                _medias["instagram_engajamento_medio"].linhas += 1;
                            }

                        }
                    });

                    linha += "</tr>";
                    $(".tabela-metricas-instagram").append(linha);

                });

                _mediaGeralMetricasInstagram();

            }
        });

    }

    function _elementoTrInstagram(item, valor) {
        return "<th data-insta='" + valor + "' data-toggle='true' class='footable-visible footable-sortable footable-first-column'>" + item + "<span class='footable-sort-indicator'></span></th>"
    }

    function _elementoTdInstagram(item, valor) {
        return "<td data-metrica-insta='" + valor + "'>" + item + "</td>"
    }

    function _mediaGeralMetricasInstagram() {

        var linha = "<tr>";
        linha += _elementoTdInstagram("Média geral");

        $('th[data-insta]').each(function () {

            if ($(this).attr('data-insta') == "instagram_total_fas") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_total_fas");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_total_fas");
                }

            }
            if ($(this).attr('data-insta') == "instagram_crescimento_fas") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_crescimento_fas");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_total_fas");
                }

            }
            if ($(this).attr('data-insta') == "instagram_curtidas_posts") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_curtidas_posts");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_curtidas_posts");
                }

            }
            if ($(this).attr('data-insta') == "instagram_comentarios") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_comentarios");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_comentarios");
                }

            }
            if ($(this).attr('data-insta') == "instagram_qtd_posts") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_qtd_posts");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_qtd_posts");
                }

            }
            if ($(this).attr('data-insta') == "instagram_total_interacoes") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(1).toString()), "instagram_total_interacoes");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_total_interacoes");
                }

            }
            if ($(this).attr('data-insta') == "instagram_engajamento_medio") {

                if (!!_medias[$(this).attr('data-insta')].linhas) {
                    var media = _medias[$(this).attr('data-insta')].soma / _medias[$(this).attr('data-insta')].linhas;

                    linha += _elementoTdInstagram(separadorMilhares(media.toFixed(2).toString()) + "%", "instagram_engajamento_medio");
                } else {
                    linha += _elementoTdInstagram("-", "instagram_engajamento_medio");
                }

            }

        });

        linha += "</tr>";
        $(".tabela-media-geral-instagram").append(linha);

    }

    /*
      end  Instagram    
    */


    /*
        youtube    
    */

    function _metricasYoutube(idRelatorio) {

        $.ajax({
            type: "GET",
            url: "/api/RelatoriosAPI/RetornaMetricasYoutube?idRelatorio=" + idRelatorio,
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data.metricasYoutube.length > 0 && data.metricasRelatorioYoutube.length > 0) {
                    $(".ibox-youtube").show();
                }

                //$.each(data.metricasRelatorioYoutube, function (key, element) {
                if (data.metricasRelatorioYoutube.indexOf("youtube_total_inscritos") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Inscritos", "youtube_total_inscritos"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_crescimento_inscritos") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Crescimento de inscritos", "youtube_crescimento_inscritos"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_qtd_videos") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Vídeos", "youtube_qtd_videos"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_visualizacoes") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Visualizações", "youtube_visualizacoes"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_curtidas") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Curtidas", "youtube_curtidas"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_descurtidas") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Descurtidas", "youtube_descurtidas"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_comentarios") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Comentários", "youtube_comentarios"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_total_interacoes") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Interações", "youtube_total_interacoes"));
                }
                if (data.metricasRelatorioYoutube.indexOf("youtube_engajamento_medio") > -1) {
                    $(".titulos-metricas-youtube").append(_elementoTrYoutube("Engajamento médio", "youtube_engajamento_medio"));
                }
                //});

                _medias["youtube_total_inscritos"] = { "soma": 0, "linhas": 0 };;
                _medias["youtube_crescimento_inscritos"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_qtd_videos"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_total_interacoes"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_visualizacoes"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_curtidas"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_descurtidas"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_comentarios"] = { "soma": 0, "linhas": 0 };
                _medias["youtube_engajamento_medio"] = { "soma": 0, "linhas": 0 };


                $.each(data.metricasYoutube, function (key, element) {

                    var linha = "<tr>";
                    linha += _elementoTdYoutube(element.nome);

                    $('th[data-youtube]').each(function () {

                        if ($(this).attr('data-youtube') == "youtube_total_inscritos") {

                            if (element.qtdSeguidoresAtual == null) {
                                linha += _elementoTdYoutube("-", "youtube_total_inscritos");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.qtdSeguidoresAtual), "youtube_total_inscritos");

                                _medias["youtube_total_inscritos"].soma += element.qtdSeguidoresAtual;
                                _medias["youtube_total_inscritos"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_crescimento_inscritos") {

                            if (element.crescimentoinscritos == null) {
                                linha += _elementoTdYoutube("-", "youtube_crescimento_inscritos");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.crescimentoinscritos), "youtube_crescimento_inscritos");

                                _medias["youtube_crescimento_inscritos"].soma += element.crescimentoinscritos;
                                _medias["youtube_crescimento_inscritos"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_qtd_videos") {

                            if (element.posts == null) {
                                linha += _elementoTdYoutube("-", "youtube_qtd_videos");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.posts), "youtube_qtd_videos");

                                _medias["youtube_qtd_videos"].soma += element.posts;
                                _medias["youtube_qtd_videos"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_total_interacoes") {

                            if (element.interacoes == null) {
                                linha += _elementoTdYoutube("-", "youtube_total_interacoes");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.interacoes), "youtube_total_interacoes");

                                _medias["youtube_total_interacoes"].soma += element.interacoes;
                                _medias["youtube_total_interacoes"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_visualizacoes") {

                            if (element.visualizacoes == null) {
                                linha += _elementoTdYoutube("-", "youtube_visualizacoes");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.visualizacoes), "youtube_visualizacoes");

                                _medias["youtube_visualizacoes"].soma += element.visualizacoes;
                                _medias["youtube_visualizacoes"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_curtidas") {

                            if (element.curtidas == null) {
                                linha += _elementoTdYoutube("-", "youtube_curtidas");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.curtidas), "youtube_curtidas");

                                _medias["youtube_curtidas"].soma += element.curtidas;
                                _medias["youtube_curtidas"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_descurtidas") {

                            if (element.descurtidas == null) {
                                linha += _elementoTdYoutube("-", "youtube_descurtidas");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.descurtidas), "youtube_descurtidas");

                                _medias["youtube_descurtidas"].soma += element.descurtidas;
                                _medias["youtube_descurtidas"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_comentarios") {

                            if (element.comentarios == null) {
                                linha += _elementoTdYoutube("-", "youtube_comentarios");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(element.comentarios), "youtube_comentarios");

                                _medias["youtube_comentarios"].soma += element.comentarios;
                                _medias["youtube_comentarios"].linhas += 1;
                            }

                        }

                        if ($(this).attr('data-youtube') == "youtube_engajamento_medio") {

                            if (element.engajamento == null) {
                                linha += _elementoTdYoutube("-", "youtube_engajamento_medio");
                            }
                            else {
                                linha += _elementoTdYoutube(separadorMilhares(((element.engajamento).toFixed(2)).toString()) + "%", "youtube_engajamento_medio");

                                _medias["youtube_engajamento_medio"].soma += element.engajamento;
                                _medias["youtube_engajamento_medio"].linhas += 1;
                            }

                        }

                    });
                    linha += "</tr>";
                    $(".tabela-metricas-youtube").append(linha);

                });

                _mediaGeralMetricasYoutube();

            }
        });


    }

    function _elementoTrYoutube(item, valor) {
        return "<th data-youtube='" + valor + "' data-toggle='true' class='footable-visible footable-sortable footable-first-column'>" + item + "<span class='footable-sort-indicator'></span></th>"
    }

    function _elementoTdYoutube(item, valor) {
        return "<td data-metrica-youtube='" + valor + "'>" + item + "</td>"
    }

    function _mediaGeralMetricasYoutube() {


        var linha = "<tr>";
        linha += _elementoTdYoutube("Média geral");

        $('th[data-youtube]').each(function () {

            if ($(this).attr('data-youtube') == "youtube_total_inscritos") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_total_inscritos");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_total_inscritos");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_crescimento_inscritos") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_crescimento_inscritos");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_crescimento_inscritos");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_qtd_videos") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_qtd_videos");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_qtd_videos");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_total_interacoes") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_total_interacoes");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_total_interacoes");
                }

            }


            if ($(this).attr('data-youtube') == "youtube_visualizacoes") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_visualizacoes");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_visualizacoes");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_curtidas") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_curtidas");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_curtidas");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_descurtidas") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_descurtidas");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_descurtidas");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_comentarios") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(1).toString()), "youtube_comentarios");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_comentarios");
                }

            }

            if ($(this).attr('data-youtube') == "youtube_engajamento_medio") {

                if (!!_medias[$(this).attr('data-youtube')].linhas) {
                    var media = _medias[$(this).attr('data-youtube')].soma / _medias[$(this).attr('data-youtube')].linhas;

                    linha += _elementoTdYoutube(separadorMilhares(media.toFixed(2).toString()) + "%", "youtube_engajamento_medio");
                } else {
                    linha += _elementoTdYoutube("-", "youtube_engajamento_medio");
                }

            }

        });

        linha += "</tr>";
        $(".tabela-media-geral-youtube").append(linha);



    }

    /*
      end  youtube    
    */

}