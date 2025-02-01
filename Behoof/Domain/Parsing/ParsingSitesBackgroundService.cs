
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
                if(DateTime.Now.Hour >= 23 &&  DateTime.Now.Hour < 24)
                {
                    await _Semaphore.WaitAsync(stoppingToken);
                    using(var scope = ServiceProvider.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                        SupplierParsing[] suppliers =
                        {
                            new SupplierFactory(Configuration, db).CreateSupplier("CityLink")
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
