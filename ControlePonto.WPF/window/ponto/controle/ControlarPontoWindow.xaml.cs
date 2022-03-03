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
using ControlePonto.WPF.framework;
using ControlePonto.WPF.window.ponto.controle.events;
using ControlePonto.WPF.window.consulta;
using ControlePonto.Domain.factories.services;

namespace ControlePonto.WPF.window.ponto.controle
{
    /// <summary>
    /// Interaction logic for ControlarPontoWindow.xaml
    /// </summary>
    public partial class ControlarPontoWindow : WindowBase
    {
        public ControlarPontoWindow(ControlarPontoViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, ViewRequestEventArgs e)
        {
            switch (e.RequestCode)
            {
                case ControlarPontoViewModel.EDITAR_PONTO:
                    var editEvent = e as EditarPontoEventArgs;
                    PontoWindowFactory.criarPontoDoFuncionarioWindow(editEvent.DiaTrabalho, ViewModel.unitOfWork).ShowDialog();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }
        }
    }
}
