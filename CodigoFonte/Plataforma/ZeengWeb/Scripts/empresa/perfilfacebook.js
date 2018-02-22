var paginacaoFace = new Paginacao();
var paginacaoFaceTimeline = new Paginacao();
var totalPostsEngajamento;
var totalPostsFaceTimeLine;
var totalComentariosSemReposta;
var totalComentariosLinhadotempo = [];
var ordenacao = $("#facebook_timeline_ordenacao").val();
var comentariosHtml = '';
var palavra;
var PostComent;



$(document).ready(function () {

    $("#FASelect").hide();

    $(".container-redesocial[data-nomerede='facebook'] #btnAnalise").click(function () {
        $("#FASelect").hide();

        $(".container-redesocial[data-nomerede='facebook'] #btnLinhaTempo").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='facebook'] .container_analise").show();
        $(".container-redesocial[data-nomerede='facebook'] .container_linhadotempo").hide();

    });



    $(".container-redesocial[data-nomerede='facebook'] #btnLinhaTempo").click(function () {

        palavra = null;

        $(".facebook_filtro_timeline").show();

        var dtInicialFace = $("#dtinicialface").datepicker('getDate');
        var dtFinalFace = $("#dtfinalface").datepicker('getDate');
        var ordenacao = $("#facebook_timeline_ordenacao").val();
        CarregaFaceLinhaDoTempo(idEmpresa, dtInicialFace, dtFinalFace, 6, 1, ordenacao, null, null);


        $("#page-wrapper > div:nth-child(6) > div:nth-child(1) > div > div.col-md-8 > div").show();

        $(".container-redesocial[data-nomerede='facebook'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='facebook'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='facebook'] .container_linhadotempo").show();


    });



    $(document).on('click', '.face_post_nuvem_termos #Spanid', function () {

        palavra = $(this).html();
        PostComent = "Post";
        $(".facebook_filtro_timeline").show();

        $("#page-wrapper > div:nth-child(6) > div:nth-child(1) > div > div.col-md-8 > div").show();

        var dtInicialFace = $("#dtinicialface").datepicker('getDate');
        var dtFinalFace = $("#dtfinalface").datepicker('getDate');
        var ordenacao = $("#facebook_timeline_ordenacao").val();

        CarregaFaceLinhaDoTempo(idEmpresa, dtInicialFace, dtFinalFace, 6, 1, ordenacao, palavra, PostComent);

        $(".container-redesocial[data-nomerede='facebook'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='facebook'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='facebook'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='facebook'] .container_linhadotempo").show();

    });

    $(document).on('click', '.face_coment_nuvem_termos #Spanid', function () {

        palavra = $(this).html();
        PostComent = "Comentario";
        $(".facebook_filtro_timeline").show();

        $("#page-wrapper > div:nth-child(6) > div:nth-child(1) > div > div.col-md-8 > div").show();

        var dtInicialFace = $("#dtinicialface").datepicker('getDate');
        var dtFinalFace = $("#dtfinalface").datepicker('getDate');
        var ordenacao = $("#facebook_timeline_ordenacao").val();
        CarregaFaceLinhaDoTempo(idEmpresa, dtInicialFace, dtFinalFace, 6, 1, ordenacao, palavra, PostComent);

        $(".container-redesocial[data-nomerede='facebook'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='facebook'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='facebook'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='facebook'] .container_linhadotempo").show();

    });

});


