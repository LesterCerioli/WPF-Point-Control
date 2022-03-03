using ControlePonto.Domain.ponto.trabalho;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class DiaTrabalhoFeriadoMap : SubclassMap<DiaTrabalhoFeriado>
    {
        public DiaTrabalhoFeriadoMap()
        {
            References(x => x.Feriado)
                .Cascade.None();
        }
    }
}
