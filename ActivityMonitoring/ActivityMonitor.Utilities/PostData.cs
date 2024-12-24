using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json.Linq;
using ActivityMonitor.entity;
using ActivityMonitor.Utilities.Implementation;


namespace ActivityMonitor.Utilities
{
    public class PostData
    {
        public  void PostActivities(List<ActivityModal> ActivityList)
        {
            var client = new RestClient("https://webactivitymonitor.azure-api.net/Api/Activity");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept","application/json");
            request.AddParameter("application/json",ActivityList, ParameterType.RequestBody);
            var response = client.Execute(request);
            var content = response.Content;
            int st = ((int)JObject.Parse(content)["start"]);
            int ed= ((int)JObject.Parse(content)["end"]);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                    ManageDB.DeleteUserActivity(st,ed);
              
            }
        }
    }
}
