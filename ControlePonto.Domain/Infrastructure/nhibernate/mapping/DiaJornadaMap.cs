using ControlePonto.Domain.jornada;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class DiaJornadaMap : ClassMap<DiaJornada>
    {
        public DiaJornadaMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.DiaSemana)
                .CustomType<int>()
                .Not.Nullable();

            Map(x => x.EntradaEsperada)
                .CustomType(typeof(TimeAsTimeSpanTypeClone))
                .Not.Nullable();

            Map(x => x.SaidaEsperada)
                .CustomType(typeof(TimeAsTimeSpanTypeClone))
                .Not.Nullable();

            Map(x => x.FolgaEsperada)
                .CustomType(typeof(TimeAsTimeSpanTypeClone))
                .Not.Nullable();
        }
    }
}
