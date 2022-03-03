using ControlePonto.Domain.feriado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaFeriado : DiaRelatorio, IDiaFeriado
    {
        private Feriado feriado;

        public string Nome
        {
            get { return feriado.Nome; }
        }

        public override ETipoDiaRelatorio TipoDia
        {
            get { return ETipoDiaRelatorio.FERIADO; }
        }

        public DiaFeriado(Feriado feriado) : base(feriado.getData())
        {
            this.feriado = feriado;
        }
    }
}
