using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class New<TFrom, TTo> : DestinationBase<TFrom, TTo> where TFrom : class where TTo : class
{
	public New(IMap map) : base(Activate<TFrom, TTo>.Default, map) {}
}

public sealed class Activate<TFrom, TTo> : IInstance<TFrom, TTo> where TFrom : class
{
	public static Activate<TFrom, TTo> Default { get; } = new();

	Activate() : this(A.New<TTo>) {}

	readonly Func<TTo> _new;

	public Activate(Func<TTo> @new) => _new = @new;

	public ValueTask<TTo> Get(Stop<MappingInput<TFrom>> parameter) => _new().ToOperation();
}