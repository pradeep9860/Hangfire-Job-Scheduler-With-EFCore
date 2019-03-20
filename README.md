# Hangfire Job Scheduler With EFCore

## Installation
There are a couple of packages for Hangfire available on NuGet. To install Hangfire into your ASP.NET application with SQL Server storage, type the following command into the Package Manager Console window:

    PM> Install-Package Hangfire
    
 ## Configuration
 
 On Configuration method in Startup.cs
 
    using Hangfire;

      // ...

      public void Configuration(IAppBuilder app)
      {
          GlobalConfiguration.Configuration.UseSqlServerStorage("<connection string or its name>");

          app.UseHangfireDashboard();
          app.UseHangfireServer();
      }
      
On configure service method of Startup.cs

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
          string connectionString = Configuration["ConnectionString:Default"];  // read connection string form appsetting.json
          
          //add hangfire to project with sqlserver storage -- NOTE: you also can use other type of storage like InMemory Storage, MySql or Postgress
          services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString);
            });
          
          //set current job storage 
          JobStorage.Current = new SqlServerStorage(connectionString);
        }
        
## Add Application.Hangfire.Helper Project to the solution and add reference to Host Project

### Register Job.cs for DI
Register your IJob.cs with Job.cs implementation in ConfigurreService pipeline in startup.cs for further injection in program

    services.AddTransient<IJob, Job>();
    
### Register All Jobs
After adding project in the solution you need to register all jobs using Bootstraper.cs static class form your ConfigureServices method in startup.cs like 
    
    Bootstraper.InitializeJobs(); 
    
## All types of Jobs Descirption

     //#Fire-and-forget
                //This is the main background job type, persistent message queues are used to handle it. Once you create a fire-and-forget job, it is saved to its queue ("default" by default, but multiple queues supported). The queue is listened by a couple of dedicated workers that fetch a job and perform it.
            BackgroundJob.Enqueue(() => FireAndForgetHelper());

            //#Delayed
            //If you want to delay the method invocation for a certain type, call the following method. After the given delay the job will be put to its queue and invoked as a regular fire-and-forget job.
            BackgroundJob.Schedule(() => FireAndForgetHelper(), TimeSpan.FromSeconds(1));

            //#Recurring
            //To call a method on a recurrent basis (hourly, daily, etc), use the RecurringJob class. You are able to specify the schedule using CRON expressions to handle more complex scenarios.
            //RecurringJob.AddOrUpdate(() => sendMail(), Cron.Minutely);

            RecurringJob.AddOrUpdate(() => RecurringHelper(), Cron.Minutely);


            //#Continuations
            //Continuations allow you to define complex workflows by chaining multiple background jobs together.
            var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));


### Public methods invokes by the Jobs

         public void FireAndForgetHelper()
        {
            Console.WriteLine("Fire-and-forget"); //add your business logic here

        } 

        public void DelayedHelper()
        {
            Console.WriteLine("Delayed");  //add your business logic here
        }

        public void RecurringHelper()
        {
            Console.WriteLine("Recurring");  //add your business logic here
        }
