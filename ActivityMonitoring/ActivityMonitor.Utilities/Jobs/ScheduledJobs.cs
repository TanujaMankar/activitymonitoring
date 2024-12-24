using Quartz;
using System;
using System.Threading.Tasks;
using ActivityMonitor.Utilities.Implementation;
using ActivityMonitor.service;
using ActivityMonitor.entity;

namespace ActivityMonitor.Utilities
{
    public class ScheduledJobs : IJob
    {
       
        public  async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Myjob executed");
            Console.WriteLine(DateTime.Now.ToString());
            ManageDB manageDb = new ManageDB();
            var result = manageDb.GetUserActivity();
            PostData apiMethod = new PostData();

            apiMethod.PostActivities(result);


        }
           
        }
    }

