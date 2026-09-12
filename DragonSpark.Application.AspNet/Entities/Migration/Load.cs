using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Load<T> : IAllocating<LoadInput<T>, T> where T : class
{
	public static Load<T> Default { get; } = new();

	Load() {}

	public Task<T> Get(Stop<LoadInput<T>> parameter)
	{
		var ((subject, include), stop) = parameter;

		var property = subject.Metadata.FindPrimaryKey()!.Properties.Single();
		var key      = subject.Property(property.Name).CurrentValue;

		return include(subject.Context.Set<T>())
			.SingleAsync(x => EF.Property<object>(x, property.Name).Equals(key), stop);
	}
}