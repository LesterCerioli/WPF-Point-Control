using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public class FeriadoFixo : Feriado
    {
        public virtual int Dia { get; protected set; }

        protected FeriadoFixo() { }

        public FeriadoFixo(string nome, int dia, int mes)
        {
            base.checkPreConstructor();

            base.Nome = nome;
            base.Mes = mes;
            this.Dia = dia;
        }

        public override DateTime getData()
        {
            return new DateTime(DateTime.Today.Year, Mes, Dia);
        }
    }
}
