using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class LoginInvalidoException : Exception
    {
        public LoginInvalidoException() { }
        public LoginInvalidoException(string message) : base(message)
        {

        }
    }
}