function CarregaFaceLinhaDoTempo(id, dtIni, dtFim, qtdPosts, pagina, ordenacao, palavra, PostComent) {

    if (palavra == '') {

        palavra = null;

    }

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookPostsEmpresa?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina + "&ordenacao=" + ordenacao + "&palavra=" + palavra + "&PostComentario=" + PostComent;

    $.getJSON(url, function (data) {

        //if (totalPostsFaceTimeLine == undefined) {

        totalPostsFaceTimeLine = data.TotalPosts;


        paginacaoFaceTimeline.CriaPaginacao(".facebook-pag-poststimeline", parseInt((totalPostsFaceTimeLine / 6) + 0.9), totalPostsFaceTimeLine, 1);
        paginacaoFaceTimeline.setMudouPagina(mudouPaginaFaceTimeLine);
        //}

        $(".facebook_posts_timeline").empty();

        $.each(data.Posts, function (key, post) {

            var modeloPost = [];
            totalComentariosLinhadotempo[post.idPost] = post.totalsemreposta;

            modeloPost.push('<div class="social-feed-box" style="padding-left: 15px;height: 640px;min-height: 400px; margin-left: 10px;" data-idpost="' + post.idPost + '">');
            modeloPost.push('<div class="row social-body" style="height:100%">');

            if (post.imagem.caminho != null && post.imagem.caminho != "" && post.imagem.altura > 0 && post.imagem.largura > 0) {
                templateComImagemFace(modeloPost, post);
            } else {
                templateSemImagemFace(modeloPost, post);
            }

            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".facebook_posts_timeline").append(modeloPost.join(''));

        });

    });

}

function templateComImagemFace(modeloPost, post) {

    var padding = '0px 0px 0px 0px';
    var altura = 0;
    var boxAltura = 600;
    var boxLargura = 542;

    /*
        1 = Imagem + LARGA e - ALTA
        2 = Imagem + LARGA e + ALTA
        3 = Imagem - LALRGA e - ALTA
        4 = Imagem - LARGA e + ALTA
    */

    // 1 = Imagem + LARGA e - ALTA
    if (post.imagem.largura > boxLargura && post.imagem.altura < boxAltura) {
        var diferencaLargura = (boxLargura / post.imagem.largura);
        altura = post.imagem.altura * diferencaLargura;
        padding = (boxAltura - altura) / 2 + "px 0px 0px 0px";

        modeloPost.push('<div class=" col-md-6 text-center" style="background: #f3f3f4;min-height:100%; padding:' + padding + '"  id="divimagempost' + post.idPost + '"  ><img style="width: ' + boxLargura + 'px;" id="imagempost' + post.idPost + '" src="' + post.imagem.caminho + '" class="img-responsive"></div>');
    }

    // 2 = Imagem + LARGA e + ALTA
    if (post.imagem.largura > boxLargura && post.imagem.altura > boxAltura) {
        var diferencaLargura = (boxLargura / post.imagem.largura);
        altura = diferencaLargura * post.imagem.altura;
        if (altura < boxAltura) {
            padding = (boxAltura - altura) / 2 + "px 0px 0px 0px";
        } else {
            altura = boxAltura;
        }

        modeloPost.push('<div class=" col-md-6 text-center" style="background: #f3f3f4;min-height:100%; padding:' + padding + ';"  id="divimagempost' + post.idPost + '"  ><img  style="width: ' + boxLargura + 'px; height: ' + altura + 'px;" id="imagempost' + post.idPost + '" src="' + post.imagem.caminho + '" class="img-responsive"></div>');
    }

    // 3 = Imagem - LALRGA e - ALTA
    if (post.imagem.largura < boxLargura && post.imagem.altura < boxAltura) {
        padding = (boxAltura - post.imagem.altura) / 2 + "px 0px 0px 0px";

        modeloPost.push('<div class=" col-md-6 text-center" style="background: #f3f3f4;height:100%; padding:' + padding + ';"  id="divimagempost' + post.idPost + '"  ><img id="imagempost' + post.idPost + '" height="' + boxAltura + '" src="' + post.imagem.caminho + '" class="img-responsive"></div>');
    }

    // 4 = Imagem - LARGA e + ALTA
    if (post.imagem.largura < boxLargura && post.imagem.altura > boxAltura) {
        modeloPost.push('<div class=" col-md-6 text-center" style="background: #f3f3f4;height:100%;"  id="divimagempost' + post.idPost + '"  ><img style="height:' + boxAltura + 'px;" id="imagempost' + post.idPost + '" src="' + post.imagem.caminho + '" class="img-responsive"></div>');
    }

    // ----------------------------------------

    //Mostra Data e hora  do Post
    modeloPost.push('<div class="col-md-6 social-avatar" style=" height:585px; overflow-y: auto;">');
    modeloPost.push('<div class="media-body">');
    modeloPost.push('<a id="usuariopost" href="">');
    modeloPost.push('</a>');
    modeloPost.push('<strong class="text-muted" id="dataPost">');
    modeloPost.push(formataDataHoraJson(post.dataPost));
    modeloPost.push('</strong>');
    modeloPost.push('</div>');

    //-------------------------------------------

    //-- Box do post 
    modeloPost.push('<div>');
    modeloPost.push('<p id="textopost">');
    modeloPost.push(insereQuebraLinha(post.postagem));
    modeloPost.push('</p>');
    modeloPost.push('</div>');

    //----------------------------------------------

    // 3 labels de curtisas, comentarios, compartilhamento
    modeloPost.push('<hr>');
    modeloPost.push('<div class="btn-group2">');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-up"></i> <span id="curtidaspost">');
    modeloPost.push(post.totalCurtidas);
    modeloPost.push('</span> curtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-comments"></i> <span id="comentariospost">');
    modeloPost.push(post.totalComentarios);
    modeloPost.push('</span> comentários</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-share"></i> <span id="compartilhamentospost">');
    modeloPost.push(post.totalCompartilhamentos);
    modeloPost.push('</span> compartilhamentos</div>');
    modeloPost.push('</div>');
    modeloPost.push('<hr>');
    //------------------------------------------------------

    // Comentarios
    modeloPost.push('<div class="social-footer" style="min-height: 345px;" id="detalhesPostFace" data-carregados="' + post.comentarios.length + '">');

    var comentarios = MontaTemplateComentarios(post.comentarios);
    modeloPost.push(comentarios);

    modeloPost.push('</div>');

    //----------------------------------------------------------


    // Botão Ver Mais e Contagem de comentárioos
    if (post.totalComentarios > 0) {

        var TotaIncremento = 0;

        $.each(post.comentarios, function (key, com) {
            TotaIncremento = TotaIncremento + com.totalRespostas;
        });


        modeloPost.push('<button id="btnCarregaComentariosFace' + post.idPost + '" onclick="btnCarregaMaisComentariosFace(this)" class="btn btn-link"> Ver comentários</button> <div style="float: right; display:inline-table;"> <span id="ComentariosCarregados' + post.idPost + '">' + (post.comentarios.length + TotaIncremento) + ' </span> de <span> ' + post.totalComentarios + ' </span> </div>');
    }
    //-----------------------------------------------------------

    modeloPost.push('</div>');

}

