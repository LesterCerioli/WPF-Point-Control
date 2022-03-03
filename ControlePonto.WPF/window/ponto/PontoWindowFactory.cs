using ControlePonto.Domain.factories;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.feriado;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.window.consulta.controle;
using ControlePonto.WPF.window.consulta.folga;
using ControlePonto.WPF.window.consulta.funcionario;
using ControlePonto.WPF.window.ponto.controle;

namespace ControlePonto.WPF.window.consulta
{
    public class PontoWindowFactory
    {
        private static RelatorioService criarRelatorioService(UnitOfWork unitOfWork)
        {
            return new RelatorioService(
                RepositoryFactory.criarPontoRepository(unitOfWork),
                new FeriadoService(RepositoryFactory.criarFeriadoRepository()),
                RepositoryFactory.criarJornadaTrabalhoRepository(),
                unitOfWork);
        }

        public static PontoWindow criarPontoWindow(DiaTrabalho ponto, PontoService pontoService)
        {
            return new PontoWindow(new ConsultarPontoViewModel(
                (Funcionario)SessaoLogin.getSessao().UsuarioLogado,
                ponto,
                pontoService,
                RepositoryFactory.criarTipoIntervaloRepository()));
        }

        public static ControleFolgaWindow criarFolgaWindow()
        {
            var unitOfWork = UnitOfWorkFactory.criarUnitOfWork();
            return new ControleFolgaWindow(new ControleFolgaViewModel(
                RepositoryFactory.criarUsuarioRepository(),
                criarRelatorioService(unitOfWork),
                PontoServiceFactory.criarPontoService(unitOfWork),
                unitOfWork
            ));
        }

        public static PontoFuncionarioWindow criarPontoDoFuncionarioWindow(DiaTrabalho ponto, IUnitOfWork uow)
        {
            return new PontoFuncionarioWindow(new PontoFuncionarioViewModel(ponto, SessaoLogin.getSessao(), RepositoryFactory.criarPontoRepository(uow), uow));
        }        

        public static SelecaoDataWindow criarSelecaoDataWindow()
        {
            var uow = UnitOfWorkFactory.criarUnitOfWork();
            return new SelecaoDataWindow(new SelecaoDataViewModel(
                RepositoryFactory.criarPontoRepository(uow), 
                SessaoLogin.getSessao()));
        }

        public static ControlarPontoWindow criarControlarPontoWindow()
        {            
            var unitOfWork = UnitOfWorkFactory.criarUnitOfWork();
            var pontoService = PontoServiceFactory.criarPontoService(unitOfWork);
            var pontoRepository = RepositoryFactory.criarPontoRepository(unitOfWork);
            var relatorio = criarRelatorioService(unitOfWork);

            return new ControlarPontoWindow(new ControlarPontoViewModel(
                unitOfWork,
                RepositoryFactory.criarUsuarioRepository(),
                pontoService,
                pontoRepository,
                relatorio
            ));
        }
    }
}
