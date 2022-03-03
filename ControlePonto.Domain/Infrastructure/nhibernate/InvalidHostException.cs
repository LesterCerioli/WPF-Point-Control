using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Infrastructure.nhibernate
{
    public class InvalidHostException : Exception
    {
        public string Host { get; private set; }

        public InvalidHostException(string host, Exception inner) : base("Não foi possível se conectar com " + host, inner)
        {
            this.Host = host;
        }
    }
}
