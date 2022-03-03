using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario.funcionario
{
    public class FuncionarioFactory
    {
        public Funcionario criarFuncionario(string nome, string login, string senha, string rg, string cpf)
        {
            if (!Validacao.ValidaCPF(cpf))
                throw new CPFInvalidoException(cpf);

            return new Funcionario(nome, login, senha, rg, cpf);
        }
    }
}
