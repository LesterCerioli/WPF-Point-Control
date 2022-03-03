using ControlePonto.Domain.usuario.funcionario;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate.mapping
{
    public class FuncionarioMap : SubclassMap<Funcionario>
    {
        public FuncionarioMap()
        {
            Map(x => x.CPF);
            Map(x => x.RG);
        }
    }
}
