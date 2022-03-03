using ControlePonto.Domain.framework;
using ControlePonto.Domain.usuario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class UsuarioFactory
    {
        public LoginJaExisteSpecification LoginEmUsoSpecification { get; set; }
        public LoginValidoSpecification LoginValidoSpecification { get; set; }
        public SenhaValidaSpecification SenhaValidaSpecification { get; set; }        
        
        public UsuarioFactory(LoginJaExisteSpecification loginJaExiste, LoginValidoSpecification loginValido, SenhaValidaSpecification senhaValida)
        {
            this.LoginEmUsoSpecification = loginJaExiste;
            this.LoginValidoSpecification = loginValido;
            this.SenhaValidaSpecification = senhaValida;
        }

        public Usuario criarUsuario(string nome, string login, string senha)
        {
            Check.Require(nome.Length >= Usuario.MIN_LOGIN_LENGTH,
                string.Format("O nome deve ter no mínimo {0} caracteres", Usuario.MIN_NOME_LENGTH));
            Check.Require(login.Length >= Usuario.MIN_LOGIN_LENGTH,
                string.Format("O usuário deve ter no mínimo {0} caracteres", Usuario.MIN_LOGIN_LENGTH));
            Check.Require(senha.Length >= Usuario.MIN_SENHA_LENGTH,
                string.Format("A senha de usuário deve ter no mínimo {0} caracteres", Usuario.MIN_SENHA_LENGTH));

            Check.Require(nome.Length <= Usuario.MAX_NOME_LENGTH,
                    string.Format("O usuário não deve exceder {0} caracteres", Usuario.MAX_NOME_LENGTH));
            Check.Require(login.Length <= Usuario.MAX_LOGIN_LENGTH,
                    string.Format("O nome de usuário não pode exceder {0} caracteres", Usuario.MAX_LOGIN_LENGTH));
            Check.Require(senha.Length <= Usuario.MAX_SENHA_LENGTH,
                    string.Format("A senha do usuário não deve exceder {0} caracteres", Usuario.MAX_SENHA_LENGTH));

            Check.Require(login.ToLower() == login, "O login deve ser totalmente em minúsculo");
            Check.Require(LoginValidoSpecification.IsSatisfiedBy(login), "O login deve conter somente letras, números e \"_\"");
            Check.Require(SenhaValidaSpecification.IsSatisfiedBy(senha), "A senha deve conter somente letras e números");
            
            if (LoginEmUsoSpecification.IsSatisfiedBy(login))
                throw new LoginJaExisteException(login);

            return new Usuario(nome, login, senha);
        }
    }
}
