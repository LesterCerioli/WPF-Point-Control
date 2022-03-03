using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ControlePonto.WPF.window
{
    public class WindowBase : Window
    {
        protected ViewModelBase ViewModel { get; set; }       

        public WindowBase(ViewModelBase viewModel)
        {
            this.ViewModel = viewModel;
            this.Loaded += WindowBase_Loaded;
            this.Closed += WindowBase_Closed;
        }

        protected virtual void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            assignEvents();
        }

        protected void assignEvents()
        {
            ViewModel.MessageBoxRequest += messageBoxRequested;
            ViewModel.ResetDataContextRequest += resetDataContextRequested;
            ViewModel.ViewRequest += viewRequested;
            ViewModel.FocusRequest += focusRequested;
        }

        protected virtual void focusRequested(object sender, string e)
        {            
        }

        protected void removeEvents()
        {
            ViewModel.MessageBoxRequest -= messageBoxRequested;
            ViewModel.ResetDataContextRequest -= resetDataContextRequested;
            ViewModel.ViewRequest -= viewRequested;
            ViewModel.FocusRequest -= focusRequested;
        }

        protected virtual void viewRequested(object sender, ViewRequestEventArgs e)
        {
            if (e.RequestCode == ViewModelBase.CLOSE)
                Close();
        }

        protected virtual void messageBoxRequested(object sender, framework.MvvmMessageBoxEventArgs e)
        {
            e.Show(this);
        }

        protected virtual void resetDataContextRequested(object sender, framework.DataContextResetEventArgs e)
        {
            removeEvents();
            DataContext = ViewModel = e.DataContext;                        
            assignEvents();
        }

        protected virtual void WindowBase_Closed(object sender, EventArgs e)
        {
            ViewModel.Dispose();
        }
    }
}
