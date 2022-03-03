using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public class FeriadoFactory
    {
        public FeriadoRelativo criarFeriadoRelativo(string nome, int seq, DayOfWeek diaSemana, int mes)
        {
            return new FeriadoRelativo(nome, seq, diaSemana, mes);
        }

        public FeriadoFixo criarFeriadoFixo(string nome, int dia, int mes)
        {
            return new FeriadoFixo(nome, dia, mes);
        }

        public FeriadoEspecifico criarFeriadoEspecifico(string nome, int dia, int mes, int ano)
        {
            return new FeriadoEspecifico(nome, dia, mes, ano);
        }
    }
}
