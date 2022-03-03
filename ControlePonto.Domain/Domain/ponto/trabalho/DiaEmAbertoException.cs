using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class DiaEmAbertoException : Exception
    {
        public PontoDia PontoDiaAberto { get; private set; }

        public DiaEmAbertoException(PontoDia ponto) :
            base(string.Format("O ponto do dia {0} não foi encerrado", ponto.Data.ToString("dd/MM/yyyy")))
        {
            PontoDiaAberto = ponto;
        }
    }
}
