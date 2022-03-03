using ControlePonto.Domain.ponto;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class PontoDiaMap : ClassMap<PontoDia>
    {
        public PontoDiaMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Data)
                .Not.Nullable();

            Map(x => x.Tipo)
                .Not.Nullable();

            References(x => x.Funcionario)
                .Not.Nullable();            
        }
    }
}
