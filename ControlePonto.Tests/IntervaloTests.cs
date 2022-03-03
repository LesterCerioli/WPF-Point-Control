using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Domain.intervalo;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Tests.mocks.repository;

namespace ControlePonto.Tests
{
    [TestClass]
    public class IntervaloTests
    {                
        public TipoIntervaloFactory criarFactory(ITipoIntervaloRepository repository = null)
        {
            if (repository == null)
                repository = new TipoIntervaloMockRepository();

            return new TipoIntervaloFactory(new NomeIntervaloJaExisteSpecification(repository));            
        }

        [TestMethod, TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void tipoIntervaloSoDeveSerCriadoNaFactory()
        {
            try
            {
                criarFactory().criarTipoIntervalo("ALMOÇO");                
            }
            catch
            {
                Assert.Fail();
            }

            new TipoIntervalo("ALMOÇO");
        }

        [TestMethod]
        [ExpectedException(typeof(TipoIntervaloJaExisteException))]
        public void naoPodeCriarTipoIntervaloComNomeRepetido()
        {
            var repository = new TipoIntervaloMockRepository();
            var factory = criarFactory(repository);

            var tipo = factory.criarTipoIntervalo("ALMOÇO");
            repository.save(tipo);

            factory.criarTipoIntervalo("ALMOÇO");
        }

        [TestMethod]
        [ExpectedException(typeof(TipoIntervaloJaExisteException))]
        public void tipoIntervaloNaoDeveDiferenciarMaiusculaDeMinusculas()
        {
            var repository = new TipoIntervaloMockRepository();
            var factory = criarFactory(repository);

            var tipo = factory.criarTipoIntervalo("ALMOÇO");
            repository.save(tipo);

            factory.criarTipoIntervalo("almoço");   
        }
    }
}
