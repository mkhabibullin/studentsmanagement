using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WorkerService1
{
    public class WorkerClient2 : WorkerClient
    {
        public WorkerClient2(ILogger<WorkerClient2> logger, IConfiguration config) : base(logger, config)
        {

        }
    }
}
