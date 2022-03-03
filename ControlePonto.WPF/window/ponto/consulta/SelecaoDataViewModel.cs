using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.consulta
{
    public class SelecaoDataViewModel : ViewModelBase
    {
        public const int PONTO_VIEW = 1;

        private IPontoDiaRepository pontoRepository;
        private SessaoLogin sessaoLogin;

        #region Propriedades

        public DateTime Data { get; set; }
        public DateTime MaxData { get; private set; }
        public DiaTrabalho DiaTrabalhoSelecionado { get; private set; }

        public ICommand ConfirmarCommand { get; private set; }

        #endregion

        public SelecaoDataViewModel(IPontoDiaRepository pontoRepository, SessaoLogin sessaoLogin)
        {
            this.pontoRepository = pontoRepository;
            this.sessaoLogin = sessaoLogin;

            var ontem = DateTime.Today.AddDays(-1);
            this.Data = ontem;
            this.MaxData = ontem;
            this.ConfirmarCommand = new RelayCommand(confirmarData);
        }

        private void confirmarData()
        {
            DiaTrabalhoSelecionado = pontoRepository
                .findPontoTrabalho((Funcionario)sessaoLogin.UsuarioLogado, Data);

            if (DiaTrabalhoSelecionado != null)
                requestView(PONTO_VIEW);
            else
                showMessageBox("Você não trabalhou neste dia.", "Dia não trabalhado");
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
