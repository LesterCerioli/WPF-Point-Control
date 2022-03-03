using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using ControlePonto.WPF.window.consulta;
using ControlePonto.WPF.window.ponto.controle.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto.controle
{
    public class ControlarPontoViewModel : ViewModelBase
    {        
        private RelatorioService relatorioService;
        private PontoService pontoService;
        private IPontoDiaRepository pontoRepository;

        public const int EDITAR_PONTO = 1;

        public ControlarPontoViewModel(UnitOfWork unitOfWork, IUsuarioRepositorio usuarioRepository, PontoService pontoService, IPontoDiaRepository pontoRepository, RelatorioService relatorioService)
        {            
            this.pontoService = pontoService;
            this.pontoRepository = pontoRepository;
            this.relatorioService = relatorioService;

            this.Funcionarios = usuarioRepository.findFuncionarios();
            this.FuncionarioEscolhido = Funcionarios[0];
            this.ExibirCommand = new RelayCommand(exibir);

            base.unitOfWork = unitOfWork;
            unitOfWork.openConnection();
        }

        #region Topo
        public List<Funcionario> Funcionarios { get; private set; }
        public Funcionario FuncionarioEscolhido { get; set; }

        public DateTime MesDoAno { get; private set; } = DateTime.Today;

        public DateTime MaxData { get { return DateTime.Today; } }

        public string DataStringFormat { get { return "MMMM 'de' yyyy"; } }

        public ICommand ExibirCommand { get; private set; }
        #endregion

        #region Centro
        private List<DiaControlePontoViewModel> _dias;
        public List<DiaControlePontoViewModel> Dias
        {
            get { return _dias; }
            set
            {
                SetField(ref _dias, value);
            }
        }
        public DiaControlePontoViewModel DiaSelecionado { get; set; } 
        #endregion

        private void exibir()
        {
            var dataInicio = new DateTime(MesDoAno.Year, MesDoAno.Month, 1);
            var dataFim = dataInicio.AddMonths(1).AddDays(-1);
            var relatorio = relatorioService.gerarRelatorio(FuncionarioEscolhido, dataInicio, dataFim);
            
            Dias = relatorio.Dias
                .Select(dia => new DiaControlePontoViewModel(this, FuncionarioEscolhido, dia))
                .ToList();
        }

        #region Request Criar Dia
        public void requestCriarDia(Funcionario funcionario, DateTime date)
        {
            string mensagem = $"Tem certeza que deseja criar um ponto para {funcionario.Nome} no dia {date.ToShortDateString()}";
            Action<MessageBoxResult> callBack = ((MessageBoxResult r) =>
            {
                if (r == MessageBoxResult.Yes)
                {
                    criarDia(funcionario, date);
                }
            });

            showMessageBox(callBack,
                    mensagem,
                    "Confirmar ação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);
        }

        private void criarDia(Funcionario funcionario, DateTime date)
        {
            try
            {
                var ponto = pontoService.criarPontoParaFuncionarioEm(funcionario, date);
                requestEditarDia(funcionario, date);
            }
            catch (Exception ex)
            {
                showMessageBox(ex.Message, "Erro ao criar ponto", icon: MessageBoxImage.Error);
            }
        }
        #endregion

        #region Request Editar Dia
        public void requestEditarDia(Funcionario funcionario, DateTime date)
        {
            var ponto = pontoRepository.findPontoTrabalho(funcionario, date);
            requestEditarDia(ponto);
        }

        public void requestEditarDia(DiaTrabalho diaTrabalho)
        {
            requestView(new EditarPontoEventArgs(diaTrabalho));
            exibir();
        } 
        #endregion

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
