using System;

namespace DragonSpark.Application.Security.Identity.Claims;

public readonly struct Read<T>
{
	readonly T _value;
	public static Read<T> Default { get; } = new(false, default!);

	public Read(T value) : this(true, value) {}

	public Read(bool exists, T value)
	{
		_value = value;
		Exists = exists;
	}

	public bool Exists { get; }

	public T Value
		=> Exists
			   ? _value
			   : throw new InvalidOperationException("Attempted access to claim value that does not exist");
}