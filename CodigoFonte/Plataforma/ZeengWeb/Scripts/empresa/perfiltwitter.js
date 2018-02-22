var paginacaoTw = new Paginacao();
var paginacaoTwTimeline = new Paginacao();

var totalTweetsEngajamento;
var idtweetperfil;
var idtweetpost;
var totalPostsTwTimeLine;
var totalComentariosLinhaTempo = [];
var totalcomentarioTwiterAnalise;
var palavraTw;
var PostComentTw;

var ordenacao = $("#twitter_timeline_ordenacao").val();

if (!diretorioImagens && diretorioImagens == "")
    diretorioImagens = "/Images";

$(document).ready(function () {

    $("#TWSelect").hide();

    $(".container-redesocial[data-nomerede='twitter'] #btnAnalise").click(function () {
        $("#TWSelect").hide();
        $(".container-redesocial[data-nomerede='twitter'] #btnLinhaTempo").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='twitter'] .container_analise").show();
        $(".container-redesocial[data-nomerede='twitter'] .container_linhadotempo").hide();
    });

    $(".container-redesocial[data-nomerede='twitter'] #btnLinhaTempo").click(function () {

        PostComentTw = null;
        PostComentTw = null;

        $(".twitter_filtro_timeline").show();

            var dtInicialTw = $("#dtinicialtw").datepicker('getDate');
            var dtFinalTw = $("#dtfinaltw").datepicker('getDate');
            var ordenacaoTw = $("#twitter_timeline_ordenacao").val();
            CarregaTwLinhaDoTempo(idEmpresa, dtInicialTw, dtFinalTw, 6, 1, ordenacaoTw, palavraTw, PostComentTw);

        $(".container-redesocial[data-nomerede='twitter'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(this).addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='twitter'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='twitter'] .container_linhadotempo").show();
    });

    $(document).on('click', '.tw_post_nuvem_termos #Spanid', function () {

        palavraTw  = $(this).html();
        PostComentTw = "Post";

        $(".twitter_filtro_timeline").show();

        var dtInicialTw = $("#dtinicialtw").datepicker('getDate');
        var dtFinalTw = $("#dtfinaltw").datepicker('getDate');
        var ordenacaoTw = $("#twitter_timeline_ordenacao").val();
        CarregaTwLinhaDoTempo(idEmpresa, dtInicialTw, dtFinalTw, 6, 1, ordenacaoTw,palavraTw,PostComentTw);

        $(".container-redesocial[data-nomerede='twitter'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='twitter'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='twitter'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='twitter'] .container_linhadotempo").show();

    });

    $(document).on('click', '.tw_mencoes_nuvem_termos #Spanid', function () {

        palavraTw = $(this).html();
        PostComentTw = "Comentario";

        $(".twitter_filtro_timeline").show();

        var dtInicialTw = $("#dtinicialtw").datepicker('getDate');
        var dtFinalTw = $("#dtfinaltw").datepicker('getDate');
        var ordenacaoTw = $("#twitter_timeline_ordenacao").val();
        CarregaTwLinhaDoTempo(idEmpresa, dtInicialTw, dtFinalTw, 6, 1, ordenacaoTw,palavraTw,PostComentTw);

        $(".container-redesocial[data-nomerede='twitter'] #btnAnalise").removeClass("btn-primary").addClass("btn-white");
        $(".container-redesocial[data-nomerede='twitter'] #btnLinhaTempo").addClass("btn-primary").removeClass("btn-white");

        $(".container-redesocial[data-nomerede='twitter'] .container_analise").hide();
        $(".container-redesocial[data-nomerede='twitter'] .container_linhadotempo").show();

    });


    


});

