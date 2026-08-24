using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class SmartGuidGenerator : ValueGenerator<Guid>
{
	public static SmartGuidGenerator Default { get; } = new();

	SmartGuidGenerator() {}
	
	public override bool GeneratesTemporaryValues => false;

	public override Guid Next(EntityEntry entry)
	{
		var value = entry.Property("Id").CurrentValue;
		return value is Guid identity && identity != Guid.Empty ? identity : Guid.NewGuid();
	}
}