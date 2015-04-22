//Shayne Linhart

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
            bool idle = false; //true when all processes are done

            for (int t = 0; t < psm.duration + 1; t++)
            {
                if (!idle || (numProcesses != 0))
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
                            idle = true;
                            finishedProcesses.Add(runningProcess); //add the last process to the list of finished processes
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
                            idle = false;
                            q.Enqueue(p);
                            toRemove = p;
                            log.Append("Time " + t + ": " + p.name + " arrived\n");
                        }
                    }
                    if (toRemove != null) ps.Remove(toRemove); //after a process has arrived, remove it from list of processes (as it has been added to queue)

                    //Select process if none are running
                    if (runningProcess == null && !idle && q.Count > 0)
                    {
                        runningProcess = q.Dequeue();
                        idle = false; //CPU not idle since process is selected
                        runningProcess.wait = t - runningProcess.arrivalTime;
                        log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                    }
                }

                //if no processes are selected, and idle has been set (during this current cycle), append idle
                //this occurs when a process finished, there is still a process not complete, but it has yet to arrive
                if (runningProcess == null && idle && !(t == psm.duration))
                {
                    log.Append("Time " + t + ": IDLE\n");
                }
            }

            log.Append("Finished at time " + psm.duration + "\n\n");
            log.Append(generateConclusion(finishedProcesses));
            return log.ToString();
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
            List<Process> q = new List<Process>();
            Process runningProcess = null; //the process currently in the CPU
            bool idle = false; //true when all processes are done

            for (int t = 0; t < psm.duration + 1; t++)
            {
                if (!idle || (numProcesses != 0))
                {
                    //decrement remaining burst of the running process
                    if (runningProcess != null)
                    {
                        runningProcess.burstRemaining -= 1;
                    }

                    //Check for arriving proceses
                    Process toRemove = null;
                    foreach (Process p in ps)
                    {
                        if (p.arrivalTime == t)
                        {
                            idle = false;
                            q.Add(p);
                            toRemove = p;
                            log.Append("Time " + t + ": " + p.name + " arrived\n");
                        }
                    }
                    if (toRemove != null) ps.Remove(toRemove); //after a process has arrived, remove it from list of processes (as it has been added to queue)

                    //find process with shortest burst remaining
                    Process lastRunningProcess = runningProcess;
                    foreach (Process p in q)
                    {
                        if (runningProcess == null && p != null) runningProcess = p;
                        else if (p != null && p.burstRemaining < runningProcess.burstRemaining) runningProcess = p;
                    }

                    //Check if a new process was selected
                    if (lastRunningProcess != runningProcess)
                    {
                        q.Remove(runningProcess); //pop out process being placed in CPU
                        q.Add(lastRunningProcess); //push in process that is being preempted out of CPU
                        log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                    }

                    //check to see if running process complete
                    if (runningProcess != null && runningProcess.burstRemaining == 0)
                    {
                        runningProcess.wait = t - runningProcess.burst - runningProcess.arrivalTime;
                        log.Append("Time " + t + ": " + runningProcess.name + " finished\n");
                        numProcesses -= 1;

                        q.RemoveAll(item => item == null); //remove any left-over null valued items in queue
                        if (q.Count == 0) //check queue for processes
                        {
                            idle = true;
                            finishedProcesses.Add(runningProcess); //add the last process to the list of finished processes
                            runningProcess = null;
                        }
                        else //if a process exist in the queue
                        {
                            finishedProcesses.Add(runningProcess); //add finished process to list
                            runningProcess = null; //process is finished so is no longer running

                            //find process with shortest burst remaining
                            lastRunningProcess = runningProcess;
                            foreach (Process p in q)
                            {
                                if (runningProcess == null && p != null) runningProcess = p;
                                else if (p != null && p.burstRemaining < runningProcess.burstRemaining) runningProcess = p;
                            }

                            if (runningProcess != null)
                            {
                                q.Remove(runningProcess); //pop out process being placed in CPU
                                idle = false; //CPU not idle since process is selected
                                log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                            }
                        }
                    }

                    //if no processes are selected, and idle has been set (during this current cycle), append idle
                    //this occurs when a process finished, there is still a process not complete, but it has yet to arrive
                    if (runningProcess == null && idle && !(t == psm.duration))
                    {
                        log.Append("Time " + t + ": IDLE\n");
                    }
                }
                else if (!(t == psm.duration))
                {
                    log.Append("Time " + t + ": IDLE\n");
                }
            }

            log.Append("Finished at time " + psm.duration + "\n\n");
            log.Append(generateConclusion(finishedProcesses));
            return log.ToString();
        }

        private String executeRR(ProcessSetModel psm)
        {
            StringBuilder log = new StringBuilder();

            log.Append(psm.processes.Count + " processes\n"); // # processes
            log.Append("Using " + algorithmMap(psm.algorithm) + "\n\n"); //Using [alg name]

            List<Process> finishedProcesses = new List<Process>(); //list of finished processes, used to generate conclusion data
            List<Process> ps = psm.processes; //alias for brevity

            int numProcesses = ps.Count; //used to know when all processes have completed their burst
            int quantumRemaining = psm.quantum + 1;

            //some setup of the processes
            foreach (Process p in ps)
            {
                p.burstRemaining = p.burst;
            }

            //execution loop
            Queue<Process> q = new Queue<Process>();
            Process runningProcess = null; //the process currently in the CPU
            bool idle = false; //true when all processes are done

            for (int t = 0; t < psm.duration + 1; t++)
            {
                quantumRemaining -= 1;

                if (!idle || (numProcesses != 0))
                {

                    //Check for arriving proceses
                    Process toRemove = null;
                    foreach (Process p in ps)
                    {
                        if (p.arrivalTime == t)
                        {
                            idle = false;
                            q.Enqueue(p);
                            toRemove = p;
                            log.Append("Time " + t + ": " + p.name + " arrived\n");
                        }
                    }
                    if (toRemove != null) ps.Remove(toRemove); //after a process has arrived, remove it from list of processes (as it has been added to queue)

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
                        runningProcess.wait = t - runningProcess.burst - runningProcess.arrivalTime;

                        if (q.Count == 0) //check queue for processes
                        {
                            idle = true;
                            finishedProcesses.Add(runningProcess); //add the last process to the list of finished processes
                            runningProcess = null;
                        }
                        else //if a process exist in the queue
                        {
                            finishedProcesses.Add(runningProcess); //add finished process to list
                            runningProcess = q.Dequeue(); //get the next process in the queue
                            log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                        }
                    }

                    //see if quantum has ended
                    if (quantumRemaining == 0)
                    {
                        if(runningProcess != null) q.Enqueue(runningProcess);
                        runningProcess = null;
                        quantumRemaining = psm.quantum;
                    }

                    //Select process if none are running
                    if (runningProcess == null && !idle && q.Count > 0)
                    {
                        runningProcess = q.Dequeue();
                        idle = false; //CPU not idle since process is selected
                        log.Append("Time " + t + ": " + runningProcess.name + " selected (burst " + runningProcess.burstRemaining + ")\n");
                    }
                }

                //if no processes are selected, and idle has been set (during this current cycle), append idle
                //this occurs when a process finished, there is still a process not complete, but it has yet to arrive
                if (runningProcess == null && idle && !(t == psm.duration))
                {
                    log.Append("Time " + t + ": IDLE\n");
                }
            }

            log.Append("Finished at time " + psm.duration + "\n\n");
            log.Append(generateConclusion(finishedProcesses));
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
