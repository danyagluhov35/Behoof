using Behoof.Domain.Entity.Context;
using Behoof.Domain.Parsing2;

namespace Behoof.Domain.Parsing.Mvideo
{
    public class MvideoSupplier : SupplierParsing
    {
        public MvideoSupplier(SupplierSetting setting, ApplicationContext db) : base(setting, db) { }
    }
}
