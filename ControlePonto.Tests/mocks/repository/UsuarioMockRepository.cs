using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class UsuarioMockRepositorio : IUsuarioRepositorio
    {
        private List<Usuario> usuarioList;

        public UsuarioMockRepositorio()
        {
            usuarioList = new List<Usuario>();
        }

        public Usuario findById(int id)
        {
            try
            {
                return usuarioList.Single(x => x.Id == id);
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public Usuario findByLogin(string login)
        {
            try
            {
                return usuarioList.Single(x => x.Login == login);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public int getCount()
        {
            return usuarioList.Count();
        }

        public uint save(Usuario usuario)
        {
            usuarioList.Add(usuario);
            return 1;
        }


        public bool loginExiste(string login)
        {
            return usuarioList.Any(x => x.Login == login);
        }


        public List<Domain.usuario.funcionario.Funcionario> findFuncionarios()
        {
            throw new NotImplementedException();
        }
    }
}
