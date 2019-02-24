using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Owin;
using Microsoft.Owin;
using Hangfire.Common;

[assembly: OwinStartup(typeof(MessageSender2.Startup))]
namespace MessageSender2
{
    public  partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("Data Source=SEVVALTEKKOL;Database=Project;User ID=sa;Password=sapass");
            app.UseHangfireDashboard();
            BackgroundJob.Schedule<SchedulerProject.Scheduler>(x => x.SendUnsentMessages(), TimeSpan.FromSeconds(15));
            app.UseHangfireServer();
        }
    }
}