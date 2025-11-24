using System.Diagnostics;
using Microsoft.Extensions.Logging;

// 💡 Nota: Substitua 'IRequestHandler' pela interface real do seu novo mecanismo de comandos/queries.
// Se você está usando um serviço simples, ajuste as assinaturas.

public class LoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;
    private readonly IRequestHandler<TRequest, TResponse> _decoratedHandler;

    // Injeção do Logger e do Handler real
    public LoggingDecorator(
        ILogger<LoggingDecorator<TRequest, TResponse>> logger,
        IRequestHandler<TRequest, TResponse> decoratedHandler)
    {
        _logger = logger;
        _decoratedHandler = decoratedHandler;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        var watch = Stopwatch.StartNew();

        _logger.LogDebug("Handling Request [{requestName}] with payload: {@request}", requestName, request);

        try
        {
            // ⭐️ A chamada real ao handler/serviço original
            var response = await _decoratedHandler.Handle(request, cancellationToken);

            _logger.LogDebug("Handled Request [{requestName}] with Response [{responseName}]", requestName, responseName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error on {requestName}", requestName);
            throw;
        }
        finally
        {
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            _logger.LogInformation("Request [{requestName}] elapsed {elapsedMs}ms", requestName, elapsedMs);
        }
    }
}