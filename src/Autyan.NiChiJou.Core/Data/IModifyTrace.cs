using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface IModifyTrace<T>
    {
        DateTime? ModifiedAt { get; set; }

        T ModifiedBy { get; set; }
    }
}
