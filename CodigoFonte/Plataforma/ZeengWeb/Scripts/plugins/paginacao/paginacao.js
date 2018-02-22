
function Paginacao() {
    var _totalPaginas = 0;
    var _seletor = "";
    var _paginaAtual = 1;
    var _totalRegistros = 0

    var mudouPagina;

    this.setMudouPagina = function (evento) {
        mudouPagina = evento;
    }


    this.CriaPaginacao = function (seletor, totalPaginas, totalRegistros, paginaAtual) {

        //if (totalRegistros == 0)
        //{
        //    $(seletor).empty();
        //    return;
        //}


        _totalPaginas = totalPaginas;
        _seletor = seletor;
        _totalRegistros = totalRegistros;

        var _ultimaPagina = 0;

        if (totalPaginas > 11) {
            _ultimaPagina = 11;
        }
        else {
            _ultimaPagina = totalPaginas
        }

        _paginaAtual = paginaAtual;

        SelecionaPagina();

        

    }



    function SelecionaPagina() {

        var _paginaInicial;
        var _paginaFinal;

        _paginaInicial = _paginaAtual - 5;

        if (_paginaInicial < 1) {
            _paginaInicial = 1;
        }

        _paginaFinal = _paginaInicial + 10;
        if (_paginaFinal > _totalPaginas) {
            _paginaFinal = _totalPaginas;
            _paginaInicial = _totalPaginas - 10;

            if (_paginaInicial < 1) {
                _paginaInicial = 1;
            }
        }

        $(_seletor).empty();
        var labelTotal = $("<div class='paginacao-texto'><span>" + _totalRegistros + " registros - " + _totalPaginas + " paginas</span></div>");

        var newUl = $("<ul class='tabela-paginacao pagination pull-right'>");
        $(newUl).append("<li id='btnfirst' class='footable-page-arrow btnfirst'><a data-page='First'>‹‹</a></li>");
        $(newUl).append("<li id='btnprev' class='footable-page-arrow btnprev'><a data-page='Prev'>‹</a></li>");

        for (var i = _paginaInicial; i <= _paginaFinal; i++) {

            $(newUl).append("<li class='footable-page pagina'><a data-page='" + i + "'>" + i + "</a></li>");
        }

        $(newUl).append("<li id='btnnext' class='footable-page-arrow btnnext'><a data-page='Next'>›</a></li>");
        $(newUl).append("<li id='btnlast' class='footable-page-arrow btnlast'><a data-page='Last'>››</a></li>");

        $(labelTotal).append(newUl)
        $(_seletor).append(labelTotal);

        var itemPag = $(_seletor).find(".pagina a[data-page=" + _paginaAtual + "]");

        $(_seletor).find(".pagina").removeClass("active");
        $(itemPag).parent().addClass("active");

        if (_paginaAtual == 1) {
            $(_seletor).find('.btnprev').addClass('disabled');
            $(_seletor).find('.btnfirst').addClass('disabled');
        } else {
            $(_seletor).find('.btnprev').removeClass('disabled');
            $(_seletor).find('.btnfirst').removeClass('disabled');
        }

        if (_paginaAtual == _totalPaginas) {
            $(_seletor).find('.btnnext').addClass('disabled');
            $(_seletor).find('.btnlast').addClass('disabled');
        } else {
            $(_seletor).find('.btnnext').removeClass('disabled');
            $(_seletor).find('.btnlast').removeClass('disabled');
        }

        $(_seletor).find(".pagina").click(function () {

            _paginaAtual = $(this).children().attr("data-page");
            SelecionaPagina();

            if (!!mudouPagina) {
                mudouPagina(_paginaAtual);
            }
        });

        $(_seletor).find(".btnnext").click(function () {

            if (_paginaAtual == _totalPaginas) {
                return;
            }

            _paginaAtual++;
            SelecionaPagina();

            if (!!mudouPagina) {
                mudouPagina(_paginaAtual);
            }
        });

        $(_seletor).find(".btnlast").click(function () {

            if (_paginaAtual == _totalPaginas) {
                return;
            }

            _paginaAtual = _totalPaginas;
            SelecionaPagina();

            if (!!mudouPagina) {
                mudouPagina(_paginaAtual);
            }
        });

        $(_seletor).find(".btnprev").click(function () {

            if (_paginaAtual == 1) {
                return;
            }

            _paginaAtual--;
            SelecionaPagina();

            if (!!mudouPagina) {
                mudouPagina(_paginaAtual);
            }
        });

        $(_seletor).find(".btnfirst").click(function () {

            if (_paginaAtual == 1) {
                return;
            }

            _paginaAtual = 1;
            SelecionaPagina();

            if (!!mudouPagina) {
                mudouPagina(_paginaAtual);
            }
        });


    }

}