function templateSemImagemFace(modeloPost, post) {


    modeloPost.push('<div class=" col-md-6 text-left" style="background: #f3f3f4; height:620px; overflow-y: auto; padding-top: 10px;" >');

    //Mostraa Data e hora  do Post

    modeloPost.push('<div class="media-body">');
    modeloPost.push('<a id="usuariopost" href="">');
    modeloPost.push('</a>');
    modeloPost.push('<strong class="text-muted" id="dataPost">');
    modeloPost.push(formataDataHoraJson(post.dataPost));
    modeloPost.push('</strong>');
    modeloPost.push('</div>');

    //-------------------------------------------

    //-- Box do post 
    modeloPost.push('<div>');
    modeloPost.push('<p id="textopost">');
    modeloPost.push(insereQuebraLinha(post.postagem));
    modeloPost.push('</p>');
    modeloPost.push('</div>');

    //----------------------------------------------

    // 3 labels de curtisas, comentarios, compartilhamento
    modeloPost.push('<div class="btn-group2">');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-up"></i> <span id="curtidaspost">');
    modeloPost.push(post.totalCurtidas);
    modeloPost.push('</span> curtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-comments"></i> <span id="comentariospost">');
    modeloPost.push(post.totalComentarios);
    modeloPost.push('</span> comentários</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-share"></i> <span id="compartilhamentospost">');
    modeloPost.push(post.totalCompartilhamentos);
    modeloPost.push('</span> compartilhamentos</div>');
    modeloPost.push('</div>');

    //------------------------------------------

    modeloPost.push('</div>');



    modeloPost.push('<div class="col-md-6 social-avatar" style=" height:615px; overflow-y: auto;">');

    // Comentarios

    modeloPost.push('<div class="social-footer" style="border-top: none;" id="detalhesPostFace" data-carregados="' + post.comentarios.length + '">');


    var comentarios = MontaTemplateComentarios(post.comentarios);
    modeloPost.push(comentarios);

    modeloPost.push('</div>');
    //----------------------------------------------------------


    // Botão Ver Mais e Contagem de comentárioos
    if (post.totalComentarios > 0) {

        var TotaIncremento = 0;

        $.each(post.comentarios, function (key, com) {
            TotaIncremento = TotaIncremento + com.totalRespostas;
        });


        modeloPost.push('<button id="btnCarregaComentariosFace' + post.idPost + '" onclick="btnCarregaMaisComentariosFace(this)" class="btn btn-link"> Ver comentários</button> <div style="float: right; display:inline-table;"> <span id="ComentariosCarregados' + post.idPost + '">' + (post.comentarios.length + TotaIncremento) + ' </span> de <span> ' + post.totalComentarios + ' </span> </div>');
    }
    //-----------------------------------------------------------

    modeloPost.push('</div>');

}

