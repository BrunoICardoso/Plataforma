

function inicializaFormModal() {


    $(".background-modal").click(function () {
        $(this).hide();
        $(".form-modal").hide();
    });

    $('.form-modal .btn-fechar').click(function (ev) {
        ev.preventDefault();
        $(".background-modal").hide();
        $(".form-modal").hide();
    });


}


function exibirModal(seletor, largura, altura, top, left) {

    var alturaTela = $(document).height();
    var larguraTela = $(window).width();

    //colocando o fundo preto
    $('.background-modal').css({ 'width': larguraTela, 'height': alturaTela });
    $('.background-modal').fadeIn(800);
    //$('.background-modal').fadeTo("slow", 0.3);

    if (altura == undefined) {
        altura = $(seletor).height();
    }

    if (largura == undefined) {
        largura = $(seletor).width();
    }

    if (left == undefined)
    {
        left = ($(window).width() / 2) - (largura / 2);
    }
    
    if (top == undefined)
    {
        top = ($(window).height() / 2) - (altura / 2);
    }
    
    $(seletor).css({ 'top': top, 'left': left });
    $(seletor).show();

}


