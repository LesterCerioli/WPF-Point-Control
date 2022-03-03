using ControlePonto.Domain.factories;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.usuario;
using ControlePonto.WPF.window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.usuario
{
    public static class UsuarioWindowFactory
    {
        public static LoginWindow criarLoginWindow()
        {
            return new LoginWindow(new LoginViewModel(
                LoginServiceFactory.criarLoginService(),
                RepositoryFactory.criarUsuarioRepository()
            ));
        }
    }
}
