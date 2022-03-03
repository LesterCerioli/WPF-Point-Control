using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.services.relatorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public static class RelatorioServiceFactory
    {
        public static RelatorioService criarRelatorioService(UnitOfWork uow)
        {
            return new RelatorioService(
                RepositoryFactory.criarPontoRepository(uow),
                FeriadoServiceFactory.criarFeriadoService(),
                RepositoryFactory.criarJornadaTrabalhoRepository(),
                uow
            );
        }
    }
}
