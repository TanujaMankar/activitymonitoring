using System;
using Quartz.Impl;
using Quartz;
using ActivityMonitor.service;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ActivityMonitor.Utilities;
using ActivityMonitor.watcher;
using Microsoft.Win32;
using ActivityMonitor.windows;
using System.Security.Cryptography.X509Certificates;

namespace Polling
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


            //COMReceiver.GetInstance().DeleteAllMessages();

            //EventSender.GetInstance().StartConnection(ServerInformation.LOCAL_HOST, ServerInformation.DEFAULT_PORT);

            /* Get input from COM */
            //COMReceiver.GetInstance().StartReceivingMessages();

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

            Application.Run(new Form1());
          
          
            
            // CtrlAltDel.GetInstance().StartMonitoring();

            //EventLog log = new EventLog("Security");
            //var entries = log.Entries.Cast<EventLogEntry>()
            //                         .Where(x => x.EventID == 4624)
            //                         .Select(x => new
            //                         {
            //                             x.MachineName,
            //                             x.Site,
            //                             x.Source,
            //                             x.Message,
            //                             x.UserName,
            //                             x.TimeGenerated,
            //                             x.TimeWritten,
            //                             x.Category,
            //                             x.EventID

            //                         }).ToList();

            //foreach(var e in entries)
            //{
            //    Console.WriteLine(string.Format("EventId:{5},Category :{0}/tMessage:1{1}/tTimeGenerated:{2}/tTimeWritten:{3}/tUserName:{4}",e.Category,"",e.TimeGenerated,e.TimeWritten,e.UserName,e.EventID));
            //}
            //Console.WriteLine(entries.Where(x => Convert.ToDateTime(x.TimeWritten) > new DateTime(2021, 11, 23)).Min(x =>Convert.ToDateTime(x.TimeWritten)));



            /* Monitors Folder/Directories */
            //FolderMonitor.GetInstance().StartMonitoring();

            // Monitor emails
            //EmailMonitor.GetInstance().StartMonitoring();


            while (active)
            {
                // runs indefinitely until terminated by user via task manager
            }
        }
    }
}
