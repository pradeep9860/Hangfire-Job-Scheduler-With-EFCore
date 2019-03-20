using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Hangfire.Helper.Scheduler
{
    public interface IJob
    {
        void InitializeAllJobs();
    }
}
