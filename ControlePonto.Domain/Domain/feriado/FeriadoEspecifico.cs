using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public class FeriadoEspecifico : Feriado
    {
        public virtual int Dia { get; protected set; }
        public virtual int Ano { get; protected set; }

        protected FeriadoEspecifico() { }

        public FeriadoEspecifico(string nome, int dia, int mes, int ano)
        {
            base.checkPreConstructor();

            base.Nome = nome;
            this.Dia = dia;
            base.Mes = mes;
            this.Ano = ano;
        }

        public override DateTime getData()
        {
            return new DateTime(Ano, Mes, Dia);
        }
    }
}
