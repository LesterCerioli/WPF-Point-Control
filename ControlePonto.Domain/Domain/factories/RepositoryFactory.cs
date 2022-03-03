using ControlePonto.Domain.feriado;
using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.persistence;
using ControlePonto.Domain.usuario;
using ControlePonto.Infrastructure.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.factories
{
    public class RepositoryFactory
    {
        public static IUsuarioRepositorio criarUsuarioRepository()
        {
            return new UsuarioHibernateRepository();
        }

        public static IPontoDiaRepository criarPontoRepository(IUnitOfWork unitOfWork)
        {
            return new PontoDiaHibernateRepository(unitOfWork);
        }

        public static ITipoIntervaloRepository criarTipoIntervaloRepository()
        {
            return new TipoIntervaloHibernateRepository();
        }

        public static IJornadaTrabalhoRepository criarJornadaTrabalhoRepository()
        {
            return new JornadaTrabalhoHibernateRepository();
        }

        public static IFeriadoRepository criarFeriadoRepository()
        {
            return new FeriadoHibernateRepository();
        }
    }
}
