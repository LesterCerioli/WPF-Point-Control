using ControlePonto.Domain.services.login;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.administracao
{
    public class PainelControleViewModel : ViewModelBase
    {
        public const int VIEW_RELATORIO = 0;
        public const int VIEW_FOLGA = 1;
        public const int VIEW_FERIADO = 2;
        public const int VIEW_JORNADA = 3;
        public const int VIEW_PONTO = 4;

        public PainelControleViewModel()
        {
            Titulo = string.Format("{0} - Painel de Controle", DateTime.Today.ToShortDateString());
            UsuarioLogado = SessaoLogin.getSessao().UsuarioLogado.Nome;

            RelatorioCommand = new RelayCommand(() => requestView(VIEW_RELATORIO));
            FolgaCommand = new RelayCommand(() => requestView(VIEW_FOLGA));
            FeriadoCommand = new RelayCommand(() => requestView(VIEW_FERIADO));
            JornadaCommand = new RelayCommand(() => requestView(VIEW_JORNADA));
            PontoCommand = new RelayCommand(() => requestView(VIEW_PONTO));
        }

        #region Propriedades

        public string Titulo { get; private set; }
        public string UsuarioLogado { get; private set; }

        public ICommand RelatorioCommand{ get; private set; }
        public ICommand FolgaCommand{ get; private set; }
        public ICommand FeriadoCommand{ get; private set; }
        public ICommand JornadaCommand{ get; private set; }
        public ICommand PontoCommand { get; private set; }

        #endregion

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
