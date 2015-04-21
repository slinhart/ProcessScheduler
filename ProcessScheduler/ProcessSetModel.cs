using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    public class ProcessSetModel
    {
        public List<Process> processes;
        public int algorithm; //0 = FCFS    1 = Preemptive SJF  2 = Round Robin
        public int duration;
        public int quantum;
    }
}
