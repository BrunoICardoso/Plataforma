
function Configuracoes() {

    // SETs
    this.ListarSetores = _ListarSetores;
    this.ListarEmpresasNaoSelecionada = _listarEmpresasNaoSelecionada;
    this.ListarEmpresasAssociadas = _listarEmpresasSelecionada;
    this.AssociarEmpresa = _associarEmpresa;
    this.DesassociarEmpresa = _desassociarEmpresa;
    this.DesassociarEmpresaSelecionada = _desassociarEmpresaSelecionada;
    this.botaoTodasEmpresas = _botaoTodasEmpresas;
    this.botaoTodasEmpresasSelecionadas = _botaoTodasEmpresasSelecionadas;
    this.Pesquisar = _pesquisa;

    // Métodos
    function _ListarSetores() {
        var cmbHtml = $('#cmbSetores');

        var URL = "/api/ClienteAPI/GetSetores/";

        $.getJSON(URL, function (data) {

            cmbHtml.append('<option value="0"> selecione um setor </option>');

            if (data.length > 0) {
                $.each(data, function (index, obj) {
                    cmbHtml.append('<option value="' + obj.id + '">' + obj.nome + '</option>');
                })
            }
        });
    }

    function _listarEmpresasNaoSelecionada() {

        var divHtml = $('#listaEmpresasParaSelecionar > .row');        
        var URL = "/api/ClienteAPI/GetTodasEmpresas/?paginaAtual=" + paginaAtual + '&qtdeRegistros=' + totalPorPagina + '&idCliente=' + idCliente + '&idSetor=' + (filtros.idSetor != 0 && filtros.idSetor != null ? filtros.idSetor : 0) + '&expressao=' + (filtros.expressao != null && filtros.expressao != "" ? filtros.expressao : " ");

        $('#paginacaoSelecionarEmpresa').html('');

        $.getJSON(URL, function (data) {

            divHtml.html('');

            if (data.length > 0) {
                $.each(data, function (index, obj) {
                    divHtml.append(_templateTodasEmpresas(obj));

                    Paginacao.CriaPaginacao("#paginacaoSelecionarEmpresa", parseInt((totalEmpresasSelecionar / totalPorPagina) + 0.9), totalEmpresasSelecionar, paginaAtual);
                    Paginacao.setMudouPagina(_MudouPaginaRelatorio);

                })
            } else {
                divHtml.append(_empresaNAOEncontrada("Sem empresas para listar."));
            }
        });
        //} else {
            //divHtml.append(_empresaNAOEncontrada("Sem empresas para listar.<br>Não há empresas configuradas."));

        //}

        setTimeout(function () {
            if (_verificaTotalEmpresasRestantes() == false) {
                $('i.checkSelecionaEmpresa').hide();
            }
        }, 100);

    }

    function _templateTodasEmpresas(obj) {

        var classe = obj.clienteAssociado == true ? ' fa-check-square ' : ' fa-square-o ';
        var corFundo = obj.clienteAssociado == true ? '#D9EAD3' : '#FFFFFF';

        return '<div class="col-md-6"> ' +
                            '<div class="boxEmpresaSelecionar" style="background-color:' + corFundo + '" id="empresa_' + obj.idempresa + '"> ' +
                                '<div class="row"> ' +
                                    '<div class="col-md-4"><img src="'+caminhoImagem+'/empresas/'+obj.imagem+'" style="width:100%" /></div> ' +
                                        '<div class="col-md-8"> ' +
                                            '<h4>'+obj.nome+'</h4> ' + 
                                                '<i class="fa ' + classe + ' checkSelecionaEmpresa" id="selecionar-' + idCliente + '-' + obj.idempresa + '" onClick="objConfig.botaoTodasEmpresas('+idCliente + ',' + obj.idempresa + ')" style="font-size: 25px; cursor:pointer;" data-idcliente="' + idCliente + '" data-idempresa="' + obj.idempresa + '" ></i>' +
                                                'Setor: ' + obj.nomeSetor + '<br><br> ' +
                                                (obj.urlsite != null ? '<a href="' + obj.urlsite + '" target="_blank"><i class="fa fa-link"></i>&nbsp;Ver Website</a>' : '') +
                                        '</div> ' +
                                    '</div> ' +
                                '</div> ' +
                            '</div>';
    }

    function _listarEmpresasSelecionada() {

        var divHtml = $('#listaEmpresasSelecionada > .row');
        var URL = "/api/ClienteAPI/GetTodasEmpresasAssociadas/?idCliente=" + idCliente;

        $.getJSON(URL, function (data) {

            divHtml.html('');

            if (data.length > 0) {

                var valorRestante = parseInt(totalEmpresasPermitido) - parseInt(data.length);
                $('#qtdeRestante').html(valorRestante);

                $.each(data, function (index, obj) {
                    divHtml.append(_templateTodasEmpresasAssocidas(obj));                    
                });             
            } else {
                $('#qtdeRestante').html(totalEmpresasPermitido);
                divHtml.append(_empresaNAOEncontrada("Sem empresas associas."));
            }
        });

        setTimeout(function () {
            if (_verificaTotalEmpresasRestantes() == false) {
                $('i.checkSelecionaEmpresa').hide();
            }
        },100);
        
    }

    function _templateTodasEmpresasAssocidas(obj) {
               
        var classe = obj.clienteAssociado == true ? ' fa-check-square ' : ' fa-square-o ';
        var corFundo = obj.clienteAssociado == true ? '#D9EAD3' : '#FFFFFF';

        return '<div class="col-md-12"> ' +
                            '<div class="boxEmpresaAssociada" style="background-color:' + corFundo + '" id="empresa_' + obj.idempresa + '"> ' +
                                '<div class="row"> ' +
                                    '<div class="col-md-4"><img src="' + caminhoImagem + '/empresas/' + obj.imagem + '" style="width:100%" /></div> ' +
                                        '<div class="col-md-8"> ' +
                                            '<h4>' + obj.nome + '</h4> ' +
                                                '<i class="fa ' + classe + ' checkSelecionadaEmpresa" id="selecionada-' + idCliente + '-' + obj.idempresa + '" onClick="objConfig.botaoTodasEmpresasSelecionadas(' + idCliente + ',' + obj.idempresa + ')" style="font-size: 25px; cursor:pointer;" data-idcliente="' + idCliente + '" data-idempresa="' + obj.idempresa + '" ></i>' +
                                                'Setor: ' + obj.nomeSetor + '<br><br> ' +
                                                (obj.urlsite != null ? '<a href="' + obj.urlsite + '" target="_blank"><i class="fa fa-link"></i>&nbsp;Ver Website</a>' : '') +
                                        '</div> ' +
                                    '</div> ' +
                                '</div> ' +
                            '</div>';
    }

    function _associarEmpresa(idCliente, idEmpresa) {
        //console.log('Associa: ' + idCliente + '+++++' + idEmpresa);

        if (_verificaTotalEmpresasRestantes()) {
            var URL = "/api/ClienteAPI/AssociarEmpresaCliente/?idCliente=" + idCliente + '&idEmpresa=' + idEmpresa;

            $.getJSON(URL, function (data) {
                if (data == true) {
                    $('#empresa_' + idEmpresa).css('background-color', '#D9EAD3');

                    _listarEmpresasSelecionada();

                }
            });
        } else {
            $('i.checkSelecionaEmpresa').hide();
        }
    }

    function _desassociarEmpresa(idCliente, idEmpresa) {
        //console.log('Desassocia: ' + idCliente + '+++++' + idEmpresa);

        var valorRestante = parseInt(totalEmpresasPermitido) - 1;
        $('#qtdeRestante').html(valorRestante);

        if (_verificaTotalEmpresasRestantes()) {

            var URL = "/api/ClienteAPI/DesassociarEmpresaCliente/?idCliente=" + idCliente + '&idEmpresa=' + idEmpresa;

            $.getJSON(URL, function (data) {
                if (data == true) {
                    $('#empresa_' + idEmpresa).css('background-color', '#FFFFFF');

                    _listarEmpresasSelecionada();

                }
            });
        } else {
            $('i.checkSelecionaEmpresa').show();
        }
    }

    function _desassociarEmpresaSelecionada(idCliente, idEmpresa) {
        //console.log('DesassociaSelecionada: ' + idCliente + '+++++' + idEmpresa);
        
        var valorRestante = parseInt($('#qtdeRestante').html()) + 1;
        $('#qtdeRestante').html(valorRestante);
        
        if (valorRestante <= totalEmpresasPermitido) {
            $('i.checkSelecionaEmpresa').show();
        }

        var URL = "/api/ClienteAPI/DesassociarEmpresaCliente/?idCliente=" + idCliente + '&idEmpresa=' + idEmpresa;

        $.getJSON(URL, function (data) {
            if (data == true) {
                $('#empresa_' + idEmpresa).css('background-color', '#FFFFFF');
                _listarEmpresasSelecionada();
            }
        });
    }

    function _botaoTodasEmpresasSelecionadas(idCliente, idEmpresa) {

        $('.col-md-12 > #empresa_' +idEmpresa).remove();
        $('.col-md-6 > #empresa_' + idEmpresa).css('background-color', '#FFFFFF');
        $('.col-md-6 > #empresa_' +idEmpresa + ' > div > div.col-md-8 > i').addClass('fa-square-o').removeClass('fa-check-square');

        _desassociarEmpresaSelecionada(idCliente, idEmpresa);
    }

    function _botaoTodasEmpresas(idCliente, idEmpresa) {
        var icone = $('#selecionar-' + idCliente + '-' + idEmpresa);

        if (icone.hasClass('fa-square-o')) {
            $(icone).addClass('fa-check-square').removeClass('fa-square-o');
            _associarEmpresa(idCliente, idEmpresa);
        }
        else {
            icone.addClass('fa-square-o').removeClass('fa-check-square');
            $('.col-md-12 > #empresa_' + idEmpresa).remove();
            _desassociarEmpresa(idCliente, idEmpresa);
        }
    }

    function _pesquisa() {

        $('#paginacaoSelecionarEmpresa').html('');

        filtros.idSetor = $('#cmbSetores').val();
        filtros.expressao = $('#txtPalavrasChave').val();
        filtros.pagina = 1;
        filtros.qtdregistros = totalPorPagina;
        filtros.idCliente = idCliente;

        var divHtml = $('#listaEmpresasParaSelecionar > .row');
        
        $.ajax({
            type: "POST",
            url: "/api/ClienteAPI/Pesquisa/",
            data: JSON.stringify(filtros),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var divHtml = $('#listaEmpresasParaSelecionar > .row');
                divHtml.html('');

                if (data.empresas.length > 0) {

                    totalEmpresasSelecionar = data.total;

                    $.each(data.empresas, function (index, obj) {
                        divHtml.append(_templateTodasEmpresas(obj));

                        Paginacao.CriaPaginacao("#paginacaoSelecionarEmpresa", parseInt((totalEmpresasSelecionar / filtros.qtdregistros) + 0.9), totalEmpresasSelecionar, filtros.pagina);
                        Paginacao.setMudouPagina(_MudouPagina);
                    })
                } else {
                    divHtml.append(_empresaNAOEncontrada("Não foram encontradas empresas para esta pesquisa."))
                }
            }
        });
    }

    function _verificaTotalEmpresasRestantes() {
      
        var totalAssociadas = $('div.boxEmpresaAssociada').length;

        if (totalAssociadas < totalEmpresasPermitido)
            return true;
        else
            return false;
    }

    function _empresaNAOEncontrada(msg) {                
        return '<div style="text-align:center;">' + msg + '</div>'
    }

    function _MudouPaginaRelatorio(pagina) {
        paginaAtual = pagina;
        _listarEmpresasNaoSelecionada();
    }


}