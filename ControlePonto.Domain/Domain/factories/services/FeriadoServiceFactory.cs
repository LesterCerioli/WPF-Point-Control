using ControlePonto.Domain.feriado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories.services
{
    public static class FeriadoServiceFactory
    {
        public static FeriadoService criarFeriadoService()
        {
            return new FeriadoService(RepositoryFactory.criarFeriadoRepository());
        }
    }
}
