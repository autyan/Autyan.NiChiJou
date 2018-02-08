using System;

namespace Autyan.NiChiJou.Core.Data
{
    public interface ISoftdelete
    {
        DateTimeOffset? DeletedAt { get; set; }
    }
}
