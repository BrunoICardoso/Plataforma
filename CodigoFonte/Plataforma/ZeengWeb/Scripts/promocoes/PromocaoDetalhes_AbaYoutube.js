
var TotalComentariosLidos;
var TotalComentariosParaLer;
var paginacaoYt = new Paginacao();
var paginaYoutube = 0;
var qtdeRegistrosYoutube = 6;

function PromoDetalhesYoutube() {
    var CaminhoImagem = '/Imagens/youtube';
    //var objData = {};
    var totalPostsTimeLine = 0;

    this.ListarPostsYt = ListarPosts;
    this.TotalPostsYoutube = TotalPostsYoutube;
    this.MaisComentarios = MaisComentarios;

    function ListarPosts() {

        var _html = "";

        var URL = "/api/PromocoesAPI/GetPostsYoutube/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa + "&qtdeRegistros=" + qtdeRegistrosYoutube + "&pagina=" + paginaYoutube;

        $.getJSON(URL, function (data) {

            $('#listaPostsYoutube').html('');

            if (data != null && data.length > 0) {

                $.each(data, function (index, obj) {

                    TotalComentariosLidos = 0;
                    TotalComentariosParaLer = 0;

                    _html += TemplatePosts(obj);

                    paginacaoYt.CriaPaginacao(".youtube-paginacao", parseInt((_totalPostsYoutube / 6) + 0.9), _totalPostsYoutube, paginaYoutube);
                    paginacaoYt.setMudouPagina(MudoupaginaYoutube);
                });
            } else {
                _html = TemplateSemPosts();
            }

            $('#listaPostsYoutube').html(_html);
        });
    };

    function TemplatePosts(obj) {

        //this.objData = obj;
        objDataYoutube = obj;

        TotalComentariosParaLer = obj.comentarios.length;

        var data, hora = "";

        if (obj.datahora != null) {
            data = obj.datahora.split(' ')[0];
            hora = obj.datahora.split(' ')[1];
        }

        var html = '<div class="col-md-12 ibox-content boxRedeSocial" style="margin-bottom: 10px;">' +
                '<div class="col-md-6">' +
                    '<img src="' + this.CaminhoImagem + '/' + obj.nomeimagem + '" width="100%" />' +
                '</div>' +
                '<div class="col-md-6">' +
                    '<label>' + data + ' | ' + hora + '</label>' +
                    '<p>' +
                        obj.postagem +
                    '</p>' +
                    '<div class="btn-group">' +
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-eye"></i> <span>' + obj.visualizacoes + '</span> Visualizações' +
                        '</div>' +                        
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-thumbs-up"></i> <span id="compartilhamentospost">' + obj.curtidas + '</span> curtidas' +
                        '</div>' +
                         '<div class="post-label-interacoes">' +
                            '<i class="fa fa-thumbs-down"></i> <span id="compartilhamentospost">' + obj.descurtidas + '</span> compartilhamentos' +
                        '</div>' +
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-comments"></i> <span id="comentariospost' + obj.idvideo + '">' + obj.qtdcomentarios + '</span> comentários' +
                        '</div>' +
                    '</div>' +
                    '<hr />' +
                    '<div class="divComentarios">' +
                        '<div class="social-footer" style="min-height: 345px;" id="detalhesPostTw' + obj.idvideo + '" data-carregados="5">';

        var _totalComentariosLidos = 0;
        for (var i = TotalComentariosLidos; i <= (parseInt(TotalComentariosLidos) + 4) ; i++) {
            html += TemplatePostsComentarios(obj.comentarios[i]);
            _totalComentariosLidos++;
        }

        TotalComentariosLidos = _totalComentariosLidos;

        html += '</div>' +
                            '</div>' +
                            '<hr style="margin-top: 0px;" /><a onclick="PromoTw.MaisComentarios(' + obj.idvideo + ')" id="linkMaisComentariosTw' + obj.idvideo + '">Mais comentários</a>   <div id="statusComentariosLidosTw' + obj.idvideo + '" style="float: right; display:inline-table;"> <span class="lidos">' + TotalComentariosLidos + ' </span> de <span class="total"> ' + obj.comentarios.length + ' </span> </div>  ' +
                        '</div>' +
                    '</div>';

        $('#statusComentariosLidosTw.lidos').html(TotalComentariosLidos);
        $('#statusComentariosLidosTw.total').html(TotalComentariosParaLer);

        return html;
    };

    function TemplatePostsComentarios(objCom) {

        var html = "";

        if (objCom != null && objCom != 'undefined') {
            var nomeUsuario = objCom.nomeusuario != null && objCom.nomeusuario != "" ? objCom.nomeusuario : "";

            var html = '<div class="social-comment">' +
                            '<div class="media-body">' +
                                '<div class="corusuario">' +
                                    '<strong>' + objCom.nomeusuario + '</strong>' +
                                '</div> ' + objCom.postagem + '<br>' +
                                '<span class="small corusuario"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + objCom.curtidas + ' curtidas</span> - <span class="icon-respostas small corusuario" data-qtdrespostas="0"><i class="fa fa-reply" style="color: gray;"></i> 0 respostas</span> - <small class="text-muted">' + objCom.datahora + '</small>' +
                            '</div>' +

                            TemplatePostsComentariosRespostas(objCom) +

                        '</div>';
        }

        return html;
    };

    function TemplatePostsComentariosRespostas(objCom) {
        var html = "";

        if (objCom.respostas.length > 0) {
            $.each(objCom.respostas, function (index, obj) {
                html += '<div class="social-comment resposta">' +
                            '<div class="media-body">' +
                                '<div class="corusuario"><strong>' + obj.nomeusuario + '</strong></div> ' + obj.postagem + '<br><span class="small text-success"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + obj.curtidas + ' curtidas</span> - <small class="text-muted">' + obj.datahora + '</small>' +
                            '</div>' +
                        '</div>';
            })
        }
        return html;
    };

    function MaisComentarios(idvideo) {

        var totalParaLer = $('#statusComentariosLidosTw' + idvideo + ' > span.total').html().trim();
        var totalLidos = $('#statusComentariosLidosTw' + idvideo + ' > span.lidos').html().trim();

        var html = "";

        var _totalComentariosLidos = totalLidos;

        for (var i = totalLidos; i <= (parseInt(totalLidos) + 4) ; i++) {

            if (objDataYoutube.comentarios[i] != null) {
                html += TemplatePostsComentarios(objDataYoutube.comentarios[i]);
                _totalComentariosLidos++;
            }
        }

        $('#detalhesPostTw' + idvideo).append(html);

        if (_totalComentariosLidos >= totalParaLer) {
            $('#linkMaisComentariosTw' + idvideo).hide();
        }

        $('#statusComentariosLidosTw' + idvideo + ' > span.lidos').html(_totalComentariosLidos);
        $('#statusComentariosLidosTw' + idvideo + ' > span.total').html(totalParaLer);
    }

    function TotalPostsYoutube() {
        var URL = "/api/PromocoesAPI/GetTotalPostsYoutube/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa;
        $.get(URL, function (data) {
            _totalPostsYoutube = data;
        });
    };

    function MudoupaginaYoutube(pagina) {
        paginaYoutube = pagina;
        ListarPosts();
    };

    function TemplateSemPosts() {
        var html = '';
        html = '<div class="col-md-12 ibox-content boxRedeSocial" style="margin-bottom: 10px;"><i class="fa fa-meh-o"></i> Sem posts relacionados para esta Empresa / Promoção</div>';
        return html;
    }
}
