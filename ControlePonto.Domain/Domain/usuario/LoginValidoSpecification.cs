using ControlePonto.Domain.framework;
using ControlePonto.Domain.framework.specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class LoginValidoSpecification : CompositeSpecification<string>
    {
        public override bool IsSatisfiedBy(string login)
        {
            //Se encontrar algo diferente de letra, número ou "_"; retorna falso
            for (int i = 0; i < login.Length; i++)
            {
                if (!char.IsLetterOrDigit(login[i]) && login[i] != '_')
                    return false;
            }
            return true;
        }
    }
}
