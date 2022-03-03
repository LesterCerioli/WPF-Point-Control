using ControlePonto.Domain.jornada;
using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public interface IDiaComPonto : ICalculoHoraExtra, ICalculoHoraDevedora
    {
        PontoDia PontoDia { get; }

        JornadaTrabalho JornadaTrabalhoAtiva { get; }

        TimeSpan calcularHorasTrabalhadas();
    }
}
