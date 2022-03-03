using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlePonto.WPF.window.consulta.controle
{
    /// <summary>
    /// Interaction logic for SelecaoDataWindow.xaml
    /// </summary>
    public partial class SelecaoDataWindow : WindowBase
    {
        public SelecaoDataWindow(SelecaoDataViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, framework.ViewRequestEventArgs e)
        {   
            switch (e.RequestCode)
            {
                case SelecaoDataViewModel.PONTO_VIEW:                    
                    var viewModel = ViewModel as SelecaoDataViewModel;
                    PontoWindowFactory.criarPontoDoFuncionarioWindow(viewModel.DiaTrabalhoSelecionado, viewModel.unitOfWork).ShowDialog();
                    Close();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }
        }
    }
}
