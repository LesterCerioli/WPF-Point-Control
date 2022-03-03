using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.usuario.funcionario;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public interface IPontoDiaRepository
    {
        ulong save(PontoDia ponto);

        DiaTrabalho findPontoTrabalho(Funcionario funcionario, DateTime date); 

        List<DiaTrabalho> findPontosAbertos(Funcionario funcionario);

        DiaTrabalho findPontoAberto(Funcionario funcionario, DateTime date);

        bool existePontoDia(Funcionario funcionario, DateTime date);

        List<PontoDia> findPontosNoIntervalo(Funcionario funcionario, DateTime inicio, DateTime fim, bool lazyLoadTrabalho = true, bool lazyLoadFolga = true);
    }
}
