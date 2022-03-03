using ControlePonto.Domain.intervalo;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class TipoIntervaloMap : ClassMap<TipoIntervalo>
    {
        public TipoIntervaloMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Nome).Length(TipoIntervalo.MAX_NOME_LENGTH);
        }
    }
}
