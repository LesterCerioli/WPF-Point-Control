using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ControlePonto.WPF.window.ponto.controle
{
    public partial class DiaControlePontoViewModel : ViewModelBase
    {
        public DiaRelatorio DiaRelatorio { get; private set; }
        public Funcionario  Funcionario { get; private set; }
        protected ControlarPontoViewModel parentViewModel;

        public DiaControlePontoViewModel(ControlarPontoViewModel parent, Funcionario funcionario, DiaRelatorio diaRelatorio)
        {
            this.parentViewModel = parent;
            this.Funcionario = funcionario;            
            this.DiaRelatorio = diaRelatorio;
            this.Acoes = new List<AcaoViewModel>();
            criarAcoes();
        }

        private List<AcaoViewModel> _acoes;
        public List<AcaoViewModel> Acoes
        {
            get { return _acoes; }
            set
            {
                SetField(ref _acoes, value);
            }
        }

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
                    case ETipoDiaRelatorio.FALTOU:
                        return "Funcionário não trabalhou";

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

        private void criarAcoes()
        {
            switch (DiaRelatorio.TipoDia)
            {
                case ETipoDiaRelatorio.FALTOU:
                case ETipoDiaRelatorio.FERIADO:
                    Acoes.Add(new AcaoViewModel(this, AcaoViewModel.EAcao.Criar));
                    break;

                case ETipoDiaRelatorio.TRABALHO:
                case ETipoDiaRelatorio.FERIADO_TRABALHADO:
                    Acoes.Add(new AcaoViewModel(this, AcaoViewModel.EAcao.Editar));
                    break;
            }
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
