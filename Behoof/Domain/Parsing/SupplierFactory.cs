using Behoof.Domain.Entity;
using Behoof.Domain.Entity.Context;
using Behoof.Domain.Parsing.CityLink;
using Behoof.Domain.Parsing.Mvideo;
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
                "CityLink" => new CityLinkSupplier(Config.GetSection("CityLinkSettings").Get<SupplierSetting>()!, db),
                "Mvideo" => new MvideoSupplier(Config.GetSection("MvideoSettings").Get<SupplierSetting>()!, db),
            };
        }
    }
}
