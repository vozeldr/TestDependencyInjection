using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ClassLibrary1
{
    public class AsyncMessageWriter
    {
        private readonly ILogger<AsyncMessageWriter> _logger;
        
        public AsyncMessageWriter(ILogger<AsyncMessageWriter> logger)
        {
            _logger = logger;
        }

        public Task<int> WriteMessage(string message)
        {
            _logger.LogInformation("WriteMessage called. Message: {MESSAGE}", message);

            return Task.FromResult(0);
        }
    }
}