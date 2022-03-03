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

namespace ControlePonto.WPF.window.consulta
{
    /// <summary>
    /// Interaction logic for PontoWindow.xaml
    /// </summary>
    public partial class PontoWindow : WindowBase
    {
        public PontoWindow(ConsultarPontoViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, framework.ViewRequestEventArgs e)
        {
            switch (e.RequestCode)
            {
                case ConsultarPontoViewModel.CONSULTA_VIEW:
                    PontoWindowFactory.criarSelecaoDataWindow().ShowDialog();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }
        }
    }
}
