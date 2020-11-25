using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WorkerService1
{
    public class WorkerClient3 : WorkerClient
    {
        public WorkerClient3(ILogger<WorkerClient2> logger, IConfiguration config) : base(logger, config)
        {

        }
    }
}
