using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlePonto.WPF.framework
{
    public class ViewRequestEventArgs : EventArgs
    {
        public int RequestCode { get; private set; }

        public ViewRequestEventArgs(int requestCode)
        {
            this.RequestCode = requestCode;
        }
    }
}
