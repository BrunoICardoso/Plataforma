var paginacaoYoutube = new Paginacao();
var paginacaoYoutubeTimeline = new Paginacao();
var totalYoutubeVideosEngajamento;
var totalPostsYoutubeTimeLine;
var totalcomentariosYtbotao;
var totalComentariosLinhatempo = [];
var PostComentYT ;
var palavraYT;

var ordenacao = $("#youtube_filtro_timeline").val();

$(document).ready(function () {
    $("#YTSelect").hide();

    $(".container-redesocial[data-nomerede='youtube'] #btnAnalise").click(function () {
        $("#YTSelect").hide();

        $(".container-redesocial[data-nomerede='youtube'] #btnLinhaTempo").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='youtube'] .container_analise").show();
        $(".container-redesocial[data-nomerede='youtube'] .container_linhadotempo").hide();
    });

    $(".container-redesocial[data-nomerede='youtube'] #btnLinhaTempo").click(function () {

        palavraYT = null;
        PostComentYT = null;

        $(".youtube_filtro_timeline").show()

            var dtInicialYt = $("#dtinicialyoutube").datepicker('getDate');
            var dtFinalYt = $("#dtfinalyoutube").datepicker('getDate');
            var ordenacaoYt = $("#youtube_filtro_timeline").val();
            CarregaYoutubeLinhadoTempo(idEmpresa, dtInicialYt, dtFinalYt, 6, 1, ordenacaoYt, palavraYT, PostComentYT);

        $(".container-redesocial[data-nomerede='youtube'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='youtube'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='youtube'] .container_linhadotempo").show();
    });



    $(document).on('click', '.youtube_post_nuvem_termos #Spanid', function () {

        palavraYT = $(this).html();
        PostComentYT = "Post";

        $(".youtube_filtro_timeline").show()

        var dtInicialYt = $("#dtinicialyoutube").datepicker('getDate');
        var dtFinalYt = $("#dtfinalyoutube").datepicker('getDate');
        var ordenacaoYt = $("#youtube_filtro_timeline").val();
        CarregaYoutubeLinhadoTempo(idEmpresa, dtInicialYt, dtFinalYt, 6, 1, ordenacaoYt,palavraYT, PostComentYT);

        $(".container-redesocial[data-nomerede='youtube'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='youtube'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='youtube'] .container_linhadotempo").show();
    });

    $(document).on('click', '.youtube_coment_nuvem_termos #Spanid', function () {

        palavraYT = $(this).html();
        PostComentYT = "Comentario";

        $(".youtube_filtro_timeline").show()

        var dtInicialYt = $("#dtinicialyoutube").datepicker('getDate');
        var dtFinalYt = $("#dtfinalyoutube").datepicker('getDate');
        var ordenacaoYt = $("#youtube_filtro_timeline").val();
        CarregaYoutubeLinhadoTempo(idEmpresa, dtInicialYt, dtFinalYt, 6, 1, ordenacaoYt, palavraYT, PostComentYT);

        $(".container-redesocial[data-nomerede='youtube'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='youtube'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='youtube'] .container_linhadotempo").show();

    });

    
   
});

$('#youtubedatas').datepicker({
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    format: 'dd/mm/yyyy',
});

function CarregaYoutubeLinhadoTempo(id, dtInicial, dtFinal, qtdVideos, pagina, ordenacao, palavra, PostComent) {

    if (palavra == '') {

        palavra = null;

    }

    var url = "/api/EmpresaRedesSociaisAPI/GetTimelineYoutube?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtInicial) + "&dtFinal=" + ConverteDataParaDataJson(dtFinal) + "&postsPagina=" + qtdVideos + "&pagina=" + pagina + "&ordenacao=" + ordenacao+"&palavra=" + palavra + "&PostComentario=" + PostComent;;

    $.getJSON(url, function (data) {

     
            totalPostsYoutubeTimeLine = data.TotalDeVideos;

            paginacaoYoutubeTimeline.CriaPaginacao(".youtube-pag-poststimeline", parseInt((totalPostsYoutubeTimeLine / 6) + 0.9), totalPostsYoutubeTimeLine, pagina);
            paginacaoYoutubeTimeline.setMudouPagina(mudouPaginaYoutubeTimeLine);
     
        $(".youtube_posts_timeline").empty();

        $.each(data.Videos, function (key, post) {
            var modeloPost = [];

            totalComentariosLinhatempo[post.idVideo] = post.qtdComentariosReal;
            modeloPost.push('<div class="social-feed-box" data-idpost="' + post.idVideo + '">');
            modeloPost.push('<div class="row social-avatar">');
           


            if (post.nomeImagem != null) {
                templateComImagemYouTube(modeloPost, post);
            } else {
                templateSemImagemYouTube(modeloPost, post)
            }
    
                

            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".youtube_posts_timeline").append(modeloPost.join(''));

        });
    });
}

