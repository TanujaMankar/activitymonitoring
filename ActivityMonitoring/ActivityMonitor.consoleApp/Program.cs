using System;
using Quartz.Impl;
using Quartz;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActivityMonitor.Utilities;
using ActivityMonitor.watcher;
using Microsoft.Win32;

namespace ActivityMonitor.consoleApp
{
    /// <summary>
    /// Main class.
    /// </summary>
    public class Program
    {

        public static Boolean active = true;
        public event EventHandler PowerModeHandler;
        public delegate void MyEventHandler(object sender, PowerModeChangedEventArgs e);

        /// <summary>
        /// Entry method.
        /// </summary>
        /// <param name="args">arguments</param>
        public static async Task Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           
            /* Get WebPage URL from IE */
            WebPageWatcher.GetInstance().StartMonitoring();

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();

            // 2. Get and start a scheduler
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            // 3. Create a job
            IJobDetail job = JobBuilder.Create<ScheduledJobs>()
                    .WithIdentity("My Jobs", "My Job Group")
                    .Build();

            // 4. Create a trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("My trigger", "My Job Group")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(3).RepeatForever())
                .Build();

            // 5. Schedule the job using the job and trigger 
            await scheduler.ScheduleJob(job, trigger);
           
            /* Monitors Apps/Programs */
            ProgramWatcher.GetInstance().StartMonitoring();

            while (active)
            {
                // runs indefinitely until terminated by user via task manager
            }
        }
    }
}
