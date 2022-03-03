using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto.controle
{
    public partial class DiaControlePontoViewModel
    {
        public class AcaoViewModel
        {
            internal enum EAcao
            {
                Criar,
                Editar
            }

            private DiaControlePontoViewModel parentViewModel;
            public string Nome { get; private set; }
            public ICommand AcaoCommand { get; private set; }

            internal AcaoViewModel(DiaControlePontoViewModel parentViewModel, EAcao acao)
            {
                this.parentViewModel = parentViewModel;
                if (acao == EAcao.Criar)
                {
                    this.Nome = "Criar";
                    this.AcaoCommand = new RelayCommand(criarDia);
                }
                else
                {
                    this.Nome = "Editar";
                    this.AcaoCommand = new RelayCommand(editarDia);
                }                
            }            

            private void editarDia()
            {
                var grandparent = parentViewModel.parentViewModel;
                grandparent.requestEditarDia(parentViewModel.Funcionario, parentViewModel.DiaRelatorio.Data);
            }

            private void criarDia()
            {
                var grandparent = parentViewModel.parentViewModel;
                grandparent.requestCriarDia(parentViewModel.Funcionario, parentViewModel.DiaRelatorio.Data);                
            }
        }
    }
}
