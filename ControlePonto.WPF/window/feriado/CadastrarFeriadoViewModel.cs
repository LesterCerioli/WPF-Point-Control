using ControlePonto.Domain.feriado;
using ControlePonto.Infrastructure.utils;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.feriado
{
    public class CadastrarFeriadoViewModel : ViewModelBase
    {
        private FeriadoFactory feriadoFactory;
        private IFeriadoRepository feriadoRepository;

        public CadastrarFeriadoViewModel(FeriadoFactory feriadoFactory, IFeriadoRepository feriadoRepository)
        {
            this.feriadoFactory = feriadoFactory;
            this.feriadoRepository = feriadoRepository;

            TipoFixo = true;
            Data = DateTime.Today;
            SequenciaDiaEscolhido = 1;
            DiaSemanaEscolhido = DateTime.Today.DayOfWeek;

            SalvarCommand = new RelayCommand(salvar, base.isModelValid);
            FecharCommand = new RelayCommand(fechar);
        }

        #region Propriedades

        private bool _tipoFixo;
        public bool TipoFixo 
        {
            get { return _tipoFixo; }
            set
            {
                if (SetField(ref _tipoFixo, value))
                {
                    mudouTipo();
                }
            }
        }

        private bool _tipoRelativo;
        public bool TipoRelativo 
        {
            get { return _tipoRelativo; }
            set 
            {
                if (SetField(ref _tipoRelativo, value))
                {
                    mudouTipo();
                }
            }
        }

        private bool _tipoEspecifico;
        public bool TipoEspecifico 
        {
            get { return _tipoEspecifico; }
            set 
            {
                if (SetField(ref _tipoEspecifico, value))
                {
                    mudouTipo();
                }
            }
        }

        public Visibility FixoOuEspecificoVisivel 
        { 
            get
            {
                if (TipoRelativo)
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

        public Visibility RelativoVisivel
        {
            get 
            {
                if (TipoRelativo)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public string Nome { get; set; }

        public int NomeMaxLenght { get { return Feriado.MAX_NOME_LENGHT; } }

        private DateTime _data;
        public DateTime Data
        {
            get { return _data; }
            set { SetField(ref _data, value); }
        }
        
        public string DataStringFormat 
        { 
            get 
            { 
                if (TipoFixo)
                    return "dd 'de' MMMM"; 
                if (TipoEspecifico)
                    return "dd 'de' MMMM 'de' yyyy";
                return "MMMM";
            } 
        }

        #region Relativo

        public int[] SequenciasDiaSemana 
        { 
            get
            {
                return new int[] { 1, 2, 3, 4, 5 };
            }
        }

        private int _sequenciaDiaEscolhido;
        public int SequenciaDiaEscolhido
        {
            get { return _sequenciaDiaEscolhido; }
            set 
            {
                if (SetField(ref _sequenciaDiaEscolhido, value))
                {
                    RaisePropertyChanged("Data");
                }
            }
        }
        
        public List<string> DiasSemana 
        {
            get
            {
                return
                Enum.GetValues(typeof(DayOfWeek))
                              .OfType<DayOfWeek>()                              
                              .Select(x => DiaSemanaTradutor.traduzir(x))
                              .ToList();
            }            
        }

        private DayOfWeek _diaSemanaEscolhido;
        public DayOfWeek DiaSemanaEscolhido
        {
            get { return _diaSemanaEscolhido; }
            set
            {
                if (SetField(ref _diaSemanaEscolhido, value))
                {
                    RaisePropertyChanged("Data");
                }
            }
        }
        

        #endregion

        public ICommand FecharCommand { get; private set; }

        public ICommand SalvarCommand { get; private set; }

        #endregion

        private void mudouTipo()
        {
            RaisePropertiesChanged("RelativoVisivel", "FixoOuEspecificoVisivel", "DataStringFormat", "Data");
        }

        private void fechar()
        {
            requestView(CLOSE);
        }

        private void salvar()
        {
            try
            {
                Feriado feriado;
                if (TipoFixo)
                    feriado = feriadoFactory.criarFeriadoFixo(Nome, Data.Day, Data.Month);
                else if (TipoEspecifico)
                    feriado = feriadoFactory.criarFeriadoEspecifico(Nome, Data.Day, Data.Month, Data.Year);
                else
                    feriado = feriadoFactory.criarFeriadoRelativo(Nome, SequenciaDiaEscolhido, DiaSemanaEscolhido, Data.Month);

                feriadoRepository.save(feriado);
                showMessageBox(cadastrarOutro, "Feriado cadastrado com sucesso. Deseja cadastrar outro?", "Sucesso", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            }
            catch (Exception ex)
            {
                showMessageBox(ex.Message, "Não foi possível completar a operação", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cadastrarOutro(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                base.resetDataContext(new CadastrarFeriadoViewModel(feriadoFactory, feriadoRepository));
            else
                fechar();
        }

        protected override string validar(string propertyName)
        {
            switch (propertyName)
            {
                case "Nome":
                    if (string.IsNullOrWhiteSpace(Nome))
                        return "O nome do feriado é obrigatório";
                    break;

                case "Data":
                    if (TipoRelativo)
                    {
                        if (SequenciaDiaEscolhido > calcularNumeroDiasSemana(DiaSemanaEscolhido, Data.Month))
                            return string.Format("O mês {0} não possui {1} {2}S",
                                Data.ToString("MMM"),
                                SequenciaDiaEscolhido,
                                DiaSemanaTradutor.traduzir(DiaSemanaEscolhido).Replace("-FEIRA", ""));
                    }
                    break;
            }
            return null;
        }

        private int calcularNumeroDiasSemana(DayOfWeek diaSemana, int mes)
        {
            int ano = DateTime.Today.Year;
            var inicio = new DateTime(ano, mes, 1);
            var fim = new DateTime(ano, mes + 1, 1).AddDays(-1);

            int total = 0;
            for (var dt = inicio; dt <= fim; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == diaSemana)
                {
                    total++;
                    dt = dt.AddDays(6);
                }
            }
            return total;
        }
    }
}
