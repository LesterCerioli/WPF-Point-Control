using ControlePonto.Domain.intervalo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class IntervaloNaoRegistradoException : Exception
    {
        public TipoIntervalo TipoIntervalo { get; private set; }

        public IntervaloNaoRegistradoException(TipoIntervalo tipoIntervalo) : 
            base(string.Format("O intervalo de {0} não foi iniciado", tipoIntervalo.Nome))
        {
            this.TipoIntervalo = tipoIntervalo;
        }
    }
}
