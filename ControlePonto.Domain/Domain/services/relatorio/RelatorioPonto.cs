using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public class RelatorioPonto
    {
        #region Propriedades

        public Funcionario Funcionario { get; set; }

        public DateTime PeriodoInicio { get; private set; }

        public DateTime PeriodoFim { get; private set; }

        public List<DiaRelatorio> Dias { get; private set; }        

        #endregion

        private JornadaTrabalho jornadaAtiva;

        public RelatorioPonto(Funcionario funcionario, DateTime inicio, DateTime fim, JornadaTrabalho jornadaAtiva, List<DiaRelatorio> todosDias)
        {
            this.Funcionario = funcionario;
            this.PeriodoInicio = inicio;
            this.PeriodoFim = fim;
            this.jornadaAtiva = jornadaAtiva;
            this.Dias = todosDias;

            int difDias = (fim - inicio).Days + 1;

            Check.Ensure(Dias.Count == difDias, string.Format(
                "Uma quantidade errada de dias foi gerada para o período: {0} e {1}. Esperado: {2} Encontrado: {3}", 
                inicio.ToShortDateString(), 
                fim.ToShortDateString(),
                difDias,
                Dias.Count));
        }

        public List<IDiaFeriado> getFeriados()
        {
            return
                Dias
                .Where(x => x.TipoDia == ETipoDiaRelatorio.FERIADO || 
                    x.TipoDia == ETipoDiaRelatorio.FERIADO_TRABALHADO)
                .Cast<IDiaFeriado>()
                .ToList();
        }

        public List<IDiaComPonto> getDiasTrabalhados()
        {
            return
                Dias
                .Where(x => 
                    x.TipoDia == ETipoDiaRelatorio.TRABALHO ||
                    x.TipoDia == ETipoDiaRelatorio.FERIADO_TRABALHADO)
                .Cast<IDiaComPonto>()
                .ToList();
        }

        public List<IDiaComPonto> getFeriadosTrabalhados()
        {
            return
                Dias
                .Where(x => x.TipoDia == ETipoDiaRelatorio.FERIADO_TRABALHADO)
                .Cast<IDiaComPonto>()
                .ToList();
        }

        public List<DiaPonto> getFolgas()
        {
            return Dias.Where(x => x.TipoDia == ETipoDiaRelatorio.FOLGA)
                .Cast<DiaPonto>()
                .ToList();
        }

        public TimeSpan calcularHorasExtras(double valorHoraExtra)
        {
            return
                new TimeSpan(
                    getDiasTrabalhados()
                        .Cast<ICalculoHoraExtra>()
                        .Where(x => x.calcularValorHoraExtra() == valorHoraExtra)
                        .Sum(x => x.calcularHorasExtras().Ticks)
                );
        }

        public TimeSpan calcularHorasExtras()
        {
            return
                new TimeSpan(
                    getDiasTrabalhados()
                        .Cast<ICalculoHoraExtra>()
                        .Sum(x => x.calcularHorasExtras().Ticks)
                );
        }
    
        public TimeSpan calcularHorasDevedoras()
        {
            return
                new TimeSpan(
                    Dias.Where(x => 
                        x.TipoDia != ETipoDiaRelatorio.FERIADO_TRABALHADO &&
                        x.TipoDia != ETipoDiaRelatorio.FERIADO)
                        .Cast<ICalculoHoraDevedora>()
                        .Sum(x => x.calcularHorasDevedoras().Ticks)
                );
        }

        public TimeSpan calcularHorasTrabalhadas()
        {
            return
                new TimeSpan(
                    getDiasTrabalhados()
                        .Sum(x => x.calcularHorasTrabalhadas().Ticks));
        }
    }
}