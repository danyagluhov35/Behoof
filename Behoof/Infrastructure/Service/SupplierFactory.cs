using Microsoft.Extensions.Configuration;
using Behoof.Infrastructure.Data;
using Behoof.Core.Entities;

namespace Behoof.Infrastructure.Service
{
    public class SupplierFactory
    {
        private readonly IConfiguration Config;
        private ApplicationContext db;
        public SupplierFactory(IConfiguration configuration, ApplicationContext db)
        {
            Config = configuration;
            this.db = db;
        }
        public SupplierParsing CreateSupplier(string name)
        {
            return name switch
            {
                "CityLink" => new CityLinkSupplier(Config.GetSection("CityLinkSettings").Get<SupplierSetting>()!, db),
                "Mvideo" => new MvideoSupplier(Config.GetSection("MvideoSettings").Get<SupplierSetting>()!, db),
            };
        }
    }
}
