using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    public class Process
    {
        public string name;
        public int arrivalTime;
        public int burst;
        public int burstRemaining;
        public int wait;

        public Process(string n, int a, int b)
        {
            this.name = n;
            this.arrivalTime = a;
            this.burst = b;
        }
    }
}
