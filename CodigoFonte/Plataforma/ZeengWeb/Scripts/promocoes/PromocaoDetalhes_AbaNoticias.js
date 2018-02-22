
var paginacaoNoti = new Paginacao();
var paginaNoti = 0;
var qtdeRegistrosNoti = 6;

function PromoDetalhesNoticias() {

    this.ListarNoticias = Listar;
    this.TotaNoticias = Total;

    function Listar() {
        var _html = "";
        
        var filtro = {
            idPromocao: idPromocao,
            idEmpresa: idEmpresa,
            qtdeRegistros: qtdeRegistrosNoti,
            pagina: paginaNoti,
            expressao: $('#txtPesquisaNoticia').val()
        }

        $('#listaNoticias').html('');

        $.ajax({
            url: "/api/PromocoesAPI/GetNoticias/",
            type: "POST",
            data: JSON.stringify(filtro),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {

                if (data != null && data.length > 0) {
                    $.each(data, function (index, obj) {                     
                        _html += TemplateBox(obj);

                        _totalNoticias = data.length;

                        paginacaoNoti.CriaPaginacao(".noticia-paginacao", parseInt((_totalNoticias / 6) + 0.9), _totalNoticias, paginaNoti);
                        paginacaoNoti.setMudouPagina(MudouPaginaNoticia);

                    });
                } else {
                    _html = SemNoticias();
                    $(".noticia-paginacao").html('');
                }

                $('#listaNoticias').html(_html);
            }
        });
    }

    function TemplateBox(obj) {
        console.log(obj);

        var html = '';
     
        

        var idnoticiaempresa = 0;        
        if (obj.empresas != null && obj.empresas.length > 0) {
            $.each(obj.empresas, function (index, emp) {
                if (emp.idEmpresa == idEmpresa) {
                    idnoticiaempresa = emp.idNoticiaEmpresa;
                }
            })            
        }

        html = '<div class="col-lg-4">' +
                    '<div class="ibox">' +
                        '<div class="ibox-content" style="height:375px;">' +
                                '<h4 class="titulo_noticia" title="' + obj.titulo + '"><a href="/Empresa/NoticiaDetalhes/?id=' + idnoticiaempresa + '&idPromo=' + idPromocao + '">' + obj.titulo + '</a></h4>' +
                            '<div class="small m-b-xs">' + obj.nomefonte + '  <span class="text-muted datahoranoticia"> ' + obj.datapublicacao.split(' ')[0] + '</span></div>' +
                            '<p class="texto_noticia">' +
                             obj.conteudo +
                            '</p>' +
                            '<div class="row">' +
                                '<div class="col-md-6">' +
                                    '<div class="small text-left">' +
                                        '<span style="font-weight: 600"></span> <span title="' + (obj.autor != null ? obj.autor : "Não informado") + '"> ' + (obj.autor != null ? obj.autor : "Não informado") + '</span>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="col-md-6 text-right">' +
                                    (obj.url != null ? '<i class="fa fa-plus"> </i> <a href="' + (obj.url) + '" target="_blank"> Ver mais</a>' : "") +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>';

        return html;
    }

    function SemNoticias() {
        var html = "";

        html = '<div class="col-lg-12">' +
                  '<div class="ibox">' +
                      '<div class="ibox-content" style="height:75px;">' +
                          'Não há notícias para esta promoção' +
                      '</div>' +
                  '</div>' +
              '</div>';

        return html;
    }

    function Total() {

        var filtro = {
            idPromocao: idPromocao,
            idEmpresa: idEmpresa,
            qtdeRegistros: qtdeRegistrosNoti,
            pagina: paginaNoti,
            expressao: $('#txtPesquisaNoticia').val()
        }

        $.ajax({
            url: "/api/PromocoesAPI/GetTotalNoticias/",
            type: "POST",
            data: JSON.stringify(filtro),
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                _totalNoticias = data;
            }
        });
    }

    function MudouPaginaNoticia(pagina) {
        paginaNoti = pagina;
        Listar();
    }
}
