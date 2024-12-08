using DragonSpark.Runtime.Execution;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public sealed class LogicalContext : Logical<DbContext>
{
	public static LogicalContext Default { get; } = new();

	LogicalContext() {}
}