function CarregaGraficoCrescimentoFas(id, dtIni, dtFim) {

    $("#sparklinecrescfas").empty();
    $("#lblcrescfas").empty();
    $(".loadingCrescimentoFasFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetGraficoFaceCrescimentoFas?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingCrescimentoFasFace").hide();

        var totalFas = 0;
        var valores = data.map(function (x) {
            totalFas = x.valor;
            return x.valor;
        });

        var datas = data.map(function (x) {
            return x.data;
        });




        $("#lblcrescfas").text(formataNumero(totalFas));
        $("#sparklinecrescfas").sparkline(valores, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });


    });
}

function CarregaGraficosInteracoes(id, dtIni, dtFim) {

    $("#sparklineqtdposts").empty();
    $("#lblqtdposts").empty();
    $("#sparklineqtdinteracoes").empty();
    $("#lblqtdinteracoes").empty();
    $("#sparklineqtdcurtidas").empty();
    $("#lblqtdcurtidas").empty();
    $("#sparklineqtdcomentarios").empty();
    $("#lblqtdcomentarios").empty();
    $("#sparklineqtdcompartilhamentos").empty();
    $("#lblqtdcompartilhamentos").empty();

    $(".loadingGraficosInteracoesFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookInteracoes?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {



        var totalPosts = 0;
        var arrqtdposts = data.map(function (x) {
            totalPosts += x.posts;
            return x.posts;
        });

        var totalInteracoes = 0;
        var arrqtdinteracoes = data.map(function (x) {
            totalInteracoes += x.interacoes;
            return x.interacoes;
        });

        var totalReacoes = 0;
        var arrqtdreacoes = data.map(function (x) {
            totalReacoes += x.reacoes;
            return x.reacoes;
        });

        var totalComentarios = 0;
        var arrqtdcomentarios = data.map(function (x) {
            totalComentarios += x.comentarios;
            return x.comentarios;
        });

        var totalCompartilhamentos = 0;
        var arrqtdcompartilhamentos = data.map(function (x) {
            totalCompartilhamentos += x.compartilhamentos;
            return x.compartilhamentos;
        });

        var datas = data.map(function (x) {
            return formataDataJson(x.data);
        });


        $(".loadingGraficosInteracoesFace").hide();

        $("#lblqtdposts").text(formataNumero(totalPosts));
        $("#sparklineqtdposts").sparkline(arrqtdposts, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });

        $("#lblqtdinteracoes").text(formataNumero(totalInteracoes));
        $("#sparklineqtdinteracoes").sparkline(arrqtdinteracoes, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });

        $("#lblqtdcurtidas").text(formataNumero(totalReacoes));
        $("#sparklineqtdcurtidas").sparkline(arrqtdreacoes, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });


        $("#lblqtdcomentarios").text(formataNumero(totalComentarios));
        $("#sparklineqtdcomentarios").sparkline(arrqtdcomentarios, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });


        $("#lblqtdcompartilhamentos").text(formataNumero(totalCompartilhamentos));
        $("#sparklineqtdcompartilhamentos").sparkline(arrqtdcompartilhamentos, {
            type: 'line',
            width: '100%',
            height: '60',
            lineColor: '#1ab394',
            fillColor: "#ffffff",
            tooltipFormat: "{{offset:data}}: {{y:val}}",
            tooltipValueLookups: {
                data: datas
            }
        });

    });
}

