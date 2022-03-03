using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Infrastructure.utils;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ControlePonto.WPF.window.relatorio
{
    public class DiaRelatorioViewModel : ViewModelBase
    {
        public const int VIEW_PONTO = 100;

        public DiaRelatorio DiaRelatorio { get; private set; }

        public DiaRelatorioViewModel(DiaRelatorio diaRelatorio)
        {
            this.DiaRelatorio = diaRelatorio;
        }

        #region Propriedades

        public string Data
        { 
            get 
            { 
                return string.Format("{0} ({1})",
                    DiaRelatorio.Data.ToShortDateString(),
                    DiaSemanaTradutor.traduzir(DiaRelatorio.Data.DayOfWeek));
            } 
        }

        public string Info
        {
            get
            {
                switch (DiaRelatorio.TipoDia)
                {
                    case ETipoDiaRelatorio.SEM_TRABALHO:
                        return DiaSemanaTradutor.traduzir(DiaRelatorio.Data.DayOfWeek);

                    case ETipoDiaRelatorio.FALTOU:
                        return "Funcionário faltou";

                    case ETipoDiaRelatorio.FERIADO:
                    case ETipoDiaRelatorio.FERIADO_TRABALHADO:
                        return "FERIADO: " + (DiaRelatorio as IDiaFeriado).Nome;

                    case ETipoDiaRelatorio.FOLGA:
                        return "FOLGA: " + ((DiaRelatorio as DiaPonto).PontoDia as DiaFolga).Descricao;                    

                    default:
                        return "Dia trabalhado";
                }                
            }
        }

        public Brush Cor
        {
            get
            {
                switch (DiaRelatorio.TipoDia)
                {
                    case ETipoDiaRelatorio.FALTOU:
                        return Brushes.Red;

                    case ETipoDiaRelatorio.FERIADO:
                    case ETipoDiaRelatorio.FERIADO_TRABALHADO:
                        return Brushes.Blue;

                    default:
                        return Brushes.Black;
                }
            }
        }

        public bool IsFolga 
        { 
            get
            {
                return DiaRelatorio.TipoDia == ETipoDiaRelatorio.FOLGA;
            }
        }

        public string HorasTrabalhadas
        {
            get
            {                
                if (DiaRelatorio is IDiaComPonto)
                    return getHorasOuVazio((DiaRelatorio as IDiaComPonto).calcularHorasTrabalhadas());
                return "";
            }
        }

        public string HorasDevedoras
        {
            get
            {                
                TimeSpan devedoras = new TimeSpan();
                if (DiaRelatorio is ICalculoHoraDevedora)
                    devedoras = (DiaRelatorio as ICalculoHoraDevedora).calcularHorasDevedoras();

                return getHorasOuVazio(devedoras);
            }
        }

        public string HorasExtras
        {
            get
            {
                return getHoraExtrarPorValor(75);
            }
        }

        public string HorasExtras100
        {
            get
            {
                return getHoraExtrarPorValor(100);
            }
        }        

        #endregion
        
        private string getHoraExtrarPorValor(double valor)
        {
            if (DiaRelatorio is ICalculoHoraExtra)
            {
                var calculo = (DiaRelatorio as ICalculoHoraExtra);
                if (calculo.calcularValorHoraExtra() == valor)
                    return calculo.calcularHorasExtras().ToString();
            }
            return "";
        }

        private string getHorasOuVazio(TimeSpan horas)
        {
            if (horas == new TimeSpan())
                return "";
            return horas.ToString();
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
