using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks.repository
{
    public class PontoDiaMockRepository : IPontoDiaRepository
    {
        private List<PontoDia> listRep = new List<PontoDia>();

        public ulong save(PontoDia ponto)
        {
            if (listRep.Contains(ponto))
                return (uint)listRep.IndexOf(ponto);

            listRep.Add(ponto);
            return (ulong)listRep.Count();
        }

        public List<DiaTrabalho> findPontosAbertos(Funcionario funcionario)
        {
            return
                listRep
                .Where(x => x is DiaTrabalho)
                .Select(x => x as DiaTrabalho)
                .Where(x => x.isAberto && x.Funcionario.Nome.Equals(funcionario.Nome)).ToList();
        }


        public bool existePontoDia(Funcionario funcionario, DateTime date)
        {
            return
                listRep.Any(x => x.Data == date.Date && x.Funcionario == funcionario);
        }

        public List<PontoDia> findPontosNoIntervalo(Funcionario funcionario, DateTime inicio, DateTime fim, bool lazyLoadTrabalho = true, bool lazyLoadFolga = true)
        {
            return
                listRep
                .Where(x => x.Funcionario.Nome.Equals(funcionario.Nome))
                .Where(x => x.Data >= inicio && x.Data <= fim)
                .ToList();
        }


        public DiaTrabalho findPontoAberto(Funcionario funcionario, DateTime date)
        {
            throw new NotImplementedException();
        }


        public DiaTrabalho findPontoTrabalho(Funcionario funcionario, DateTime date)
        {
            return (DiaTrabalho)
                listRep
                .Where(x => x.Funcionario.Nome.Equals(funcionario.Nome))
                .Single(x => x.Data == date);
        }
    }
}
