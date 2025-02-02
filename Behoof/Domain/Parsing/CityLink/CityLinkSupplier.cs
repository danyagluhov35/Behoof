using Behoof.Domain.Entity.Context;
using Behoof.Domain.Parsing2;

namespace Behoof.Domain.Parsing.CityLink
{
    public class CityLinkSupplier : SupplierParsing
    {
        public CityLinkSupplier(SupplierSetting setting, ApplicationContext db) : base(setting, db) { }
    }
}
