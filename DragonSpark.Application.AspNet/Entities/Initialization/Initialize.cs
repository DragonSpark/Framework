using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public sealed class Initialize : IStopAware<InitializeInput>
{
	readonly ISelect<IServiceProvider, Assignments> _assignments;
	readonly ILogger<Initialize>                    _logger;
	readonly IInitialize[]                          _initializers;

	public Initialize(ILogger<Initialize> logger, IEnumerable<IInitialize> initializers)
		: this(InitializationAssignments.Default, logger, initializers.Open()) {}

	public Initialize(ISelect<IServiceProvider, Assignments> assignments, ILogger<Initialize> logger,
	                  params IInitialize[] initializers)
	{
		_assignments  = assignments;
		_logger       = logger;
		_initializers = initializers;
	}

	public async ValueTask Get(Stop<InitializeInput> parameter)
	{
		var ((services, subject), stop) = parameter;
		
		using var _ = _assignments.Get(services);

		foreach (var initializer in _initializers.Open())
		{
			try
			{
				await initializer.Off(new(subject, stop));
			}
			catch (Exception e)
			{
				_logger.LogError(e, "A problem was encountered while running storage initializations");
				throw;
			}
		}
	}
}