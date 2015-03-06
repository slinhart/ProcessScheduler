using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    public class ProcessSetModel
    {
        public Process[] processes;
        public int algorithm;
        public int duration;
        public int quantum;
    }
}