function CarregaTwLinhaDoTempo(id, dtIni, dtFim, qtdPosts, pagina, ordenacao, palavra, PostComent) {

    if (palavra == '') {

        palavra = null;

    }

    var url = "/api/EmpresaRedesSociaisAPI/GetTwitterPostsEmpresa?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina + "&ordenacao=" + ordenacao + "&palavra=" + palavra + "&PostComentario=" + PostComent;
    
    $.getJSON(url, function (data) {
      

        //if (totalPostsTwTimeLine == undefined) {

            totalPostsTwTimeLine = data.TotalPosts;
            totalcomentarioTwiterAnalise = data.totalRetweets;
            paginacaoTwTimeline.CriaPaginacao(".twitter-pag-poststimeline", parseInt((totalPostsTwTimeLine / 6) + 0.9), totalPostsTwTimeLine, 1);
            paginacaoTwTimeline.setMudouPagina(mudouPaginaTwTimeLine);
        //}

        $(".twitter_posts_timeline").empty();

        $.each(data.Posts, function (key, post) {
                       
            var modeloPost = [];
            totalComentariosLinhaTempo[post.idPost] = post.totalrespostas;
            //modeloPost.push('<div class="social-feed-box" data-idpost="' + post.idPost + '" data-idTweetPerfil="' + post.idPerfil + '">');

            modeloPost.push('<div class="social-feed-box" style="padding-left: 15px; height:630px; min-height: 400px; margin-left: 10px;"  data-idpost="' + post.idPost + '">');
            modeloPost.push('<div class="row social-body" style="height:100%">');
         

            if (post.imagemTw.caminho != null && post.imagemTw.caminho != "" && post.imagemTw.altura > 0 && post.imagemTw.largura > 0) {                
                templateComImagemTwitter(modeloPost, post);

            } else {                
                templateSemImagemTwitter(modeloPost, post);
            }
            
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".twitter_posts_timeline").append(modeloPost.join(''));
        });
    });
}

 function templateComImagemTwitter(modeloPost, post) {

     post.imagem = post.imagemTw;

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

     // Favoritas e Retweets 
     modeloPost.push('<div class="btn-group2">');
     modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-heart"></i> <span id="curtidaspost">');
     modeloPost.push(post.totalFavoritadas);
     modeloPost.push('</span> curtidas</div>');

     modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-retweet"></i> <span id="compartilhamentospost">');
     modeloPost.push(post.totalRetweets);
     modeloPost.push('</span> Retweets</div>');
     modeloPost.push('</div>');
   
     ///-----------------------------------------

     // Comentarios//
     modeloPost.push('<div class="social-footer" data-carregados="' + post.comentarios.length + '">');

     var comentarios = MontaTemplateComentariosTw(post.comentarios);
     modeloPost.push(comentarios);

     modeloPost.push('</div>');
     //----------------------------

     // Botão Ver Mais e Contagem de comentárioos

     if (post.totalrespostas > 0) {
         modeloPost.push('<button id="btnCarregaComentariosTw' + post.idPost + '" onclick="btnCarregaMaisComentarios(this)" class="btn btn-link"> Ver comentários</button> <div style="float: right; display:inline-table;"> <span id="ComentariosCarregados' + post.idPost + '">  ' + post.comentarios.length + ' </span> de <span> ' + post.totalrespostas + ' </span></div>');
     }

     //-=---------------------------------
     modeloPost.push('</div>');
   }

 function templateSemImagemTwitter(modeloPost, post) {
 
     
     modeloPost.push('<div class=" col-md-6 text-left" style="background: #f3f3f4; height:610px; padding-top: 10px;">');
         
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


     // Favoritas e Retweets 
     modeloPost.push('<div class="btn-group2">');
     modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-heart"></i> <span id="curtidaspost">');
     modeloPost.push(post.totalFavoritadas);
     modeloPost.push('</span> curtidas</div>');

     modeloPost.push('<div class="post-label-interacoes"><i class="fa fa-retweet"></i> <span id="compartilhamentospost">');
     modeloPost.push(post.totalRetweets);
     modeloPost.push('</span> Retweets</div>');
     modeloPost.push('</div>');

     ///-----------------------------------------

     modeloPost.push('</div>');

     

     modeloPost.push('<div class="col-md-6 social-avatar" style=" height:630px; overflow-y: auto;">');


     // Comentarios//
     modeloPost.push('<div class="social-footer" data-carregados="' + post.comentarios.length + '">');

     var comentarios = MontaTemplateComentariosTw(post.comentarios);
     modeloPost.push(comentarios);

     modeloPost.push('</div>');
     //----------------------------


     // Botão Ver Mais e Contagem de comentárioos

     if (post.totalrespostas > 0) {
         modeloPost.push('<button id="btnCarregaComentariosTw' + post.idPost + '" onclick="btnCarregaMaisComentarios(this)" class="btn btn-link"> Ver comentários</button> <div style="float: right; display:inline-table;"> <span id="ComentariosCarregados' + post.idPost + '"> ' + post.comentarios.length + ' </span> de <span> ' + post.totalrespostas + ' </span></div>');
     }

     //-=---------------------------------

     modeloPost.push('</div>');

 }

 function btnCarregaMaisComentarios(btnpost) {

     var idtweetpostod = $(btnpost).parent().parent().parent().data("idpost");
     var containerComentarios = $(btnpost).parent().find(".social-footer");
     var inicial = $(containerComentarios).attr("data-carregados");
     var quantidade = 5;

     CarregaMaisComentariosTwTimeLine(idtweetpostod, inicial, quantidade);
 }

 function CarregaMaisComentariosTwTimeLine(idtweetpostod, inicial, quantidade) {

    var containerComentarios = $('.social-feed-box[data-idpost="' + idtweetpostod + '"] .social-footer');

    var url = "/api/EmpresaRedesSociaisAPI/GetMaisRetweets?idpost=" + idtweetpostod + " &inicial=" + inicial + "&quantidade=" + quantidade;

    $.getJSON(url, function (comentarios) {
        
        var htmlComentarios = MontaTemplateComentariosTw(comentarios);
        var total = (parseInt(inicial) + comentarios.length);
        var ComentariosCarregados = "#ComentariosCarregados" + comentarios[0].idPost;
        var totalInicial = $(ComentariosCarregados).text();
        var totalComentarios = 0;

        totalComentarios = parseInt(totalInicial) + comentarios.length;

        $(containerComentarios).attr("data-carregados", total);
        $(containerComentarios).append(htmlComentarios);
        $(ComentariosCarregados).text(totalComentarios);

        if (total == totalComentariosLinhaTempo[comentarios[0].idPost]) {

            $('.twitter_posts_timeline').find('#btnCarregaComentariosTw' + comentarios[0].idPost).hide();
        }

});
}