function CarregaPostsMaiorEngajamento(id, dtIni, dtFim, qtdPosts, pagina) {

    $(".posts_engajamento").empty();
    //$(".pag-postsengajamento").empty();

    $(".loadingPostsMaiorEngajamentoFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookPostsMaisEngajamento?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina;

    $.getJSON(url, function (data) {


        totalPostsEngajamento = data.TotalPosts;

        paginacaoFace.CriaPaginacao(".pag-postsengajamento", parseInt((totalPostsEngajamento / 6) + 0.9), totalPostsEngajamento, pagina);
        paginacaoFace.setMudouPagina(mudouPaginaPostEngajamento);

        $(".loadingPostsMaiorEngajamentoFace").hide();

        $(".posts_engajamento").empty();

        $.each(data.Posts, function (key, post) {
            var modeloPost = [];

            modeloPost.push('<div class="col-md-4">');
            modeloPost.push('<div class="ibox">');
            modeloPost.push('<div class="ibox-content">');
            modeloPost.push('<div class="engajado_img">');

            if (post.nomeimagem != null)
                modeloPost.push('<img src="' + diretorioImagens + '/facebook/' + post.nomeimagem + '" />');
            else {
                modeloPost.push('<img src="' + diretorioImagens + '/facebook/no-photo.gif" />');
            }
            modeloPost.push('</div>');
            modeloPost.push('<div class="engajado_texto">');
            modeloPost.push('<div>');
            modeloPost.push('<span class="engajado_data">' + formataDataJson(post.dataPost) + '</span>');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-thumbs-up"></i> <span class="engajado_numero">' + post.totalCurtidas + '</span> curtidas<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-comments-o"></i> <span class="engajado_numero">' + post.totalComentarios + '</span> comentários<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-share"></i> <span class="engajado_numero">' + post.totalCompartilhamentos + '</span> compartilhamentos');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-users"></i> <span class="engajado_numero">' + post.engajamento + '</span> engajamento');
            modeloPost.push('</div>');
            modeloPost.push('<div class="engajado_vermais">');
            modeloPost.push('<span class="btn-vermais" data-idpost="' + post.idPost + '"><i class="fa fa-eye"></i> Ver mais</span>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".posts_engajamento").append(modeloPost.join(''));

        });

        $(".posts_engajamento .btn-vermais").click(function () {

            var id = $(this).attr("data-idpost");

            MostraDetalhesPostFace(id);

        });

    });
}

function MostraDetalhesPostFace(idPost) {

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookPost?idPost=" + idPost;

    $.getJSON(url, function (data) {


        totalComentariosSemReposta = data.totalsemreposta;
        $("#modalDetalhesPostFace").attr("data-idpost", data.idPost);
        $("#modalDetalhesPostFace #usuariopost").text(data.usuarioPost);
        $("#modalDetalhesPostFace #dataPost").text(formataDataHoraJson(data.dataPost));
        $("#modalDetalhesPostFace #textopost").html(insereQuebraLinha(data.postagem));

        if (data.imagem != null) {
            $("#modalDetalhesPostFace #imagempost").show();
            $("#modalDetalhesPostFace #imagempost").attr("src", data.imagem.caminho);
        }
        else {
            $("#modalDetalhesPostFace #imagempost").hide();
        }

        $("#modalDetalhesPostFace #curtidaspost").text(formataNumero(data.totalCurtidas));
        $("#modalDetalhesPostFace #comentariospost").text(formataNumero(data.totalComentarios));
        $("#modalDetalhesPostFace #compartilhamentospost").text(formataNumero(data.totalCompartilhamentos));

        if (data.totalComentarios > 3) {
            $("#btnCarregaComentariosFace").show();
        }
        else {
            $("#btnCarregaComentariosFace").hide();
        }

        var comentarios = MontaTemplateComentarios(data.comentarios);

        $("#modalDetalhesPostFace .social-footer").empty();
        $("#modalDetalhesPostFace .social-footer").attr("data-carregados", data.comentarios.length);
        $("#modalDetalhesPostFace .social-footer").append(comentarios);

        exibirModal("#modalDetalhesPostFace", undefined, undefined, 0, undefined);
    });
}

function MontaTemplateComentarios(comentarios) {

    var arrComentarios = [];

    $(comentarios).each(function (i, e) {
        arrComentarios.push('<div class="social-comment" data-id="' + e.idcomentario + '" data-idfacebook="' + e.idfacebook + '">');
        arrComentarios.push('<div class="media-body">');
        arrComentarios.push('<div class="corusuario"><strong>');
        arrComentarios.push(e.nomeusuario);
        arrComentarios.push('</strong></div> ');
        arrComentarios.push(insereQuebraLinha(e.postagem));

        if (e.urlimagem != null)
            arrComentarios.push('<br/><img src="' + e.urlimagem + '">');

        arrComentarios.push('<br />');
        arrComentarios.push('<span class="small corusuario"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + e.totalCurtidas + ' curtidas</span> - ');
        arrComentarios.push('<span class="icon-respostas small corusuario" data-qtdrespostas=' + e.totalRespostas + '><i class="fa fa-reply" style="color: gray;"></i> ' + e.totalRespostas + ' respostas</span> - ');
        arrComentarios.push('<small class="text-muted">' + formataDataHoraJson(e.datahora) + '</small>');
        arrComentarios.push('</div>');

        respostas = MontaTemplateRespostas(e.respostas, e.totalRespostas > e.respostas.length)
        arrComentarios.push(respostas);

        arrComentarios.push('</div>');

    });

    return arrComentarios.join('');
}

function MontaTemplateRespostas(respostas, mostraMaisRespostas) {

    var arrRespostas = [];

    if (respostas.length > 0) {
        $(respostas).each(function (i, r) {
            arrRespostas.push('<div class="social-comment resposta" data-id="' + r.idcomentario + '" data-idfacebook="' + r.idfacebook + '">');
            arrRespostas.push('<div class="media-body">');
            arrRespostas.push('<div class="corusuario"><strong>');
            arrRespostas.push(r.nomeusuario);
            arrRespostas.push('</strong></div> ');
            arrRespostas.push(insereQuebraLinha(r.postagem));
            arrRespostas.push('<br />');
            arrRespostas.push('<span class="small text-success"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + r.totalCurtidas + ' curtidas</span> - ');
            arrRespostas.push('<small class="text-muted">' + formataDataHoraJson(r.datahora) + '</small>');
            arrRespostas.push('</div>');
            arrRespostas.push('</div>');
        });

        if (mostraMaisRespostas) {
            arrRespostas.pop();
            arrRespostas.push('<div class="small corusuario" onclick="CarregaMaisRespostasFace(this)" ><i class="fa fa-reply" style="color: gray;"></i> ver mais respostas</div>');
            arrRespostas.push('</div>');
        }
    }

    return arrRespostas.join('');
}

function btnCarregaMaisComentariosFace(btnpost) {

    idPost = $(btnpost).parent().parent().parent().data("idpost")
    var containerComentarios = $(btnpost).parent().find(".social-footer");
    var inicial = $(containerComentarios).attr("data-carregados");
    var quantidade = 5;

    CarregaMaisComentariosFaceTimeLine(idPost, inicial, quantidade);
}

function CarregaMaisComentariosFaceTimeLine(idPost, inicial, quantidade) {

    var containerComentarios = $('.social-feed-box[data-idpost="' + idPost + '"] .social-footer');

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookComentariosPost?idPost=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {

        var htmlComentarios = MontaTemplateComentarios(comentarios);
        var ComentariosCarregados = "#ComentariosCarregados" + comentarios[0].idpost;
        var total = (parseInt(inicial) + comentarios.length);
        var totalInicial = $(ComentariosCarregados).text();
        var totalComentarios = 0;
        var TotaIncremento = 0;

        $.each(comentarios, function (key, com) {
            TotaIncremento = TotaIncremento + com.totalRespostas;
        });

        totalComentarios = parseInt(totalInicial) + comentarios.length + TotaIncremento;

        $(containerComentarios).attr("data-carregados", total);
        $(containerComentarios).append(htmlComentarios);
        $(ComentariosCarregados).text(totalComentarios);

        if (total == totalComentariosLinhadotempo[comentarios[0].idpost]) {

            $('.container_linhadotempo').find('#btnCarregaComentariosFace' + comentarios[0].idpost).hide();
        }
    });
}

function CarregaMaisComentariosFace() {

    var idPost = $("#modalDetalhesPostFace").attr("data-idpost");

    var inicial = $("#modalDetalhesPostFace .social-footer").attr("data-carregados");
    var quantidade = 5;
    var htmlComentarios = "";

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookComentariosPost?idPost=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {

        htmlComentarios = MontaTemplateComentarios(comentarios);

        $("#modalDetalhesPostFace .social-footer").attr("data-carregados", parseInt(inicial) + comentarios.length);
        $("#modalDetalhesPostFace .social-footer").append(htmlComentarios);

        var total = (parseInt(inicial) + comentarios.length);

        if (total == totalComentariosSemReposta) {

            $('.form-modal.detalhes-post').find('#btnCarregaComentariosFace').hide();
        }

    });


}

function CarregaMaisRespostasFace(b) {

    var idfacebook = $(b).parent().parent().attr("data-idfacebook");
    var inicial = $(b).parent().parent().find('.resposta').length;

    var quantidade = 0;

    var containerRespostas = $(b).parent().parent();

    $(b).remove();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookRespostasComentario?idfacebookcomentario=" + idfacebook + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (respostas) {

        var htmlRespostas = MontaTemplateRespostas(respostas);

        $(containerRespostas).append(htmlRespostas);

    });

}

