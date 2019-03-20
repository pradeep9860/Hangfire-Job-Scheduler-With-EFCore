using Application.Hangfire.Helper.Scheduler;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Hangfire.Helper
{
    public static class Bootstraper
    {
        public static void InitializeJobs()
        {




            IJob jobs = new Job();
            jobs.InitializeAllJobs();
        }
    }
}