function templateComImagemYouTube(modeloPost, post) {

    // Video YouTube
    modeloPost.push('<div class="col-md-6 text-center" style="height: 630px; margin-top: 10px;"> <iframe class="embed-responsive-item" width="100%" height="315" src="https://www.youtube.com/embed/' + post.nomeImagem.split('.')[0] + '" frameborder="0" allowfullscreen></iframe></div>');
    //-------------------
    
    //Mostraa Data e hora  do Post
    modeloPost.push('<div class="col-md-6 social-avatar" style=" height:630px; overflow-y: auto; margin-top: 10px;">');
    modeloPost.push('<div class="media-body">');
    modeloPost.push('<a id="usuariopost" href="">');
    modeloPost.push('</a>');
    modeloPost.push('<strong class="text-muted" id="dataPost">');
    modeloPost.push(formataDataHoraJson(post.dataVideo));
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

    // 4 labels Visualizações, Curtidas, Descurtidas e Comentários
    modeloPost.push('<hr>');
    modeloPost.push('<div class="btn-group2">');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-eye"></i> <span id="visualizacoespost">');
    modeloPost.push(post.qtdVisualizacoes);
    modeloPost.push('</span> Visualizações</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-up"></i> <span id="curtidaspost">');
    modeloPost.push(post.qtdCurtidas);
    modeloPost.push('</span> Curtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-down"></i> <span id="descurtidaspost">');
    modeloPost.push(post.qtdDescurtidas);
    modeloPost.push('</span> Descurtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-comments-o"></i> <span id="comentariospost">');
    modeloPost.push(post.qtdComentarios);
    modeloPost.push('</span> Comentários</div>');
    modeloPost.push('</div>');
    modeloPost.push('<hr>');
    //-----------------------------------------------------------

    // Comentarios
    modeloPost.push('<div class="social-footer" data-carregados="'+post.comentarios.length+'">');

    var comentarios = MontaTemplateComentariosYoutube(post.comentarios);
    modeloPost.push(comentarios);

    modeloPost.push('</div>');
    //----------------------------------------------------------

    // Botão Ver Mais e Contagem de comentárioos
    if (post.qtdComentarios > 0) {

        var TotaIncremento = 0;

        $.each(post.comentarios, function (key, com) {

            TotaIncremento = TotaIncremento + com.totalRespostas;

        });

        //modeloPost.push('<button id="btnCarregaComentariosVideoTimeline' + post.idVideo + '" onclick="btnMaisComentariosYoutube(this)" class="btn btn-link"> Ver Comentários </button> <div style="float: right; display:inline-table; margin-top: 10px;"> <span id="ComentariosCarregados' + post.idVideo + '"> ' + (post.comentarios.length + TotaIncremento) + ' </span> de <span> ' + post.qtdComentarios + ' </span></div>');
        modeloPost.push(  ((post.comentarios.length + TotaIncremento) == post.qtdComentarios ? '' : '<button id="btnCarregaComentariosVideoTimeline' + post.idVideo + '" onclick="btnMaisComentariosYoutube(this)" class="btn btn-link"> Ver Comentários </button>') + '<div style="float: right; display:inline-table; margin-top: 10px;"> <span id="ComentariosCarregados' + post.idVideo + '"> ' + (post.comentarios.length + TotaIncremento) + ' </span> de <span> ' + post.qtdComentarios + ' </span></div>');

        

       }
    //-----------------------------------------------------------

    modeloPost.push('</div>');
}

