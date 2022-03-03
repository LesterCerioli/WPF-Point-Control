using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlePonto.Domain.framework;
using ControlePonto.Domain.feriado;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.jornada;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.services.persistence;

namespace ControlePonto.Domain.services.relatorio
{
    public class RelatorioService
    {
        private IPontoDiaRepository pontoRepository;
        private IJornadaTrabalhoRepository jornadaRepository;
        private FeriadoService feriadoService;
        private JornadaTrabalho jornadaAtiva;
        private IUnitOfWork unitOfWork;

        public RelatorioService(IPontoDiaRepository pontoRepository, FeriadoService feriadoService, IJornadaTrabalhoRepository jornadaRepository, IUnitOfWork unitOfWork)
        {
            this.pontoRepository = pontoRepository;
            this.feriadoService = feriadoService;
            this.jornadaRepository = jornadaRepository;
            this.jornadaAtiva = jornadaRepository.findJornadaAtiva();
            this.unitOfWork = unitOfWork;            
        }

        public RelatorioPonto gerarRelatorio(Funcionario funcionario, DateTime inicio, DateTime fim)
        {
            Check.Require(fim >= inicio, "Período inválido. O início deve vir antes do fim!");
            Check.Require(funcionario != null, "O funcionário deve ser válido");

            var todosPontos = pontoRepository.findPontosNoIntervalo(funcionario, inicio, fim, false, false)
                .GroupBy(x => x.Data)
                .Select(group => group.First());
            var diasFaltando = inicio.Range(fim).Except(todosPontos.Select(x => x.Data));
            var feriadosNaoTrabalhados = diasFaltando
                .Where(x => feriadoService.isFeriado(x));

            //Se é um feriado, não deve ser contado como falta
            diasFaltando = diasFaltando.Except(feriadosNaoTrabalhados);

            var todosDias = todosPontos
                .Select(x => criarDia(x))
                .Concat(
                    diasFaltando.Select(x => criarDia(x))
                )
                .Concat(feriadosNaoTrabalhados
                    .Select(x => 
                        criarDia(feriadoService.getFeriado(x))
                    )
                )
                .OrderBy(x => x.Data)
                .ToList();                        

            return new RelatorioPonto(funcionario, inicio, fim, jornadaAtiva, todosDias);
        }

        private DiaRelatorio criarDia(DateTime date)
        {
            return new DiaFalta(date, jornadaAtiva);
        }

        private DiaRelatorio criarDia(PontoDia ponto)
        {
            if (ponto is DiaTrabalhoFeriado)
                return new DiaFeriadoTrabalhado(ponto, (ponto as DiaTrabalhoFeriado).Feriado, jornadaAtiva);
            return new DiaPonto(ponto, jornadaAtiva);
        }

        private DiaRelatorio criarDia(Feriado feriado)
        {
            return new DiaFeriado(feriado);
        }
    }
}
