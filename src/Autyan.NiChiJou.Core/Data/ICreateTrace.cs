using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface ICreateTrace<T>
    {
        DateTime? CreatedAt { get; set; }

        T CreatedBy { get; set; }
    }
}
