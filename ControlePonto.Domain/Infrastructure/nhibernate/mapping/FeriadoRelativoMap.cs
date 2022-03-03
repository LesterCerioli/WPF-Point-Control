using ControlePonto.Domain.feriado;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class FeriadoRelativoMap : SubclassMap<FeriadoRelativo>
    {
        public FeriadoRelativoMap()
        {
            Map(x => x.Sequencia).Not.Nullable();

            Map(x => x.DiaSemana).CustomType<int>()
                .Not.Nullable();            
        }
    }
}
