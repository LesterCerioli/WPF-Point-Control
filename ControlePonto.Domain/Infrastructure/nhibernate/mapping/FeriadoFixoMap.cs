using ControlePonto.Domain.feriado;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class FeriadoFixoMap : SubclassMap<FeriadoFixo>
    {
        public FeriadoFixoMap()
        {
            Map(x => x.Dia).Not.Nullable();            
        }
    }
}
