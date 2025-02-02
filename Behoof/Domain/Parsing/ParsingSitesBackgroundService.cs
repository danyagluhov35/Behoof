
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
                if(DateTime.Now.Hour >= 0 &&  DateTime.Now.Hour < 15)
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

                        var tasks = suppliers.Select(async item =>
                        {
                            await item.LoadPage();
                            await item.SaveOnDb();
                            await item.Update();
                        });
                        await Task.WhenAll(tasks);
                    }
                    _Semaphore.Release();
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
