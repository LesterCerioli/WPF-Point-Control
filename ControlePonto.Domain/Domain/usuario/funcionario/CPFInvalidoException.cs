using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario.funcionario
{
    public class CPFInvalidoException : Exception
    {
        public string RGInvalido { get; private set; }

        public CPFInvalidoException(string rg)
        {
            RGInvalido = rg;
        }

        public CPFInvalidoException(string rg, string message) : base(message)
        {
            RGInvalido = rg;
        }
    }
}