function templateSemImagemYouTube(modeloPost, post) {

    modeloPost.push('<div class=" col-md-6 text-left" style="background: #f3f3f4; height:630px; overflow-y: auto;" >');
    
    //Mostraa Data e hora  do Post
    modeloPost.push('<div class="media-body">');
    modeloPost.push('<a id="usuariopost" href="">');
    modeloPost.push('</a>');
    modeloPost.push('<strong class="text-muted" id="dataPost">');
    modeloPost.push(formataDataHoraJson(post.dataVideo));
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

    // 4 labels Visualizações, Curtidas, Descurtidas e Comentários
    
    modeloPost.push('<div class="btn-group2">');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-eye"></i> <span id="visualizacoespost">');
    modeloPost.push(post.qtdVisualizacoes);
    modeloPost.push('</span> Visualizações</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-up"></i> <span id="curtidaspost">');
    modeloPost.push(post.qtdCurtidas);
    modeloPost.push('</span> Curtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-thumbs-down"></i> <span id="descurtidaspost">');
    modeloPost.push(post.qtdDescurtidas);
    modeloPost.push('</span> Descurtidas</div>');
    modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-comments-o"></i> <span id="comentariospost">');
    modeloPost.push(post.qtdComentarios);
    modeloPost.push('</span> Comentários</div>');
    modeloPost.push('</div>');
    //-----------------------------------------------------------

    modeloPost.push('</div>');
    




   modeloPost.push('<div class="col-md-6 social-avatar" style=" height:630px; overflow-y: auto; margin-top: 10px;">');


    // Comentarios
   modeloPost.push('<div class="social-footer" data-carregados="' + post.comentarios.length + '">');

    var comentarios = MontaTemplateComentariosYoutube(post.comentarios);
    modeloPost.push(comentarios);

    modeloPost.push('</div>');
    //----------------------------------------------------------

    // Botão Ver Mais e Contagem de comentárioos
    if (post.qtdComentarios > 0) {

        var TotaIncremento = 0;

        $.each(post.comentarios, function (key, com) {

            TotaIncremento = TotaIncremento + com.totalRespostas;

        });
                
        modeloPost.push('<button id="btnCarregaComentariosVideoTimeline' + post.idVideo + '" onclick="btnMaisComentariosYoutube(this)" class="btn btn-link"> ' + (post.comentarios.length + TotaIncremento) == post.qtdComentarios ? '' : 'Ver comentários' + ' </button> <div style="float: right; display:inline-table; margin-top: 10px;"> <span id="ComentariosCarregados' + post.idVideo + '"> ' + (post.comentarios.length + TotaIncremento) + ' </span> de <span> ' + post.qtdComentarios + ' </span></div>');
    }
    //-----------------------------------------------------------

    modeloPost.push('</div>');


}

function btnMaisComentariosYoutube(btnpost) {

    var idPost = $(btnpost).parent().parent().parent().data("idpost");
    var containerComentarios = $(btnpost).parent().find(".social-footer");
    var inicial = $(containerComentarios).attr("data-carregados");
    var quantidade = 5;

    CarregaMaisComentariosYoutubeTimeLine(idPost, inicial, quantidade);

}

function CarregaMaisComentariosYoutubeTimeLine(idPost, inicial, quantidade) {

    var containerComentarios = $('.social-feed-box[data-idpost="' + idPost + '"] .social-footer');
    
    var url = "/api/EmpresaRedesSociaisAPI/GetRetornaComentariosDeUmVideo?id=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {
        
        var htmlComentariosVideo = MontaTemplateComentariosYoutube(comentarios);

        var ComentariosCarregados = "#ComentariosCarregados" + comentarios[0].idvideo;
        var total = (parseInt(inicial) + comentarios.length);
        var totalInicial = $(ComentariosCarregados).text();
        var totalComentarios = 0;
        var TotaIncremento = 0;
        
        $.each(comentarios, function (key, com) {

            TotaIncremento = TotaIncremento + com.totalRespostas; 

        });


        totalComentarios = parseInt(totalInicial) + comentarios.length + TotaIncremento;

        $(containerComentarios).attr("data-carregados", total);
        $(containerComentarios).append(htmlComentariosVideo);
        $(ComentariosCarregados).text(totalComentarios);
        
        var total = parseInt(inicial) + comentarios.length;
        
        if (total == totalComentariosLinhatempo[comentarios[0].idvideo]) {
            
            $('.youtube_posts_timeline').find('#btnCarregaComentariosVideoTimeline' + comentarios[0].idvideo).hide();
        }


    });

}

