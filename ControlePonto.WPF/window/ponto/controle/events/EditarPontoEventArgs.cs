using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.ponto.controle.events
{
    public class EditarPontoEventArgs : ViewRequestEventArgs
    {
        public DiaTrabalho DiaTrabalho { get; private set; }

        public EditarPontoEventArgs(DiaTrabalho ponto) : base(ControlarPontoViewModel.EDITAR_PONTO)
        {
            this.DiaTrabalho = ponto;
        }
    }
}
