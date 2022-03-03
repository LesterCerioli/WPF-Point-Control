using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.usuario
{
    public interface IUsuarioRepositorio
    {
        Usuario findById(int id);
        Usuario findByLogin(string login);
        bool loginExiste(string login);
        int getCount();
        uint save(Usuario usuario);

        List<Funcionario> findFuncionarios();
    }
}
