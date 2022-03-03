using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class LoginJaExisteException : Exception
    {
        public string Login { get; private set; }

        public LoginJaExisteException(string login) : this(login, "Esse login já está em uso por outro usuário")
        {
        }

        public LoginJaExisteException(string login, string message) : base(message)
        {
            this.Login = login;
        }
    }
}
