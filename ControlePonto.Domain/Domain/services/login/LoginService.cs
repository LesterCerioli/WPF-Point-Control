using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.login
{
    public class LoginService : ILoginService
    {
        private IUsuarioRepositorio usuarioRepositorio;
        private UsuarioFactory usuarioFactory;
        private SessaoLogin sessaoLogin;

        public LoginService(IUsuarioRepositorio usuarioRepositorio, UsuarioFactory usuarioFactory, SessaoLogin sessaoLogin)
        {
            this.usuarioRepositorio = usuarioRepositorio;
            this.usuarioFactory = usuarioFactory;
            this.sessaoLogin = sessaoLogin;
        }

        public Usuario Logar(string login, string senha)
        {
            Usuario usuario = null;
            try
            {
                usuario = usuarioRepositorio.findByLogin(login);
                if (usuario == null || usuario.Senha != senha)
                    throw new LoginInvalidoException();
                sessaoLogin.iniciar(usuario);
                return usuario;
            }
            finally { }
        }        
    }
}
