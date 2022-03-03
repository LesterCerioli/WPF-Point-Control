using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.services.persistence;

namespace ControlePonto.WPF.window.consulta.folga
{
    public class ControleFolgaViewModel : ViewModelBase
    {
        private IUsuarioRepositorio usuarioRepository;
        private RelatorioService relatorioService;
        private PontoService pontoService;

        private List<DiaFolgaDTO> diasAlterados;

        public ControleFolgaViewModel(IUsuarioRepositorio usuarioRep, RelatorioService relatorioService, PontoService pontoService, IUnitOfWork unitOfWork)
        {
            this.usuarioRepository = usuarioRep;
            this.relatorioService = relatorioService;
            this.pontoService = pontoService;
            base.unitOfWork = unitOfWork;

            var today = DateTime.Today;
            this.PeriodoInicio = new DateTime(today.Year, today.Month, 1);
            this.PeriodoFim = PeriodoInicio.AddMonths(1).AddDays(-1);

            this.Funcionarios = usuarioRepository.findFuncionarios().OrderBy(x => x.Nome).ToList();
            this.FuncionarioEscolhido = Funcionarios[0];

            diasAlterados = new List<DiaFolgaDTO>();

            ExibirCommand = new RelayCommand(validarExibicao);
            SalvarCommand = new RelayCommand(confirmarSalvar, podeSalvar);
            FecharCommand = new RelayCommand(() => requestView(CLOSE));

            unitOfWork.openConnection();
        }

        #region Propriedades

        public List<Funcionario> Funcionarios{ get; private set; }

        public Funcionario FuncionarioEscolhido { get; set; }

        public Funcionario FuncionarioEmExibicao { get; private set; }

        private DateTime _periodoInicio;
        public DateTime PeriodoInicio
        {
            get { return _periodoInicio; }
            set 
            {
                if (SetField(ref _periodoInicio, value))
                {
                    if (value > PeriodoFim)
                        PeriodoFim = value;
                    PeriodoFimMinimo = value;
                }
            }
        }

        private DateTime _periodoFim;
        public DateTime PeriodoFim
        {
            get { return _periodoFim; }
            set
            {
                SetField(ref _periodoFim, value);
            }
        }

        private DateTime _periodoFimMinimo;  
        public DateTime PeriodoFimMinimo
        {
            get { return _periodoFimMinimo; }
            set { SetField(ref _periodoFimMinimo, value); }
        }
        
        
        private bool _exibirSomenteFolgas;
        public bool ExibirSomenteFolgas
        {
            get { return _exibirSomenteFolgas; }
            set 
            { 
                if (SetField(ref _exibirSomenteFolgas, value) && DiasPeriodo != null)
                {
                    aplicarFiltro(value);
                }
            }
        }        

        private List<DiaFolgaDTO> DiasPeriodo { get; set; }

        private List<DiaFolgaDTO> _diasPeriodoFiltro;
        public List<DiaFolgaDTO> DiasPeriodoFiltro
        {
            get { return _diasPeriodoFiltro; }
            private set { SetField(ref _diasPeriodoFiltro, value); }
        }

        public ICommand ExibirCommand { get; private set; }

        public ICommand SalvarCommand { get; private set; }

        public ICommand FecharCommand { get; private set; }

        #endregion

        #region Exibir Command

        private void validarExibicao()
        {
            if (diasAlterados.Count > 0)
            {
                showMessageBox(confirmarResetDiasAlterados,
                    "Você tem mudanças que ainda não foram salvas! Se exibir novos dados, tudo que você alterou até agora será perdido. Tem certeza que deseja continuar?",
                    "Confirmar operação",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning,
                    MessageBoxResult.No);
            }
            else
            {
                exibir();
            }
        }

        private void confirmarResetDiasAlterados(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                diasAlterados.Clear();
                exibir();
            }
        }

        private void exibir()
        {
            FuncionarioEmExibicao = FuncionarioEscolhido;

            DiasPeriodo = relatorioService
                .gerarRelatorio(FuncionarioEscolhido, PeriodoInicio, PeriodoFim).Dias
                .Select(x => new DiaFolgaDTO(x, FuncionarioEmExibicao))
                .ToList();
                        
            DiasPeriodo.ForEach(x => x.PropertyChanged += DiaFolgaChanged);
            aplicarFiltro(ExibirSomenteFolgas);
            RaisePropertyChanged("DiasPeriodoFiltro");
        }

        #endregion

        private void DiaFolgaChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dia = sender as DiaFolgaDTO;
            if (dia.IsDiaFolga)
            {
                if (!diasAlterados.Contains(sender))
                {
                    diasAlterados.Add(sender as DiaFolgaDTO);
                }
            }
            else
            {
                diasAlterados.Remove(dia);
            }
        }

        private void aplicarFiltro(bool somenteFolga)
        {
            if (somenteFolga)
                DiasPeriodoFiltro = DiasPeriodo.Where(x => x.IsDiaFolga).ToList();
            else
                DiasPeriodoFiltro = DiasPeriodo;
        }

        #region Salvar Command

        private void confirmarSalvar()
        {
            string dadosAlterados = getDadosAlterados();

            string mensagem = string.Format("Confirmar a mudanças abaixo: {0}{0}{1}{0}" + 
                "Tem certeza que deseja dar essa(s) folga(s)? A operação não poderá ser desfeita " +
                "e o funcionário não conseguirá registrar seu(s) ponto(s) neste(s) dia(s)",
                Environment.NewLine,
                dadosAlterados);

            showMessageBox(salvar,
                mensagem,
                "Confirmar ação", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning, 
                MessageBoxResult.No);
        }

        private string getDadosAlterados()
        {
            string dadosAlterados = "";
            foreach (DiaFolgaDTO folga in diasAlterados)
            {
                dadosAlterados += string.Format("• {0} ganhará o dia {1:dd'/'MM'/'yyyy} ({2}) de folga{3}",
                    folga.Funcionario.Nome,
                    folga.Data,
                    DiaSemanaTradutor.traduzir(folga.Data.DayOfWeek),
                    Environment.NewLine);
            }
            return dadosAlterados;
        }

        private void salvar(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                var salvos = new List<DiaFolgaDTO>();
                foreach (DiaFolgaDTO folga in diasAlterados)
                {
                    try
                    {
                        pontoService.darFolgaPara(folga.Funcionario, folga.Data, folga.Descricao);
                        salvos.Add(folga);
                    }
                    catch(Exception ex)
                    {
                        showMessageBox(string.Format("Não foi possível salvar a folga do dia {0}. {1}", 
                                folga.Data.ToShortDateString(),
                                ex.Message), 
                            "Não foi possível salvar uma folga", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                    }
                }
                diasAlterados = diasAlterados.Except(salvos).ToList();
                FuncionarioEscolhido = FuncionarioEmExibicao;
                exibir();
            }
        }

        private bool podeSalvar()
        {
            if (diasAlterados.Count > 0)
            {
                if (diasAlterados.Any(x => string.IsNullOrWhiteSpace(x.Descricao)))
                    return false;
                return true;
            }
            return false;
        }

        #endregion

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
