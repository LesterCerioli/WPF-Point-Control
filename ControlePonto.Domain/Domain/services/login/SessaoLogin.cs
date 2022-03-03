using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlePonto.Domain.usuario;

namespace ControlePonto.Domain.services.login
{
    public class SessaoLogin
    {
        public Usuario UsuarioLogado { get; protected set; }

        private static SessaoLogin _sessaoAtual;

        protected SessaoLogin() { }

        public static SessaoLogin getSessao()
        {
            if (_sessaoAtual == null)
                _sessaoAtual = new SessaoLogin();
            return _sessaoAtual;
        }

        public void iniciar(Usuario usuario)
        {
            if (UsuarioLogado != null)
                throw new SessaoNaoEncerradaException(UsuarioLogado);
            UsuarioLogado = usuario;
        }

        public void encerrar()
        {
            UsuarioLogado = null;            
        }
    }
}
