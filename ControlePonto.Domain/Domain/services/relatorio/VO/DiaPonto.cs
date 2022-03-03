using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public class DiaPonto : DiaRelatorio, IDiaComPonto
    {
        public PontoDia PontoDia { get; private set; }

        public JornadaTrabalho JornadaTrabalhoAtiva { get; private set; }

        public override ETipoDiaRelatorio TipoDia
        {
            get
            {
                if (PontoDia.Tipo == ETipoPonto.TRABALHO)
                    return ETipoDiaRelatorio.TRABALHO;

                return ETipoDiaRelatorio.FOLGA;
            }
        }

        public DiaPonto(PontoDia pontoDia, JornadaTrabalho jornadaAtiva)
            : base(pontoDia.Data)
        {
            Check.Require(pontoDia.Tipo != ETipoPonto.FERIADO_TRABALHADO,
                "Feriado trabalhado não deve ser criado desta forma.");
            Check.Require(pontoDia != null, "O ponto deve ser válido");

            this.PontoDia = pontoDia;
            this.JornadaTrabalhoAtiva = jornadaAtiva;
        }

        public TimeSpan calcularHorasExtras()
        {
            return PontoDia.calcularHorasExtras(JornadaTrabalhoAtiva);
        }

        public double calcularValorHoraExtra()
        {
            return PontoDia.calcularValorHoraExtra();
        }

        public TimeSpan calcularHorasDevedoras()
        {
            return PontoDia.calcularHorasDevedoras(JornadaTrabalhoAtiva);
        }


        public TimeSpan calcularHorasTrabalhadas()
        {
            return PontoDia.calcularHorasTrabalhadas();
        }
    }
}
