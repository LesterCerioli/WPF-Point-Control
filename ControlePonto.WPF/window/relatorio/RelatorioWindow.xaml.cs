using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.WPF.window.consulta;
using Microsoft.Win32;
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

namespace ControlePonto.WPF.window.relatorio
{
    /// <summary>
    /// Interaction logic for RelatorioWindow.xaml
    /// </summary>
    public partial class RelatorioWindow : WindowBase
    {
        public RelatorioWindow(RelatorioViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        protected override void viewRequested(object sender, framework.ViewRequestEventArgs e)
        {
            switch (e.RequestCode)
            {
                case RelatorioViewModel.VIEW_PONTO:
                    var viewModel = ViewModel as RelatorioViewModel;
                    var dia = (viewModel.DiaSelecionado.DiaRelatorio as IDiaComPonto).PontoDia as DiaTrabalho;
                    PontoWindowFactory.criarPontoDoFuncionarioWindow(dia, viewModel.unitOfWork).ShowDialog();
                    break;

                default:
                    base.viewRequested(sender, e);
                    break;
            }
        }

        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (ViewModel as RelatorioViewModel).ExibirPontoCommand.Execute(null);
        }

        private void ExportarExcel_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel as RelatorioViewModel);

            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Planilha Excel | *.xlsx";
            saveDialog.FileName = viewModel.getSugestaoFilename();
            saveDialog.AddExtension = true;
            if (saveDialog.ShowDialog() == true)
            {
                viewModel.ExportarCommand.Execute(saveDialog.FileName);
            }
        }
    }
}
