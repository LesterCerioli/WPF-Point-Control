using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.framework
{
    public abstract class ViewModelDialogBase : ViewModelBase
    {
        protected ICommand _fecharCommand;
        public ICommand FecharCommand
        {
            get
            {
                if (_fecharCommand == null)
                {
                    _fecharCommand = new RelayCommand(() => DialogResult = false);
                }
                return _fecharCommand;
            }
        }

        protected ICommand _confirmarCommand;
        public ICommand ConfirmarCommand
        {
            get
            {
                if (_confirmarCommand == null)
                {
                    _confirmarCommand = new RelayCommand(confirmar, podeConfirmar);
                }
                return _confirmarCommand;
            }
        }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get { return _dialogResult; }
            set { SetField(ref _dialogResult, value); }
        }

        protected abstract void confirmar();

        protected virtual bool podeConfirmar()
        {
            return true;
        }
    }
}
