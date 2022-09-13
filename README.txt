-----------------------------

1 - Instalar as dependencias do HangFire

Hangfire.Core
Hangfire.AspNetCore
Hangfire.SqlServer;

-----------------------------

2 - Adicionar as configurações no AppSettings

"Logging": {
    "LogLevel": {
      "Hangfire": "Information"
    }
  },
  "ConnectionStrings": {
    "HangFireConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Hangfire;Trusted_Connection=True;"
  }

-----------------------------

3 - Adicionar as configurações na Program.cs

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseFilter(new AutomaticRetryAttribute
        {
            Attempts = 3,
            DelaysInSeconds = new int[] { 5 }
        })
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

app.UseHangfireDashboard();

-----------------------------

4 - Caso a aplicação tenha rotas, adicionar na Program.cs

app.UseEndpoints(endpoints =>
{    
    endpoints.MapHangfireDashboard();
});