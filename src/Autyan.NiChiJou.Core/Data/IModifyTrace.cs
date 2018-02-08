using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface IModifyTrace<T>
    {
        DateTimeOffset? ModifiedAt { get; set; }

        T ModifiedBy { get; set; }
    }
}
