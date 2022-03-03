using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.intervalo
{
    public interface ITipoIntervaloRepository
    {
        List<TipoIntervalo> findAll();
        TipoIntervalo findByName(string nome);
        uint save(TipoIntervalo tipoIntervalo);
    }
}
