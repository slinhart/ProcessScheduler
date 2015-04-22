//Shayne Linhart

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    public class Parser
    {
        string[] lines;
        int numProcesses;


        public Parser(string inFile)
        {
            try
            {
                lines = System.IO.File.ReadAllLines(@inFile);
            }
            catch { Console.Write("Input file not found. Place input file in same directory as the executable."); }
        }

        public ProcessSetModel createPsm() {
            ProcessSetModel psm = new ProcessSetModel();
            psm.quantum = -1; //-1 for unitialized
            psm.processes = new List<Process>();

            foreach(string line in lines) {
                //Remove Comments
                string lineNoComments = line.Split('#')[0];
                //Tokenize line
                string[] tokens = lineNoComments.Split(' ');

                switch (tokens[0])
                {
                    case "processcount":
                        numProcesses = Convert.ToInt32(tokens[1]);
                        break;
                    case "runfor":
                        psm.duration = Convert.ToInt32(tokens[1]);
                        break;
                    case "quantum":
                        psm.quantum = Convert.ToInt32(tokens[1]);
                        break;
                    case "process":
                        psm.processes.Add(new Process(tokens[2], Convert.ToInt32(tokens[4]), Convert.ToInt32(tokens[6])));
                        break;
                    case "use":
                        if (tokens[1] == "fcfs") psm.algorithm = 0;
                        if (tokens[1] == "sjf") psm.algorithm = 1;
                        if (tokens[1] == "rr") psm.algorithm = 2;
                        break;
                    default:
                        break;

                }
            }
            return psm;
        }
    }

}
