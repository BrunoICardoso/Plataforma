
function RetornaDiaSemana(diasemana) {
    switch (diasemana.toString()) {
        case "1":
            return "Domingo";
        case "2":
            return "Segunda";
        case "3":
            return "Terça";
        case "4":
            return "Quarta";
        case "5":
            return "Quinta";
        case "6":
            return "Sexta";
        case "7":
            return "Sábado";
        default:
            return "Dia inexistente";

    }

}

function separadorMilhares(valor) {

    return valor.toString().replace(/[.]/g, ",").replace(/\d(?=(?:\d{3})+(?:\D|$))/g, "$&.");

}

function retornaValorParaCalculo(valor) {

    return parseFloat(valor.toString().replace(/\./g, '').replace(/\,/g, '.'));

}

function insereQuebraLinha(t) {

    if (t == null)
        return "";

    return t.replace(/\n/g, "<br />");
}

function getTextWidth(text, fontSize, fontFace) {
    var a = document.createElement('canvas');
    var b = a.getContext('2d');
    b.font = fontSize + 'px ' + fontFace;
    return b.measureText(text).width;
}

function formataNumero(n) {

    return n.toString().replace(/[.]/g, ",").replace(/\d(?=(?:\d{3})+(?:\D|$))/g, "$&.")
}

function formataDataJson(dateJson) {

    var dataehora = dateJson.split("T");
    var data = dataehora[0].split("-");
    var hora = dataehora[1].split(":");

    var dd = data[2];
    var mm = data[1];
    var yy = data[0];

    var dataFormatada = dd + '/' + mm + '/' + yy;

    return dataFormatada;
}

//Formata as datas no formato YYYY-MM-DD e apresenta no formato dd/MM/YYYY
function formataDataInvertida(dataInvertida) {

    var data = dataInvertida.split("-");

    var dd = data[2];
    var mm = data[1];
    var yy = data[0];

    var dataFormatada = dd + '/' + mm + '/' + yy;

    return dataFormatada;
}


function formataDataHoraJson(dateJson) {

    var dataehora = dateJson.split("T");
    var data = dataehora[0].split("-");
    var hora = dataehora[1].split(":");

    var dd = data[2];
    var mm = data[1];
    var yy = data[0];

    //var hora = hora[0];
    //var min = hora[1];
    //var seg = hora[2];

    var dataFormatada = dd + '/' + mm + '/' + yy + " " + dataehora[1];

    return dataFormatada;
}

function ConverteDataStringParaData(d) {

    var date = d.split("/");
    var dd = date[0];
    var mm = date[1];
    var yy = date[2];
    var fromdt = new Date(Date.UTC(yy, mm - 1, dd, 00, 00, 00));

    return fromdt;
}

function ConverteDataStringComPonto(dateJson) {
    var dataehora = dateJson.split("T");
    var data = dataehora[0].split("-");
    var hora = dataehora[1].split(":");

    var dd = data[2];
    var mm = data[1];
    var yy = data[0];

    var dataFormatada = dd + '.' + mm + '.' + yy;

    return dataFormatada;
}

function ConverteDataParaDataJson(d) {

    var dd = ("0" + d.getDate()).slice(-2);
    var mm = ("0" + (d.getMonth() + 1)).slice(-2);
    var yy = d.getFullYear();

    var hh = ("0" + d.getHours()).slice(-2);
    var min = ("0" + d.getMinutes()).slice(-2);
    var ss = ("0" + d.getSeconds()).slice(-2);

    var fromdt = yy + "-" + mm + "-" + dd + "T" + hh + ":" + min + ":" + ss + "Z";

    return fromdt;
}

function SubtrairData(numero) {
    var d = new Date();
    d.setDate(d.getDate() - numero);
    return d.toLocaleString();
}

function ConverteDataBRparaUNIX(date) {
    var dataHoraTMP = date.split(' ');
    var HoraMinutoSegundo = dataHoraTMP[1];
    var dataTMP = dataHoraTMP[0].split('/');
    var ano = dataTMP[2];
    var mes = dataTMP[1];
    var dia = dataTMP[0];
    return ano + "-" + mes + "-" + dia + " " + HoraMinutoSegundo;
}

function ConverteDataUNIXparaBR(date, comHoraMinutoSegundo) {
    var dataHoraTMP = date.split(' ');
    var HoraMinutoSegundo = dataHoraTMP[1];
    var dataTMP = dataHoraTMP[0].split('-');
    var ano = dataTMP[0];
    var mes = dataTMP[1];
    var dia = dataTMP[2];
    if (comHoraMinutoSegundo == false)
        return dia + "/" + mes + "/" + ano;
    else
        return dia + "/" + mes + "/" + ano + " " + HoraMinutoSegundo;
}

function CalculaPercentualDiferencaTotais(valorAtual, valorAnterior) {

    var valor = (parseInt(valorAtual) / parseInt(valorAnterior) - 1) * 100;

    return valor.toFixed(0).replace('.', ',');
}

function IconeCarregando(tamanhoIcone) {
    if (tamanhoIcone == null)
        tamanhoIcone = 70;

    if (!diretorioImagens && diretorioImagens == "")
        diretorioImagens = "/Images";

    return '<img src="/Images/Loading_icon.gif" width="' + tamanhoIcone + 'px">';
}

// Soma  e média de horas
function toSeconds(time) {
    var parts = time.split(':');
    return (+parts[0]) * 60 * 60 + (+parts[1]) * 60 + (+parts[2]);
}

function toHHMMSS(sec) {
    var sec_num = parseInt(sec, 10); // don't forget the second parm
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours < 10) { hours = "0" + hours; }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    var time = hours + ':' + minutes + ':' + seconds;
    return time;
}

function MediasHoras(times) {
    var count = times.length
    var timesInSeconds = [];
    // loop through times
    for (var i = 0; i < count; i++) {
        // parse
        var pieces = times[i].split(':');
        var ampm = pieces[2].split(' ');
        var hrs = Number(pieces[0]);
        var mins = Number(pieces[1]);
        var secs = Number(ampm[0]);
        var ampm = ampm[1];
        // convert to 24 hr format (military time)
        if (ampm == 'PM') hrs = hrs + 12;
        // find value in seconds of time
        var totalSecs = hrs * 60 * 60;
        totalSecs += mins * 60;
        totalSecs += secs;
        // add to array
        timesInSeconds[i] = totalSecs;
    }
    // find average timesInSeconds
    var total = 0;
    //console.log(timesInSeconds);
    for (var j = 0; j < count; j++) {
        total = total + Number(timesInSeconds[j]);
    }
    var avg = Math.round(total / count);
    //console.log('avg secs: ' + avg);
    // turn seconds back into a time
    var avgMins = Math.floor(avg / 60);
    var avgSecs = avg - (60 * avgMins);
    var avgHrs = Math.floor(avgMins / 60);
    //console.log('hours: ' + avgHrs);
    avgMins = avgMins - (60 * avgHrs);
    // convert back to 12 hr. format
    var avgAmpm = 'AM';
    if (avgHrs > 12) {
        avgAmpm = 'PM';
        avgHrs = avgHrs - 12;
    }
    // add leading zeros for seconds, minutes
    avgSecs = ('0' + avgSecs).slice(-2);
    avgMins = ('0' + avgMins).slice(-2);
    // your answer

    if (avgHrs <= 9)
        avgHrs = '0' + avgHrs;

    return avgHrs + ':' + avgMins + ':' + avgSecs;
}