using ControlePonto.Domain.jornada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaFalta : DiaRelatorio, ICalculoHoraDevedora
    {
        public JornadaTrabalho JornadaTrabalhoAtiva { get; private set; }

        public override ETipoDiaRelatorio TipoDia
        {
            get
            {
                if (calcularHorasDevedoras() == new TimeSpan(0))
                    return ETipoDiaRelatorio.SEM_TRABALHO;
                return ETipoDiaRelatorio.FALTOU;
            }
        }

        public DiaFalta(DateTime date, JornadaTrabalho jornadaAtiva) : base(date) 
        {
            this.JornadaTrabalhoAtiva = jornadaAtiva;
        }

        public TimeSpan calcularHorasDevedoras()
        {
            return JornadaTrabalhoAtiva.getDia(Data.Date.DayOfWeek).calcularHorasTrabalhoEsperado();
        }
    }
}
