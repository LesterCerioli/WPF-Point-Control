using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.usuario
{
    public class LoginViewModel : ViewModelBase
    {
        private ILoginService loginService;
        private IUsuarioRepositorio usuarioRepositorio;

        private ICommand _logarCommand;        

        public LoginViewModel(ILoginService loginService, IUsuarioRepositorio usuarioRepositorio)
        {
            this.loginService = loginService;
            this.usuarioRepositorio = usuarioRepositorio;

            _logarCommand = new RelayParameterCommand<System.Windows.Controls.PasswordBox>(logar, base.isModelValid);
        }

        #region Propriedades

        private string _login;
        public string Login
        {
            get { return _login; }
            set { SetField(ref _login, value); }
        }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetField(ref _dialogResult, value); }
        }

        public ICommand LogarCommand { get { return _logarCommand; } }
        
        #endregion       

        private void logar(System.Windows.Controls.PasswordBox pbox)
        {
            try
            {
                string senha = pbox.Password;
                Usuario usuario = loginService.Logar(Login, senha);
                DialogResult = true;
            }
            catch (LoginInvalidoException)
            {
                showMessageBox("Não foi possível efetuar login. Verifique seu usuário/senha.", "Login inválido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        protected override string validar(string propertyName)
        {
            switch (propertyName)
            {
                case "Login":
                    if (string.IsNullOrWhiteSpace(Login))
                        return "O login é obrigatório";
                    break;
            }
            return null;
        }

        private bool alguemJaCadastrou()
        {
            return (usuarioRepositorio.getCount() > 0);            
        }

        private bool precisaCriarSenha(Usuario usuario)
        {
            return (string.IsNullOrEmpty(usuario.Senha));
        }
    }
}
