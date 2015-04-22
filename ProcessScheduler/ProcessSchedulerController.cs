using System;
using System.Collections.Generic;
using System.Linq;
//Shayne Linhart

using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    class ProcessSchedulerController
    {
        static string inFile = "processes.in";

        static void Main(string[] args)
        {
            Parser parser = new Parser(inFile);
            ProcessSetModel psm = parser.createPsm();
            Scheduler scheduler = new Scheduler();
            String output = scheduler.executeAlgorithm(psm);
            System.IO.File.WriteAllText(@"processes.out", output);
            Console.Write(output);
            Console.Read();
        }
    }
}
