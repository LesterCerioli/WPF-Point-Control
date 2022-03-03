using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.framework
{
    class RelayParameterEvaluatorCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action<T> methodToExecute;
        private Func<T, bool> canExecuteEvaluator;

        public RelayParameterEvaluatorCommand(Action<T> methodToExecute, Func<T, bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayParameterEvaluatorCommand(Action<T> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null)
                return true;

            return this.canExecuteEvaluator.Invoke((T)parameter);
        }

        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke((T)parameter);
        }

    }
}
