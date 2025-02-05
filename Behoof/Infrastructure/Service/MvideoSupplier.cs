using Behoof.Core.Entities;
using Behoof.Core.Services;
using Behoof.Infrastructure.Data;

namespace Behoof.Infrastructure.Service
{
    public class MvideoSupplier : SupplierParsing
    {
        public MvideoSupplier(SupplierSetting setting, ApplicationContext db) : base(setting, db) { }
    }
}
