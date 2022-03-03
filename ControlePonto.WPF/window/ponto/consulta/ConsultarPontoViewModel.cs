using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.consulta
{
    public class ConsultarPontoViewModel : ViewModelBase
    {
        public const int CONSULTA_VIEW = 1;

        private Funcionario funcionario;
        private DiaTrabalho ponto;
        private PontoService pontoService;
        private ITipoIntervaloRepository tipoIntervaloRepository;

        public ICommand EncerrarDiaCommand { get; private set; }
        public ICommand EntrarIntervaloCommand { get; private set; }
        public ICommand SairIntervaloCommand { get; private set; }
        public ICommand ConsultarPontoCommand { get; private set; }

        public ConsultarPontoViewModel(Funcionario funcionario, DiaTrabalho ponto, PontoService pontoService, ITipoIntervaloRepository tipoRepository)
        {
            this.funcionario = funcionario;
            this.ponto = ponto;
            this.pontoService = pontoService;
            this.tipoIntervaloRepository = tipoRepository;

            EncerrarDiaCommand = new RelayCommand(confirmarEncerrarDia);
            ConsultarPontoCommand = new RelayCommand(abrirConsulta);
            EntrarIntervaloCommand = new RelayParameterEvaluatorCommand<TipoIntervalo>(registrarIntervalo, podeEntrarIntervalo);
            SairIntervaloCommand = new RelayParameterEvaluatorCommand<TipoIntervalo>(registrarIntervalo, podeSairIntervalo);            
        }

        #region Propriedades

        public string Titulo
        {
            get
            {
                return string.Format("Ponto de {0}", funcionario.Nome);
            }
        }

        public string DataHojeHeader
        {
            get
            {
                return DateTime.Today.ToShortDateString();
            }
        }

        public string Entrada
        {
            get
            {
                return string.Format("ENTRADA: {0}", ponto.Inicio);
            }
        }

        private List<TipoIntervalo> _intervalos;

        public List<TipoIntervalo> Intervalos
        {
            get
            {
                if (_intervalos == null)
                    _intervalos = tipoIntervaloRepository.findAll();
                return _intervalos;
            }
        }        

        #endregion

        private void registrarIntervalo(TipoIntervalo tipoIntervalo)
        {
            try
            {
                pontoService.registrarIntervalo(tipoIntervalo, ponto);
            }
            catch (Exception ex)
            {
                showMessageBox(ex.Message, "Não foi possível completar a operação", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private bool podeEntrarIntervalo(TipoIntervalo tipoIntervalo)
        {            
            return !ponto.algumIntervaloEmAberto() && !ponto.intervaloFoiRegistrado(tipoIntervalo);
        }

        private bool podeSairIntervalo(TipoIntervalo tipoIntervalo)
        {
            if (ponto.intervaloFoiRegistrado(tipoIntervalo))
            {
                var intervalo = ponto.getIntervalo(tipoIntervalo);
                return !intervalo.Saida.HasValue; //Se não houver saída, então pode registrar
            }
            return false;
        }

        private void confirmarEncerrarDia()
        {
            showMessageBox(encerrarDia, "Tem certeza que deseja encerrar sua jornada de trabalho hoje?",
                "Encerrar jornada de trabalho",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning,
                System.Windows.MessageBoxResult.No);
        }

        private void encerrarDia(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    pontoService.encerrarDia(ponto);
                    requestView(CLOSE);
                }
                catch (Exception e)
                {
                    showMessageBox(e.Message, "Não foi possível encerrar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void abrirConsulta()
        {
            requestView(CONSULTA_VIEW);
        }

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
