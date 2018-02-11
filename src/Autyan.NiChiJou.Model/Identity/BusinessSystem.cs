using Autyan.NiChiJou.Core.Data;

namespace Autyan.NiChiJou.Model.Identity
{
    public class BusinessSystem : LongKeyBaseEntity
    {
        public string Code { get; set; }

        public string MainDomain { get; set; }
    }
}
