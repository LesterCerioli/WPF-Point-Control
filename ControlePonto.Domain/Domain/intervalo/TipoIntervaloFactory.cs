using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class TipoIntervaloFactory
    {
        private NomeIntervaloJaExisteSpecification intervaloJaExiste;

        public TipoIntervaloFactory(NomeIntervaloJaExisteSpecification intervaloJaExiste)
        {
            this.intervaloJaExiste = intervaloJaExiste;
        }

        public TipoIntervalo criarTipoIntervalo(string nome)
        {
            if (intervaloJaExiste.IsSatisfiedBy(nome))
                throw new TipoIntervaloJaExisteException(nome);
            return new TipoIntervalo(nome);
        }
    }
}
