using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

sealed class LogicalContext : Logical<DbContext>
{
	public static LogicalContext Default { get; } = new();

	LogicalContext() {}
}