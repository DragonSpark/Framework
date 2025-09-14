using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;
using System;
using System.Security.Cryptography;

namespace DragonSpark.Application.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
sealed class Salt : ILease<uint, byte>
{
	public static Salt Default { get; } = new();

	Salt() : this(NewLeasing<byte>.Default) {}

	readonly INewLeasing<byte> _new;

	public Salt(INewLeasing<byte> @new) => _new = @new;

	public Leasing<byte> Get(uint parameter)
	{
		var       result = _new.Get(parameter);
		using var random = RandomNumberGenerator.Create();
		random.GetBytes(result.AsSpan());
		return result;
	}
}

// TODO

public sealed class Base64Nonce : NonceBase
{
	public static Base64Nonce Default { get; } = new();

	Base64Nonce() : base(x => Convert.ToBase64String(x)) {}
}

public sealed class HexNonce : NonceBase
{
	public static HexNonce Default { get; } = new();

	HexNonce() : base(Convert.ToHexString) {}
}

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