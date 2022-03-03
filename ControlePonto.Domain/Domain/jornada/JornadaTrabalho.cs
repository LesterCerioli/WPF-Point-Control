using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.jornada
{
    public class JornadaTrabalho : Entity<uint>
    {
        public virtual ICollection<DiaJornada> Dias { get; protected set; }

        public static TimeSpan NAO_DEFINIDO { get { return new TimeSpan(0, 0, 0); } }

        public JornadaTrabalho() 
        {            
            this.checkPreConstructor();

            Dias = new List<DiaJornada>(7);
            for (DayOfWeek day = DayOfWeek.Sunday; day <= DayOfWeek.Saturday; day++)
                Dias.Add(new DiaJornada(day, NAO_DEFINIDO, NAO_DEFINIDO, NAO_DEFINIDO));
        }//

        protected override void checkPreConstructor()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var type = stackTrace.GetFrame(2).GetMethod().DeclaringType;

            if (type != null)
            {
                string invoker = type.Name;
                Check.Require(this.GetType().Name.Equals(invoker) ||
                    invoker.Contains("Factory") ||
                    invoker.Equals("RuntimeMethodHandle"),
                    "O construtor só deve ser invocado por uma factory ou para clonagem. Invocado por: " + invoker);
            }
        }

        public virtual void cadastrarDia(DayOfWeek weekDay, TimeSpan entradaEsperada, TimeSpan saidaEsperada, TimeSpan horasFolga)
        {
            Check.Require(saidaEsperada >= entradaEsperada,
                string.Format("A saída esperada deve ser após a entrada.\nFoi recebido:\nDia: {2}\nEntrada: {0}\nSaída: {1}",
                    entradaEsperada, saidaEsperada, DiaSemanaTradutor.traduzir(weekDay)));

            var dia = Dias.Single(x => x.DiaSemana == weekDay);
            dia.EntradaEsperada = entradaEsperada;
            dia.SaidaEsperada = saidaEsperada;
            dia.FolgaEsperada = horasFolga;
        }

        public virtual void cadastrarDia(DayOfWeek from, DayOfWeek until, TimeSpan entradaEsperada, TimeSpan saidaEsperada, TimeSpan horasFolga)
        {
            Check.Require(from < until, "O intervalo de dias da semana está incorreto");            

            for (DayOfWeek i = from; i <= until; i++)
                cadastrarDia(i, entradaEsperada, saidaEsperada, horasFolga);            
        }

        public virtual DiaJornada getDia(DayOfWeek week)
        {
            return Dias.Single(x => x.DiaSemana == week).asReadOnly();
        }
    }
}
