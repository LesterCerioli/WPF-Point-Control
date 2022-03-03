using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.usuario;
using ControlePonto.Tests.mocks;
using ControlePonto.Domain.framework;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.usuario.funcionario;

namespace ControlePonto.Tests
{
    [TestClass]
    public class UsuarioTests
    {
        IUsuarioRepositorio usuarioRepositorio;
        UsuarioFactory usuarioFactory;
        FuncionarioFactory funcionarioFactory;

        [TestInitialize]
        public void SetupTest()
        {
            usuarioRepositorio = new UsuarioMockRepositorio();
            usuarioFactory = new UsuarioFactory(new LoginJaExisteSpecification(usuarioRepositorio), new LoginValidoSpecification(), new SenhaValidaSpecification());
            funcionarioFactory = new FuncionarioFactory();

            usuarioRepositorio.save(usuarioFactory.criarUsuario("João", "joaozinho", "123456"));
        }

        [TestMethod]
        public void testCriarUsuario()
        {
            usuarioRepositorio.save(
                usuarioFactory.criarUsuario("Guilherme", "guilherme_latrova", "latrova123")
            );

            Assert.IsNotNull(usuarioRepositorio.findByLogin("guilherme_latrova"));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testLoginInvalido()
        {
            usuarioFactory.criarUsuario("Guilherme", "guilherme#latrova@!-", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testSenhaInvalida()
        {
            usuarioFactory.criarUsuario("Guilherme", "latrova", "123_123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(LoginJaExisteException))]
        public void testLoginEmUso()
        {
            usuarioFactory.criarUsuario("Mario", "joaozinho", "111111");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemNome()
        {
            usuarioFactory.criarUsuario("", "joaozinho", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemLogin()
        {
            usuarioFactory.criarUsuario("Mario", "", "123123");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testUsuarioSemSenha()
        {
            usuarioFactory.criarUsuario("Mario", "joaozinho", "");
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuNome()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH + 1),
                new string('b', Usuario.MAX_LOGIN_LENGTH),
                new string('c', Usuario.MAX_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuLogin()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH),
                new string('b', Usuario.MAX_LOGIN_LENGTH + 1),
                new string('c', Usuario.MAX_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioExcedeuSenha()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MAX_NOME_LENGTH),
                new string('b', Usuario.MAX_LOGIN_LENGTH),
                new string('c', Usuario.MAX_SENHA_LENGTH + 1));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioNomePequeno()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH - 1),
                new string('b', Usuario.MIN_LOGIN_LENGTH),
                new string('c', Usuario.MIN_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioLoginPequeno()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH),
                new string('b', Usuario.MIN_LOGIN_LENGTH - 1),
                new string('c', Usuario.MIN_SENHA_LENGTH));
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void testCriarUsuarioSenhaPequenaNaoPermitido()
        {
            usuarioFactory.criarUsuario(
                new string('a', Usuario.MIN_NOME_LENGTH),
                new string('b', Usuario.MIN_LOGIN_LENGTH),
                new string('c', Usuario.MIN_SENHA_LENGTH - 1));
        }

        [TestMethod, TestCategory("Construtor")]
        [ExpectedException(typeof(PreconditionException))]
        public void criarUsuarioSomenteNaFactory()
        {
            Usuario u = new Usuario(new String('a', 100), new String('a', 100), new String('a', 100));
        }

        [TestMethod, TestCategory("Construtor")]
        [ExpectedException(typeof(PreconditionException))]
        public void criarFuncionarioSomenteNaFactory()
        {
            Funcionario f = new Funcionario(new String('a', 100), new String('a', 100), new String('a', 100));
        }

        [TestMethod]
        [ExpectedException(typeof(CPFInvalidoException))]
        public void criarFuncionarioComCPFInvalidoNaoPossivel()
        {
            funcionarioFactory.criarFuncionario("Guilherme", "gui", "123456", "456364596", "41617099865");
        }

        [TestMethod]
        public void criarFuncionarioCorretamente()
        {
            funcionarioFactory.criarFuncionario("Guilherme", "gui", "123456", "456364596", "41617099864");
        }
    }
}
