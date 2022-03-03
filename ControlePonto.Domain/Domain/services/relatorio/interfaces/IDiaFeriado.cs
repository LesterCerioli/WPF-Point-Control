using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public interface IDiaFeriado
    {
        DateTime Data { get; }

        string Nome { get; }
    }
}
