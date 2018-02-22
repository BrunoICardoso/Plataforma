var paginacaoInsta = new Paginacao();
var paginacaoInstaTimeline = new Paginacao();
var totalInstaPostsEngajamento;
var totalPostsInstagramTimeline;
var TotalComentariosInstaBotao;
var totalComentariosLinhaTempo = [];
var ordenacao = $("#instagram_filtro_timeline").val();
var palavraInst;
var PostComentInst;
$(document).ready(function () {
    $("#INSelect").hide();

    $(".container-redesocial[data-nomerede='instagram'] #btnAnalise").click(function () {
        $("#INSelect").hide();
        $(".container-redesocial[data-nomerede='instagram'] #btnLinhaTempo").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='instagram'] .container_analise").show();
        $(".container-redesocial[data-nomerede='instagram'] .container_linhadotempo").hide();
    });

    $(".container-redesocial[data-nomerede='instagram'] #btnLinhaTempo").click(function () {
        $(".instagram_filtro_timeline").show();

        palavraInst=null;
        PostComentInst=null;

            var dtInicialInst = $("#dtinicialinsta").datepicker('getDate');
            var dtFinalInst = $("#dtfinalinsta").datepicker('getDate');
            var ordenacaoInst = $("#instagram_filtro_timeline").val();
            CarregaInstagramTimeline(idEmpresa, dtInicialInst, dtFinalInst, 6, 1, ordenacaoInst, palavraInst, PostComentInst);

        $(".container-redesocial[data-nomerede='instagram'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='instagram'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='instagram'] .container_linhadotempo").show();
    });
  
    

    $(document).on('click', '.insta_post_nuvem_termos #Spanid', function ()
    {

        palavraInst = $(this).html();
        PostComentInst = "Post";

        $(".instagram_filtro_timeline").show();

        $("#page-wrapper > div:nth-child(6) > div:nth-child(1) > div > div.col-md-8 > div").show();

        var dtInicialInst = $("#dtinicialinsta").datepicker('getDate');
        var dtFinalInst = $("#dtfinalinsta").datepicker('getDate');
        var ordenacaoInst = $("#instagram_filtro_timeline").val();
        CarregaInstagramTimeline(idEmpresa, dtInicialInst, dtFinalInst, 6, 1, ordenacaoInst, palavraInst, PostComentInst);

        $(".container-redesocial[data-nomerede='instagram'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='instagram'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='instagram'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='instagram'] .container_linhadotempo").show();

    });

    $(document).on('click', '.insta_coment_nuvem_termos #Spanid', function () {

        palavraInst = $(this).html();
        PostComentInst = "Comentario";

        $(".instagram_filtro_timeline").show();

        $("#page-wrapper > div:nth-child(6) > div:nth-child(1) > div > div.col-md-8 > div").show();

        var dtInicialInst = $("#dtinicialinsta").datepicker('getDate');
        var dtFinalInst = $("#dtfinalinsta").datepicker('getDate');
        var ordenacaoInst = $("#instagram_filtro_timeline").val();
        CarregaInstagramTimeline(idEmpresa, dtInicialInst, dtFinalInst, 6, 1, ordenacaoInst, palavraInst, PostComentInst);

        $(".container-redesocial[data-nomerede='instagram'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='instagram'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='instagram'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='instagram'] .container_linhadotempo").show();

    });


});

$('#instagramdatas').datepicker({
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    format: 'dd/mm/yyyy',
});

