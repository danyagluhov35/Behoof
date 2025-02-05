using Behoof.Core.Entities;
using Behoof.Core.Services;
using Behoof.Infrastructure.Data;

namespace Behoof.Infrastructure.Service
{
    public class CityLinkSupplier : SupplierParsing
    {
        public CityLinkSupplier(SupplierSetting setting, ApplicationContext db) : base(setting, db) { }
    }
}
