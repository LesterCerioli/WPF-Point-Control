using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class DataHoraMockListStrategy : IDataHoraStrategy
    {
        private Queue<DateTime> queueDateTimes = new Queue<DateTime>();
        public int Count { get { return queueDateTimes.Count; } }
        public bool manualDequeue { get; private set; }

        public DataHoraMockListStrategy(bool manual, params DateTime[] dateTimes) 
            : this(dateTimes)
        {
            this.manualDequeue = manual;
        }

        public DataHoraMockListStrategy(params DateTime[] dateTimes)
        {            
            foreach (DateTime dateTime in dateTimes)
                queueDateTimes.Enqueue(dateTime);            
        }

        public DateTime getDataHoraAtual()
        {
            if (manualDequeue)
                return queueDateTimes.Peek();
            return queueDateTimes.Dequeue();
        }

        public DateTime dequeue()
        {
            return queueDateTimes.Dequeue();
        }
    }
}
