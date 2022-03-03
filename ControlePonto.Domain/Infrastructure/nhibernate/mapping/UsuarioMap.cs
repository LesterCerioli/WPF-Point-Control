using FluentNHibernate.Mapping;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Nome).Length(Usuario.MAX_NOME_LENGTH);
            Map(x => x.Login).Length(Usuario.MAX_NOME_LENGTH);
            Map(x => x.Senha).Length(Usuario.MAX_SENHA_LENGTH);
        }
    }
}