function CarregaNuvemTermosPostsFace(id, dtIni, dtFim) {

    $('.fb_nuvem_semtermos').hide();
    $(".face_post_nuvem_termos").hide();
    $(".face_coment_nuvem_termos").hide();
    $(".loadingNuvemFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookTermosPost?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {
        if (data.length > 0) {
            //Nuvem de termos

            var minFreq = data[0].frequencia;
            var maxFreq = data[0].frequencia;

            var listaTermos = data.map(function (t) {

                if (t.frequencia <= minFreq)
                    minFreq = t.frequencia;

                if (t.frequencia >= maxFreq)
                    maxFreq = t.frequencia;

                return [t.termo, t.frequencia, t.idpost];
            });

            var nuvem = new NuvemTermos();

            $(".loadingNuvemFace").hide();
            $(".face_post_nuvem_termos").show();

            nuvem.inicializaNuvemTermos(".face_post_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {

            $(".loadingNuvemFace").hide();
            $('.fb_nuvem_semtermos').show();
        }
    });

}

function CarregaNuvemTermosComentariosFace(id, dtIni, dtFim) {

    $('.fb_nuvem_semtermos').hide();
    $(".face_post_nuvem_termos").hide();
    $(".face_coment_nuvem_termos").hide();
    $(".loadingNuvemFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookTermosComentario?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        if (data.length > 0) {
            //Nuvem de termos
            var minFreq = data[0].frequencia;
            var maxFreq = data[0].frequencia;

            var listaTermos = data.map(function (t) {

                if (t.frequencia < minFreq)
                    minFreq = t.frequencia;

                if (t.frequencia > maxFreq)
                    maxFreq = t.frequencia;

                return [t.termo, t.frequencia];
            });

            $(".loadingNuvemFace").hide();
            $(".face_coment_nuvem_termos").show();

            var nuvem = new NuvemTermos();
            nuvem.inicializaNuvemTermos(".face_coment_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {

            $(".loadingNuvemFace").hide();
            $('.fb_nuvem_semtermos').show();

        }

    });

}

