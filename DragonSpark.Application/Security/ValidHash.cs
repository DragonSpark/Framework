using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace DragonSpark.Application.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
public sealed class ValidHash : ICondition<HashInput>
{
	public static ValidHash Default { get; } = new();

	ValidHash() : this(16_384, 16) {}

	readonly ushort _iterations;
	readonly byte   _size;

	public ValidHash(ushort iterations, byte size)
	{
		_iterations = iterations;
		_size       = size;
	}

	public bool Get(HashInput parameter)
	{
		var (hash, input) = parameter;
		var parts  = hash.Split(':');
		var salt   = Convert.FromBase64String(parts[0]);
		var bytes  = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, _iterations, _size);
		var result = parts[1].Equals(Convert.ToBase64String(bytes));
		return result;
	}
}