using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.Domain.services.relatorio
{
    public abstract class DiaRelatorio
    {
        public DateTime Data { get; private set; }

        public abstract ETipoDiaRelatorio TipoDia { get; }

        public DiaRelatorio(DateTime data)
        {
            this.Data = data;
        }
    }
}
