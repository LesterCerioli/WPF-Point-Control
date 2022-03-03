using ControlePonto.Domain.jornada;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.jornada
{
    public class JornadaTrabalhoViewModel : ViewModelBase
    {
        private IJornadaTrabalhoRepository repository;
        private JornadaTrabalho jornada;

        private ICommand _salvarCommand;
        private ICommand _fecharCommand;

        public JornadaTrabalhoViewModel(IJornadaTrabalhoRepository repository)
        {
            this.repository = repository;
            this.jornada = repository.findJornadaAtiva();
            criarDias();
            this.DiaJornadaSelecionado = Dias[1]; //Segunda-feira

            _salvarCommand = new RelayCommand(salvar, podeSalvar);
            _fecharCommand = new RelayCommand(fechar);
        }

        private void criarDias()
        {
            Dias = new List<DiaJornadaViewModel>(7);
            foreach (DiaJornada dia in jornada.Dias)
            {
                var diaVM = new DiaJornadaViewModel(jornada, dia.DiaSemana);
                diaVM.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => { RaisePropertyChanged("PodeTrocarDia"); };
                Dias.Add(diaVM);
            }
        }

        #region Propriedades

        public List<DiaJornadaViewModel> Dias { get; private set; }
        
        private DiaJornadaViewModel _diaJornadaSelecionado;
        public DiaJornadaViewModel DiaJornadaSelecionado
        {
            get { return _diaJornadaSelecionado; }
            set { SetField(ref _diaJornadaSelecionado, value); }
        }

        public bool PodeTrocarDia { get { return podeSalvar(); } }

        public ICommand SalvarCommand { get { return _salvarCommand; } }
        public ICommand FecharCommand { get { return _fecharCommand; } }
        #endregion

        private void salvar()
        {
            try
            {
                foreach (DiaJornadaViewModel diaVM in Dias)
                {
                    jornada.cadastrarDia(diaVM.dia.DiaSemana,
                        diaVM.Entrada,
                        diaVM.Saida,
                        diaVM.Folga);
                }

                repository.save(jornada);
                showMessageBox("Todos os dias da jornada de trabalho foram salvos com sucesso", "Sucesso", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                showMessageBox(ex.Message, "Não foi possível salvar", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private bool podeSalvar()
        {
            foreach (DiaJornadaViewModel dia in Dias)
            {
                if (!dia.isModelValid())
                    return false;
            }
            return true;
        }

        private void fechar()
        {
            requestView(CLOSE);
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
