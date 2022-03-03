using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.framework.specification
{
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> Wrapped;

        public NotSpecification (ISpecification<T> x) {
            Wrapped = x;
        }

        public override bool IsSatisfiedBy(T candidato)
        {
            return !Wrapped.IsSatisfiedBy(candidato);
        }
    }
}
