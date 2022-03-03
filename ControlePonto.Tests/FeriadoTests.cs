using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.feriado;
using ControlePonto.Infrastructure.utils;

namespace ControlePonto.Tests
{
    [TestClass]
    public class FeriadoTests
    {
        private FeriadoFactory factory;

        [TestInitialize]
        public void setUp()
        {
            factory = new FeriadoFactory();
        }

        [TestMethod]
        public void feriadoRelativoDeveCalcularDia()
        {
            FeriadoRelativo feriado;
            int ano = DateTime.Today.Year;

            feriado = factory.criarFeriadoRelativo("Dia dos pais", 2, DayOfWeek.Sunday, 8); //2º domingo de agosto            
            Assert.AreEqual(new DateTime(ano, 8, 14), feriado.getData());
            
            feriado = factory.criarFeriadoRelativo("Primeira sexta-feira do mês", 1, DayOfWeek.Friday, 6);
            Assert.AreEqual(new DateTime(ano, 6, 3), feriado.getData());
        }

        [TestMethod]
        [ExpectedException(typeof(PostconditionException))]
        public void feriadoRelativoNaoDeveSerImpossivel()
        {
            FeriadoRelativo feriado = factory.criarFeriadoRelativo("Dia impossível", 5, DayOfWeek.Monday, 6);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void feriadoFixoSoDeveSerCriadoNaFactory()
        {
            var feriado = factory.criarFeriadoFixo("Dia do trabalho", 1, 8);
            Assert.IsNotNull(feriado);

            new FeriadoFixo("Dia do trabalho", 1, 8);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void feriadoEspecificoSoDeveSerCriadoNaFactory()
        {
            var feriado = factory.criarFeriadoEspecifico("Recesso da prefeitura", 1, 8, 2016);
            Assert.IsNotNull(feriado);

            new FeriadoEspecifico("Recesso da prefeitura", 1, 8, 2016);
        }

        [TestMethod]
        [ExpectedException(typeof(PreconditionException))]
        public void feriadoRelativoSoDeveSerCriadoNaFactory()
        {
            var feriado = factory.criarFeriadoRelativo("Dia dos pais", 2, DayOfWeek.Monday, 8);
            Assert.IsNotNull(feriado);

            new FeriadoRelativo("Dia dos pais", 2, DayOfWeek.Monday, 8);
        }
    }
}
