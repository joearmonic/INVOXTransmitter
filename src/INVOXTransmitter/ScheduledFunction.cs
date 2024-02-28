using INVOXTransmitter.Application.ServiceEngine;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace INVOXTransmitter
{
    [ExcludeFromCodeCoverage]
    public class ScheduledFunction
    {
        private readonly ITransmitterEngine _transmitter;
        private readonly ILogger<ScheduledFunction> _logger;

        public ScheduledFunction(ITransmitterEngine transmitter, ILogger<ScheduledFunction> logger)
        {
            _transmitter = transmitter;
            _logger = logger;
        }


        [FunctionName("INVOXTransmitterFunction")]
        public async Task Run(
#if DEBUG
        [TimerTrigger("0 */5 * * * *", RunOnStartup =true)]
#else
         [/* for 21:00 every day use: "0 0 21 * * *")*/ TimerTrigger("0 0 21 * * *"))]
#endif
        TimerInfo timerInfo)
        {
            _logger.LogInformation("C# Timer trigger function processing.");

            await _transmitter.RunAsync();
        }
    }
}
