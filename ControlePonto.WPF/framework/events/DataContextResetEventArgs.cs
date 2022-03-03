using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.framework
{
    public class DataContextResetEventArgs : EventArgs
    {
        ViewModelBase newDataContext;

        public ViewModelBase DataContext
        {
            get { return newDataContext; }
        }

        public DataContextResetEventArgs(ViewModelBase newDataContext)
        {
            this.newDataContext = newDataContext;
        }
    }
}
