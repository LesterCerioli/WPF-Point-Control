using ControlePonto.Domain.jornada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks.repository
{
    public class JornadaTrabalhoMockRepository : IJornadaTrabalhoRepository
    {
        private JornadaTrabalho jornada;

        public JornadaTrabalho findJornadaAtiva()
        {
            return jornada;
        }

        public bool existeAlgumaJornada()
        {
            return jornada != null;
        }

        public uint save(JornadaTrabalho jornada)
        {
            this.jornada = jornada;
            return 1;
        }
    }
}
