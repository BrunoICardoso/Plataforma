
var TotalComentariosLidosIns;
var TotalComentariosParaLerIns;
var paginacaoIns = new Paginacao();
var paginaInstagram = 0;
var qtdeRegistrosInstagram = 6;

function PromoDetalhesInstagram() {
    var CaminhoImagem = '/Imagens/instagram';
    //var objData = {};
    var totalPostsTimeLine = 0;

    this.ListarPostsIns = ListarPosts;
    this.TotalPostsInstagram = TotalPostsInstagram;
    this.MaisComentarios = MaisComentarios;

    function ListarPosts() {

        var _html = "";

        var URL = "/api/PromocoesAPI/GetPostsInstagram/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa + "&qtdeRegistros=" + qtdeRegistrosInstagram + "&pagina=" + paginaInstagram;

        $.getJSON(URL, function (data) {

            $('#listaPostsInstagram').html('');

            if (data != null && data.length > 0) {

                $.each(data, function (index, obj) {

                    TotalComentariosLidosIns = 0;
                    TotalComentariosParaLerIns = 0;

                    _html += TemplatePosts(obj);

                    paginacaoIns.CriaPaginacao(".instagram-paginacao", parseInt((_totalPostsInstagram / 6) + 0.9), _totalPostsInstagram, paginaInstagram);
                    paginacaoIns.setMudouPagina(MudoupaginaInstagram);
                });
            } else {
                _html = TemplateSemPosts();
            }

            $('#listaPostsInstagram').html(_html);
        });
    };

    function TemplatePosts(obj) {

        //this.objData = obj;
        objDataInstagram = obj;

        TotalComentariosParaLerIns = obj.comentarios.length;

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
                            '<i class="fa fa-thumbs-up"></i> <span id="curtidaspost">' + obj.curtidas + '</span> curtidas' +
                        '</div>' +
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-comments"></i> <span id="comentariospost' + obj.idpost + '">' + obj.qtdcomentarios + '</span> comentários' +
                        '</div>' +
                    '</div>' +
                    '<hr />' +
                    '<div class="divComentarios">' +
                        '<div class="social-footer" style="min-height: 345px;" id="detalhesPostIns' + obj.idpost + '" data-carregados="5">';

        var _totalComentariosLidos = 0;
        for (var i = TotalComentariosLidosIns; i <= (parseInt(TotalComentariosLidosIns) + 4) ; i++) {
            html += TemplatePostsComentarios(obj.comentarios[i]);
            _totalComentariosLidos++;
        }

        TotalComentariosLidosIns = _totalComentariosLidos;

        html += '</div>' +
                            '</div>' +
                            '<hr style="margin-top: 0px;" /><a onclick="PromoIns.MaisComentarios(' + obj.idpost + ')" id="linkMaisComentariosIns' + obj.idpost + '">Mais comentários</a>   <div id="statusComentariosLidosIns' + obj.idpost + '" style="float: right; display:inline-table;"> <span class="lidos">' + TotalComentariosLidosIns + ' </span> de <span class="total"> ' + obj.comentarios.length + ' </span> </div>  ' +
                        '</div>' +
                    '</div>';

        $('#statusComentariosLidosIns.lidos').html(TotalComentariosLidosIns);
        $('#statusComentariosLidosIns.total').html(TotalComentariosParaLerIns);

        return html;
    };

    function TemplatePostsComentarios(objCom) {

        var html = "";

        if (objCom != null && objCom != 'undefined') {
            var nomeUsuario = objCom.nomeusuario != null && objCom.nomeusuario != "" ? objCom.nomeusuario : "";

            html = '<div class="social-comment">' +
                            '<div class="media-body">' +
                                '<div class="corusuario">' +
                                    '<strong>' + objCom.nomeusuario + '</strong>' +
                                '</div> ' + objCom.postagem + '<br>' +
                                '<span class="small corusuario"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + objCom.curtidas + ' curtidas</span> - <span class="icon-respostas small corusuario" data-qtdrespostas="0"><i class="fa fa-reply" style="color: gray;"></i> 0 respostas</span> - <small class="text-muted">' + objCom .datahora+ '</small>' +
                            '</div>' +
                            //TemplatePostsComentariosRespostas(objCom) +
                        '</div>';
        }
        
        return html;
    };

    //function TemplatePostsComentariosRespostas(objCom) {
    //    var html = "";

    //    if (objCom.respostas.length > 0) {
    //        $.each(objCom.respostas, function (index, obj) {
    //            html += '<div class="social-comment resposta">' +
    //                        '<div class="media-body">' +
    //                            '<div class="corusuario"><strong>' + obj.nomeusuario + '</strong></div> ' + obj.postagem + '<br><span class="small text-success"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + obj.curtidas + ' curtidas</span> - <small class="text-muted">' + obj.datahora + '</small>' +
    //                        '</div>' +
    //                    '</div>';
    //        })
    //    }
    //    return html;
    //};

    function MaisComentarios(idpost) {

        var totalParaLer = $('#statusComentariosLidosIns' + idpost + ' > span.total').html().trim();
        var totalLidos = $('#statusComentariosLidosIns' + idpost + ' > span.lidos').html().trim();

        var html = "";

        var _totalComentariosLidos = totalLidos;


        for (var i = totalLidos; i <= (parseInt(totalLidos) + 4) ; i++) {

            if (objDataInstagram.comentarios[i] != null) {
                html += TemplatePostsComentarios(objDataInstagram.comentarios[i]);
                _totalComentariosLidos++;
            }
        }

        $('#detalhesPostIns' + idpost).append(html);

        if (_totalComentariosLidos >= totalParaLer) {
            $('#linkMaisComentariosIns' + idpost).hide();
        }

        $('#statusComentariosLidosIns' + idpost + ' > span.lidos').html(_totalComentariosLidos);
        $('#statusComentariosLidosIns' + idpost + ' > span.total').html(totalParaLer);
    }

    function TotalPostsInstagram() {
        var URL = "/api/PromocoesAPI/GetTotalPostsInstagram/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa;
        $.get(URL, function (data) {
            _totalPostsInstagram = data;
        });
    };

    function MudoupaginaInstagram(pagina) {
        paginaInstagram = pagina;
        ListarPosts();
    };

    function TemplateSemPosts() {
        var html = '';
        html = '<div class="col-md-12 ibox-content boxRedeSocial" style="margin-bottom: 10px;"><i class="fa fa-meh-o"></i> Sem posts relacionados para esta Empresa / Promoção</div>';
        return html;
    }
}
