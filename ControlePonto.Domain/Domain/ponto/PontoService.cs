using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoService
    {
        protected PontoFactory pontoFactory;
        protected SessaoLogin sessaoLogin;
        protected IDataHoraStrategy dataHoraStrategy;
        protected IPontoDiaRepository pontoRepository;
        protected ITipoIntervaloRepository tipoIntervaloRepository;
        protected FuncionarioPossuiPontoAbertoSpecification deixouPontoAberto;
        protected FuncionarioJaTrabalhouHojeSpecification jaTrabalhouHoje;

        public PontoService(PontoFactory pontoFactory, IDataHoraStrategy dataHoraStrategy, FuncionarioPossuiPontoAbertoSpecification pontoAbertoSpec,  FuncionarioJaTrabalhouHojeSpecification funcTrabSpec, SessaoLogin sessaoLogin, IPontoDiaRepository pontoRepository, ITipoIntervaloRepository tipoIntervaloRepository)
        {
            this.pontoFactory = pontoFactory;
            this.dataHoraStrategy = dataHoraStrategy;
            this.deixouPontoAberto = pontoAbertoSpec;
            this.jaTrabalhouHoje = funcTrabSpec;
            this.jaTrabalhouHoje.Data = dataHoraStrategy.getDataHoraAtual();
            this.sessaoLogin = sessaoLogin;
            this.pontoRepository = pontoRepository;
            this.tipoIntervaloRepository = tipoIntervaloRepository;
        }

        public DiaTrabalho iniciarDia()
        {            
            if (deixouPontoAberto.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new DiaEmAbertoException(deixouPontoAberto.PontoDiaAbertoEncontrado);

            if (jaTrabalhouHoje.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new PontoDiaJaExisteException(jaTrabalhouHoje.Data);

            var ponto = pontoFactory.criarDiaTrabalho(dataHoraStrategy, sessaoLogin);
            pontoRepository.save(ponto);
            return ponto;
        }

        public void encerrarDia(DiaTrabalho ponto)
        {
            if (ponto.algumIntervaloEmAberto())
                throw new IntervaloEmAbertoException(ponto.getIntervaloEmAberto());            

            ponto.Fim = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
            pontoRepository.save(ponto);
        }

        public DiaTrabalho recuperarPontoAbertoFuncionario(Funcionario funcionario)
        {
            return pontoRepository.findPontoAberto(funcionario, dataHoraStrategy.getDataHoraAtual());
        }

        public void registrarIntervalo(TipoIntervalo tipoIntervalo, DiaTrabalho ponto)
        {
            ponto.registrarIntervalo(tipoIntervalo, dataHoraStrategy);
            pontoRepository.save(ponto);
        }

        public DiaFolga darFolgaPara(Funcionario funcionario, DateTime data, string descricao)
        {
            if (data.Date < DateTime.Today)
                throw new FolgaDiaInvalidoException(data);

            var folga = pontoFactory.criarDiaFolga(funcionario, data, descricao);
            pontoRepository.save(folga);

            return folga;
        }

        /// <summary>
        /// Se o administrador, por alguma razão, desejar criar um ponto para o funcionário, este método será usado.
        /// Somente o login do administrador pode ocasionar a invocação deste método.
        /// </summary>
        /// <param name="funcionario">Funcionário o qual terá um dia de trabalho criado</param>
        /// <param name="date">Data do ponto que será adicionado</param>
        /// <returns></returns>
        public DiaTrabalho criarPontoParaFuncionarioEm(Funcionario funcionario, DateTime date)
        {
            Check.Require(!(sessaoLogin.UsuarioLogado is Funcionario));

            var ponto = pontoFactory.criarDiaTrabalhoEmDiaEspecifico(funcionario, date);
            ponto.Fim = new TimeSpan(0, 0, 0);

            foreach (var tipoIntervalo in tipoIntervaloRepository.findAll())
            {
                var novoIntervalo = new Intervalo(tipoIntervalo, new TimeSpan(0, 0, 0));
                ponto.Intervalos.Add(novoIntervalo);
                novoIntervalo.Saida = new TimeSpan(0, 0, 0);
            }

            pontoRepository.save(ponto);

            return ponto;
        }
    }
}
