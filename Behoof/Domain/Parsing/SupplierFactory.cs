using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Microsoft.Extensions.Configuration;

namespace Behoof.Domain.Parsing2
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
                "CityLink" => new CityLinkSupplier(Config.GetSection("CityLinkSettings").Get<SupplierSetting>()!, db)
            };
        }
    }
}
