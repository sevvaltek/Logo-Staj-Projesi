using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageSender2.Models;
using System.Data;
using System.Data.SqlClient;
using KafkaNet;
using KafkaNet.Protocol;
using KafkaNet.Model;
using System.Text;


namespace MessageSender2.Controllers
{
    public class MessageController : Controller
    {
        //sa
        //sapass
        string connectionString = "Data Source=SEVVALTEKKOL;Database=Project;User ID=sa;Password=sapass";

        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblMessage = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT*FROM TBLMESSAGE", sqlCon);
                sqlDa.Fill(dtblMessage);
            }
            return View(dtblMessage);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new MessageModel());
        }
        [HttpPost]
        public ActionResult Create(MessageModel messageModel)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO TBLMESSAGE VALUES(@MESSAGECONTENT,@MESSAGEDATE,@MESSAGELOG)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);

                sqlCmd.Parameters.AddWithValue("@MESSAGECONTENT", messageModel.MESSAGECONTENT);
                sqlCmd.Parameters.AddWithValue("@MESSAGEDATE", messageModel.MESSAGEDATE);
                sqlCmd.Parameters.AddWithValue("@MESSAGELOG", messageModel.MESSAGELOG);

                sqlCmd.ExecuteNonQuery(); // sorgudan bir dönüş beklenmiyorsa kullanılır.
            }
            return RedirectToAction("Index");
        }
        //-------------------------------------------------------------------------------------------

        public JsonResult SendMessage(string mesajIcerik)
        {
            try
            {
                var tar = DateTime.Now;
                int idLog = 0;          // başta bütün id ler 0 olarak girilir hatasız gittiyse 1 e güncellenir.

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO TBLMESSAGE VALUES(@MESSAGECONTENT,@MESSAGEDATE,@MESSAGELOG)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    if (mesajIcerik != null && tar != null)
                    {
                        sqlCmd.Parameters.AddWithValue("@MESSAGECONTENT", mesajIcerik);
                        sqlCmd.Parameters.AddWithValue("@MESSAGEDATE", tar);
                        sqlCmd.Parameters.AddWithValue("@MESSAGELOG", idLog);
                    }
                    sqlCmd.ExecuteNonQuery();
                    string query2 = "SELECT MESSAGEID FROM TBLMESSAGE WHERE MESSAGEID=(SELECT MAX(MESSAGEID) FROM TBLMESSAGE)";
                    //en son kaydedilen mesajın id si çekilir.

                    SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
                    Int32 newId = (Int32)sqlCmd2.ExecuteScalar();

                    var model = new { id = newId };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json("hata");
            }
           
        }

        //-------------------------------------------------------------------------------------------------------------

        public JsonResult SendKafka(string mesajIcerik)
        {
            try
            {
                int idLog = 0;    // başta bütün id ler 0 olarak girilir hatasız gittiyse 1 e güncellenir.
                Message msg = new Message(mesajIcerik);
                Uri uri = new Uri("http://localhost:9092");
                var options = new KafkaOptions(uri);
                var router = new BrokerRouter(options);
                var producer = new Producer(router);
                var result = producer.SendMessageAsync("mytopic", new List<Message> { msg });

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query2 = "SELECT MESSAGEID FROM TBLMESSAGE WHERE MESSAGEID=(SELECT MAX(MESSAGEID) FROM TBLMESSAGE)";
                    //en son kaydedilen mesajın id si çekilir.

                    SqlCommand sqlCmd2 = new SqlCommand(query2, sqlCon);
                    Int32 newId = (Int32)sqlCmd2.ExecuteScalar();

                    Int32 kafkaID = 1;      // kafkaId yi çekemediğimiz için 1 olarak geldiğini varsaydık.
                    string sonuc = " ";

                    if (newId > 0 && kafkaID > 0)   // son id çekilmiş mi ve kafkadan mesaj gitmiş mi diye kontrol yapılır
                    {
                        sonuc = "mesajıniz başarıyla gönderildi";

                        string query3 = "UPDATE TBLMESSAGE SET MESSAGELOG=1  WHERE MESSAGEID=(SELECT MAX(MESSAGEID) FROM TBLMESSAGE) ";
                        //eger if bloğunun içine girerse update sorgusu ile son id li mesajın log sütünü 1 e set edilir.

                        SqlCommand sqlCmd3 = new SqlCommand(query3, sqlCon);
                        idLog = (Int32)sqlCmd3.ExecuteNonQuery();
                    }
                    else
                    {
                        sonuc = "mesaj gönderilirken hata oluştu";
                    }
                    var model = new { id = newId, sonuc = sonuc };
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json("hata");
            }
         
        }

    }
}
