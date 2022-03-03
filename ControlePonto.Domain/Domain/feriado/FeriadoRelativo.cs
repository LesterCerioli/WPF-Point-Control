using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public class FeriadoRelativo : Feriado
    {
        public virtual DayOfWeek DiaSemana { get; protected set; }
        public virtual int Sequencia { get; protected set; }

        private DateTime cache;

        protected FeriadoRelativo() { }

        public FeriadoRelativo(string nome, int seq, DayOfWeek diaSemana, int mes)
        {
            base.checkPreConstructor();

            base.Nome = nome;
            base.Mes = mes;
            this.DiaSemana = diaSemana;
            this.Sequencia = seq;

            this.cache = calcular();
            Check.Ensure(cache.Month == mes);
        }

        private DateTime calcular()
        {
            int ano = DateTime.Today.Year;
            var primeiroDia = new DateTime(ano, Mes, 1);
            int diaRef = (int)primeiroDia.DayOfWeek;

            diaRef = (int)DiaSemana - diaRef;
            if (diaRef < 1)
                diaRef += 7;

            if (Sequencia > 1)
                diaRef += (7 * (Sequencia - 1));

            return primeiroDia.AddDays(diaRef);
        }

        public override DateTime getData()
        {
            return cache;
        }
    }
}
