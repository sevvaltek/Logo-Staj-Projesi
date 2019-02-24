using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace SchedulerProject
{
    public class Scheduler
    {
        public void SendUnsentMessages()
        {
            string connectionString = "Data Source=SEVVALTEKKOL;Database=Project;User ID=sa;Password=sapass";

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                {
                    //Gönderilmeyen mesajarı al
                    string query = "SELECT * FROM TBLMESSAGE WHERE MESSAGELOG=0";
                    SqlCommand sqlCmd2 = new SqlCommand(query, sqlCon);
                    var dt = new DataTable();
                    var reader = sqlCmd2.ExecuteReader();
                    dt.Load(reader);

                    // gönderilmeyen mesajların(logId si 1 olanların) teker teker çekildiği yer.
                    foreach (var row in dt.AsEnumerable())
                    {
                        var id = row["MESSAGEID"];
                        var content = row["MESSAGECONTENT"];

                        //Gönderilmeyen mesajın kafkaSend methodu cagırılarak gönderildiği kısım
                        //????
                        
                    

                    }

                    Console.WriteLine("Hangfire Server started. Press any key to exit...");

                    Console.ReadKey();
                }
            }

        }
    }
}
