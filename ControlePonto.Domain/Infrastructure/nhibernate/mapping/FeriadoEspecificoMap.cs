using ControlePonto.Domain.feriado;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class FeriadoEspecificoMap : SubclassMap<FeriadoEspecifico>
    {
        public FeriadoEspecificoMap()
        {
            Map(x => x.Dia).Not.Nullable();
            Map(x => x.Ano).Not.Nullable();
        }
    }
}
