using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Caspara.Services
{
    public abstract class ThreadService : Service, IThreadService
    {
        public int SleepInterval { get; set; } = 100;
        protected Thread ExecutionThread { get; set; }
        public Boolean Active { get; set; }
        public Double ExecutionTime { get; set; } = 0;

        public abstract void PerformAction();
        public override void Start()
        {
            if (!IsRunning && Enabled)
            {
                Active = true;
                try
                {
                    ExecutionThread = new Thread(new ThreadStart(ExecuteMethod));
                    ExecutionThread.Start();
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to start ThreadService: " + Name + " -> " + err.ToString());
                }
            }
        }
        public override void Stop()
        {
            if (IsRunning)
            {
                Active = false;
                while (IsRunning)
                    Thread.Sleep(10);
                try
                {
                    ExecutionThread.Join();
                }
                catch (Exception err)
                {
                    Console.WriteLine("Failed to stop ThreadService: " + Name + " -> " + err.ToString());
                }
            }
        }
   
        private DateTime LastRun = DateTime.Now;
        private void ExecuteMethod()
        {
            IsRunning = true;
            //Interlocked.Increment(ref CoreStatic.Application.ThreadsRunning);
            Console.WriteLine("ThreadService: " + Name + " Started");
            Stopwatch sw = new Stopwatch();
            while (Active)
            {

                if (Enabled && (DateTime.Now - LastRun).TotalMilliseconds > SleepInterval)
                {
                    sw.Start();
                    LastRun = DateTime.Now;
                    //Interlocked.Increment(ref SuiteSharedStatic.ExecutingProcesses);
                    PerformAction();
                    sw.Stop();
                    ExecutionTime = sw.Elapsed.TotalMilliseconds;
                    sw.Reset();
                    //Interlocked.Decrement(ref SuiteSharedStatic.ExecutingProcesses);
                }
                else
                {
                    Thread.Sleep(SleepInterval / 2);
                }

            }
            IsRunning = false;
            //Interlocked.Decrement(ref CoreStatic.Application.ThreadsRunning);
            Console.WriteLine("ThreadService: " + Name + " Stopped");
        }
    }
}
