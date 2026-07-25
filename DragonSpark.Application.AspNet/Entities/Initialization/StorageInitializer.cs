using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class StorageInitializer<T> : IHostInitializer where T : DbContext
{
    readonly ISelect<IServiceProvider, Assignments> _assignments;
    readonly ILogger<StorageInitializer<T>>         _logger;
    readonly Array<IInitialize>                     _initializers;

    public StorageInitializer(ILogger<StorageInitializer<T>> logger, IEnumerable<IInitialize> initializers)
        : this(InitializationAssignments.Default, logger, initializers.Open()) {}

    public StorageInitializer(ISelect<IServiceProvider, Assignments> assignments, ILogger<StorageInitializer<T>> logger,
                              params IInitialize[] initializers)
    {
        _assignments  = assignments;
        _logger       = logger;
        _initializers = initializers;
    }

    public async Task Get(IHost parameter)
    {
        await using var context = await parameter.Services.GetRequiredService<IDbContextFactory<T>>()
                                                 .CreateDbContextAsync()
                                                 .Off();
        using var _ = _assignments.Get(parameter.Services);
        var stop = parameter.Services.GetService<IHttpContextAccessor>()?.HttpContext?.RequestAborted ??
                   CancellationToken.None;

        foreach (var initializer in _initializers.Open())
        {
            try
            {
                await initializer.Off(new(context, stop));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A problem was encountered while running storage initializations");
                throw;
            }
        }
    }
}