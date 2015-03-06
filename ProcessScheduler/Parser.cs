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


        public Parser(string inFile)
        {
            try
            {
                lines = System.IO.File.ReadAllLines(@inFile);
                Console.Write(lines[0]);
            }
            catch { Console.Write("Input file not found. Place input file in same directory as the executable."); }
        }

        public ProcessSetModel createPsm() {
            ProcessSetModel pcm = new ProcessSetModel();

            foreach(string line in lines) {
                //Remove Comments
                string lineNoComments = line.Split('#')[0];
                //Tokenize line
                string[] tokens = lineNoComments.Split(' ');

                switch (tokens[0])
                {
                    case "processcount":
                        //do some process stuff
                        break;
                    case "runfor":
                        //do some other things
                        break;

                }
            }
            return pcm;
        }
    }

}
