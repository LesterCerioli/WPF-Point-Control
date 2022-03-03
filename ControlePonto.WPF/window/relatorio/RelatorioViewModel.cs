using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.relatorio
{
    public class RelatorioViewModel : ViewModelBase
    {
        public const int VIEW_PONTO = 1;

        private IUsuarioRepositorio usuarioRepository;
        private ITipoIntervaloRepository tipoIntervaloRepository;
        private RelatorioService relatorioService;
        private RelatorioPonto ultimoRelatorioGerado;

        public RelatorioViewModel(IUsuarioRepositorio usuarioRepository, ITipoIntervaloRepository tipoIntervaloRepository, RelatorioService relatorioService, IUnitOfWork unitOfWork)
        {
            this.usuarioRepository = usuarioRepository;
            this.tipoIntervaloRepository = tipoIntervaloRepository;
            this.relatorioService = relatorioService;

            this.Funcionarios = usuarioRepository.findFuncionarios();
            this.FuncionarioEscolhido = Funcionarios.FirstOrDefault();

            var today = DateTime.Today;
            this.PeriodoInicio = new DateTime(today.Year, today.Month, 1);
            this.PeriodoFim = PeriodoInicio.AddMonths(1).AddDays(-1);

            this.ExibirCommand = new RelayCommand(exibir);
            this.ExportarCommand = new RelayParameterCommand<string>(exportarExcel);
            this.ExibirPontoCommand = new RelayCommand(exibirPonto);

            this.unitOfWork = unitOfWork;
            unitOfWork.openConnection();
        }

        #region Propriedades

        public List<Funcionario> Funcionarios { get; private set; }

        public Funcionario FuncionarioEscolhido { get; set; }
        
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
            set { SetField(ref _periodoFim, value); }
        }

        private DateTime _periodoFimMinimo;
        public DateTime PeriodoFimMinimo
        {
            get { return _periodoFimMinimo; }
            set { SetField(ref _periodoFimMinimo, value); }
        }
        

        public ICommand ExibirCommand { get; private set; }
        public ICommand ExportarCommand { get; private set; }
        public bool PodeExportar { get { return ultimoRelatorioGerado != null; } }

        private List<DiaRelatorioViewModel> _dias;
        public List<DiaRelatorioViewModel> Dias
        {
            get { return _dias; }
            set 
            { 
                SetField(ref _dias, value);
            }
        }

        public DiaRelatorioViewModel DiaSelecionado { get; private set; }

        public ICommand ExibirPontoCommand { get; private set; }        

        #region Rodapé

        private string _tituloRelatorioPeriodo;

        public string TituloRelatorioPeriodo
        {
            get { return _tituloRelatorioPeriodo; }
            set { SetField(ref _tituloRelatorioPeriodo, value); }
        }
        
        private int _totalDiasTrabalhados;
        public int TotalDiasTrabalhados
        {
            get { return _totalDiasTrabalhados; }
            set { SetField(ref _totalDiasTrabalhados, value); }
        }

        private string _totalHorasTrabalhadas;
        public string TotalHorasTrabalhadas
        {
            get { return _totalHorasTrabalhadas; }
            set { SetField(ref _totalHorasTrabalhadas , value); }
        }

        private string _totalHorasDevedoras;
        public string TotalHorasDevedoras
        {
            get { return _totalHorasDevedoras; }
            set { SetField(ref _totalHorasDevedoras , value); }
        }

        private string _diferencaDevedorTrabalhado;
        public string DiferencaDevedorTrabalhado
        {
            get { return  _diferencaDevedorTrabalhado; }
            set { SetField(ref _diferencaDevedorTrabalhado , value); }
        }

        private string _totalHorasExtras75;
        public string TotalHorasExtras75
        {
            get { return _totalHorasExtras75; }
            set { SetField(ref _totalHorasExtras75, value); }
        }

        private string _totalHorasExtras100;
        public string TotalHorasExtras100
        {
            get { return _totalHorasExtras100; }
            set { SetField(ref _totalHorasExtras100, value); }
        }

        private int _totalFeriadosTrabalhados;
        public int TotalFeriadosTrabalhados
        {
            get { return _totalFeriadosTrabalhados; }
            set { SetField(ref _totalFeriadosTrabalhados, value); }
        }

        private int _totalFolgas;
        public int TotalFolgas
        {
            get { return _totalFolgas; }
            set { SetField(ref _totalFolgas, value); }
        }
        
        #endregion

        #endregion

        private void exibir()
        {
            var relatorio = relatorioService.gerarRelatorio(FuncionarioEscolhido, PeriodoInicio, PeriodoFim);
            Dias = relatorio.Dias
                .Select(x => new DiaRelatorioViewModel(x))
                .ToList();

            TotalDiasTrabalhados = relatorio.getDiasTrabalhados().Count;
            var trabalhado = relatorio.calcularHorasTrabalhadas();
            var devedor = relatorio.calcularHorasDevedoras();

            TotalHorasTrabalhadas = formatarHora(trabalhado);
            TotalHorasDevedoras = formatarHora(devedor);
            DiferencaDevedorTrabalhado = formatarHora(devedor.Subtract(trabalhado));
            TotalHorasExtras75 = formatarHora(relatorio.calcularHorasExtras(75));
            TotalHorasExtras100 = formatarHora(relatorio.calcularHorasExtras(100));
            TotalFeriadosTrabalhados = relatorio.getFeriadosTrabalhados().Count;
            TotalFolgas = relatorio.getFolgas().Count;

            ultimoRelatorioGerado = relatorio;
            RaisePropertyChanged("PodeExportar");
        }

        private void exportarExcel(string path)
        {
            var exporter = new ExportExcelService(ultimoRelatorioGerado, Dias, tipoIntervaloRepository);
            exporter.Exportar(path);
        }
        
        private string formatarHora(TimeSpan hora)
        {
            return
                string.Format("{0:D2}:{1:D2}:{2:D2}", 
                (hora.Days * 24) + hora.Hours, 
                hora.Minutes,
                hora.Seconds);
        }

        private void exibirPonto()
        {            
            if (isDiaTrabalho())
                requestView(VIEW_PONTO);
        }

        private bool isDiaTrabalho()
        {
            if (DiaSelecionado != null)
            {
                if (DiaSelecionado.DiaRelatorio is IDiaComPonto)
                {
                    var dia = (DiaSelecionado.DiaRelatorio as IDiaComPonto);
                    return (dia.PontoDia is DiaTrabalho);
                }
            }
            return false;
        }

        public string getSugestaoFilename()
        {
            return $"{ultimoRelatorioGerado.Funcionario.Nome}_" +
                $"{ultimoRelatorioGerado.PeriodoInicio.ToString("dd-MM-yyyy")}___" +
                $"{ultimoRelatorioGerado.PeriodoFim.ToString("dd-MM-yyyy")}.xlsx";
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
