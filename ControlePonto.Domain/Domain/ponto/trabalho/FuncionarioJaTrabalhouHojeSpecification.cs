using ControlePonto.Domain.framework.specification;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class FuncionarioJaTrabalhouHojeSpecification : CompositeSpecification<Funcionario>
    {
        private IPontoDiaRepository pontoRepository;
        public DateTime Data { get; set; }

        public FuncionarioJaTrabalhouHojeSpecification(IPontoDiaRepository repository)
        {
            this.pontoRepository = repository;
            this.Data = DateTime.Today;
        }

        public override bool IsSatisfiedBy(Funcionario candidato)
        {
            return pontoRepository.existePontoDia(candidato, Data);
        }
    }
}
