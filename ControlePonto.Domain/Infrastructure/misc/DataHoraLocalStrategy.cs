using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.misc
{
    public class DataHoraLocalStrategy : IDataHoraStrategy
    {
        public DateTime getDataHoraAtual()
        {
            return DateTime.Now;
        }
    }
}
