using System;

namespace DragonSpark.Application.Security.Identity.Claims.Access;

public readonly struct Claim<T>
{
	readonly T _value;
	public static Claim<T> Default { get; } = new(false, default!);

	public Claim(T value) : this(true, value) {}

	public Claim(bool exists, T value)
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