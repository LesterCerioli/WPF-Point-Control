using ControlePonto.Domain.feriado;
using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Tests.mocks;
using ControlePonto.Tests.mocks.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests
{
    public static class FactoryHelper
    {
        public static PontoService criarPontoService(SessaoLogin sessao, IDataHoraStrategy dataHoraStrategy = null, IPontoDiaRepository pontoRepository = null, bool mock = false, IFeriadoRepository feriadoRepository = null)
        {
            if (dataHoraStrategy == null)
                dataHoraStrategy = new DataHoraMockStrategy(DateTime.Today);            

            if (pontoRepository == null)
                pontoRepository = new PontoDiaMockRepository();

            var tipoIntervaloRepository = new TipoIntervaloMockRepository();
            tipoIntervaloRepository.save(new TipoIntervalo("Almoço"));

            if (mock)
            {
                return new PontoServiceMock(criarPontoFactory(pontoRepository, feriadoRepository),
                    dataHoraStrategy,
                    new FuncionarioPossuiPontoAbertoSpecification(pontoRepository),
                    new FuncionarioJaTrabalhouHojeSpecification(pontoRepository),
                    sessao,
                    pontoRepository,
                    tipoIntervaloRepository);
            }

            return new PontoService(criarPontoFactory(pontoRepository, feriadoRepository),
                dataHoraStrategy,
                new FuncionarioPossuiPontoAbertoSpecification(pontoRepository),
                new FuncionarioJaTrabalhouHojeSpecification(pontoRepository),
                sessao,
                pontoRepository,
                tipoIntervaloRepository);
        }

        public static PontoService criarPontoService(Funcionario logado, IDataHoraStrategy dataHoraStrategy = null, IPontoDiaRepository pontoRepository = null, bool mock = false, IFeriadoRepository feriadoRepository = null)
        {
            return criarPontoService(new SessaoLoginMock(logado), dataHoraStrategy, pontoRepository, mock, feriadoRepository);
        }

        public static PontoFactory criarPontoFactory(FeriadoService feriadoService, IPontoDiaRepository pontoRepository = null)
        {
            if (pontoRepository == null)
                pontoRepository = new PontoDiaMockRepository();

            return new PontoFactory(pontoRepository, feriadoService);
        }
        
        public static PontoFactory criarPontoFactory(IPontoDiaRepository pontoRepository = null, IFeriadoRepository feriadoRepository = null)
        {
            if (feriadoRepository == null)
                feriadoRepository = new FeriadoMockRepository();

            return criarPontoFactory(new FeriadoService(feriadoRepository), pontoRepository);
        }
    }
}
