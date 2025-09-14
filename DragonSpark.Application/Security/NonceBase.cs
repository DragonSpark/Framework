using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;
using System;

namespace DragonSpark.Application.Security;

public class NonceBase : IFormatter<uint>
{
	readonly ILease<uint, byte>               _salt;
	readonly Func<ReadOnlySpan<byte>, string> _text;

	public NonceBase(Func<ReadOnlySpan<byte>, string> text) : this(Salt.Default, text) {}

	public NonceBase(ILease<uint, byte> salt, Func<ReadOnlySpan<byte>, string> text)
	{
		_salt = salt;
		_text = text;
	}

	public string Get(uint parameter)
	{
		using var salt = _salt.Get(parameter);
		return _text(salt.AsSpan());
	}
}