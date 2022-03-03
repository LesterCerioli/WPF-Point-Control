using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class SessaoLoginMock : SessaoLogin
    {
        public SessaoLoginMock(Usuario usuario) 
        {
            UsuarioLogado = usuario;
        }        
    }
}
