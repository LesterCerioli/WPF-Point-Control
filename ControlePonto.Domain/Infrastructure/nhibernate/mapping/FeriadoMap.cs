using ControlePonto.Domain.feriado;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class FeriadoMap : ClassMap<Feriado>
    {
        public FeriadoMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Nome).Length(Feriado.MAX_NOME_LENGHT)
                .Not.Nullable();

            Map(x => x.Mes).Not.Nullable();
        }
    }
}
