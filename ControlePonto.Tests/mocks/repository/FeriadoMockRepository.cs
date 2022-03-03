using ControlePonto.Domain.feriado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks.repository
{
    public class FeriadoMockRepository : IFeriadoRepository
    {
        private List<Feriado> listRep = new List<Feriado>();

        public uint save(Feriado feriado)
        {
            if (listRep.Contains(feriado))
                return (uint)listRep.IndexOf(feriado);
                
            listRep.Add(feriado);
            return (uint)listRep.Count;
        }

        public List<Feriado> findFromMonth(int mes)
        {
            return listRep
                .Where(x => x.Mes == mes)
                .ToList();
        }
    }
}
