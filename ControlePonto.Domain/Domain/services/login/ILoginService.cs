using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.login
{
    public interface ILoginService
    {
        Usuario Logar(string login, string senha);
    }
}
