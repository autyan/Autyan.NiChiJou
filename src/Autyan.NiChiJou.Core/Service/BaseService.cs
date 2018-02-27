using Microsoft.Extensions.Logging;

namespace Autyan.NiChiJou.Core.Service
{
    public abstract class BaseService
    {
        protected ILogger Logger { get; set; }

        protected BaseService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().FullName);
        }
    }
}
