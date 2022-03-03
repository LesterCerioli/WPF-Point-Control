using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.consulta.funcionario
{
    public class PontoFuncionarioViewModel : ViewModelBase
    {
        private DiaTrabalho ponto;
        private IPontoDiaRepository pontoRepository;

        public PontoFuncionarioViewModel(DiaTrabalho ponto, SessaoLogin sessao, IPontoDiaRepository pontoRepository, IUnitOfWork unitOfWork)
        {
            this.ponto = ponto;            

            this.Data = ponto.Data.ToShortDateString();
            this.Funcionario = ponto.Funcionario.Nome;
            this.Entrada = ponto.Inicio;
            this.Saida = ponto.Fim ?? new TimeSpan(0, 0, 0);
            this.pontoRepository = pontoRepository;
            this.unitOfWork = unitOfWork;
            if (sessao.UsuarioLogado is Funcionario)
            {
                this.AllowEdit = false;
                this.Titulo = "Ver ponto";
            }
            else
            {
                this.AllowEdit = true;
                this.Titulo = "Editar/ver ponto";
            }

            Intervalos = ponto.Intervalos.ToList();

            SalvarCommand = new RelayCommand(confirmarSalvar, podeSalvar);
            FecharCommand = new RelayCommand(() => requestView(CLOSE));
        }

        public string Titulo { get; private set; }
        public string Data { get; private set; }
        public string Funcionario { get; private set; }
        public bool AllowEdit { get; private set; }

        private List<Intervalo> _intervalos;
        public List<Intervalo> Intervalos
        {
            get { return _intervalos; }
            set { _intervalos = value; }
        }


        private TimeSpan entrada;
        public TimeSpan Entrada
        {
            get { return entrada; }
            set
            {
                SetField(ref entrada, value);
                RaisePropertyChanged("Saida");
            }
        }

        private TimeSpan saida;
        public TimeSpan Saida
        {
            get { return saida; }
            set
            {
                SetField(ref saida, value);
                RaisePropertyChanged("Entrada");
            }
        }

        public ICommand FecharCommand { get; private set; }
        public ICommand SalvarCommand { get; private set; }

        private bool podeSalvar()
        {
            foreach (var intervalo in ponto.Intervalos)
            {
                if (intervalo.Entrada > intervalo.Saida)
                    return false;
            }

            return isModelValid();
        }

        private void confirmarSalvar()
        {
            showMessageBox(salvarAlteracoes,
                $"Tem certeza que seja alterar as horas do funcionário {Funcionario} para do dia {Data}?",
                "Confirmar ação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);
        }

        private void salvarAlteracoes(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ponto.Inicio = Entrada;
                ponto.Fim = saida;
                pontoRepository.save(ponto);
                requestView(CLOSE);
            }
        }

        protected override string validar(string propertyName)
        {
            switch (propertyName)
            {
                case "Entrada":
                case "Saida":                    
                    if (Saida < Entrada)
                    {
                        return "A saída deve ser superior ao horário de entrada";
                    }
                    return null;

                default:
                    return null;
            }
        }        
    }
}
