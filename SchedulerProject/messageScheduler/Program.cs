using System;
using System.Collections.Generic;
using System.Linq;
    
using System.Threading.Tasks;
using messageScheduler;
using Hangfire;


namespace messageScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
           // RecurringJob.AddOrUpdate(() => Console.Write("Easy!"), Cron.MinuteInterval);
           // SchedulerProject.Scheduler schh = new SchedulerProject.Scheduler();

         //   RecurringJob.AddOrUpdate(() => schh, Cron.MinuteInterval);
            using (var server = new BackgroundJobServer())
            {
                BackgroundJob.Schedule<SchedulerProject.Scheduler>(x => x.SendUnsentMessages(), TimeSpan.FromSeconds(15));
            }

            Console.ReadLine();


        }
    }
}
