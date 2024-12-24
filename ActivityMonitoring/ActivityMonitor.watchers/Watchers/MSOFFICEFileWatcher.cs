using System;
using ActivityMonitor.service;
using ActivityMonitor.watcher;
using ActivityMonitor.Network;
namespace ActivityMonitor.watcher
{
    /// <summary>
    /// Used to monitor Microsoft Office application files.
    /// </summary>
    public class MSOFFICEFileWatcher
    {
        private static MSOFFICEFileWatcher instance = new MSOFFICEFileWatcher();

        private MSOFFICEFileWatcher() { }

        /// <summary>
        /// Singleton implementation.
        /// </summary>
        /// <returns>instance</returns>
        public static MSOFFICEFileWatcher GetInstance()
        {
            return instance;
        }

        public void Start()
        {

        }

        /// <summary>
        /// File view for MS FILES.. (WORD/EXCEL/POWERPOINT/VISIO/PROJECT)
        /// </summary>
        /// <param name="eventmessage"></param>
        public void FileViewed(String eventmessage)
        {
            String[] spliitedMessage = eventmessage.Split('|');

            //String filename = spliitedMessage[1]

            String message = ResourceIdentifiers.FILE_IDENTIFIER + Constants.SPACE + Constants.SPLITTER + Constants.SPACE +
                             spliitedMessage[1] + Constants.SPACE + Constants.SPLITTER + Constants.SPACE +
                             FileMonitor.VIEW_ACTION + Constants.SPACE + Constants.SPLITTER + Constants.SPACE +
                             spliitedMessage[2];

            EventSender.GetInstance().ProcessMessage(message);


        }
    }
}
