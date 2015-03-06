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
        ProcessSetModel psm;

        static void Main(string[] args)
        {
            Parser parser = new Parser(inFile);
            psm = parser.parse();
            Console.Read();
        }
    }
}
