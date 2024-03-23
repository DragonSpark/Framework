using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.Entities.Diagnostics;

public sealed class ModelStatistics : ICommand<IModel>
{
	[UsedImplicitly]
	public static ModelStatistics Default { get; } = new();

	ModelStatistics() : this(DefaultInitializeLog<ModelStatistics>.Default.Get()) {}

	readonly ILogger _logger;

	public ModelStatistics(ILogger logger) => _logger = logger;

	public void Execute(IModel parameter)
	{

		var properties    = parameter.GetEntityTypes().SelectMany(e => e.GetDeclaredProperties()).Count();
		var relationships = parameter.GetEntityTypes().SelectMany(e => e.GetDeclaredForeignKeys()).Count();

		_logger.LogInformation("Model has:");
		_logger.LogInformation("  {Count} entity types", parameter.GetEntityTypes().Count());
		_logger.LogInformation("  {Properties} properties", properties);
		_logger.LogInformation("  {Relationships} relationships", relationships);
		_logger.LogInformation("  {Total} total compilation targets", properties + relationships);
	}
}