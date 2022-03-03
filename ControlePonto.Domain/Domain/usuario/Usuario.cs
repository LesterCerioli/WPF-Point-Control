using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class Usuario : Entity<uint>
    {
        #region Propriedades

        #region Limite dos campos

        public const int MAX_NOME_LENGTH = 50;
        public const int MAX_LOGIN_LENGTH = 20;
        public const int MAX_SENHA_LENGTH = 20;

        public const int MIN_NOME_LENGTH = 4;
        public const int MIN_LOGIN_LENGTH = 4;
        public const int MIN_SENHA_LENGTH = 6;

        #endregion

        public virtual string Login { get; set; }

        public virtual string Nome { get; set; }
        
        public virtual string Senha { get; set; }

        #endregion

        protected Usuario() { }

        public Usuario(string nome, string login, string senha)
        {
            checkPreConstructor();

            this.Nome = nome;
            this.Login = login;
            this.Senha = senha;
        }
    }
}
