using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Infrastructure.misc;
using ControlePonto.Infrastructure.nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public class PontoServiceFactory
    {
        public static PontoService criarPontoService(UnitOfWork unitOfWork = null)
        {
            var uow = unitOfWork ?? UnitOfWorkFactory.criarUnitOfWork();
            var pontoRepository = RepositoryFactory.criarPontoRepository(uow);
            var tipoIntervaloRepository = RepositoryFactory.criarTipoIntervaloRepository();

            return new PontoService(
                criarPontoFactory(pontoRepository),
                new DataHoraLocalStrategy(),
                new FuncionarioPossuiPontoAbertoSpecification(pontoRepository),
                new FuncionarioJaTrabalhouHojeSpecification(pontoRepository),
                SessaoLogin.getSessao(),
                pontoRepository,
                tipoIntervaloRepository);
        }

        private static PontoFactory criarPontoFactory(IPontoDiaRepository repo)
        {
            return new PontoFactory(repo, FeriadoServiceFactory.criarFeriadoService());
        }
    }
}
