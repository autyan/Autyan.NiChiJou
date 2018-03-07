using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface ISoftdelete
    {
        DateTime? DeletedAt { get; set; }
    }
}
