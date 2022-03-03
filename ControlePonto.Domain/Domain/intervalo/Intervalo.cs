using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public class Intervalo : Entity<ulong>
    {
        public virtual TimeSpan Entrada { get; set; }
        public virtual TimeSpan? Saida { get; set; }
        public virtual bool isAberto
        {
            get
            {
                return !Saida.HasValue;
            }
        }
        public virtual TipoIntervalo TipoIntervalo { get; protected set; }

        protected Intervalo() { }

        public Intervalo(TipoIntervalo tipo, TimeSpan entrada)
        {
            TipoIntervalo = tipo;
            Entrada = entrada;
            Saida = null;
        }
    }
}