function CarregaInstagramTimeline(id, dtInicial, dtFinal, qtdPosts, pagina, ordenacao, palavra,PostComent) {

    if (palavra == '') {

        palavra = null;

    }

    var url = "/api/EmpresaRedesSociaisAPI/GetTimelineInsta?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtInicial) + "&dtFinal=" + ConverteDataParaDataJson(dtFinal) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina + "&ordenacao=" + ordenacao + "&palavra=" + palavra + "&PostComentario=" + PostComent;

    $.getJSON(url, function (data) {

        //if (totalPostsInstagramTimeline == undefined) {

            totalPostsInstagramTimeline = data.TotalPosts;

            paginacaoInstaTimeline.CriaPaginacao(".instagram-pag-poststimeline", parseInt((totalPostsInstagramTimeline / 6) + 0.9), totalPostsInstagramTimeline, 1);
            paginacaoInstaTimeline.setMudouPagina(mudouPaginaInstagramTimeLine);
        //}

        $(".instagram_posts_timeline").empty();

        $.each(data.Posts, function (key, post) {
    
            
            var modeloPost = [];
            totalComentariosLinhaTempo[post.idPost] = post.totalComentarios;

            modeloPost.push('<div class="social-feed-box" style="padding-left: 15px;height: 630px;min-height: 400px; margin-left: 10px;" data-idpost="' + post.idPost + '">');
            modeloPost.push('<div class="row social-body"style="height:100%">');
           
            if (post.imagem.caminho != null && post.imagem.caminho != "" && post.imagem.altura > 0 && post.imagem.largura > 0) {
                templateComImagemInsta(modeloPost, post);

            } else {
                templateSemImagemInsta(modeloPost, post);
            }
            
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".instagram_posts_timeline").append(modeloPost.join(''));
        });
    });
}

function templateComImagemInsta(modeloPost, post) {
        
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

    //Mostraa Data e hora  do Post
    modeloPost.push('<div class="col-md-6 social-avatar" style=" height:630px; overflow-y: auto;">');
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

    //// 2 labels de curtisas, comentarios
    modeloPost.push('<hr>');
    modeloPost.push('<div class="btn-group2">');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-up"></i> <span id="curtidaspost">');
    modeloPost.push(post.totalCurtidas);
    modeloPost.push('</span> curtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-comments"></i> <span id="comentariospost">');
    modeloPost.push(post.totalComentarios);
    modeloPost.push('</span> comentários</div>');
    modeloPost.push('</div>');
    modeloPost.push('<hr>');
    //------------------------------------------------------

    // Comentarios
    modeloPost.push('<div class="social-footer" id="detalhesPostFace" data-carregados="' + post.comentarios.length + '">');

    var comentarios = MontaTemplateComentariosInsta(post.comentarios);
    modeloPost.push(comentarios);


    modeloPost.push('</div>');
    //----------------------------------------------------------


    // Botão Ver Mais e Contagem de comentárioos
    if (post.totalComentarios > 0) {
        modeloPost.push('<button id="btnCarregaComentariosInsta' + post.idPost + '" onclick="btnMaisComentariosinsta(this)" class="btn btn-link"> Ver comentários</button> <div style="float: right; display:inline-table;"> <span id="ComentariosCarregados' + post.idPost + '">' + post.comentarios.length + '</span> de <span> ' + post.totalComentarios + ' </span></div>');
    
        if (post.comentarios.length == post.totalComentarios) {
            $(document).find("#btnCarregaComentariosInsta" + post.idPost).hide();
        }
            
    }
    //-----------------------------------------------------------

    modeloPost.push('</div>');

}


function templateSemImagemInsta(modeloPost, post) {

    modeloPost.push('<div class=" col-md-6 text-left" style="background: #f3f3f4; height:610px; overflow-y: auto; padding-top: 10px;" >');

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
    modeloPost.push('</div>');

    //------------------------------------------

    modeloPost.push('</div>');



    modeloPost.push('<div class="col-md-6 social-avatar" style=" height:610px; overflow-y: auto;">');

    // Comentarios
    modeloPost.push('<div class="social-footer" style="border-top: none;" id="detalhesPostFace" data-carregados="'+post.comentarios.length+'">');

    var comentarios = MontaTemplateComentariosInsta(post.comentarios);
    modeloPost.push(comentarios);

   
    modeloPost.push('</div>');
    //----------------------------------------------------------


    // Botão Ver Mais e Contagem de comentárioos
    if (post.totalComentarios > 0) {
        
        var _Display = "initial";
        if (post.comentarios.length == post.totalComentarios) {
            _Display = "none";
        }

        modeloPost.push('<button id="btnCarregaComentariosInsta' + post.idPost + '" onclick="btnMaisComentariosinsta(this)" class="btn btn-link" style="display:' + _Display + ';">Ver comentários</button> <div style="float: right; margin-top: 10px;"> <span id="ComentariosCarregados' + post.idPost + '">' + post.comentarios.length + '</span> de <span> ' + post.totalComentarios + ' </span></div>');

    }
    //-----------------------------------------------------------

    modeloPost.push('</div>');

}


