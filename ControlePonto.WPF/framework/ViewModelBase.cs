using ControlePonto.Domain.services.persistence;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.framework
{
    public abstract class ViewModelBase : NotifyPropertyChangedBase, IDataErrorInfo, IDisposable
    {
        public const string FOCUS_START = "START";
        public const string FOCUS_MAIN = "MAIN";

        public const int CLOSE = -1;

        public IUnitOfWork unitOfWork { get; protected set; }

        #region Evento para resetar o DataContext
        public event EventHandler<DataContextResetEventArgs> ResetDataContextRequest;

        protected void resetDataContext(ViewModelBase newDataContext)
        {
            if (this.ResetDataContextRequest != null)
            {
                this.ResetDataContextRequest(this, new DataContextResetEventArgs(newDataContext));
            }
        }
        #endregion

        #region Evento para exibir messagebox

        public event EventHandler<MvvmMessageBoxEventArgs> MessageBoxRequest;
        protected void showMessageBox(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            if (this.MessageBoxRequest != null)
            {
                this.MessageBoxRequest(this, new MvvmMessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon, defaultResult, options));
            }
        }

        protected void showMessageBox(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            if (this.MessageBoxRequest != null)
            {
                this.MessageBoxRequest(this, new MvvmMessageBoxEventArgs(null, messageBoxText, caption, button, icon, defaultResult, options));
            }
        }
        
        #endregion

        #region Evento para solicitar nova View
        public event EventHandler<ViewRequestEventArgs> ViewRequest;

        protected void requestView(int code)
        {
            if (this.ViewRequest != null)
            {
                this.ViewRequest(this, new ViewRequestEventArgs(code));
            }
        }

        protected void requestView(ViewRequestEventArgs args)
        {
            if (this.ViewRequest != null)
            {
                this.ViewRequest(this, args);
            }
        }
        
        #endregion

        #region Evento para solicitar foco em um elemento da view
        public event EventHandler<string> FocusRequest;

        protected void requestFocus(string element)
        {
            if (this.FocusRequest != null)
            {
                this.FocusRequest(this, element);
            }
        }

        #endregion

        #region Tratativa de erros

        public bool isModelValid()
        {
            if (algumCampoNulo())
                return false;

            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in properties)
            {
                if (!p.CanWrite || !p.CanRead)
                {
                    continue;
                }
                if (this[p.Name] != null)
                {
                    return false;                    
                }
            }
            return true;
        }

        public string Error
        { get { return String.Empty;} }
        
        public string this[string columnName] 
        {
            get 
            {
                if (GetType().GetProperty(columnName).GetValue(this) == null) //Se for nulo nem preciso validar
                    return null;
                return validar(columnName); 
            }
        }

        protected abstract string validar(string propertyName);

        protected bool algumCampoNulo()
        {
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var ignoreNames = new string[] { "DialogResult", "unitOfWork" };

            foreach (PropertyInfo p in properties)
            {
                if (p.CanRead && p.GetIndexParameters().Length == 0 && !ignoreNames.Contains(p.Name))
                {
                    if (p.GetValue(this) == null)
                        return true;
                }
            }
            return false;
        }

        #endregion        
    
        public void Dispose()
        {
            if (unitOfWork != null)
                unitOfWork.Dispose();
        }
    }
}
