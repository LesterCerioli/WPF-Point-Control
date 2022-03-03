using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.administracao
{
    public static class PainelControleWindowFactory
    {
        public static PainelControleWindow criarPainelControleWindow()
        {
            return new PainelControleWindow(new PainelControleViewModel());
        }
    }
}
