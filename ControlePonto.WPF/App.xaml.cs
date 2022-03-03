using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.nhibernate;
using ControlePonto.WPF.window.administracao;
using ControlePonto.WPF.window.consulta;
using ControlePonto.WPF.window.usuario;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ControlePonto.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {  
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            tratarArgumentos(e.Args);

            bool loginAgain;

            do
            {
                loginAgain = false;
                var loginResult = UsuarioWindowFactory.criarLoginWindow().ShowDialog();
                if (loginResult.HasValue && loginResult.Value)
                {
                    loginAgain = true;
                    try
                    {
                        if (SessaoLogin.getSessao().UsuarioLogado is Funcionario)
                        {
                            var pontoService = PontoServiceFactory.criarPontoService();
                            var ponto = recuperarOuIniciarPonto(pontoService);
                            PontoWindowFactory.criarPontoWindow(ponto, pontoService).ShowDialog();
                        }
                        else
                        {
                            PainelControleWindowFactory.criarPainelControleWindow().ShowDialog();
                        }
                    }
                    catch (PontoDiaJaExisteException ex)
                    {
                        MessageBox.Show(string.Format("O ponto do dia {0} já foi encerrado",
                            ex.DataPonto.ToShortDateString()),
                            "Não é possível iniciar",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Não foi possível completar a operação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                SessaoLogin.getSessao().encerrar();
            } while (loginAgain);

            Current.Shutdown();
        }

        private DiaTrabalho recuperarOuIniciarPonto(PontoService pontoService)
        {
            var ponto = pontoService.recuperarPontoAbertoFuncionario(SessaoLogin.getSessao().UsuarioLogado as Funcionario);
            if (ponto == null)
                return pontoService.iniciarDia();
            return ponto;
        }

        private void tratarArgumentos(string[] args)
        {
            string host = (args.Length == 1) ? args[0] : "127.0.0.1";
            Task.Factory.StartNew(() => aplicarHost(host));
        }

        private void aplicarHost(string host)
        {
            try
            {
                ControlePonto.Infrastructure.nhibernate.NHibernateHelper.Host = host;
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}\nDetails:\n{1}", ex.Message, ex.GetBaseException().Message);
                MessageBox.Show(msg, string.Format("Não foi possível conectar-se ao host {0}", host), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
