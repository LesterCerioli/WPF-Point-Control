using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Infrastructure.nhibernate;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class DiaTrabalhoMap : SubclassMap<DiaTrabalho>
    {
        public DiaTrabalhoMap()
        {
            Map(x => x.Inicio)
                .CustomType(typeof(TimeAsTimeSpanTypeClone))
                .Not.Nullable();
            Map(x => x.Fim)
                .CustomType(typeof(TimeAsTimeSpanTypeClone));            

            HasMany(x => x.Intervalos)
                .Cascade.All();
        }
    }
}