function btnMaisComentariosinsta(btnpost) {

    var idPost = $(btnpost).parent().parent().parent().data("idpost")
    var containerComentarios = $(btnpost).parent().find(".social-footer");
    var inicial = $(containerComentarios).attr("data-carregados");
    var quantidade = 5;

    CarregaMaisComentariosInstaTimeLine(idPost, inicial, quantidade);
}

function CarregaMaisComentariosInstaTimeLine(idPost, inicial, quantidade) {
    
    var containerComentarios = $('.social-feed-box[data-idpost="' + idPost + '"] .social-footer');

    var url = "/api/EmpresaRedesSociaisAPI/GetMaisComentariosDeUmPostInsta?idPost=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {

        var htmlComentarios = MontaTemplateComentariosInsta(comentarios);
        
        var ComentariosCarregados = "#ComentariosCarregados" + comentarios[0].idPost;
        var total = (parseInt(inicial) + comentarios.length);
        var totalInicial = $(ComentariosCarregados).text();
        var totalComentarios = 0;
       
        totalComentarios = parseInt(totalInicial) + comentarios.length;


        $(containerComentarios).attr("data-carregados", total);
        $(containerComentarios).append(htmlComentarios);
        $(ComentariosCarregados).text(totalComentarios);

       

        if (total == totalComentariosLinhaTempo[comentarios[0].idPost]) {
            
            $('.instagram_posts_timeline').find('#btnCarregaComentariosInsta' + comentarios[0].idPost).hide();
        }


    });
}

function mudouPaginaInstagramTimeLine(pagina) {

    if (PostComentInst == undefined) {
        PostComentInst == null
    }

    if (palavraInst == undefined) {
        palavraInst == null
    }
    var dtInicial = $("#dtinicialinsta").datepicker('getDate');
    var dtFinal = $("#dtfinalinsta").datepicker('getDate');

    CarregaInstagramTimeline(idEmpresa, dtInicial, dtFinal, 6, pagina, ordenacao, palavraInst,PostComentInst)
}

