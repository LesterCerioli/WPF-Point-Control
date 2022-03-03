using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class JornadaTrabalhoJaExisteException : Exception
    {
        public JornadaTrabalhoJaExisteException() : base("O sistema não suporta mais de uma jornada de trabalho") { }
    }
}
