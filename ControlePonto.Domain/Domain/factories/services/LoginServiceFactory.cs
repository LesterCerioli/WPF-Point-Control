using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public class LoginServiceFactory
    {
        public static ILoginService criarLoginService()
        {
            usuario.IUsuarioRepositorio repo = RepositoryFactory.criarUsuarioRepository();

            return new LoginService(
                repo,
                new UsuarioFactory(new LoginJaExisteSpecification(repo), new LoginValidoSpecification(), new SenhaValidaSpecification()),
                SessaoLogin.getSessao());
        }
    }
}
