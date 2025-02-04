
using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.Domain.Parsing2;

namespace Behoof.Domain.Parsing
{
    public class ParsingSitesBackgroundService : BackgroundService
    {
        private readonly SemaphoreSlim _Semaphore;
        private readonly IServiceProvider ServiceProvider;
        private IConfiguration Configuration;
        public ParsingSitesBackgroundService(IConfiguration configuration, IServiceProvider provider)
        {
            _Semaphore = new SemaphoreSlim(1,1);
            ServiceProvider = provider;
            Configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if(DateTime.Now.Hour >= 1 &&  DateTime.Now.Hour < 2)
                {
                    await _Semaphore.WaitAsync(stoppingToken);
                    using(var scope = ServiceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                        var factory = new SupplierFactory(Configuration, db);

                        SupplierParsing[] suppliers =
                        {
                            factory.CreateSupplier("CityLink"),
                            factory.CreateSupplier("Mvideo")
                        };

                        foreach(var item in suppliers)
                        {
                            await Task.Run(async() =>
                            {
                                await item.LoadPage();
                                await item.SaveOnDb();
                                await item.Update();
                            });
                        }
                    }
                    _Semaphore.Release();
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
