using DragonSpark.Compose;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public sealed class Mapped : IMapped
{
	public static Mapped Default { get; } = new();

	Mapped() : this(Map.Default) {}

	readonly Func<Type, object> _new;
	readonly IMap               _map;

	public Mapped(IMap map) : this(A.New, map) {}

	public Mapped(Func<Type, object> @new, IMap map)
	{
		_new = @new;
		_map = map;
	}

	public object Get(MappingInput parameter)
	{
		var (source, destination, from, to) = parameter;
		var result = _new(to);
		_map.Execute(new(source.Entry(from), destination.Entry(result)));
		return result;
	}
}