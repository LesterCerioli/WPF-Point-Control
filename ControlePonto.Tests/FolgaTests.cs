using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlePonto.Tests.mocks;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario;
using ControlePonto.Tests.mocks.repository;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.ponto.trabalho;

namespace ControlePonto.Tests
{    
    public partial class PontoTests
    {
        [TestMethod, TestCategory("Folga")]
        [ExpectedException(typeof(PontoDiaJaExisteException))]
        public void naoPodeHaverDuasFolgasNoMesmoDia()
        {
            var service = criarService();
            var folga = service.darFolgaPara(funcionario, DateTime.Today.AddDays(7),"Quitar horas extras ref. ao mês de Maio");            
            
            var folgaRepetida = service.darFolgaPara(funcionario, DateTime.Today.AddDays(7), "Quitar horas extras ref. ao mês passado");
        }

        [TestMethod, TestCategory("Folga")]
        [ExpectedException(typeof(FolgaDiaInvalidoException))]
        public void naoPodeDarFolgaParaDiasPassados()
        {
            var service = criarService();
            try
            {                
                var folga = service.darFolgaPara(funcionario, DateTime.Today, "Folga hoje ok!");
                Assert.IsNotNull(folga);
            }
            catch
            {
                Assert.Fail();
            }

            service.darFolgaPara(funcionario, new DateTime(2001, 1, 1), "Folga no passado");
        }

        [TestMethod, TestCategory("Folga")]
        [ExpectedException(typeof(PreconditionException))]
        public void folgaSoPodeSerCriadaPelaFactory()
        {
            var factory = criarFactory();
            var data = DateTime.Today;
            var desc = "Criação da folga";
 
            try
            {
                var folga = factory.criarDiaFolga(funcionario, data, desc);
                Assert.IsNotNull(folga);
            }
            catch
            {
                Assert.Fail();
            }

            new DiaFolga(funcionario, data, desc);
        }

        [TestMethod, TestCategory("Folga")]
        public void folgaDevePossuirUmaDescricaoValida()
        {
            Assert.IsFalse(darFolgaComDescricao("     "));
            Assert.IsFalse(darFolgaComDescricao(null));
            Assert.IsFalse(darFolgaComDescricao(""));
        }

        private bool darFolgaComDescricao(string desc)
        {
            try
            {
                criarService().darFolgaPara(funcionario, DateTime.Today, desc);
                return true;
            }
            catch(PreconditionException)
            {
                return false;
            }
        }

        [TestMethod, TestCategory("Folga")]
        [ExpectedException(typeof(PontoDiaJaExisteException))]
        public void folgaNaoPodeSerCriadaSeJaPontoJaExiste()
        {
            var repo = new PontoDiaMockRepository();
            var ponto = criarService(new DataHoraMockStrategy(DateTime.Today), repo).iniciarDia();
            
            criarService(null, repo).darFolgaPara(funcionario, DateTime.Today, "Já trabalhou neste dia...");
        }
    }
}