function CarregaGraficoEngajamentoHora(id, dtIni, dtFim) {

    $(".face-horarios-engajamento tbody").empty();

    $(".loadingEngajamentoHoraFace").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetFacebookEngajamentoPorHora?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingEngajamentoHoraFace").hide();
        $(".face-horarios-engajamento tbody").empty();
        var modeloTabela = [];

        color = d3.scale.linear().domain([1, data.valorMaximo])
            .interpolate(d3.interpolateHcl)
            .range([d3.rgb("#e5faf7"), d3.rgb('#068a71')]);


        var i = 0;
        for (var dia = 1; dia <= 7; dia++) {
            modeloTabela.push('<tr>');

            modeloTabela.push('<td class="diasemana" data-valuedia="'+dia+'">' + RetornaDiaSemana(dia) + '</td>');

            for (var hora = 0; hora <= 23; hora++) {

                var engajamento = data.Itens.filter(function (obj) {
                    return obj.DiaSemana == dia && obj.Hora == hora;
                });

                if (engajamento.length == 0 || engajamento === undefined || engajamento === null) {
                    modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "'  style=background-color:#edfffc data-toggle='ajuda-tooltip' data-placement='top' title='0 interações'></td>");

                }
                else {

                    modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "' style=background-color:" + color(engajamento[0].Valor) + " data-toggle='ajuda-tooltip' data-placement='top' title='<div class=\"tooltip-diahora\">" + formataNumero(engajamento[0].Valor) + " interações <br>&#183; " + formataNumero(engajamento[0].reacoes) + " curtidas<br>&#183; " + formataNumero(engajamento[0].compartilhamentos) + " compartilhamentos <br>&#183; " + formataNumero(engajamento[0].comentarios) + " comentarios </div>'></td>");


                }

            }

            modeloTabela.push('</tr>');

        }
        modeloTabela.push('<tr class=legenda_hora>');
        modeloTabela.push("<td></td>");

        for (var hora = 0; hora <= 23; hora++) {
            modeloTabela.push("<td data-valuehora='" + hora + "'>" + hora + "</td>");
        }

        modeloTabela.push('</tr>');

        $(".face-horarios-engajamento tbody").append(modeloTabela.join(''));

        $(".face-horarios-engajamento tbody td[data-toggle='ajuda-tooltip']").tooltip({ container: 'body', html: 'true' });
        
      
        $("td.HoverEnga").hover(function () {

            var dia = $(this).data("dia");
            var hora = $(this).data("hora");

            $("tbody > tr >  td[data-valuedia='" + dia + "']").toggleClass("active");

            $(".legenda_hora > td[data-valuehora='" + hora + "']").toggleClass("active");

            
        });


       


    });


}

function mudouPaginaPostEngajamento(pagina) {

    var dtInicial = $("#dtinicialface").datepicker('getDate');
    var dtFinal = $("#dtfinalface").datepicker('getDate');

    CarregaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, 6, pagina)
}

function mudouPaginaFaceTimeLine(pagina) {

    if (palavra == undefined) {
        palavra = null
    }


    if (PostComent == undefined) {
        PostComent == null
    }

    var dtInicial = $("#dtinicialface").datepicker('getDate');
    var dtFinal = $("#dtfinalface").datepicker('getDate');

    CarregaFaceLinhaDoTempo(idEmpresa, dtInicial, dtFinal, 6, pagina, ordenacao, palavra, PostComent)
}