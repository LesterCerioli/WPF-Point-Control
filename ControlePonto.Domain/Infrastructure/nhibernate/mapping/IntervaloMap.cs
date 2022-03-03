using ControlePonto.Domain.intervalo;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class IntervaloMap : ClassMap<Intervalo>
    {
        public IntervaloMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Entrada).CustomType(typeof(TimeAsTimeSpanTypeClone)).Not.Nullable();
            Map(x => x.Saida).CustomType(typeof(TimeAsTimeSpanTypeClone));

            References(x => x.TipoIntervalo);                
        }
    }
}
