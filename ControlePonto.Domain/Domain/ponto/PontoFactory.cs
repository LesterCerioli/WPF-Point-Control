using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.feriado;

namespace ControlePonto.Domain.ponto
{
    public class PontoFactory
    {
        private IPontoDiaRepository repository;
        private FeriadoService feriadoService;

        public PontoFactory(IPontoDiaRepository repository, FeriadoService feriadoService)
        {
            this.repository = repository;
            this.feriadoService = feriadoService;
        }

        public DiaTrabalho criarDiaTrabalho(IDataHoraStrategy dataHoraStrategy, SessaoLogin sessaoLogin)
        {
            DateTime dt = dataHoraStrategy.getDataHoraAtual();
            if (repository.existePontoDia(sessaoLogin.UsuarioLogado as Funcionario, dt))
                throw new PontoDiaJaExisteException(dt);

            if (feriadoService.isFeriado(dt))
                return new DiaTrabalhoFeriado(feriadoService.getFeriado(dt), dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);

            return new DiaTrabalhoComum(dt.Date, dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);
        }

        /// <summary>
        /// Método para criar ponto para o funcionário em um dia específico. 
        /// Pode ser usado somente pelo administrador.
        /// </summary>
        /// <param name="funcionario">Funcionário o qual vai receber um dia de trabalho</param>
        /// <param name="date">Data que vai ser criado o dia de trabalho</param>
        /// <returns></returns>
        public DiaTrabalho criarDiaTrabalhoEmDiaEspecifico(Funcionario funcionario, DateTime date)
        {
            if (repository.existePontoDia(funcionario, date))
                throw new PontoDiaJaExisteException(date);

            if (feriadoService.isFeriado(date))
                return new DiaTrabalhoFeriado(feriadoService.getFeriado(date), date.TimeOfDay, funcionario);

            return new DiaTrabalhoComum(date.Date, new TimeSpan(0, 0, 0), funcionario);
        }

        public DiaFolga criarDiaFolga(Funcionario funcionario, DateTime data, string descricao)
        {
            Check.Require(!string.IsNullOrWhiteSpace(descricao), "A descrição deve ser válida");

            if (repository.existePontoDia(funcionario, data))
                throw new PontoDiaJaExisteException(data);
                        
            return new DiaFolga(funcionario, data, descricao);
        }
    }
}
