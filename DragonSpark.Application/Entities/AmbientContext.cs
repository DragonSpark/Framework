using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

sealed class AmbientContext : Result<DbContext?>, IAmbientContext
{
	public static AmbientContext Default { get; } = new();

	AmbientContext() : base(LogicalContext.Default) {}
}