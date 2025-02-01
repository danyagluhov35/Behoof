using Behoof.Domain.Entity.Context;

namespace Behoof.Domain.Parsing2
{
    public class CityLinkSupplier : SupplierParsing
    {
        public CityLinkSupplier(SupplierSetting setting, ApplicationContext db) : base(setting, db) { }
    }
}
