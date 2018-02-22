var Timeline = {

    CarregarTimeline: function (pag, quantidade) {

        var dados = retornaFiltro();
        
        dados.pagina = pag == 0 ? 0 : pag;
        dados.qtdregistros = quantidade;
        
        $('.Carregartimeline').html('');

        $.ajax({
            type: "POST",
            url: "/api/PromocoesAPI/RetornaTimelinePromocoes",
            data: JSON.stringify(dados),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                var timeline = [];
                var informa = "Não informado";

                if (data != null && data.Promocoes != null) {

                    var totalregistros = data.totalRegistros;

                    paginacaoPromoTimeline.CriaPaginacao(".paginacao_promocao", parseInt((totalregistros / 10) + 0.9), totalregistros, pag);
                    paginacaoPromoTimeline.setMudouPagina(mudouPaginaPromocao);

                    $.each(data.Promocoes, function (index, obj) {

                        timeline.push('<div class="ibox-content">');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12" style="min-height:50px">');
                        timeline.push('<label class="font-bold">');

                        timeline.push(obj.nomePromocao != null && obj.nomePromocao != "" ? obj.nomePromocao : informa);

                        timeline.push('</label>');
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12">');
                        timeline.push('<div class="col-md-6">');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Empresas associadas: ');
                        timeline.push('</label>');

                        if (obj.empresas != null && obj.empresas.length > 0) {
                            var empresas = [];
                            $.each(obj.empresas, function (index, empresa) {

                                empresas.push(empresa.nome);

                            });

                            timeline.push(" " + empresas.join(", "));

                        } else {

                            timeline.push(informa);

                        }

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Modalidade da promoção: ');
                        timeline.push('</label>');

                        timeline.push(" " + obj.NomeModalidade != null && obj.NomeModalidade != null ? ' ' + obj.NomeModalidade : ' ' + informa);

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Estados participantes: ');
                        timeline.push('</label>');

                        if (obj.Abrangecia != null && obj.Abrangecia.Estados != null && obj.Abrangecia.Estados.length > 0) {
                            var estados = []
                            $.each(obj.Abrangecia.Estados, function (index, estado) {

                                estados.push(estado.UF);

                            });

                            timeline.push(' ' + estados.join(", "));

                        } else {

                            timeline.push(' ' + informa);

                        }

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Municipios participantes: ');
                        timeline.push('</label>');

                        if (obj.Abrangecia != null && obj.Abrangecia.Municipios != null && obj.Abrangecia.Municipios.length > 0) {

                            var munis = [];

                            $.each(obj.Abrangecia.Municipios, function (index, muni) {

                                munis.push(muni.Nome + '/' + muni.UF);

                            });

                            timeline.push(' ' + munis.join(", "));

                        } else {

                            timeline.push(' ' + informa);

                        }

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="col-md-6">');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Data do cadastro:');
                        timeline.push('</label>');

                        timeline.push(' ' + obj.dtCadastro != null && obj.dtCadastro != "" ? ' ' + formataDataJson(obj.dtCadastro) : ' ' + informa);

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Vigência:');
                        timeline.push('</label>');

                        if (obj.dtvigenciaini != null) {

                            timeline.push(' ' + obj.dtvigenciafim != null ? ' ' + formataDataJson(obj.dtvigenciaini) + ' a ' + formataDataJson(obj.dtvigenciafim) : ' ' + formataDataJson(obj.dtvigenciafim));
                        } else {

                            timeline.push(' ' + informa);

                        }

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Orgão regulador:');
                        timeline.push('</label>');

                        timeline.push(obj.NomeOrgaoRegulador != null && obj.NomeOrgaoRegulador != "" ? ' ' + obj.NomeOrgaoRegulador : ' ' + informa);

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-12 item-lista">');
                        timeline.push('<label class="font-bold">');
                        timeline.push('Número do registro:');
                        timeline.push('</label>');
                        timeline.push('  ' + obj.CertificadoAutorizacao != null && obj.CertificadoAutorizacao != "" ? ' ' + obj.CertificadoAutorizacao : ' ' + informa);
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('<div class="row">');
                        timeline.push('<div class="col-md-2 col-md-push-10">');


                        timeline.push('<p><i class="fa fa-plus-circle" style="color:#1ab394";></i><a href="/Empresa/promodetalhes/?idPromo=' + obj.idPromocao + '&idEmpresa=' + _idEmpresa + '" class="iconesRedeSocial"> Ver detalhes</a></p>');

                        timeline.push('</div>');
                        timeline.push('</div>');
                        timeline.push('</div><br>');



                    });

                    $(".Carregartimeline").append(timeline.join(''));
                }

            }

        });



    }

}

function mudouPaginaPromocao(pagina) {


    Timeline.CarregarTimeline(pagina, quantidadePag);
}