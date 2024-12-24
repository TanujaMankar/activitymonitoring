using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActivityMonitor.watcher
{
    /// <summary>
    /// Interface for specifying the need to capturing event data for activity
    /// monitoring use.
    /// </summary>
    public interface IResourceMonitor
    {
        /// <summary>
        /// Start the monitoring process.
        /// </summary>
        void StartMonitoring();


        /// <summary>
        /// Stops the monitoring process.
        /// </summary>
        void EndMonitoring();
    }
}
