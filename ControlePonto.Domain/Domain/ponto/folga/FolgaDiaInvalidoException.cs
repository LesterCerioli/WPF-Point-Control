using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.folga
{
    public class FolgaDiaInvalidoException : Exception
    {
        public DateTime DataInvalida { get; private set; }

        public FolgaDiaInvalidoException(DateTime data) : 
            base(string.Format("Não é possível dar a folga, pois a data pretendida foi há {0} dias atrás", -data.Subtract(DateTime.Today).Days))
        {
            this.DataInvalida = data;
        }
    }
}
