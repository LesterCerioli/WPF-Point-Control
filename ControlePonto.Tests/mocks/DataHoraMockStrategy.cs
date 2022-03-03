using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class DataHoraMockStrategy : IDataHoraStrategy
    {
        public DateTime DateTimeMock { get; set; }

        public DataHoraMockStrategy() { }

        public DataHoraMockStrategy(DateTime dt)
        {
            DateTimeMock = dt;
        }

        public DataHoraMockStrategy(int dia, int mes, int ano, int hora = 9, int minuto = 0, int segundo = 0) : this(new DateTime(ano, mes, dia, hora, minuto, segundo))
        {

        }

        public DateTime getDataHoraAtual()
        {
            return DateTimeMock;
        }
    }
}
