using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.login
{
    public class SessaoNaoEncerradaException : Exception
    {
        public Usuario UsuarioSessaoAberta { get; private set; }

        public SessaoNaoEncerradaException(Usuario usuario) 
            : base(string.Format("Não é possível iniciar uma nova sessão, pois {0} ainda não encerrou sua sessão", usuario.Nome))
        {
            UsuarioSessaoAberta = usuario;
        }
    }
}
