// ReSharper disable TemplateIsNotCompileTimeConstantProblem
using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DragonSpark.Application.Entities.Diagnostics;

sealed class ModelStatistics : ICommand<IModel>
{
	public static ModelStatistics Default { get; } = new();

	ModelStatistics() : this(DefaultInitializeLog<ModelStatistics>.Default.Get()) {}

	readonly ILogger _logger;

	public ModelStatistics(ILogger logger) => _logger = logger;

	public void Execute(IModel parameter)
	{

		var properties    = parameter.GetEntityTypes().SelectMany(e => e.GetDeclaredProperties()).Count();
		var relationships = parameter.GetEntityTypes().SelectMany(e => e.GetDeclaredForeignKeys()).Count();

		_logger.LogInformation("Model has:");
		_logger.LogInformation($"  {parameter.GetEntityTypes().Count()} entity types");
		_logger.LogInformation($"  {properties} properties");
		_logger.LogInformation($"  {relationships} relationships");
		_logger.LogInformation($"  {properties + relationships} total compilation targets");
	}
}