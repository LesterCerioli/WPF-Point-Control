using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Tests.mocks;
using ControlePonto.Tests.mocks.repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests
{
    [TestClass]
    public partial class PontoTests
    {
        private TipoIntervaloFactory tipoIntervaloFactory;
        private TipoIntervalo tipoAlmoco;
        private SessaoLogin sessaoLogin;
        private Funcionario funcionario;

        [TestInitialize]
        public void setUp()
        {
            funcionario = new FuncionarioFactory().criarFuncionario("Jhon Doe", "doe", "123456", "", "41617099864");
            sessaoLogin = new SessaoLoginMock(funcionario);            
            tipoIntervaloFactory = new TipoIntervaloFactory(new NomeIntervaloJaExisteSpecification(new TipoIntervaloMockRepository()));
            tipoAlmoco = tipoIntervaloFactory.criarTipoIntervalo("ALMOÇO");            
        }

        #region Helper methods

        private Funcionario criarFuncionario()
        {
            return new FuncionarioFactory().criarFuncionario("Guilherme", "gui", "123", "", "41617099864");
        }

        private DiaTrabalho criarPontoTrabalhoDoDia(int dia, int mes, int ano, int hora = 9, int minuto = 0)
        {
            var dt = new DataHoraMockStrategy(new DateTime(ano, mes, dia, hora, minuto, 0));
            return criarFactory().criarDiaTrabalho(dt, sessaoLogin);
        }

        private PontoService criarService(IDataHoraStrategy dataHoraStrategy = null, IPontoDiaRepository repository = null, Usuario logado = null)
        {            
            var sessao = sessaoLogin;
            if (logado != null)
                sessao = new SessaoLoginMock(logado);

            return FactoryHelper.criarPontoService(sessao, dataHoraStrategy, repository);
        }

        private Usuario criarUsuarioAdministrador()
        {
            var repository = new UsuarioMockRepositorio();

            return new UsuarioFactory(new LoginJaExisteSpecification(repository), new LoginValidoSpecification(), new SenhaValidaSpecification())
                .criarUsuario("Administrador", "admin", "12345678");
        }

        private JornadaTrabalho criarJornada()
        {
            return new JornadaTrabalhoFactory(new JornadaTrabalhoMockRepository()).criarJornadaTrabalho();
        }

        private PontoFactory criarFactory(IPontoDiaRepository repo = null)
        {
            if (repo == null)
                repo = new PontoDiaMockRepository();

            return FactoryHelper.criarPontoFactory(repo);
        }

        #endregion

        [TestMethod, TestCategory("Trabalho"), TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void diaTrabalhoSoDeveSerCriadoPelaFactory()
        {
            var ponto = criarFactory().criarDiaTrabalho(new DataHoraMockStrategy(22, 8, 2014), sessaoLogin);
            Assert.IsNotNull(ponto);

            ponto = new DiaTrabalhoComum(new DateTime(2014, 8, 22), new TimeSpan(19, 30, 0), null);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveSerCriadoComDataHoraCorreta()
        {
            var esperado = new DataHoraMockStrategy(22, 8, 2014, 19, 30);
            var ponto = criarFactory().criarDiaTrabalho(esperado, sessaoLogin);

            Assert.AreEqual(new DateTime(2014, 8, 22, 0, 0, 0), ponto.Data);
            Assert.AreEqual(new TimeSpan(19, 30, 0), ponto.Inicio);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveSerEncerradoComDataHoraCorreta()
        {
            var fimEsperado = new DataHoraMockStrategy(new DateTime(2014, 8, 22, 22, 30, 0));
            var service = criarService(fimEsperado);

            var ponto = service.iniciarDia();
            service.encerrarDia(ponto);

            Assert.AreEqual(new TimeSpan(22, 30, 0), ponto.Fim);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveSerCriadoAssociadoAoFuncionario()
        {
            var ponto = criarFactory().criarDiaTrabalho(new DataHoraMockStrategy(22, 8, 2016), sessaoLogin);

            Assert.AreEqual(funcionario, ponto.Funcionario);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveContabilizarIntervalos()
        {
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var saidaAlmoco = new DateTime(2014, 8, 22, 13, 30, 0);
            var entradaLanche = new DateTime(2014, 8, 22, 16, 0, 0);
            var saidaLanche = new DateTime(2014, 8, 22, 16, 15, 0);            
            var tipoLanche = tipoIntervaloFactory.criarTipoIntervalo("LANCHE");
            var dtMock = new DataHoraMockListStrategy(
                    new DateTime(2014, 8, 22, 9, 0, 0),
                    entradaAlmoco,
                    saidaAlmoco,
                    entradaLanche,
                    saidaLanche);

            var ponto = criarFactory().criarDiaTrabalho(dtMock, sessaoLogin);

            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            ponto.registrarIntervalo(tipoLanche, dtMock);
            ponto.registrarIntervalo(tipoLanche, dtMock);
            
            Assert.AreEqual(entradaAlmoco.TimeOfDay, ponto.getIntervalo(tipoAlmoco).Entrada);
            Assert.AreEqual(saidaAlmoco.TimeOfDay, ponto.getIntervalo(tipoAlmoco).Saida);
            Assert.AreEqual(entradaLanche.TimeOfDay, ponto.getIntervalo(tipoLanche).Entrada);
            Assert.AreEqual(saidaLanche.TimeOfDay, ponto.getIntervalo(tipoLanche).Saida);
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(IntervaloEmAbertoException))]
        public void diaNaoDeveEncerrarComPontoAberto()
        {
            var inicioDia = new DateTime(2014, 8, 22, 9, 0, 0);
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var encerramentoDia = new DateTime(2014, 8, 22, 15, 0, 0);
            
            var dtMock = new DataHoraMockListStrategy(
                    inicioDia,
                    entradaAlmoco,
                    encerramentoDia);

            var service = criarService(dtMock);
            var ponto = service.iniciarDia();

            ponto.registrarIntervalo(tipoAlmoco, dtMock);
            service.encerrarDia(ponto);            
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(DiaEmAbertoException))]
        public void diaNaoDeveIniciarSeHouverPontoAbertoEmDiasAnteriores()
        {
            var pontoAntigo = criarPontoTrabalhoDoDia(10, 6, 2016);
            var repositorio = new PontoDiaMockRepository();

            repositorio.save(pontoAntigo);

            //Vou simular o dia de hoje
            var service = criarService(new DataHoraMockStrategy(13, 6, 2016), repositorio);
            var pontoHoje = service.iniciarDia();
        }

        [TestMethod, TestCategory("Trabalho")]
        public void diaPodeIniciarSeFuncionarioNaoPossuirPontoEmAberto()
        {
            var funcionarioCorreto = new FuncionarioFactory().criarFuncionario("Thais", "tatacs", "123456", "", "41617099864");
            var pontoAntigo = criarPontoTrabalhoDoDia(10, 6, 2016);
            var repositorio = new PontoDiaMockRepository();
            var inicioDoDia = new DataHoraMockStrategy(13, 6, 2016);

            repositorio.save(pontoAntigo);

            try
            {
                var service = criarService(inicioDoDia, repositorio);
                service.iniciarDia();
                Assert.Fail("O dia não deveria ter iniciado!");
            }
            catch (DiaEmAbertoException ex)
            {
                Assert.AreEqual("O ponto do dia 10/06/2016 não foi encerrado", ex.Message);
            }

            var serviceCorreto = criarService(inicioDoDia, repositorio, funcionarioCorreto);
            var ponto = serviceCorreto.iniciarDia();

            Assert.AreEqual(new DateTime(2016, 6, 13), ponto.Data.Date);
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(PontoDiaJaExisteException))]
        public void funcionarioNaoPodeTerDoisPontosParaMesmoDia()
        {
            var rep = new PontoDiaMockRepository();
            var mesmoDia = new DataHoraMockStrategy(22, 8, 2014);
            var service = criarService(mesmoDia, rep);

            var ponto = service.iniciarDia();
            service.encerrarDia(ponto);

            rep.save(ponto);

            service.iniciarDia();
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(IntervaloJaRegistradoException))]
        public void pontoNaoDeveRegistrarIntervalosRepetidos()
        {
            var saidaAlmoco = new DateTime(2014, 8, 22, 12, 30, 0);
            var horarios = new DataHoraMockListStrategy(
                new DateTime(2014, 8, 22, 9, 0, 0), //Inicio do dia
                new DateTime(2014, 8, 22, 12, 30, 0), //Entrada almoço
                saidaAlmoco,
                new DateTime(2014, 8, 22, 17, 00, 0) //Horário de saída/erro
            );
            var service = criarService(horarios);

            var ponto = service.iniciarDia();
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoAlmoco, horarios);
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(IntervaloNaoRegistradoException))]
        public void pontoDeveAlertarIntervalosNaoRegistradosQuandoForSolicitadoPeloIntervalo()
        {
            var service = criarService(new DataHoraMockStrategy(22, 8, 2014));
            var ponto = service.iniciarDia();

            ponto.getIntervalo(tipoAlmoco);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveCalcularHorasExtras()
        {
            var ponto = criarPontoTrabalhoDoDia(22, 8, 2014);                                           //09:00 - INÍCIO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 15));//12:15 - ALMOÇO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 13, 00));//13:00 - SAÍDA ALMOÇO
            criarService(new DataHoraMockStrategy(22, 8, 2014, 19, 00)).encerrarDia(ponto);     //19:00 - FIM

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Sunday, DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));

            /* DESCRIÇÃO | ENTRADA | SAÍDA | DURAÇÃO | TOTAL
             * TRABALHO  |  09:00  | 19:00 | 10:00   | 10:00
             * ALMOÇO    |  12:15  | 13:00 | 00:45   | 09:15
             *                               EXTRA   | 01:15 (Jornada configurada para 8 horas) */            
            Assert.AreEqual(new TimeSpan(1, 15, 0), ponto.calcularHorasExtras(jornada));
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(DiaEmAbertoException))]
        public void pontoNaoDeveCalcularHorasExtrasQuandoEstiverAberto()
        {
            var ponto = criarPontoTrabalhoDoDia(22, 8, 2014);                                           //09:00 - INÍCIO
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 15));//12:15 - ALMOÇO

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Sunday, DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(1, 0, 0));
            ponto.calcularHorasExtras(jornada);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveCalcularHorasExtrasDoDiaCorreto()
        {
            var ponto = criarPontoTrabalhoDoDia(11, 6, 2016, 10, 0); //Sábado
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 00));
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 30));
            criarService(new DataHoraMockStrategy(22, 8, 2014, 19, 00)).encerrarDia(ponto);

            var jornada = criarJornada();
            //Configurado para 6 horas (7 horas - 1 hora de folga)
            jornada.cadastrarDia(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(17, 0, 0), new TimeSpan(1, 0, 0));
            Assert.AreEqual(new TimeSpan(2, 30, 0), ponto.calcularHorasExtras(jornada));
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveCalcularHorasTrabalhadas()
        {
            var ponto = criarPontoTrabalhoDoDia(22, 8, 2014);
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12));
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 13));
            criarService(new DataHoraMockStrategy(22, 8, 2014, 18)).encerrarDia(ponto);

            Assert.AreEqual(new TimeSpan(8, 0, 0), ponto.calcularHorasTrabalhadas());
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveCalcularHorasExtrasEmDiasDeFolga()
        {
            var ponto = criarPontoTrabalhoDoDia(12, 6, 2016, 10, 0); //Domingo
            criarService(new DataHoraMockStrategy(12, 6, 2016, 12, 00)).encerrarDia(ponto);

            var jornada = criarJornada();
            Assert.AreEqual(new TimeSpan(2, 0, 0), ponto.calcularHorasExtras(jornada));
            Assert.AreEqual(new TimeSpan(2, 0, 0), ponto.calcularHorasTrabalhadas());
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(IntervaloEmAbertoException))]
        public void pontoNaoDeveEntrarEmDuasPausasAoMesmoTempo()
        {
            var dia = new DateTime(2014, 8, 22, 9, 0, 0);
            var entradaAlmoco = new DateTime(2014, 8, 22, 12, 00, 0);
            var entradaLanche = new DateTime(2014, 8, 22, 12, 30, 0);
            var horarios = new DataHoraMockListStrategy(
                dia,
                dia,
                entradaAlmoco,
                entradaLanche               
            );
            var service = criarService(horarios);

            var ponto = service.iniciarDia();
            ponto.registrarIntervalo(tipoAlmoco, horarios);
            ponto.registrarIntervalo(tipoIntervaloFactory.criarTipoIntervalo("LANCHE"), horarios);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoDeveCalcularHorasDevedoras()
        {
            var ponto = criarPontoTrabalhoDoDia(18, 6, 2016, 9);
            criarService(new DataHoraMockStrategy(18, 6, 2016, 16)).encerrarDia(ponto);

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(0, 0, 0));
            Assert.AreEqual(new TimeSpan(2, 0, 0), ponto.calcularHorasDevedoras(jornada));
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(DiaEmAbertoException))]
        public void pontoNaoDeveCalcularHorasDevedorasSeEstiverAberto()
        {
            var ponto = criarPontoTrabalhoDoDia(18, 6, 2016, 9);
            var jornada = criarJornada();
            
            ponto.calcularHorasDevedoras(jornada);
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoNaoDeveCalcularHorasExtrasNegativas()
        {
            //Quando possuo hora devedora, não devo calcular extras como negativo
            var ponto = criarPontoTrabalhoDoDia(18, 6, 2016, 9);
            criarService(new DataHoraMockStrategy(18, 6, 2016, 16)).encerrarDia(ponto);

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Saturday, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0), new TimeSpan(0, 0, 0));
            //2:00 devedoras
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.calcularHorasExtras(jornada));
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoNaoDeveCalcularHorasDevedorasNegativas()
        {
            var ponto = criarPontoTrabalhoDoDia(11, 6, 2016, 10, 0); //Sábado
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 00));
            ponto.registrarIntervalo(tipoAlmoco, new DataHoraMockStrategy(22, 8, 2014, 12, 30));
            criarService(new DataHoraMockStrategy(22, 8, 2014, 19, 00)).encerrarDia(ponto);

            var jornada = criarJornada();
            jornada.cadastrarDia(DayOfWeek.Saturday, new TimeSpan(10, 0, 0), new TimeSpan(17, 0, 0), new TimeSpan(1, 0, 0));
            //2:30 extras
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.calcularHorasDevedoras(jornada)); 
        }

        [TestMethod, TestCategory("Trabalho")]
        [ExpectedException(typeof(PontoDiaJaExisteException))]
        public void naoDeveExistirMaisDeUmPontoDoFuncionarioNoDia()
        {
            var repo = new PontoDiaMockRepository();
            var service = criarService(new DataHoraMockStrategy(DateTime.Today), repo);
            var ponto = service.iniciarDia();
            service.encerrarDia(ponto);

            criarFactory(repo).criarDiaTrabalho(new DataHoraMockStrategy(DateTime.Today), sessaoLogin);            
        }

        [TestMethod, TestCategory("Trabalho")]
        public void pontoRepositoryDeveRecuperarDia()
        {
            var repo = new PontoDiaMockRepository();
            var horarios = new DataHoraMockListStrategy(
                new DateTime(2016, 8, 13, 9, 0, 0),
                new DateTime(2016, 8, 13, 9, 0, 0),
                new DateTime(2016, 8, 13, 10, 0, 0),
                new DateTime(2016, 8, 13, 11, 0, 0),
                new DateTime(2016, 8, 13, 13, 0, 0)
            );
            var service = criarService(horarios, repo);
            
            var dia = service.iniciarDia();            
            service.registrarIntervalo(tipoAlmoco, dia);
            service.registrarIntervalo(tipoAlmoco, dia);
            service.encerrarDia(dia);

            var diaRecuperado = repo.findPontoTrabalho(dia.Funcionario, dia.Data);            

            Assert.AreEqual(dia.Data, diaRecuperado.Data);
            Assert.AreEqual(dia.Funcionario.Nome, diaRecuperado.Funcionario.Nome);
        }

        [TestMethod, TestCategory("Trabalho"), TestCategory("Administrador")]
        public void administradorPodeCriarPontoParaFuncionarioNoDiaQueNãoExistir()
        {
            var repository = new PontoDiaMockRepository();            
            var date = DateTime.Today;
            var admLogado = criarUsuarioAdministrador();
            var service = criarService(repository: repository, logado: admLogado);

            var novoDiaTrabalho = service.criarPontoParaFuncionarioEm(funcionario, date);

            var ponto = repository.findPontoTrabalho(funcionario, date);

            Assert.AreEqual(date, ponto.Data);            
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Inicio);
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Fim);
            Assert.AreEqual(novoDiaTrabalho.Data, ponto.Data);
            Assert.AreEqual(novoDiaTrabalho.Inicio, ponto.Inicio);
            Assert.AreEqual(novoDiaTrabalho.Fim, ponto.Fim);
            Assert.AreEqual(funcionario, novoDiaTrabalho.Funcionario);
        }

        [TestMethod, TestCategory("Trabalho"), TestCategory("Administrador"), TestCategory("Quebra de contrato")]
        [ExpectedException(typeof(PreconditionException))]
        public void somenteAdministradorPodeCriarPontoParaFuncionarioEmDiaEspecifico()
        {
            var service = criarService(logado: funcionario);
            var date = DateTime.Today;

            var novoDiaTrabalho = service.criarPontoParaFuncionarioEm(funcionario, date);
        }

        [TestMethod, TestCategory("Trabalho"), TestCategory("Administrador")]
        public void quandoCriarPontoEmDiaEspecificoTodosOsIntervalosDevemSerIniciadosComZero()
        {
            var repository = new PontoDiaMockRepository();
            var date = DateTime.Today;
            var admLogado = criarUsuarioAdministrador();
            var service = criarService(repository: repository, logado: admLogado);

            var novoDiaTrabalho = service.criarPontoParaFuncionarioEm(funcionario, date);

            var ponto = repository.findPontoTrabalho(funcionario, date);

            Assert.AreEqual(date, ponto.Data);
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Inicio);
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Fim);
            Assert.AreEqual(novoDiaTrabalho.Data, ponto.Data);
            Assert.AreEqual(novoDiaTrabalho.Inicio, ponto.Inicio);
            Assert.AreEqual(novoDiaTrabalho.Fim, ponto.Fim);
            Assert.AreEqual(funcionario, novoDiaTrabalho.Funcionario);

            Assert.AreEqual(1, ponto.Intervalos.Count);
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Intervalos.First().Entrada);
            Assert.AreEqual(new TimeSpan(0, 0, 0), ponto.Intervalos.First().Saida);
        }
    }
}
