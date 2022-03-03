using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ControlePonto.WPF.framework
{
    public class WeekDayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DiaSemanaTradutor.traduzir((DayOfWeek)value);            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return
                Enum.GetValues(typeof(DayOfWeek))
                .OfType<DayOfWeek>()
                .Single(x => DiaSemanaTradutor.traduzir(x).Equals(value));
        }
    }
}
