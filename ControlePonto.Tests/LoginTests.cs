using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.factories;
using ControlePonto.Tests.mocks;

namespace ControlePonto.Tests
{
    [TestClass]
    public class LoginTests
    {
        ILoginService loginService;
        IUsuarioRepositorio usuarioRepositorio;
        UsuarioFactory usuarioFactory;

        [TestInitialize]
        public void SetupTest()
        {
            usuarioRepositorio = new UsuarioMockRepositorio();
            usuarioFactory = new UsuarioFactory(new LoginJaExisteSpecification(usuarioRepositorio), new LoginValidoSpecification(), new SenhaValidaSpecification());
            LoginJaExisteSpecification loginJaExiste = new LoginJaExisteSpecification(usuarioRepositorio);
            usuarioFactory.LoginEmUsoSpecification = loginJaExiste;
            loginService = new LoginService(usuarioRepositorio, usuarioFactory, SessaoLogin.getSessao());            

            usuarioRepositorio.save(usuarioFactory.criarUsuario("João", "joaozinho", "123456"));
            usuarioRepositorio.save(usuarioFactory.criarUsuario("Maria", "maria", "123456"));
        }

        [TestMethod]
        public void testLoginCorreto()
        {
            SessaoLogin.getSessao().encerrar();
            Usuario u = loginService.Logar("joaozinho", "123456");
            SessaoLogin.getSessao().encerrar();

            Assert.AreEqual(u.Nome, "João");
        }

        [TestMethod]
        [ExpectedException(typeof(LoginInvalidoException))]
        public void testLoginInvalido()
        {
            loginService.Logar("joao", "132");
        }

        [TestMethod]
        [ExpectedException(typeof(SessaoNaoEncerradaException))]
        public void testLoginSessaoNaoEncerrada()
        {
            SessaoLogin.getSessao().encerrar();
            loginService.Logar("joaozinho", "123456");
            loginService.Logar("maria", "123456");
        }
    }
}
