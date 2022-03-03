using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.feriado
{
    public class FeriadoService
    {
        private IFeriadoRepository repository;
        private Dictionary<int, List<Feriado>> cache; //Mês x Feriados

        public FeriadoService(IFeriadoRepository repository)
        {
            this.repository = repository;
            cache = new Dictionary<int, List<Feriado>>();
        }

        public bool isFeriado(DateTime date)
        {
            iniciarCache(date);
            return cache[date.Month].Any(x => date.Date.Equals(x.getData()));
        }

        public Feriado getFeriado(DateTime date)
        {
            iniciarCache(date);
            return cache[date.Month].First(x => x.getData().Equals(date.Date));
        }

        private void iniciarCache(DateTime date)
        {
            if (!cache.ContainsKey(date.Month))
                cache.Add(date.Month, repository.findFromMonth(date.Month));
        }
    }
}
