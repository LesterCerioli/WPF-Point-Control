using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.framework.specification
{
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private ISpecification<T> One;
        private ISpecification<T> Other;

        public OrSpecification(ISpecification<T> x, ISpecification<T> y)
        {
            One = x;
            Other = y;
        }

        public override bool IsSatisfiedBy(T candidato)
        {
            return One.IsSatisfiedBy(candidato) || Other.IsSatisfiedBy(candidato);
        }
    }
}
