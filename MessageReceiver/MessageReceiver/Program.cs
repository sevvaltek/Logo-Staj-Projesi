using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;

namespace MessageReceiver
{
    class Program
    {
        //consumer
        static void Main(string[] args)
        {
            string topic = "mytopic";
            Uri uri = new Uri("http://localhost:9092");
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topic, router));
            foreach (var message in consumer.Consume())
            {
                //Console.WriteLine("Partition ID:{0}\nOffset:{1}\nValue:{2}",message.Meta.PartitionId, message.Meta.Offset, Encoding.UTF8.GetString(message.Value));               
                   Console.WriteLine(Encoding.UTF8.GetString(message.Value));

            }
            Console.ReadLine();
        }
       
    }
}
