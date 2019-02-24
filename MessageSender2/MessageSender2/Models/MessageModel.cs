using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MessageSender2.Models
{
    public class MessageModel
    {
        //public int MESSAGEID { get; set; }
        [DisplayName("Log")]
        public int MESSAGELOG { get; set; }

        [DisplayName("Mesaj")]
        public string MESSAGECONTENT { get; set; }

        [DisplayName("Tarih")]
        public DateTime MESSAGEDATE
        {
            get
            {
                return DateTime.Now;
            }
            set { }

        }
      


    }
   
}