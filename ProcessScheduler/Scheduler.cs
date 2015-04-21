using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduler
{
    class Scheduler
    {
        public String executeAlgorithm(ProcessSetModel psm) {
            String log = "";
            if (psm.algorithm == 0) log = executeFCFS(psm);
            if (psm.algorithm == 1) log = executeSJF(psm);
            if (psm.algorithm == 2) log = executeRR(psm);

            return log;
        }

        private String executeFCFS(ProcessSetModel psm)
        {
            StringBuilder log = new StringBuilder();

            log.Append(psm.processes.Count + " processes\n"); // # processes
            log.Append("Using " + algorithmMap(psm.algorithm) + "\n\n"); //Using [alg name]

            List<Process> finishedProcesses = new List<Process>(); //list of finished processes, used to generate conclusion data
            List<Process> ps = psm.processes; //alias for brevity

            int numProcesses = ps.Count; //used to know when all processes have completed their burst

            //some setup of the processes
            foreach (Process p in ps)
            {
                p.burstRemaining = p.burst;
            }

            //execution loop
            Queue<Process> q = new Queue<Process>();
            Process runningProcess = null; //the process currently in the CPU

            for (int t = 0; t < psm.duration + 1; t++)
            {
                //decrement remaining burst of the running process
                if (runningProcess != null)
                {
                    runningProcess.burstRemaining -= 1;
                }

                //check to see if running process complete
                if (runningProcess != null && runningProcess.burstRemaining == 0)
                {
                    log.Append("Time " + t + ": " + runningProcess.name + " finished\n");
                    numProcesses -= 1;

                    if (q.Count == 0) //check queue for processes
                    {
                        if (numProcesses == 0) //check if all processes have completed their burst
                        {
                            log.Append("Finished at time " + t + "\n\n");
                            finishedProcesses.Add(runningProcess); //add the last process to the list of finished proccesses
                            log.Append(generateConclusion(finishedProcesses));
                            return log.ToString();
                        }
                        runningProcess = null;
                    }
                    else //if a process exist in the queue
                    {
                        finishedProcesses.Add(runningProcess); //add finished process to list
                        runningProcess = q.Dequeue(); //get the next process in the queue
                        runningProcess.wait = t - runningProcess.arrivalTime;
                        log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                    }
                }

                //check to see if any process should preempt (but not in fcfs)

                //Check for arriving proceses
                Process toRemove = null;
                foreach (Process p in ps)
                {
                    if (p.arrivalTime == t)
                    {
                        q.Enqueue(p);
                        toRemove = p;
                        log.Append("Time " + t + ": " + p.name + " arrived\n");
                    }
                }
                if (toRemove != null) ps.Remove(toRemove); //after a process has arrived, remove it from list of processes (as it has been added to queue)

                //Select process if none are running
                if (runningProcess == null)
                {
                    runningProcess = q.Dequeue();
                    runningProcess.wait = t - runningProcess.arrivalTime;
                    log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                }
            }

            return log.ToString(); //fallback return
        }

        private String executeSJF(ProcessSetModel psm)
        {
            StringBuilder log = new StringBuilder();

            log.Append(psm.processes.Count + " processes\n"); // # processes
            log.Append("Using " + algorithmMap(psm.algorithm) + "\n\n"); //Using [alg name]

            List<Process> finishedProcesses = new List<Process>(); //list of finished processes, used to generate conclusion data
            List<Process> ps = psm.processes; //alias for brevity

            int numProcesses = ps.Count; //used to know when all processes have completed their burst

            //some setup of the processes
            foreach (Process p in ps)
            {
                p.burstRemaining = p.burst;
            }

            //execution loop
            Queue<Process> q = new Queue<Process>();
            Process runningProcess = null; //the process currently in the CPU

            for (int t = 0; t < psm.duration + 1; t++)
            {
                //decrement remaining burst of the running process
                if (runningProcess != null)
                {
                    runningProcess.burstRemaining -= 1;
                }

                //check to see if running process complete
                if (runningProcess != null && runningProcess.burstRemaining == 0)
                {
                    log.Append("Time " + t + ": " + runningProcess.name + " finished\n");
                    numProcesses -= 1;

                    if (q.Count == 0) //check queue for processes
                    {
                        if (numProcesses == 0) //check if all processes have completed their burst
                        {
                            log.Append("Finished at time " + t + "\n\n");
                            finishedProcesses.Add(runningProcess); //add the last process to the list of finished proccesses
                            log.Append(generateConclusion(finishedProcesses));
                            return log.ToString();
                        }
                        runningProcess = null;
                    }
                    else //if a process exist in the queue
                    {
                        finishedProcesses.Add(runningProcess); //add finished process to list
                        runningProcess = q.Dequeue(); //get the next process in the queue
                        runningProcess.wait = t - runningProcess.arrivalTime;
                        log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                    }
                }

                //check to see if any process should preempt (but not in fcfs)

                //Check for arriving proceses
                Process toRemove = null;
                foreach (Process p in ps)
                {
                    if (p.arrivalTime == t)
                    {
                        q.Enqueue(p);
                        toRemove = p;
                        log.Append("Time " + t + ": " + p.name + " arrived\n");
                    }
                }
                if (toRemove != null) ps.Remove(toRemove); //after a process has arrived, remove it from list of processes (as it has been added to queue)

                //Select process if none are running
                if (runningProcess == null)
                {
                    runningProcess = q.Dequeue();
                    runningProcess.wait = t - runningProcess.arrivalTime;
                    log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                }
            }

            return log.ToString(); //fallback return
        }

        private String executeRR(ProcessSetModel psm)
        {
            StringBuilder log = new StringBuilder();


            return log.ToString();
        }

        private String generateConclusion(List<Process> ps)
        {
            StringBuilder log = new StringBuilder();
            foreach (Process p in ps)
            {
                log.Append(p.name + " wait " + p.wait + " turnaround " + (p.wait + p.burst) + "\n");
            }
            return log.ToString();
        }

        private String algorithmMap(int n)
        {
            if (n == 0) return "First Come First Served";
            if (n == 1) return "Shortest Job First (Pre)";
            if (n == 2) return "Round-Robin";
            return "error";
        }
    }
}