function CarregaInstaGraficoCrescimentoSeguidores(id, dtIni, dtFim) {

    $("#lblinstacrescfas").empty();
    $("#sparklineinstacrescfas").empty();

    $(".loadingCrescimentoFasInsta").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetGraficoInstaCrescimentoSeguidores?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingCrescimentoFasInsta").hide();

        var totalFas = 0;
        var valores = data.map(function (x) {
            totalFas = x.valor;
            return x.valor;
        });

        var datas = data.map(function (x) {
            return x.data;  
        });

        $("#lblinstacrescfas").text(formataNumero(totalFas));
        $("#sparklineinstacrescfas").sparkline(valores, {
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

function CarregaInstaGraficosInteracoes(id, dtIni, dtFim) {

    $("#lblinstaqtdposts").empty();
    $("#sparklineinstaqtdposts").empty();
    $("#lblinstaqtdinteracoes").empty();
    $("#sparklineinstaqtdinteracoes").empty();
    $("#lblinstaqtdcurtidas").empty();
    $("#sparklineinstaqtdcurtidas").empty();
    $("#lblinstaqtdcomentarios").empty();
    $("#sparklineinstaqtdcomentarios").empty();
    
    $(".loadingInteracoesInsta").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetInstagramInteracoes?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

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

        var totalCurtidas = 0;
        var arrqtdcurtidas = data.map(function (x) {
            totalCurtidas += x.curtidas;
            return x.curtidas;
        });

        var totalComentarios = 0;
        var arrqtdcomentarios = data.map(function (x) {
            totalComentarios += x.comentarios;
            return x.comentarios;
        })

        var datas = data.map(function (x) {
            return formataDataJson(x.data);
        });

        $(".loadingInteracoesInsta").hide();
                
        $("#lblinstaqtdposts").text(formataNumero(totalPosts));
        $("#sparklineinstaqtdposts").sparkline(arrqtdposts, {
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

        $("#lblinstaqtdinteracoes").text(formataNumero(totalInteracoes));
        $("#sparklineinstaqtdinteracoes").sparkline(arrqtdinteracoes, {
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

        $("#lblinstaqtdcurtidas").text(formataNumero(totalCurtidas));
        $("#sparklineinstaqtdcurtidas").sparkline(arrqtdcurtidas, {
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


        $("#lblinstaqtdcomentarios").text(formataNumero(totalComentarios));
        $("#sparklineinstaqtdcomentarios").sparkline(arrqtdcomentarios, {
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

function CarregaInstaPostsMaiorEngajamento(id, dtIni, dtFim, qtdPosts, pagina) {

    $(".instaposts_engajamento").empty();
    //$(".pag-instapostsengajamento").empty();

    $(".loadingPostsMaiorEngajamentoInsta").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetInstagramPostsMaisEngajamento?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina;

    $.getJSON(url, function (data) {

    
            totalInstaPostsEngajamento = data.TotalPosts;

            paginacaoInsta.CriaPaginacao(".pag-instapostsengajamento", parseInt((totalInstaPostsEngajamento / 6) + 0.9), totalInstaPostsEngajamento, pagina);
            paginacaoInsta.setMudouPagina(mudouPaginaInstaPostEngajamento);

        $(".loadingPostsMaiorEngajamentoInsta").hide();

        $(".instaposts_engajamento").empty();

        $.each(data.Posts, function (key, post) {
            var modeloPost = [];

            modeloPost.push('<div class="col-md-4">');
            modeloPost.push('<div class="ibox">');
            modeloPost.push('<div class="ibox-content">');

            if (post.urlImagem != null) {
                modeloPost.push('<div class="engajado_img">');
                modeloPost.push('<img src="' + post.imagem.caminho + '" />');
                modeloPost.push('</div>');
            } else {
                modeloPost.push('<div class="engajado_img">');
                modeloPost.push('<img src="' + diretorioImagens + '/instagram/no-photo.gif" />');
                modeloPost.push('</div>');
            }
            
            modeloPost.push('<div class="engajado_texto">');
            modeloPost.push('<div>');
            modeloPost.push('<span class="engajado_data">' + formataDataJson(post.dataPost) + '</span>');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-heart"></i> <span class="engajado_numero">' + post.totalCurtidas + '</span> Curtidas<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-comments-o"></i> <span class="engajado_numero">' + post.totalComentarios + '</span> Comentários<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div class="engajado_vermais">');
            //modeloPost.push('<a href="#"><i class="fa fa-eye"></i>Ver mais</a>');
            modeloPost.push('<a><span class="btn-vermais" data-idpostinsta="'+post.idPost+'"><i class="fa fa-eye"></i> Ver mais</span></a>');

            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".instaposts_engajamento").append(modeloPost.join(''));
        });


        $(".instaposts_engajamento .btn-vermais").click(function () {
            var id = $(this).attr("data-idpostinsta");

            mostraDetalhesDeUmPostInstagram(id);
        });

    });
}

function CarregaInstaGraficoEngajamentoHora(id, dtIni, dtFim) {


    $(".insta-horarios-engajamento tbody").empty();

    $(".loadingEngajamentoHoraInsta").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetInstaEngajamentoPorHora?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {
        
        $(".loadingEngajamentoHoraInsta").hide();
        $(".insta-horarios-engajamento tbody").empty();
        var modeloTabela = [];

        if (data != null && data.valorMaximo != null) {

            color = d3.scale.linear().domain([1, data.valorMaximo])
                .interpolate(d3.interpolateHcl)
                .range([d3.rgb("#e5faf7"), d3.rgb('#068a71')]);

            var i = 0;
            for (var dia = 1; dia <= 7; dia++) {
                modeloTabela.push('<tr>');

                modeloTabela.push('<td class="diasemana" data-valuedia="' + dia + '">' + RetornaDiaSemana(dia) + '</td>');

                for (var hora = 0; hora <= 23; hora++) {

                    var engajamento = data.Itens.filter(function (obj) {
                        return obj.DiaSemana == dia && obj.Hora == hora;
                    });

                    if (engajamento.length == 0 || engajamento === undefined || engajamento === null) {
                        modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "'  style=background-color:#edfffc data-toggle='ajuda-tooltip' data-placement='top' title='0 interações'></td>");

                    }
                    else {
                        modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "'  style=background-color:" + color(engajamento[0].Valor) + " data-toggle='ajuda-tooltip' data-placement='top' title='<div class=\"tooltip-diahora\">" + formataNumero(engajamento[0].Valor) + " interações <br>&#183; " + formataNumero(engajamento[0].reacoes) + " curtidas;<br>&#183; " + formataNumero(engajamento[0].comentarios) + " comentarios. </div>'></td>");

                    }

                }

                modeloTabela.push('</tr>');

            }
        }

        modeloTabela.push('<tr class=legenda_hora>');
        modeloTabela.push("<td></td>");

        for (var hora = 0; hora <= 23; hora++) {
            modeloTabela.push("<td data-valuehora='" + hora + "'>" + hora + "</td>");
        }

        modeloTabela.push('</tr>');

        $(".insta-horarios-engajamento tbody").append(modeloTabela.join(''));
        $(".insta-horarios-engajamento tbody td[data-toggle='ajuda-tooltip']").tooltip({ container: 'body', html: 'true' });

        $("td.HoverEnga").hover(function () {

            var dia = $(this).data("dia");
            var hora = $(this).data("hora");

            $("tbody > tr >  td[data-valuedia='" + dia + "']").toggleClass("active");

            $(".legenda_hora > td[data-valuehora='" + hora + "']").toggleClass("active");


        });

    });
}

function mudouPaginaInstaPostEngajamento(pagina) {

    var dtInicial = $("#dtinicialinsta").datepicker('getDate');
    var dtFinal = $("#dtfinalinsta").datepicker('getDate');

    CarregaInstaPostsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, 6, pagina)
}

function mostraDetalhesDeUmPostInstagram(id) {

    var url = "/api/EmpresaRedesSociaisAPI/GetPostInsta?idPost=" + id;

    $.getJSON(url, function (data) {
        TotalComentariosInstaBotao = data.totalComentarios;
        $("#detalhesPostInsta").attr("data-idpost", data.idPost);
        $("#detalhesPostInsta #usuariopostinsta").text(data.usuarioPost);
        $("#detalhesPostInsta #dataPostInsta").text(formataDataHoraJson(data.dataPost));
        $("#detalhesPostInsta #textopostinsta").html(insereQuebraLinha(data.postagem));


        if (data.imagem != null) {
            $("#detalhesPostInsta #imagempostinsta").show();
            $("#detalhesPostInsta #imagempostinsta").attr("src", data.imagem.caminho);
        }
        else {
            $("#detalhesPostInsta #imagempostinsta").hide();
        }


        $("#detalhesPostInsta #curtidaspostinsta").text(formataNumero(data.totalCurtidas));
        $("#detalhesPostInsta #comentariospostinsta").text(formataNumero(data.totalComentarios));

        if (data.totalComentarios > 3) {
            $("#btnCarregaComentariosInsta").show();
        }
        else {
            $("#btnCarregaComentariosInsta").hide();
        }

        var comentarios = MontaTemplateComentariosInsta(data.comentarios);

        $("#detalhesPostInsta .social-footer").empty();
        $("#detalhesPostInsta .social-footer").attr("data-carregados", data.comentarios.length);
        $("#detalhesPostInsta .social-footer").append(comentarios);

        exibirModal("#detalhesPostInsta", undefined, undefined, 0, undefined);
    });

}

function MontaTemplateComentariosInsta(comentarios) {

    var arrComentarios = [];
    $(comentarios).each(function (i, e) {
        arrComentarios.push('<div class="social-comment" data-id="' + e.idPost + '" data-idinstagram="' + e.idPost + '">');
        arrComentarios.push('<div class="media-body">');
        arrComentarios.push('<div class="corusuario" ><strong>');
        arrComentarios.push(e.nomeUsuario);
        arrComentarios.push('</strong></div> ');
        arrComentarios.push(insereQuebraLinha(e.postagem));
        arrComentarios.push('<br />');
        arrComentarios.push('<small class="text-muted">' + formataDataHoraJson(e.dataComentario) + '</small>');
        //arrComentarios.push('</div>');
        arrComentarios.push('</div>');
        arrComentarios.push('</div>');

    });
    return arrComentarios.join('');
}

function CarregaMaisComentariosInsta(btnpost) {

    var idPost = $("#detalhesPostInsta").attr("data-idpost");
    var inicial = $("#detalhesPostInsta .social-footer").attr("data-carregados");
    var quantidade = 5;

    var url = "/api/EmpresaRedesSociaisAPI/GetMaisComentariosDeUmPostInsta?idPost=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {

        var htmlComentarios = MontaTemplateComentariosInsta(comentarios);

        $("#detalhesPostInsta .social-footer").attr("data-carregados", parseInt(inicial) + comentarios.length);
        $("#detalhesPostInsta .social-footer").append(htmlComentarios);
        
        var total = (parseInt(inicial) + comentarios.length);
        
        if (total == TotalComentariosInstaBotao) {
            
           $('.form-modal.detalhes-post').find('#btnCarregaComentariosInsta').hide();
        }


    });

}

function CarregaNuvemTermosPostsInstagram(id, dtIni, dtFim) {

    $('.insta_nuvem_semtermos').hide();
    $(".insta_post_nuvem_termos").hide();
    $(".insta_coment_nuvem_termos").hide();
    $(".loadingNuvemInsta").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetInstagramTermosPosts?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        //Nuvem de termos
        if (data.length != 0) {
            var minFreq = data[0].frequencia;
            var maxFreq = data[0].frequencia;


            var listaTermos = data.map(function (t) {

                if (t.frequencia < minFreq)
                    minFreq = t.frequencia;

                if (t.frequencia > maxFreq)
                    maxFreq = t.frequencia;

                return [t.termo, t.frequencia];
            });

            var nuvem = new NuvemTermos();

            $(".loadingNuvemInsta").hide();
            $(".insta_post_nuvem_termos").show();


            nuvem.inicializaNuvemTermos(".insta_post_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {
            $(".loadingNuvemInsta").hide();
            $('.insta_nuvem_semtermos').show();
        }
    });

}

function CarregaNuvemTermosComentariosInstagram(id, dtIni, dtFim) {
   
    $(".insta_post_nuvem_termos").hide();
    $(".insta_coment_nuvem_termos").hide();
    $(".loadingNuvemInsta").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetInstagramTermosComentarios?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {
        if (data.length != 0) {
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

            var nuvem = new NuvemTermos();

            $(".loadingNuvemInsta").hide();
            $(".insta_coment_nuvem_termos").show();

            nuvem.inicializaNuvemTermos(".insta_coment_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {
            $(".loadingNuvemInsta").hide();
            $('.insta_coment_nuvem_termos').html('<div style="top:50%; left:50%; margin-top:50px; margin-left:50%;" ><img src="' + diretorioImagens + '/icon_chateado.png">&nbsp; Não há dados para esta consulta</div>').show();
        }
    });
}