using ControlePonto.Domain.framework;
using ControlePonto.Domain.framework.specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public class LoginJaExisteSpecification : CompositeSpecification<string>
    {
        private IUsuarioRepositorio usuarioRepositorio;

        public override bool IsSatisfiedBy(string candidato)
        {
            return usuarioRepositorio.loginExiste(candidato);            
        }

        public LoginJaExisteSpecification(IUsuarioRepositorio repositorio)
        {
            usuarioRepositorio = repositorio;
        }
    }
}
