using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ActivityMonitor.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ActivityMonitor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly string connectionString;
       
        public ActivityController(IConfiguration configuration)
        {
            connectionString = "Server=techteamautomation.database.windows.net;Initial Catalog=dbActivityMonitor;Persist Security Info=False;User ID=devadmin;Password=P@$$w0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        // GET: api/<ActivityController>
        [HttpGet]
        public int Get()
        {
             int Id = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                String sql = "select top 1 ActivityId from UserActivity order by Id desc";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                         Id = rdr.GetInt32("ActivityId");
                           
                        
                    }
                }
            }
            return Id;

        }

        // GET api/<ActivityController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public indexes AddActivity([FromBody] List<ActivityModal> ActivityList)
        {
           indexes index=new indexes();
            index.start = ActivityList.First().Id;
            index.end = ActivityList.Last().Id;
            using (SqlConnection serverdb = new SqlConnection(connectionString))
            {
                serverdb.Open();
                foreach (var activity in ActivityList)
                {
                    String sql = "INSERT INTO UserActivity (UserName,ProcessName,AppName,StartTime) VALUES(@UserName,@ProcessName,@AppName, @StartTime)";
                    using (SqlCommand cmd = new SqlCommand(sql, serverdb))
                    {
                        //cmd.Parameters.AddWithValue("@Id", activity.Id);
                        cmd.Parameters.AddWithValue("@UserName", activity.UserName);
                        cmd.Parameters.AddWithValue("@ProcessName", activity.ProcessName);
                        cmd.Parameters.AddWithValue("@AppName", activity.AppName);
                        cmd.Parameters.AddWithValue("@StartTime", activity.StartTime);
                        activity.Id = Convert.ToInt32(cmd.ExecuteScalar());


                    }
                }
                serverdb.Close();
            }
            return index;

        }

        // PUT api/<ActivityController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ActivityController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqliteConnection litedb = new SqliteConnection(connectionString))
            {
                litedb.Open();
                String sql = "Delete from UserActivity where Id=@Id";
                using (SqliteCommand cmd = new SqliteCommand(sql, litedb))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    litedb.Close();
                    
                }
            }
        }
    }
}
