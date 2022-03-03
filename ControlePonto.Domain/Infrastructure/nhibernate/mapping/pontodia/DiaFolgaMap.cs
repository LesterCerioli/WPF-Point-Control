using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.folga;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class DiaFolgaMap : SubclassMap<DiaFolga>
    {
        public DiaFolgaMap()
        {
            Map(x => x.Descricao)
                .Length(DiaFolga.MAX_DESCRICAO_LENGTH)
                .Not.Nullable();
        }
    }
}
