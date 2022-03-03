using ControlePonto.Domain.framework.specification;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto.trabalho
{
    public class FuncionarioPossuiPontoAbertoSpecification : CompositeSpecification<Funcionario>
    {
        private IPontoDiaRepository pontoRepository;
        public PontoDia PontoDiaAbertoEncontrado { get; private set; }

        public FuncionarioPossuiPontoAbertoSpecification(IPontoDiaRepository repository)
        {
            this.pontoRepository = repository;
        }

        public override bool IsSatisfiedBy(Funcionario candidato)
        {
            var abertos = pontoRepository.findPontosAbertos(candidato);            
            if (abertos.Count > 0)
            {
                PontoDiaAbertoEncontrado = abertos.First();
                return true;
            }
            return false;
        }
    }
}
