using System;
using System.Security.Principal;
using Microsoft.Win32;
using ActivityMonitor.service;
using ActivityMonitor.watcher;
using ActivityMonitor.Network;

namespace ActivityMonitor.core
{
    public class PowerModeWatcher
    {
        public const String SLEEP_MODE = "SLEEP";
        public const String RESUME_MODE = "RESUME";
        private static PowerModeWatcher instance = new PowerModeWatcher();
       public PowerModeWatcher()
        {
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
     
        }
        public static PowerModeWatcher GetInstance()
        {
            return instance;
        }

        public void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if(e.Mode==PowerModes.Suspend)
            {

                String message = ResourceIdentifiers.POWERMODE_IDENTIFIER + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + e.Mode + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + SLEEP_MODE + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + DateTime.Now;
                EventSender.GetInstance().ProcessMessage(message);

            }
            else if(e.Mode==PowerModes.Resume) 
            {
                String message = ResourceIdentifiers.POWERMODE_IDENTIFIER + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + e.Mode + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + RESUME_MODE + Constants.SPACE + Constants.SPLITTER + Constants.SPACE
                                 + DateTime.Now;
                EventSender.GetInstance().ProcessMessage(message);
                Console.WriteLine("Your were away");
            }

           
        }


    }
}
