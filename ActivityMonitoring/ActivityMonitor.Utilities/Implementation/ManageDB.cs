using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using ActivityMonitor;
using ActivityMonitor.entity;
namespace ActivityMonitor.Utilities.Implementation
{
    public class ManageDB :IManageDB
    {
        public void CreateSqlLiteDatabase()
        {
            SQLiteConnection.CreateFile("SmartData.db3");
        }
        

        public void Changepassword()
        {
            string fileLoc = Path.GetFullPath("SmartData.db3");
            using (SQLiteConnection con = new
           SQLiteConnection("data source=" + fileLoc))
            {
                con.Open();
                con.ChangePassword("@@#DEMOSMART#@@");
            }
        }

        public void Setpassword()
        {
            string fileLoc = Path.GetFullPath("SmartData.db3");
            using (SQLiteConnection con = new
           SQLiteConnection("data source=" + fileLoc))
            {
                con.SetPassword("@@#DEMOSMART#@@");
            }
        }
        public void Removepassword()
        {
            string fileLoc = Path.GetFullPath("SmartData.db3");
            using (SQLiteConnection con = new
           SQLiteConnection("data source=" + fileLoc))
            {
                con.SetPassword("@@#DEMOSMART#@@");
                con.Open();
                con.ChangePassword("");
            }
        }

        public void CreateUserActivityTable()
        {
            try
            {
                if(!File.Exists("SmartData.db3"))
                {
                    //var databasePath = Path.Combine("data source=C:\\Aktines\\Data","SmartData.db3");
                    SQLiteConnection.CreateFile("SmartData.db3");
                    string fileLoc = Path.GetFullPath("SmartData.db3"); // Server.MapPath("SmartData.db3");
                                                                        // string FullPath = Path.Combine("data source=",fileLoc.ToString());
                                                                        //Removepassword();
                    using (SQLiteConnection con = new
                   SQLiteConnection("data source=" + fileLoc))
                    {

                        con.Open();
                        using (SQLiteCommand com = new SQLiteCommand(con))
                        {
                            string createTableQuery =
                            @"CREATE Table IF NOT EXISTS [UserActivity] (
                        [Id] INTEGER NOT NULL,
                        [UserName] TEXT,
	                    [ProcessName] TEXT,
                        [AppName] TEXT,
                        [StartTime] TEXT,
                        PRIMARY KEY([Id] AUTOINCREMENT)); ";
                            com.CommandText = createTableQuery;
                            com.ExecuteNonQuery();
                        }
                    }
                } 
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public void InsertUserActivity(string processName,string appName,string startTime)
        {
            try
            {
                string fileLoc = Path.GetFullPath("SmartData.db3");
                using (SQLiteConnection con = new
               SQLiteConnection("data source=" + fileLoc))
                {
                    //write code to use primary keyto acccess lost focus app name row and updte end date column
                    con.Open();
                    using (SQLiteCommand com = new SQLiteCommand(con))
                    {
                        string insertTableQuery = string.Format("insert into UserActivity (ProcessName,AppName,StartTime) values ('{0}','{1}','{2}');",processName,appName,startTime);
                        com.CommandText = insertTableQuery;
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public  List<ActivityModal> GetUserActivity()
        {
            try
            {
                string fileLoc = Path.GetFullPath("SmartData.db3");
                using (SQLiteConnection con = new
               SQLiteConnection("data source=" + fileLoc))
                {
                    List<ActivityModal> list = new List<ActivityModal>();
                    //write code to use primary keyto acccess lost focus app name row and updte end date column
                    con.Open();
                    using (SQLiteCommand fmd = new SQLiteCommand(con))
                    {
                        fmd.CommandText = @"select Id,UserName,ProcessName,AppName,StartTime from UserActivity order by Id asc limit 10";
                        fmd.CommandType = CommandType.Text;
                        SQLiteDataReader r = fmd.ExecuteReader();
                        while (r.Read())
                        {
                            list.Add(new ActivityModal()
                            {
                                Id = Convert.ToInt32(r["Id"]),  
                                ProcessName = Convert.ToString(r["ProcessName"]),
                                AppName = Convert.ToString(r["AppName"]), 
                                StartTime = Convert.ToString(r["StartTime"]),
                                UserName = Convert.ToString(r["UserName"])

                            });
                        }
                    }
                    return list;
                }
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        DateTime PauseTime;
        public DateTime GetLastTime()
        {
            try
            {
                string fileLoc = Path.GetFullPath("SmartData.db3");
                using (SQLiteConnection con = new
               SQLiteConnection("data source=" + fileLoc))
                {
                   
                   
                    con.Open();
                    using (SQLiteCommand fmd = new SQLiteCommand(con))
                    {
                        fmd.CommandText = @"select StartTime from UserActivity order by Id desc limit 1";
                        fmd.CommandType = CommandType.Text;
                        SQLiteDataReader r = fmd.ExecuteReader();
                        while (r.Read())
                        {


                            PauseTime = Convert.ToDateTime(r["StartTime"]);
                        }
                    }
                    return PauseTime;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool checkInsertedRecords(List<ActivityModal> list)
        {
           
            return true;
        }
        public static void DeleteUserActivity(int start,int end)
        {
            try
            {
               string fileLoc = Path.GetFullPath("SmartData.db3");
                using (SQLiteConnection con = new
               SQLiteConnection("data source=" + fileLoc))
                {
                    List<ActivityModal> list = new List<ActivityModal>();
                    //write code to use primary keyto acccess lost focus app name row and updte end date column
                    con.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(con))
                    {
                        cmd.CommandText = @"Delete from UserActivity where Id>= @start AND Id<= @end ";
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@start",start );
                        cmd.Parameters.AddWithValue("@end", end);
                        SQLiteDataReader r = cmd.ExecuteReader();
                   
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}