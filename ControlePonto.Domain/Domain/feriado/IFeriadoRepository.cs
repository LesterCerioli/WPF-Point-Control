using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public interface IFeriadoRepository
    {
        uint save(Feriado feriado);

        List<Feriado> findFromMonth(int mes);
    }
}
