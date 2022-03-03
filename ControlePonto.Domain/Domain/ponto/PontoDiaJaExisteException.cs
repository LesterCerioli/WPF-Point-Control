using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoDiaJaExisteException : Exception
    {
        public DateTime DataPonto { get; private set; }

        public PontoDiaJaExisteException(DateTime data) : base(string.Format("Ponto do dia {0:dd'/'MM'/'yyyy} já existe", data))
        {
            this.DataPonto = data;
        }
    }
}
