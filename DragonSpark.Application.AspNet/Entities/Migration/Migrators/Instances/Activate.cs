using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;

public sealed class Activate<TFrom, TTo> : IInstance<TFrom, TTo> where TFrom : class
{
	public static Activate<TFrom, TTo> Default { get; } = new();

	Activate() : this(A.New<TTo>) {}

	readonly Func<TTo> _new;

	public Activate(Func<TTo> @new) => _new = @new;

	public ValueTask<TTo> Get(Stop<MappingInput<TFrom>> parameter) => _new().ToOperation();
}