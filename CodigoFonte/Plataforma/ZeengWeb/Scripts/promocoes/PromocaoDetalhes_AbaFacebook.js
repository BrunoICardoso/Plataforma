
var TotalComentariosLidos;
var TotalComentariosParaLer;
var paginacaoFace = new Paginacao();
var paginaFacebook = 0;
var qtdeRegistrosFacebook = 6;

function PromoDetalhesFacebook() {
    var CaminhoImagem = '/Imagens/facebook';
    //var objData = {};
    var totalPostsTimeLine = 0;

    this.ListarPostsFace = ListarPosts;
    this.TotalPostsFacebook = TotalPostsFacebook;
    this.MaisComentarios = MaisComentarios;

    function ListarPosts() {        

        var _html = "";

        var URL = "/api/PromocoesAPI/GetPostsFacebook/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa + "&qtdeRegistros=" + qtdeRegistrosFacebook + "&pagina=" + paginaFacebook;

        $.getJSON(URL, function (data) {

            $('#listaPostsFacebook').html('');

            if (data != null && data.length > 0) {

                $.each(data, function (index, obj) {

                    TotalComentariosLidos = 0;
                    TotalComentariosParaLer = 0;

                    _html += TemplatePosts(obj);

                    paginacaoFace.CriaPaginacao(".facebook-paginacao", parseInt((_totalPostsFacebook / 6) + 0.9), _totalPostsFacebook, paginaFacebook);
                    paginacaoFace.setMudouPagina(MudouPaginaFacebook);
                });
                                
            } else {
                _html = TemplateSemPosts();
            }

            $('#listaPostsFacebook').html(_html);
        });
    };

    function TemplatePosts(obj) {

        //this.objData = obj;
        objDataFacebook = obj;

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
                            '<i class="fa fa-thumbs-up"></i> <span id="curtidaspost">' + obj.reacoes + '</span> reações' +
                        '</div>' +
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-comments"></i> <span id="comentariospost' + obj.idpost + '">' + obj.comentarios.length + '</span> comentários' +
                        '</div>' +
                        '<div class="post-label-interacoes">' +
                            '<i class="fa fa-share"></i> <span id="compartilhamentospost">' + obj.compartilhamentos + '</span> compartilhamentos' +
                        '</div>' +
                    '</div>' +
                    '<hr />' +
                    '<div class="divComentarios">' +
                        '<div class="social-footer" style="min-height: 345px;" id="detalhesPostFace' + obj.idpost + '" data-carregados="5">';
                            
        var _totalComentariosLidos = 0;
        for (var i = TotalComentariosLidos; i <= (parseInt(TotalComentariosLidos) + 4); i++  ){
            html += TemplatePostsComentarios(obj.comentarios[i]);
            _totalComentariosLidos++;
        }
                            
        TotalComentariosLidos = _totalComentariosLidos;

        html +=                 '</div>' +
                            '</div>' +
                            '<hr style="margin-top: 0px;" /><a onclick="PromoFace.MaisComentarios(' + obj.idpost + ')" id="linkMaisComentariosFace' + obj.idpost + '">Mais comentários</a>   <div id="statusComentariosLidosFace' + obj.idpost + '" style="float: right; display:inline-table;"> <span class="lidos">' + TotalComentariosLidos + ' </span> de <span class="total"> ' + obj.comentarios.length + ' </span> </div>  ' +
                        '</div>' +
                    '</div>';

        $('#statusComentariosLidosFace.lidos').html(TotalComentariosLidos);
        $('#statusComentariosLidosFace.total').html(TotalComentariosParaLer);

        return html;
    };

    function  TemplatePostsComentarios(objCom) {

        var html = "";

        if (objCom != null && objCom != 'undefined') {
            var nomeUsuario = objCom.nomeusuario != null && objCom.nomeusuario != "" ? objCom.nomeusuario : "";

            html = '<div class="social-comment">' +
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

    function MaisComentarios(idpost) {
               
        var totalParaLer = $('#statusComentariosLidosFace' + idpost + ' > span.total').html().trim();
        var totalLidos = $('#statusComentariosLidosFace' + idpost + ' > span.lidos').html().trim();

        var html = "";

        var _totalComentariosLidos = totalLidos;
            
        for (var i = totalLidos; i <= (parseInt(totalLidos) + 4) ; i++) {
            
            if (objDataFacebook.comentarios[i] != null) {
                html += TemplatePostsComentarios(objDataFacebook.comentarios[i]);
                _totalComentariosLidos++;
            }
        }
     
        $('#detalhesPostFace' + idpost).append(html);

        if (_totalComentariosLidos >= totalParaLer) {
            $('#linkMaisComentariosFace' + idpost).hide();
        }

        $('#statusComentariosLidosFace' + idpost + ' > span.lidos').html(_totalComentariosLidos);
        $('#statusComentariosLidosFace' + idpost + ' > span.total').html(totalParaLer);
    }

    function TotalPostsFacebook() {        
        var URL = "/api/PromocoesAPI/GetTotalPostsFacebook/?idPromocao=" + idPromocao + "&idEmpresa=" + idEmpresa;
        $.get(URL, function (data) {            
            _totalPostsFacebook = data;
        });
    };

    function MudouPaginaFacebook(pagina) {
        paginaFacebook = pagina;
        ListarPosts();
    };

    function TemplateSemPosts() {
        var html = '';
        html = '<div class="col-md-12 ibox-content boxRedeSocial" style="margin-bottom: 10px;"><i class="fa fa-meh-o"></i> Sem posts relacionados para esta Empresa / Promoção</div>';
        return html;
    }
}
