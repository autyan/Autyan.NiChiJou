using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface ICreateTrace<T>
    {
        DateTimeOffset? CreatedAt { get; set; }

        T CreatedBy { get; set; }
    }
}
