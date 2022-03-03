using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class JornadaTrabalhoFactory
    {
        private IJornadaTrabalhoRepository repository;

        public JornadaTrabalhoFactory(IJornadaTrabalhoRepository repository)
        {
            this.repository = repository;
        }

        public JornadaTrabalho criarJornadaTrabalho()
        {
            if (repository.existeAlgumaJornada())
                throw new JornadaTrabalhoJaExisteException();
            return new JornadaTrabalho();
        }
    }
}
