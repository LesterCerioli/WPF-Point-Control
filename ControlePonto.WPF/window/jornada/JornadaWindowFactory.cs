using ControlePonto.Domain.factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.jornada
{
    public class JornadaWindowFactory
    {
        public static JornadaTrabalhoWindow criarJornadaWindow()
        {
            return new JornadaTrabalhoWindow(new JornadaTrabalhoViewModel(
                RepositoryFactory.criarJornadaTrabalhoRepository()
            ));
        }
    }
}
