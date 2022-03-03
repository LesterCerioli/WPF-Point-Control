using ControlePonto.Domain.factories;
using ControlePonto.Domain.feriado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.feriado
{
    public class FeriadoWindowFactory
    {
        public static CadastrarFeriadoWindow criarCadastroFeriadoWindow()
        {
            return new CadastrarFeriadoWindow(new CadastrarFeriadoViewModel(
                new FeriadoFactory(),
                RepositoryFactory.criarFeriadoRepository()
            ));
        }
    }
}
