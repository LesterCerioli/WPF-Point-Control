using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.framework
{
    public abstract class ViewModelAlertaBase : ViewModelBase
    {
        public List<string> Alertas
        {
            get
            {
                List<string> alertas = new List<string>();
                PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo p in properties)
                {
                    if (!p.CanWrite || !p.CanRead)
                    {
                        continue;
                    }

                    string alerta = verificarAlerta(p.Name);
                    if (!string.IsNullOrEmpty(alerta))
                    {
                        alertas.Add(alerta);
                    }
                }
                return alertas;
            }
        }

        protected abstract string verificarAlerta(string propertyName);
    }
}
