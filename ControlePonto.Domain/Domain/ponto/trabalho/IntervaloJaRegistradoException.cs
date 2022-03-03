using ControlePonto.Domain.intervalo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class IntervaloJaRegistradoException : Exception
    {
        public TipoIntervalo TipoIntervalo { get; private set; }

        public IntervaloJaRegistradoException(TipoIntervalo tipo) : base(string.Format("O horário de {0} já foi registrado", tipo.Nome))
        {
            this.TipoIntervalo = tipo;
        }
    }
}
