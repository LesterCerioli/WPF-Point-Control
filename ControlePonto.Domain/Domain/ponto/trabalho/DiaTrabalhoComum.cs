using ControlePonto.Domain.jornada;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class DiaTrabalhoComum : DiaTrabalho
    {
        public override ETipoPonto Tipo { get; protected set; }

        protected DiaTrabalhoComum() { }

        public DiaTrabalhoComum(DateTime data, TimeSpan inicio, Funcionario funcionario) 
            : base(data, inicio, funcionario) 
        {
            Tipo = ETipoPonto.TRABALHO;
        }

        public override double calcularValorHoraExtra()
        {
            if (Data.DayOfWeek == DayOfWeek.Sunday)
                return 100;
            return 75;
        }

        public override TimeSpan calcularHorasExtras(JornadaTrabalho jornada)
        {
            DiaJornada diaJornada = jornada.getDia(Data.DayOfWeek);
            var trabalhado = calcularHorasTrabalhadas();
            var esperado = diaJornada.calcularHorasTrabalhoEsperado();

            var resultado = trabalhado.Subtract(esperado);
            if (resultado > new TimeSpan(0, 10, 59))
                return resultado;
            return new TimeSpan(0, 0, 0);
        }

        public override TimeSpan calcularHorasDevedoras(JornadaTrabalho jornada)
        {
            DiaJornada diaJornada = jornada.getDia(Data.DayOfWeek);
            var trabalhado = calcularHorasTrabalhadas();
            var esperado = diaJornada.calcularHorasTrabalhoEsperado();

            var resultado = esperado.Subtract(trabalhado);
            if (resultado > new TimeSpan(0, 10, 59))
                return resultado;
            return new TimeSpan(0, 0, 0);
        }
    }
}
