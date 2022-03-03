using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class PontoServiceMock : PontoService
    {
        public PontoServiceMock(PontoFactory pontoFactory, IDataHoraStrategy dataHoraStrategy, FuncionarioPossuiPontoAbertoSpecification pontoAbertoSpec, FuncionarioJaTrabalhouHojeSpecification funcTrabSpec, SessaoLogin sessaoLogin, IPontoDiaRepository pontoRepository, ITipoIntervaloRepository tipoIntervaloRepository) :
            base(pontoFactory, dataHoraStrategy, pontoAbertoSpec, funcTrabSpec, sessaoLogin, pontoRepository, tipoIntervaloRepository)
        {
        }

        public new DiaFolga darFolgaPara(Funcionario funcionario, DateTime data, string descricao)
        {
            var folga = base.pontoFactory.criarDiaFolga(funcionario, data, descricao);
            pontoRepository.save(folga);

            return folga;
        }
    }
}
