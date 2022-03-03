using ControlePonto.Domain.jornada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public interface ICalculoHoraDevedora
    {
        TimeSpan calcularHorasDevedoras();   
    }
}
