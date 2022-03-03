using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public interface IJornadaTrabalhoRepository
    {        
        JornadaTrabalho findJornadaAtiva();
        bool existeAlgumaJornada();
        uint save(JornadaTrabalho jornada);
    }
}
