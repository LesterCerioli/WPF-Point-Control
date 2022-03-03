using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlePonto.WPF.window.usuario
{
    public partial class LoginWindow : WindowBase
    {
        public LoginWindow(LoginViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
