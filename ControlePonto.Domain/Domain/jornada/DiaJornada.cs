using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class DiaJornada : Entity<uint>
    {
        public virtual DayOfWeek DiaSemana { get; protected set; }
        public virtual TimeSpan EntradaEsperada { get; set; }
        public virtual TimeSpan SaidaEsperada { get; set; }
        public virtual TimeSpan FolgaEsperada { get; set; }

        protected internal DiaJornada(DayOfWeek diaSemana, TimeSpan entradaEsperada, TimeSpan saidaEsperada, TimeSpan horasFolga)
        {
            this.DiaSemana = diaSemana;
            this.EntradaEsperada = entradaEsperada;
            this.SaidaEsperada = saidaEsperada;
            this.FolgaEsperada = horasFolga;            
        }

        protected internal DiaJornada(DayOfWeek diaSemana)
        {
            this.DiaSemana = diaSemana;
        }

        protected DiaJornada() { }

        public virtual TimeSpan calcularHorasTrabalhoEsperado()
        {
            return SaidaEsperada.Subtract(EntradaEsperada).Subtract(FolgaEsperada);
        }

        public virtual DiaJornadaReadOnly asReadOnly()
        {
            return new DiaJornadaReadOnly(this);
        }
    }
}
