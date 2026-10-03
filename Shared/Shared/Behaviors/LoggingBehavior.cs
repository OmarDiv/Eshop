using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace Shared.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Start]Handling {RequestName} - {ResponseName} with request: {@Request}", typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = Stopwatch.StartNew();

            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed; ;
            if (timeTaken.Seconds > 3)
                logger.LogWarning("Handling {RequestName} - {ResponseName} took {TimeTaken} ms", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);

            logger.LogWarning("Handling {RequestName} - {ResponseName} took {TimeTaken} ms", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);
            logger.LogInformation("[End]Handled {RequestName} - {ResponseName} with request: {@Request}", typeof(TRequest).Name, typeof(TResponse).Name, request);

            return response;
        }
    }
}
