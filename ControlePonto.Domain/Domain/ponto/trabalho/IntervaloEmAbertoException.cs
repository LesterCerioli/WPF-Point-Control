using ControlePonto.Domain.intervalo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class IntervaloEmAbertoException : Exception
    {
        public Intervalo IntervaloAberto { get; private set; }

        public IntervaloEmAbertoException(Intervalo intervalo) : 
            base(string.Format("O ponto de {0} foi iniciado às {1}, mas não foi encerrado", intervalo.TipoIntervalo.Nome, intervalo.Entrada))
        {
            this.IntervaloAberto = intervalo;            
        }
    }
}
