using ControlePonto.WPF.window.feriado;
using ControlePonto.WPF.window.jornada;
using ControlePonto.WPF.window.consulta;
using ControlePonto.WPF.window.relatorio;
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

namespace ControlePonto.WPF.window.administracao
{
    /// <summary>
    /// Interaction logic for PainelControleWindow.xaml
    /// </summary>
    public partial class PainelControleWindow : WindowBase
    {
        public PainelControleWindow(PainelControleViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, framework.ViewRequestEventArgs e)
        {
            switch (e.RequestCode)
            {
                case PainelControleViewModel.VIEW_FERIADO:
                    FeriadoWindowFactory.criarCadastroFeriadoWindow().ShowDialog();
                    break;

                case PainelControleViewModel.VIEW_FOLGA:
                    PontoWindowFactory.criarFolgaWindow().ShowDialog();
                    break;

                case PainelControleViewModel.VIEW_JORNADA:
                    JornadaWindowFactory.criarJornadaWindow().ShowDialog();
                    break;

                case PainelControleViewModel.VIEW_RELATORIO:
                    RelatorioWindowFactory.criarRelatorioWindow().ShowDialog();
                    break;

                case PainelControleViewModel.VIEW_PONTO:
                    PontoWindowFactory.criarControlarPontoWindow().ShowDialog();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }

        }
    }
}
