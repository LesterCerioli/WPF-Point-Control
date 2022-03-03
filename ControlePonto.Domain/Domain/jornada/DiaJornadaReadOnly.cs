using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class DiaJornadaReadOnly : DiaJornada
    {
        private DiaJornada diaProtegido;

        protected internal DiaJornadaReadOnly(DiaJornada diaJornada) : 
            base(diaJornada.DiaSemana)
        {
            diaProtegido = diaJornada;
        }

        public override TimeSpan EntradaEsperada
        {
            get { return diaProtegido.EntradaEsperada; }
            set { throw new InvalidOperationException(); }
        }

        public override TimeSpan SaidaEsperada
        {
            get { return diaProtegido.SaidaEsperada; }
            set { throw new InvalidOperationException(); }
        }

        public override TimeSpan FolgaEsperada
        {
            get { return diaProtegido.FolgaEsperada; }
            set { throw new InvalidOperationException(); }
        }
    }
}
