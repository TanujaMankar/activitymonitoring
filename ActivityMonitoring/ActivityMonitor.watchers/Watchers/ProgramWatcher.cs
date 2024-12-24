using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ActivityMonitor.entity;
using System.Threading;
using ActivityMonitor.windows;
using ActivityMonitor.Utilities;
using ActivityMonitor.Utilities.Implementation;
using ActivityMonitor.Network;
using Microsoft.Win32;
using System.Windows.Forms;
using ActivityMonitor.service;
using Timer = System.Threading.Timer;

namespace ActivityMonitor.watcher

{
    /// <summary>
    /// Class for watching the opening of programs.
    /// </summary>
    public class ProgramWatcher : IResourceMonitor
    {
        private List<ActivityModal> runningProgramList = new List<ActivityModal>();
        private List<Process> procList;
        private Timer timer = null;
        private static ProgramWatcher instance = new ProgramWatcher();
        private ManageDB mdb = null;
        private string userName = "";

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hwnd, StringBuilder ss, int count);
        [DllImport("user32.dll")]
        static extern IntPtr GetAncestor(IntPtr hwnd, int count);
        /// <summary>
        /// Constructor
        /// </summary>

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns>instance</returns>
        public static ProgramWatcher GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Helper callback method for use of the timer object in order to poll.
        /// </summary>
        /// <param name="temp">needed argument</param>
        private void WatchProgram(object temp)
        {
            Console.WriteLine("Start");
            //Check new process opening
            foreach (Process P in Process.GetProcesses())
            {

                if (P.MainWindowTitle.Length > 0)
                {
                    try
                    {

                    }
                    catch (Exception e)
                    {

                    }//swallow

                }

            }
        }
        //AutomationFocusChangedEventHandler focusHandler = null;
        /// <summary>
        /// Create an event handler and register it.
        /// </summary>
        //public void SubscribeToFocusChange()
        //{
        //    focusHandler = new AutomationFocusChangedEventHandler(OnFocusChange);
        //    Automation.AddAutomationFocusChangedEventHandler(focusHandler);
        //}
        /// <summary>
        /// Handle the event.
        /// </summary>
        /// <param name="src">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        //public void OnFocusChange(object src, AutomationFocusChangedEventArgs e)
        //{
        //    IntPtr hwnd = GetAncestor(GetForegroundWindow(), 2);
        //    IntPtr Last = Process.GetCurrentProcess().Handle;

        //    uint pid;
        //    string processName = "";
        //    GetWindowThreadProcessId(hwnd, out pid);
        //    Process p = Process.GetProcessById((int)pid);
        //    processName = p.ProcessName;


        //    if (hwnd != Last)
        //    {
        //        String message = processName + "  LostFocus at  " + Constants.SPACE + DateTime.Now;
        //        EventSender.GetInstance().ProcessLostMessage(message);
        //        // hwnd=Last;
        //    }
        //}
        /// <summary>
        /// Cancel subscription to the event.
        /// </summary>
        //public void UnsubscribeFocusChange()
        //{
        //    if (focusHandler != null)
        //    {
        //        Automation.RemoveAutomationFocusChangedEventHandler(focusHandler);
        //    }
        //}
        private void GetActiveWindow(object temp)
        {
            //Create the variable
            const int nChar = 256;
            StringBuilder ss = new StringBuilder(nChar);
            //Run GetForeGroundWindows and get active window informations
            //assign them into handle pointer variable
            IntPtr handle = IntPtr.Zero;
            handle = GetForegroundWindow();
            string appName = "";
            string processName = "";
            string startTime = "";

            if (GetWindowText(handle, ss, nChar) > 0)
            {

                uint pid;
                GetWindowThreadProcessId(handle, out pid);
                Process p = Process.GetProcessById((int)pid);
                processName = p.ProcessName;

                appName = ss.ToString();
                if (appName == "Task Switching")
                    return;

                if (runningProgramList.Count() > -1
                    && (runningProgramList.IndexOf(runningProgramList.AsEnumerable().Reverse().Skip(1).FirstOrDefault()) < runningProgramList.Count - 1)
                    && (runningProgramList.Last().AppName != appName))
                {
                    runningProgramList.Add(new ActivityModal { Id = p.Id, AppName = appName, ProcessName = processName, StartTime = DateTime.Now.ToString("dd MMM yyy hh:mm:ss tt") });
                    startTime = DateTime.Now.ToString("dd MMM yyy hh:mm:ss tt");

                   // Console.WriteLine(String.Format("{0}\t - {1}\t - {2}", "Lost Focus", runningProgramList[runningProgramList.Count - 2].ProcessName, startTime));
                    Console.WriteLine(String.Format("{0}\t - {1}\t - {2}", processName, appName, startTime));

                    mdb.InsertUserActivity(processName, appName, startTime);

                }
                else if (runningProgramList.Count() <= 0)
                {
                    runningProgramList.Add(new ActivityModal { AppName = appName, ProcessName = processName, StartTime = DateTime.Now.ToString("dd MMM yyy hh:mm:ss tt") });
                    startTime = DateTime.Now.ToString("dd MMM yyy hh:mm:ss tt");
                    Console.WriteLine(String.Format("{0}\t - {1}\t - {2}", processName, appName, startTime));
                    mdb.InsertUserActivity(processName, appName, startTime);

                }
            }
        }
        public const String SLEEP_MODE = "SLEEP";
        public const String RESUME_MODE = "RESUME";
        public void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {

                String message = ResourceIdentifiers.POWERMODE_IDENTIFIER + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + e.Mode + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + SLEEP_MODE + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + DateTime.Now;
                EventSender.GetInstance().ProcessMessage(message);

            }
            else if (e.Mode == PowerModes.Resume)
            {
                String message = ResourceIdentifiers.POWERMODE_IDENTIFIER + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + e.Mode + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + RESUME_MODE + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + DateTime.Now;
                EventSender.GetInstance().ProcessMessage(message);
                ManageDB m = new ManageDB();
                DateTime PauseTime = m.GetLastTime();
                Console.WriteLine("Your were away from "+ PauseTime + " to " +DateTime.Now);
                var diffOfDates = DateTime.Now - PauseTime;

                var form = new Reason();
                Application.EnableVisualStyles();
                //Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Reason());
                form.Activate();
                form.Focus();
                Application.Run(form);
                if (diffOfDates.Minutes>1)
                {
                    
                    var form1 = new Reason();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Reason());
                    form.Activate();
                    form.Focus();
                    Application.Run(form1);
                    Console.WriteLine("Enter reason for being away");

                }
                
            }
        }
        #region IResourceMonitor Members
        //public delegate void MyEventHandler(object sender, PowerModeChangedEventArgs e);

        public async void StartMonitoring()
        {
            userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            mdb = new ManageDB();
            mdb.CreateUserActivityTable();
            procList = new List<Process>();
            TimerCallback timerDelegate = new TimerCallback(GetActiveWindow);
            timer = new Timer(timerDelegate, null, 0, 1000);
            timerDelegate.Invoke(new object());

            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

            //focusHandler = new AutomationFocusChangedEventHandler(OnFocusChange);
            //Automation.AddAutomationFocusChangedEventHandler(focusHandler);

        }
       
        public void EndMonitoring()
        {
            timer = null;
            runningProgramList = null;
            procList = null;
        }


        private void WatchApplicationIdealEvent()
        {
            MouseHook.Start();
            MouseHook.MouseAction += new EventHandler(Event);
        }
        private void Event(object sender, EventArgs e) { Console.WriteLine("Left mouse click!"); }
        #endregion
    }
}
