using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class TipoIntervaloJaExisteException : Exception
    {
        public string NomeJaExistente { get; private set; }

        public TipoIntervaloJaExisteException(string nome) :
            base(string.Format("Já existe um tipo de intervalo chamado \"{0}\"", nome))
        {
            this.NomeJaExistente = nome;
        }
    }
}
