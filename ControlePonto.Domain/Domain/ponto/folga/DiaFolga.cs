using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.ponto.folga
{
    public class DiaFolga : PontoDia
    {
        public const int MAX_DESCRICAO_LENGTH = 100;

        public virtual string Descricao { get; protected set; }

        public override ETipoPonto Tipo { get; protected set; }

        protected DiaFolga() { }

        public DiaFolga(Funcionario funcionario, DateTime date, string descricao)
        {
            base.checkPreConstructor();

            this.Tipo = ETipoPonto.FOLGA;
            base.Data = date;            
            base.Funcionario = funcionario;            
            this.Descricao = descricao;
        }

        public override TimeSpan calcularHorasTrabalhadas()
        {
            return new TimeSpan(0);
        }

        public override TimeSpan calcularHorasExtras(JornadaTrabalho jornada)
        {
            return new TimeSpan(0);
        }

        public override TimeSpan calcularHorasDevedoras(JornadaTrabalho jornada)
        {
            DiaJornada diaJornada = jornada.getDia(Data.DayOfWeek);
            return diaJornada.calcularHorasTrabalhoEsperado();
        }

        public override double calcularValorHoraExtra()
        {
            return 0;
        }
    }
}
