using ControlePonto.Domain.framework;
using ControlePonto.Domain.framework.specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class SenhaValidaSpecification : CompositeSpecification<string>
    {
        public override bool IsSatisfiedBy(string senha)
        {
            //Se encontrar algo diferente de letra ou número, retorna falso
            for (int i = 0; i < senha.Length; i++)
            {
                if (!char.IsLetterOrDigit(senha[i]))
                    return false;
            }
            return true;
        }
    }
}
