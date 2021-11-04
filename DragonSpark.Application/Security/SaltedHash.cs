using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace DragonSpark.Application.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
public sealed class SaltedHash : IAlteration<string>
{
	public static SaltedHash Default { get; } = new();

	SaltedHash() : this(Salt.Default.Then().Subject.Bind(16u), 16_384, DataAsText.Default.Get) {}

	readonly Func<Array<byte>>    _salt;
	readonly ushort               _iterations;
	readonly Func<byte[], string> _text;

	public SaltedHash(Func<Array<byte>> salt, ushort iterations, Func<byte[], string> text)
	{
		_salt       = salt;
		_iterations = iterations;
		_text       = text;
	}

	public string Get(string parameter)
	{
		var salt = _salt();

		var bytes = KeyDerivation.Pbkdf2(parameter, salt, KeyDerivationPrf.HMACSHA512, _iterations,
		                                 salt.Length.Degrade());

		var result = $"{_text(salt)}:{_text(bytes)}";
		return result;
	}
}