function CarregaTwGraficoCrescimentoSeguidores(id, dtIni, dtFim) {

    $("#lbltwcrescfas").empty();
    $("#sparklinetwcrescfas").empty();
    $(".loadingCrescimentoFasTw").show();

    var url = "/api/EmpresaRedesSociaisAPI/GetGraficoTwCrescimentoFas?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingCrescimentoFasTw").hide();

        var totalFas = 0;

        var valores = data.map(function (x) {
            totalFas = x.valor;
            return x.valor;
        });

        var datas = data.map(function (x) {
            return x.data;
        });



        $("#lbltwcrescfas").text(formataNumero(totalFas));
        $("#sparklinetwcrescfas").sparkline(valores, {
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

function CarregaTwGraficoQuantidadeDeInteracoes(id, dtIni, dtFim) {

    $("#lbltwqtdtweets").empty();
    $("#sparklinetwqtdtweets").empty();
    $("#lbltwqtdtweets").empty();
    $("#sparklinetwqtdtweets").empty();
    $("#lbltwqtdinteracoes").empty();
    $("#sparklinetwqtdinteracoes").empty();
    $("#lbltwqtdcurtidas").empty();
    $("#sparklinetwqtdcurtidas").empty();
    $("#lbltwqtdretweets").empty();
    $("#sparklinetwqtdretweets").empty();

    $(".loadingInteracoesTw").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetTwInteracoes?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {

        $(".loadingInteracoesTw").hide();

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

        var totalFavoritados = 0;
        var arrqtdfavoritados = data.map(function (x) {
            totalFavoritados += x.favoritados;
            return x.favoritados;
        });

        var totalRetweets = 0;
        var arrqtdretweets = data.map(function (x) {
            totalRetweets += x.retweets;
            return x.retweets;
        });

        var datas = data.map(function (x) {
            return formataDataJson(x.data);
        });


        $("#lbltwqtdtweets").text(formataNumero(totalPosts));
        $("#sparklinetwqtdtweets").sparkline(arrqtdposts, {
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

        $("#lbltwqtdinteracoes").text(formataNumero(totalInteracoes));
        $("#sparklinetwqtdinteracoes").sparkline(arrqtdinteracoes, {
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

        $("#lbltwqtdcurtidas").text(formataNumero(totalFavoritados));
        $("#sparklinetwqtdcurtidas").sparkline(arrqtdfavoritados, {
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

        $("#lbltwqtdretweets").text(formataNumero(totalRetweets));
        $("#sparklinetwqtdretweets").sparkline(arrqtdretweets, {
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

function CarregaTwGraficoEnganjamentoHora(id, dtIni, dtFim) {

    $(".tw-horarios-engajamento tbody").empty();

    $(".loadingEngajamentoHoraTw").show();
      
    var url = "/api/EmpresaRedesSociaisAPI/GetTwEngajamentoPorHora?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

    $.getJSON(url, function (data) {


        $(".loadingEngajamentoHoraTw").hide();
        $(".tw-horarios-engajamento tbody").empty();
        var modeloTabela = [];

        color = d3.scale.linear().domain([1, data.valorMaximo])
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

                    modeloTabela.push("<td class='HoverEnga' data-dia='" + dia + "' data-hora='" + hora + "' style=background-color:" + color(engajamento[0].Valor) + " data-toggle='ajuda-tooltip' data-placement='top' title='<div class=\"tooltip-diahora\">" + formataNumero(engajamento[0].Valor) + " interações <br>&#183; " + formataNumero(engajamento[0].favoritados) + " curtidas;<br>&#183; " + formataNumero(engajamento[0].retweets) + " retweets. </div>'></td>");


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

        $(".tw-horarios-engajamento tbody").append(modeloTabela.join(''));
        $(".tw-horarios-engajamento tbody td[data-toggle='ajuda-tooltip']").tooltip({ container: 'body', html: 'true' });

        $("td.HoverEnga").hover(function () {

            var dia = $(this).data("dia");
            var hora = $(this).data("hora");

            $("tbody > tr >  td[data-valuedia='" + dia + "']").toggleClass("active");

            $(".legenda_hora > td[data-valuehora='" + hora + "']").toggleClass("active");


        });






    });


}

function CarregaTweetsMaiorEngajamento(id, dtIni, dtFim, qtdPosts, pagina) {

    $(".tweets_engajamento").empty();
    //$(".pag-tweetsengajamento").empty();

    $(".loadingPostsMaiorEngajamentoTw").show();
   

    var url = "/api/EmpresaRedesSociaisAPI/GetTwPostsMaisEngajamento?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim) + "&postsPagina=" + qtdPosts + "&pagina=" + pagina;
    
    $.getJSON(url, function (data) {
            totalTweetsEngajamento = data.TotalPosts;

            paginacaoTw.CriaPaginacao(".pag-tweetsengajamento", parseInt((totalTweetsEngajamento / 6) + 0.9), totalTweetsEngajamento, pagina);
            paginacaoTw.setMudouPagina(mudouPaginaTweetEngajamento);
       
        $(".loadingPostsMaiorEngajamentoTw").hide();

        $(".tweets_engajamento").empty();

        $.each(data.Posts, function (key, post) {
            var modeloPost = [];

            modeloPost.push('<div class="col-md-4">');
            modeloPost.push('<div class="ibox">');
            modeloPost.push('<div class="ibox-content">');
            modeloPost.push('<div class="engajado_img">');
            
            if (post.imagem != null) {

                modeloPost.push('<img src="'+ diretorioImagens +'/twitter/' + post.imagem + '" />');
            }
            else {
                modeloPost.push('<img src="' + diretorioImagens + '/twitter/no-photo.gif" />');
            }
                

            modeloPost.push('</div>');
            modeloPost.push('<div class="engajado_texto">');
            modeloPost.push('<div>');
            modeloPost.push('<span class="engajado_data">' + formataDataJson(post.dataPost) + '</span>');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-heart"></i> <span class="engajado_numero">' + post.totalFavoritadas + '</span> Curtidas<br />');
            modeloPost.push('</div>');
            modeloPost.push('<div>');
            modeloPost.push('<i class="fa fa-share"></i> <span class="engajado_numero">' + post.totalRetweets + '</span> Retweets');
            modeloPost.push('</div>');
            modeloPost.push('<div class="engajado_vermais">');
            //modeloPost.push('<a href="#"><i class="fa fa-eye"></i>Ver mais</a>');
            modeloPost.push('<a><span class="btn-vermais" data-idposttw="' + post.idPost + '"><i class="fa fa-eye"></i> Ver mais</span></a>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');
            modeloPost.push('</div>');

            $(".tweets_engajamento").append(modeloPost.join(''));

        });

        $(".tweets_engajamento .btn-vermais").click(function () {
            var id = $(this).attr("data-idposttw");
            MostraDetalhesPostTw(id);
        });
    });
}

function mudouPaginaTweetEngajamento(pagina) {

    var dtInicial = $("#dtinicialtw").datepicker('getDate');
    var dtFinal = $("#dtfinaltw").datepicker('getDate');

    CarregaTweetsMaiorEngajamento(idEmpresa, dtInicial, dtFinal, 6, pagina)
}
//detalhes de um post
function MostraDetalhesPostTw(id) {

    var url = "/api/EmpresaRedesSociaisAPI/GetTwPost?idPost=" + id;

    $.getJSON(url, function (data) {

        $("#detalhesPostTw").attr("data-idpost", data.idPost);
        $("#detalhesPostTw #usuarioposttw").text(data.usuarioPost);
        $("#detalhesPostTw #dataPosttw").text(formataDataHoraJson(data.dataPost));
        $("#detalhesPostTw #textoposttw").html(insereQuebraLinha(data.postagem));

        if (data.imagem != null) {
            $("#detalhesPostTw #imagemposttw").show();
            $("#detalhesPostTw #imagemposttw").attr("src", diretorioImagens + "/twitter/" + data.imagem);
        }
        else {
            $("#detalhesPostTw #imagemposttw").hide();
        }
            

        $("#detalhesPostTw #curtidasposttw").text(formataNumero(data.totalFavoritadas));
        $("#detalhesPostTw #comentariosposttw").text(formataNumero(data.totalRetweets));
        idtweetperfil = data.idPerfil;
        idtweetpost = data.idPost;
        if (data.totalRetweets > 3) {
            $("#btnCarregaRetweets").show();
        }
        else {
            $("#btnCarregaRetweets").hide();
        }

        var retweets = MontaTemplateComentariosTw(data.postsReply);

        $("#detalhesPostTw .social-footer").empty();
        $("#detalhesPostTw .social-footer").attr("data-carregados", data.postsReply.length);
        $("#detalhesPostTw .social-footer").append(retweets);

        exibirModal("#detalhesPostTw", undefined, undefined, 0, undefined);
    });

}

function MontaTemplateComentariosTw(retweets) {

    var arrComentarios = [];
    $(retweets).each(function (i, e) {
        arrComentarios.push('<div class="social-comment" data-id="' + e.idPost + '" data-idtwitter="' + e.idTwitterPost + '">');
        arrComentarios.push('<div class="media-body">');
        arrComentarios.push('<div class="corusuario"><strong>');
        arrComentarios.push(e.usuarioPost);
        arrComentarios.push('</strong></div> ');
        arrComentarios.push(insereQuebraLinha(e.postagem));
        arrComentarios.push('<br />');
        arrComentarios.push('<span class="small text-success"><i class="fa fa-heart" style="color: gray;"></i> ' + e.totalFavoritadas + ' curtidas</span> - ');
        arrComentarios.push('<span class="icon-respostas small corusuario" data-qtdrespostas=' + e.totalRetweets + '><i class="fa fa-share" style="color: gray;"></i> ' + e.totalRetweets + ' retweets</span> - ');
        arrComentarios.push('<small class="text-muted">' + formataDataHoraJson(e.dataPost) + '</small>');
        arrComentarios.push('</div>');
        arrComentarios.push('</div>');

    });
    return arrComentarios.join('');
}

function CarregaNuvemTermosPostsTw(id, dtIni, dtFim) {

    
    $(".tw_nuvem_semtermos").hide();
    $(".tw_post_nuvem_termos").hide();
    $(".tw_mencoes_nuvem_termos").hide();
    $(".loadingNuvemTw").show();
    
    var url = "/api/EmpresaRedesSociaisAPI/GetTwitterTermosPost?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

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

            $(".loadingNuvemTw").hide();
            $(".tw_post_nuvem_termos").show();

            var nuvem = new NuvemTermos();
            nuvem.inicializaNuvemTermos(".tw_post_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {            
            $(".loadingNuvemTw").hide();
            $(".tw_nuvem_semtermos").show();

        }
    });

}

function CarregaNuvemTermosMencoesTw(id, dtIni, dtFim) {

    $(".tw_nuvem_semtermos").hide();
    $(".tw_post_nuvem_termos").hide();
    $(".tw_mencoes_nuvem_termos").hide();
    $(".loadingNuvemTw").show();


    var url = "/api/EmpresaRedesSociaisAPI/GetTwitterTermosMencoes?idEmpresa=" + id + "&dtInicial=" + ConverteDataParaDataJson(dtIni) + "&dtFinal=" + ConverteDataParaDataJson(dtFim);

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

            $(".loadingNuvemTw").hide();
            $(".tw_mencoes_nuvem_termos").show();


            var nuvem = new NuvemTermos();
            nuvem.inicializaNuvemTermos(".tw_mencoes_nuvem_termos", listaTermos, minFreq, maxFreq);
        } else {
            
            $(".loadingNuvemTw").hide();
            $(".tw_nuvem_semtermos").show();
        }
    });

}

function CarregaMaisRetweets() {

    var inicial = $("#detalhesPostTw .social-footer").attr("data-carregados");
    var quantidade = 5;
    
    var url = "/api/EmpresaRedesSociaisAPI/GetMaisRetweets?idpost=" + idtweetpost + "&inicial=" + inicial + "&quantidade=" + quantidade;
  
    $.getJSON(url, function (retweets) {

        var htmlComentarios = MontaTemplateComentariosTw(retweets);

        $("#detalhesPostTw .social-footer").attr("data-carregados", parseInt(inicial) + retweets.length);
        $("#detalhesPostTw .social-footer").append(htmlComentarios);

        var total = parseInt(inicial) + retweets.length;

        if (totalcomentarioTwiterAnalise == total) {

            $('.twitter_posts_timeline').find('#btnCarregaComentariosTw').hide();

        }
    });
}

function mudouPaginaTwTimeLine(pagina) {

    if (PostComentTw == undefined) {
        PostComentTw == null
    }

    if (palavraTw == undefined) {
        palavraTw == null
    }

    var dtInicial = $("#dtinicialtw").datepicker('getDate');
    var dtFinal = $("#dtfinaltw").datepicker('getDate');

    CarregaTwLinhaDoTempo(idEmpresa, dtInicial, dtFinal, 6, pagina, ordenacao, palavraTw, PostComentTw)
}

$("#btntwpostcloud").click(function () {

    $("#btntwpostcloud").addClass("active");
    $("#btntwmencoescloud").removeClass("active");

    $(".tw_post_nuvem_termos").show();
    $(".tw_mencoes_nuvem_termos").hide();

    var dtInicialFace = $("#dtinicialtw").datepicker('getDate');
    var dtFinalFace = $("#dtfinaltw").datepicker('getDate');

    CarregaNuvemTermosPostsTw(idEmpresa, dtInicialFace, dtFinalFace);
});

$("#btntwmencoescloud").click(function () {

    $("#btntwmencoescloud").addClass("active");
    $("#btntwpostcloud").removeClass("active");

    $(".tw_mencoes_nuvem_termos").show();
    $(".tw_post_nuvem_termos").hide();

    var dtInicialFace = $("#dtinicialface").datepicker('getDate');
    var dtFinalFace = $("#dtfinalface").datepicker('getDate');

    CarregaNuvemTermosMencoesTw(idEmpresa, dtInicialFace, dtFinalFace);

});
