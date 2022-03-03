using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.relatorio
{
    public enum ETipoDiaRelatorio : int
    {
        TRABALHO,
        FERIADO,
        FERIADO_TRABALHADO,
        SEM_TRABALHO,
        FOLGA,
        FALTOU
    }
}
