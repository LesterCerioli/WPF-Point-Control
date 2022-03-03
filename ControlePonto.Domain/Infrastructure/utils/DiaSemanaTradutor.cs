using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.utils
{
    public class DiaSemanaTradutor
    {
        public static string traduzir(DayOfWeek day)
        {
            var culture = new System.Globalization.CultureInfo("pt-BR");
            return culture.DateTimeFormat.GetDayName(day).ToUpper();
        }
    }
}
