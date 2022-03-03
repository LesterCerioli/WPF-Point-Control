using ControlePonto.Domain.framework.specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class NomeIntervaloJaExisteSpecification : CompositeSpecification<string>
    {
        private ITipoIntervaloRepository repository;
        public NomeIntervaloJaExisteSpecification(ITipoIntervaloRepository repository)
        {
            this.repository = repository;
        }

        public override bool IsSatisfiedBy(string candidato)
        {
            return repository.findByName(candidato) != null;
        }
    }
}
