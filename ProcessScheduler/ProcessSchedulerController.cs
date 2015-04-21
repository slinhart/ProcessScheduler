using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    class ProcessSchedulerController
    {
        static string inFile = "processes.in";
        static string outFile = "processes.out";

        static void Main(string[] args)
        {
            Parser parser = new Parser(inFile);
            ProcessSetModel psm = parser.createPsm();
            Scheduler scheduler = new Scheduler();
            String output = scheduler.executeAlgorithm(psm);
            Console.Write(output);
            Console.Read();
        }
    }
}
