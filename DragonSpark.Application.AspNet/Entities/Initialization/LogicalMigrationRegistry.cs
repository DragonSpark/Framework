using DragonSpark.Runtime.Execution;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

sealed class LogicalMigrationRegistry : Logical<IDataMigrationRegistry>
{
	public static LogicalMigrationRegistry Default { get; } = new();

	LogicalMigrationRegistry() {}
}