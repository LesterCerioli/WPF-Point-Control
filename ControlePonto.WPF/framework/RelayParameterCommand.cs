using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.framework
{
    public class RelayParameterCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<T> methodToExecute;
        private Func<bool> canExecuteEvaluator;

        public RelayParameterCommand(Action<T> methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayParameterCommand(Action<T> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null)
                return true;

            return this.canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke((T)parameter);
        }
    }
}
