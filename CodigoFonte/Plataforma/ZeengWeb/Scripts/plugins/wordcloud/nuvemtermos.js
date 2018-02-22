function NuvemTermos() {


    var _listaTermos;
    var _freqMax;
    var _freqMin;
    var _seletor;
    var _tamMax = 100;
    var _tamMin = 12;

    this.inicializaNuvemTermos = function (seletor, listaTermos, freqmin, freqmax) {

        
        _listaTermos = listaTermos;
        _freqMax = freqmax;
        _freqMin = freqmin;
        _seletor = seletor;

        desenhaNuvem(listaTermos, freqmin, freqmax);

        $(_seletor + " .range").ionRangeSlider({
            min: _freqMin,
            max: _freqMax,
            type: 'double',
            prettify: false,
            hasGrid: true,
            step: 1,
            color: '#666',
            onFinish: function (data) {
                recarregaNuvem(data.fromNumber, data.toNumber);
            },
        });

    }

    function desenhaNuvem(listaTermos, freqmin, freqmax)
    {

        $(_seletor + " .termos").empty();

        var options =
          {
              list: listaTermos,
              gridSize: 1, //Math.round(16 * $('.face_nuvem_termos').width() / 1024),
              //weightFactor: function (size) {
              //    return Math.pow(size, 2); // * $('.face_nuvem_termos').width() / 1024;
              //},
              weightFactor: function (freq) {

                  if (freqmin == freqmax)
                      return _tamMax;
                  else
                      return ((freq - freqmin) * (_tamMax - _tamMin) / (freqmax - freqmin)) + _tamMin;
              },
              fontWeight: 'Bold',
              //minSize: '12px',
              fontFamily: 'Arial',
              color: 'zeeng-color',
              rotateRatio: 0.0,
              backgroundColor: '#fff',
              drawOutOfBound: false
          }

        WordCloud($(_seletor + " .termos")[0], options);
    }

    function recarregaNuvem(valorMin, valorMax) {

        var fmin = _listaTermos[0][1];
        var fmax = _listaTermos[0][1];

        var termos = _listaTermos.filter(function (t) {
            if (t[1] >= valorMin && t[1] <= valorMax) {
                if (fmin > t[1])
                    fmin = t[1];

                if (fmax < t[1])
                    fmax = t[1];

                return true;
            } else {
                return false;
            }
        });

        desenhaNuvem(termos, fmin, fmax);

    }
}

