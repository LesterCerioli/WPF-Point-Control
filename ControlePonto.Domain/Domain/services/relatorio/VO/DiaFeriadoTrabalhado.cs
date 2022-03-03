using ControlePonto.Domain.feriado;
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
    public class DiaFeriadoTrabalhado : DiaRelatorio, IDiaFeriado, IDiaComPonto
    {
        public PontoDia PontoDia { get; private set; }
        public Feriado Feriado { get; private set; }
        public JornadaTrabalho JornadaTrabalhoAtiva { get; private set; }

        public string Nome
        {
            get { return Feriado.Nome; }
        }

        public override ETipoDiaRelatorio TipoDia
        {
            get { return ETipoDiaRelatorio.FERIADO_TRABALHADO; }
        }

        public DiaFeriadoTrabalhado(PontoDia pontoDia, Feriado feriado, JornadaTrabalho jornadaAtiva)
            : base(pontoDia.Data)
        {
            Check.Require(pontoDia.Data == feriado.getData(), 
                "O dia do feriado deve ser o mesmo do dia de trabalho");
            Check.Require(feriado != null, "O feriado deve ser válido");
            Check.Require(pontoDia != null, "O ponto deve ser válido");

            this.PontoDia = pontoDia;
            this.Feriado = feriado;
            this.JornadaTrabalhoAtiva = jornadaAtiva;
        }

        public TimeSpan calcularHorasExtras()
        {
            return PontoDia.calcularHorasExtras(JornadaTrabalhoAtiva);
        }

        public TimeSpan calcularHorasDevedoras()
        {
            return PontoDia.calcularHorasDevedoras(JornadaTrabalhoAtiva);
        }


        public double calcularValorHoraExtra()
        {
            return PontoDia.calcularValorHoraExtra();
        }


        public TimeSpan calcularHorasTrabalhadas()
        {
            return PontoDia.calcularHorasTrabalhadas();
        }
    }
}
