using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.jornada;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Tests.mocks.repository;

namespace ControlePonto.Tests
{
    [TestClass]
    public class JornadaTrabalhoTests
    {
        private JornadaTrabalhoFactory criarFactory(IJornadaTrabalhoRepository repository = null)
        {
            if (repository == null)
                repository = new JornadaTrabalhoMockRepository();

            return new JornadaTrabalhoFactory(repository);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void diaJornadaTrabalhoNaoPodeSerAlterado()
        {
            var jornada = criarFactory().criarJornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            Assert.AreEqual(new TimeSpan(18, 0, 0), jornada.getDia(DayOfWeek.Monday).SaidaEsperada);
            jornada.getDia(DayOfWeek.Monday).SaidaEsperada = new TimeSpan(10, 0, 0);
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void horarioDeSaidaNaoDeveSerAntesDoDeEntrada()
        {
            var jornada = new JornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(8, 0, 0), new TimeSpan(1, 0, 0));
        }

        [TestMethod]
        public void jornadaDeveCalcularHorasTrabalhoEsperado()
        {
            var factory = criarFactory();
            var jornada = factory.criarJornadaTrabalho();
            jornada.cadastrarDia(DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            Assert.AreEqual(new TimeSpan(8,0 ,0), jornada.getDia(DayOfWeek.Monday).calcularHorasTrabalhoEsperado());
        }

        [TestMethod, TestCategory("Temporário")]
        [ExpectedException(typeof(JornadaTrabalhoJaExisteException))]
        public void soPodeExistirUmaJornadaDeTrabalho()
        {
            //Por enquanto o sistema não vai permitir o cadastro de novas jornadas de trabalho
            var repository = new JornadaTrabalhoMockRepository();
            var factory = criarFactory(repository);
            
            var jornada = factory.criarJornadaTrabalho();
            repository.save(jornada);
            factory.criarJornadaTrabalho();
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void jornadaTrabalhoSoPodeSerCriadoNaFactory()
        {
            new JornadaTrabalho();
        }
    }
}