function mudouPaginaYoutubeTimeLine(pagina) {

    if (PostComentYT == undefined) {
        PostComentYT == null
    }

    if (palavraYT == undefined) {
        palavraYT == null
    }


    var dtInicial = $("#dtinicialyoutube").datepicker('getDate');
    var dtFinal = $("#dtfinalyoutube").datepicker('getDate');

    CarregaYoutubeLinhadoTempo(idEmpresa, dtInicial, dtFinal, 6, pagina, ordenacao, palavraYT, PostComentYT);
}

function CarregaYoutubeGraficoCrescimentoInscritos(id, dtIni, dtFim) {

    $("#lblyoutubecrescfas").empty();
    $("#sparklineyoutubecrescfas").empty();

    $(".loadingCrescimentoInscritosYt").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetGraficoYoutubeCrescimentoInscritos?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingCrescimentoInscritosYt").hide();

        var totalFas = 0;
        var valores = data.map(function (x) {
            totalFas = x.valor;
            return x.valor;
        });

        var datas = data.map(function (x) {
            return x.data;
        });

        $("#lblyoutubecrescfas").text(formataNumero(totalFas));
        $("#sparklineyoutubecrescfas").sparkline(valores, {
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

function CarregaYoutubeGraficosInteracoes(id, dtIni, dtFim) {


    $("#lblyoutubeqtdposts").empty();
    $("#sparklineyoutubeqtdposts").empty();
    $("#lblyoutubeqtdinteracoes").empty();
    $("#sparklineyoutubeqtdinteracoes").empty();
    $("#lblyoutubeqtdvisualizacoes").empty();
    $("#sparklineyoutubeqtdvisualizacoes").empty();
    $("#lblyoutubeqtdcurtidas").empty();
    $("#sparklineyoutubeqtdcurtidas").empty();
    $("#lblyoutubeqtdcomentarios").empty();
    $("#sparklineyoutubeqtdcomentarios").empty();

    $(".loadingInteracoesYt").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetYoutubeInteracoes?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        var totalVideos = 0;
        var arrqtdvideos = data.map(function (x) {
            totalVideos += x.videos;
            return x.videos;
        });

        var totalInteracoes = 0;
        var arrqtdinteracoes = data.map(function (x) {
            totalInteracoes += x.interacoes;
            return x.interacoes;
        });

        var totalVisualizacoes = 0;
        var arrqtdvisualizacoes = data.map(function (x) {
            totalVisualizacoes += x.visualizacoes;
            return x.visualizacoes;
        })

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

        $(".loadingInteracoesYt").hide();
        
        $("#lblyoutubeqtdposts").text(formataNumero(totalVideos));
        $("#sparklineyoutubeqtdposts").sparkline(arrqtdvideos, {
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

        $("#lblyoutubeqtdinteracoes").text(formataNumero(totalInteracoes));
        $("#sparklineyoutubeqtdinteracoes").sparkline(arrqtdinteracoes, {
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

        $("#lblyoutubeqtdvisualizacoes").text(formataNumero(totalVisualizacoes));
        $("#sparklineyoutubeqtdvisualizacoes").sparkline(arrqtdvisualizacoes, {
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

        $("#lblyoutubeqtdcurtidas").text(formataNumero(totalCurtidas));
        $("#sparklineyoutubeqtdcurtidas").sparkline(arrqtdcurtidas, {
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
        
        $("#lblyoutubeqtdcomentarios").text(formataNumero(totalComentarios));
        $("#sparklineyoutubeqtdcomentarios").sparkline(arrqtdcomentarios, {
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

function CarregaYoutubeVideosMaiorEngajamento(id, dtIni, dtFim, qtdPosts, pagina) {

    $(".youtubevideos_engajamento").empty();
    $(".pag-youtubevideosengajamento").empty();

    $(".loadingPostsMaiorEngajamentoYoutube").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetYoutubeVideosMaisEngajamento?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina;

    $.getJSON(url, function (data) {

        $(".loadingPostsMaiorEngajamentoYoutube").hide();

        totalYoutubeVideosEngajamento = data.TotalDeVideos;
        paginacaoYoutube.CriaPaginacao(".pag-youtubevideosengajamento", parseInt((totalYoutubeVideosEngajamento / 6) + 0.9), totalYoutubeVideosEngajamento, pagina);
        paginacaoYoutube.setMudouPagina(mudouPaginaYoutubeVideoEngajamento);
        
        $(".youtubevideos_engajamento").empty();

        $.each(data.Videos, function (key, post) {
            var modeloPost = [];

            modeloPost.push('<div class="col-md-4">');
            modeloPost.push('<div class="ibox">');
            modeloPost.push('<div class="ibox-content">');

            if (post.nomeImagem != null) {
                modeloPost.push('<div class="engajado_img">');
                modeloPost.push('<img src="' + diretorioImagens + '/youtube/' + post.nomeImagem + '" />');
                modeloPost.push('</div>');
            }
            else {
                modeloPost.push('<div class="engajado_img">');
                modeloPost.push('<img src="' + diretorioImagens + '/youtube/no-photo.gif" />');
                modeloPost.push('</div>');
            }

            modeloPost.push('<div class="engajado_texto">');
            modeloPost.push('<div>');
            modeloPost.push('<span class="engajado_data">' + formataDataJson(post.dataVideo) + '</span>');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-thumbs-up"></i> <span class="engajado_numero">' + post.qtdCurtidas + '</span> Gostaram<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-thumbs-down"></i> <span class="engajado_numero">' + post.qtdDescurtidas + '</span> Não gostaram<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-eye"></i> <span class="engajado_numero">' + post.qtdVisualizacoes + '</span> Visualizações<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-comments"></i> <span class="engajado_numero">' + post.qtdComentarios + '</span> Comentários<br />');
            modeloPost.push('</div>');

            modeloPost.push('<div class="engajado_vermais">');
            modeloPost.push('<a class="VerMaisVideoYoutube" data-idpostyoutube="' + post.idVideo + '" ><i class="fa fa-eye"></i>Ver mais</a>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".youtubevideos_engajamento").append(modeloPost.join(''));

            $(".youtubevideos_engajamento .VerMaisVideoYoutube").click(function () {
                var id = $(this).attr("data-idpostyoutube");
                mostraDetalhesDeUmVideoYoutube(id);
            });

        });
    });


}

function CarregaYoutubeGraficoEngajamentoHora(id, dtIni, dtFim) {

    $(".youtube-horarios-engajamento tbody").empty();

    $(".loadingEngajamentoHoraYoutube").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetYoutubeEngajamentoPorHora?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {
      
        $(".loadingEngajamentoHoraYoutube").hide();
        $(".youtube-horarios-engajamento tbody").empty();
        var modeloTabela = [];

        var _valor = 0;

        if (data != null) {

            if (data.valorMaximo != null)
                _valor = data.valorMaximo;

            color = d3.scale.linear().domain([1, _valor])
            .interpolate(d3.interpolateHcl)
            .range([d3.rgb("#e5faf7"), d3.rgb('#068a71')]);

            var i = 0;
            for (var dia = 1; dia <= 7; dia++) {
                modeloTabela.push('<tr>');

                modeloTabela.push('<td class="diasemana" data-valuedia="' + dia +'">' + RetornaDiaSemana(dia) + '</td>');

                for (var hora = 0; hora <= 23; hora++) {

                    var engajamento = data.Itens.filter(function (obj) {
                        return obj.DiaSemana == dia && obj.Hora == hora;
                    });

                    if (engajamento.length == 0 || engajamento === undefined || engajamento === null) {
                        modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "' style=background-color:#edfffc data-toggle='ajuda-tooltip' data-placement='top' title='0 interações'></td>");

                    }
                    else {

                        modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "' style=background-color:" + color(engajamento[0].Valor) + " data-toggle='ajuda-tooltip' data-placement='top' title='<div class=\"tooltip-diahora\">" + formataNumero(engajamento[0].Valor) + " interações <br>&#183; " + formataNumero(engajamento[0].reacoes) + " curtidas;<br>&#183; " + formataNumero(engajamento[0].comentarios) + " comentarios. </div>'></td>");

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
        }

        
        $(".youtube-horarios-engajamento tbody").append(modeloTabela.join(''));
        $(".youtube-horarios-engajamento  tbody td[data-toggle='ajuda-tooltip']").tooltip({ container: 'body', html: 'true' });


        $("td.HoverEnga").hover(function () {

            var dia = $(this).data("dia");
            var hora = $(this).data("hora");

            $("tbody > tr >  td[data-valuedia='" + dia + "']").toggleClass("active");

            $(".legenda_hora > td[data-valuehora='" + hora + "']").toggleClass("active");


        });


    });
}

function mudouPaginaYoutubeVideoEngajamento(pagina) {

    var dtInicial = $("#dtinicialyoutube").datepicker('getDate');
    var dtFinal = $("#dtfinalyoutube").datepicker('getDate');

    CarregaYoutubeVideosMaiorEngajamento(idEmpresa, dtInicial, dtFinal, 6, pagina)
}

function mostraDetalhesDeUmVideoYoutube(id) {

    var url = "/api/EmpresaRedesSociaisAPI/GetVideoYoutube?id=" + id;

    $.getJSON(url, function (data) {
        totalcomentariosYtbotao = data.qtdComentariosReal;
        $("#detalhesVideoYoutube").attr("data-idpostyoutube", data.idVideo);
        $("#detalhesVideoYoutube #usuariovideoyoutube").text(data.usuariopost);
        $("#detalhesVideoYoutube #dataVideoYoutube").text(formataDataHoraJson(data.dataVideo));
        $("#detalhesVideoYoutube #textovideoyoube").html(insereQuebraLinha(data.postagem));
        $("#detalhesVideoYoutube #visualizacoesvideoyoutube").text(formataNumero(data.qtdVisualizacoes));
        $("#detalhesVideoYoutube #curtidasvideoyoutube").text(formataNumero(data.qtdCurtidas));
        $("#detalhesVideoYoutube #descurtidasvideoyoutube").text(formataNumero(data.qtdDescurtidas));
        //$("#detalhesVideoYoutube #favoritadosvideoyoutube").text(formataNumero(data.qtdFavoritados));
        $("#detalhesVideoYoutube #comentariosvideoyoutube").text(formataNumero(data.qtdComentarios));

        $("#detalhesVideoYoutube #videoYoutube").html('<iframe width="590" height="335" src="https://www.youtube.com/embed/' + data.nomeImagem.split('.')[0] + '" frameborder="0" allowfullscreen></iframe>');

        if (data.nomeimagem != null) {
            $("#imagemvideoyoutube").show();
            $("#imagemvideoyoutube").attr('src', diretorioImagens + "/youtube/" + data.nomeImagem);
        }
        else {
            $("#imagemvideoyoutube").hide();
        }


        if (data.qtdComentarios > 3) {
            $("#btnCarregaComentariosYoutube").show();
        }
        else {
            $("#btnCarregaComentariosYoutube").hide();
        }
        
        var comentarios = MontaTemplateComentariosYoutube(data.comentarios);

        $("#detalhesVideoYoutube .social-footer").empty();
        $("#detalhesVideoYoutube .social-footer").attr("data-carregados", data.comentarios.length);
        $("#detalhesVideoYoutube .social-footer").append(comentarios);

        exibirModal("#detalhesVideoYoutube", undefined, undefined, 0, undefined);
    });

}

function MontaTemplateComentariosYoutube(comentarios) {

    var arrComentarios = [];
    $(comentarios).each(function (i, e) {
        arrComentarios.push('<div class="social-comment" data-id="' + e.idcomentario + '" data-idyoutube="' + e.idVideo + '">');
        arrComentarios.push('<div class="media-body">');
        arrComentarios.push('<div class="corusuario"><strong>');
        arrComentarios.push(e.nomeUsuario);
        arrComentarios.push('</strong></div> ');
        arrComentarios.push(insereQuebraLinha(e.postagem));
        arrComentarios.push('<br />');
        arrComentarios.push('<span class="small" style="color: gray;" > <i class="fa fa-thumbs-o-up" style="color: gray;"></i> ' + e.totalCurtidas + ' </span> Curtidas  <span class="small"><i class="fa fa-comments-o" style="color: gray;"></i>  ' + e.totalRespostas + '  Respostas </span> ');
        arrComentarios.push('<small class="text-muted"> - ' + formataDataHoraJson(e.dataComentario) + '</small>');
        arrComentarios.push('</div>');
        arrComentarios.push('</div>');

        arrComentarios.push('<div style="margin-left:40px;">');
        respostasYouTube = MontaTemplateRespostasYoutube(e.respostas, e.totalRespostas > e.respostas.length);
        arrComentarios.push(respostasYouTube);
        //arrComentarios.push('</div>');
        arrComentarios.push('</div>');

    });
    return arrComentarios.join('');
}

function MontaTemplateRespostasYoutube(respostasYoutube, mostraMaisRespostasYouTube) {

    var arrRespostas = [];
    
    if (respostasYoutube.length > 0) {
        $(respostasYoutube).each(function (i, r) {
            arrRespostas.push('<div class="social-comment resposta respostayoutube" data-id="' + r.idcomentarioresposta + '" data-idyoutube="' + r.idcomentarioresposta + '">');
            arrRespostas.push('<div class="media-body">');
            arrRespostas.push('<div class="corusuario"><strong>');
            arrRespostas.push(r.nomeUsuario);
            arrRespostas.push('</strong></div> ');
            arrRespostas.push(insereQuebraLinha(r.postagem));
            arrRespostas.push('<br />');
            arrRespostas.push('<span class="small" style="color: gray;"><i class="fa fa-thumbs-up" style="color: gray;"></i> ' + r.totalCurtidas + ' curtidas</span> - ');
            arrRespostas.push('<small class="text-muted">' + formataDataHoraJson(r.dataComentario) + '</small>');
            arrRespostas.push('</div>');
            arrRespostas.push('</div>');
        });

        if (mostraMaisRespostasYouTube) {
            
            arrRespostas.pop();
            arrRespostas.push('<div class="small text-success" onclick="CarregaMaisRespostasYouTube(this)"><i class="fa fa-reply"></i> ver mais respostas</div>');
            arrRespostas.push('</div>');
        }
       
    }
    return arrRespostas.join('');

}
    
function CarregaMaisComentariosYouTube() {

    var idPost = $("#detalhesVideoYoutube").attr("data-idpostyoutube");
    var inicial = $("#detalhesVideoYoutube .social-footer").attr("data-carregados");
    var quantidade = 5;

    var url = "/api/EmpresaRedesSociaisAPI/GetRetornaComentariosDeUmVideo?id=" + idPost + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {

        var htmlComentarios = MontaTemplateComentariosYoutube(comentarios);

        $("#detalhesVideoYoutube .social-footer").attr("data-carregados", parseInt(inicial) + comentarios.length);
        $("#detalhesVideoYoutube .social-footer").append(htmlComentarios);

        var total = parseInt(inicial) + comentarios.length;

        if (total == totalcomentariosYtbotao) {

            $('.form-modal.detalhes-post').find('#btnCarregaComentariosYoutube').hide();
        }

    });

}

function CarregaMaisRespostasYouTube(b){
    
    var idyoutube = $(b).parent().attr("data-idyoutube");
    var inicial = $(b).parent().parent().find('.resposta').length;

    var quantidade = 0;

    var containerRespostas = $(b).parent().parent();

    $(b).remove();

    var url = "/api/EmpresaRedesSociaisAPI/GetRetornaRespostasComentarioVideo?idComentario=" + idyoutube + "&inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (respostas) {

        var htmlRespostas = MontaTemplateRespostasYoutube(respostas);

        $(containerRespostas).append(htmlRespostas);

    });

}

function CarregaNuvemDeTermosVideosYoutube(id, dtIni, dtFim) {

    $(".yt_nuvem_semtermos").hide();
    $(".youtube_post_nuvem_termos").hide();
    $(".youtube_coment_nuvem_termos").hide();
    $(".loadingNuvemYoutube").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetYoutubeTermosVideos?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

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

            $(".loadingNuvemYoutube").hide();
            $(".youtube_post_nuvem_termos").show();
           

            nuvem.inicializaNuvemTermos(".youtube_post_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {
            $(".loadingNuvemYoutube").hide();
            $(".yt_nuvem_semtermos").show();

        }
    });

}

function CarregaNuvemDeTermosComentariosYoutube(id, dtIni, dtFim) {


    $(".youtube_post_nuvem_termos").hide();
    $(".youtube_coment_nuvem_termos").hide();
    $(".loadingNuvemYoutube").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetYoutubeTermosComentarios?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

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

            $(".loadingNuvemYoutube").hide();
            $(".youtube_coment_nuvem_termos").show();

            nuvem.inicializaNuvemTermos(".youtube_coment_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {
            $(".loadingNuvemYoutube").hide();
            $('.youtube_coment_nuvem_termos').html('<div style="top:50%; left:50%; margin-top:50px; margin-left:50%;" ><img src="' + diretorioImagens + '/icon_chateado.png">&nbsp; Não há dados para esta consulta</div>').show();

        }
    });
}

function FecharModalYoutube(obj){
    $('#detalhesVideoYoutube').find('#videoYoutube').html('');
}