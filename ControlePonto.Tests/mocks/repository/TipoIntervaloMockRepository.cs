using ControlePonto.Domain.intervalo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks.repository
{
    public class TipoIntervaloMockRepository : ITipoIntervaloRepository
    {
        private List<TipoIntervalo> listRep = new List<TipoIntervalo>();

        public List<TipoIntervalo> findAll()
        {
            return
                listRep;
        }

        public TipoIntervalo findByName(string nome)
        {
            return
                listRep.SingleOrDefault(x => x.Nome.ToUpper() == nome.ToUpper());
        }


        public uint save(TipoIntervalo tipoIntervalo)
        {
            listRep.Add(tipoIntervalo);
            return (uint)listRep.Count;
        }
    }
}
