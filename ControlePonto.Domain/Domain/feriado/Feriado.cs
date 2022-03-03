using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public abstract class Feriado : Entity<uint>
    {
        public const int MAX_NOME_LENGHT = 45;

        public virtual string Nome { get; protected set; }
        public virtual int Mes { get; protected set; }

        public abstract DateTime getData();
    }
}
