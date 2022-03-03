using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public interface IDataHoraStrategy
    {
        DateTime getDataHoraAtual();
    }
}
