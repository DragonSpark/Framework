using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace DragonSpark.Application.Security
{
	/// <summary>
	/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
	/// </summary>
	public sealed class Hash : IAlteration<string>
	{
		public static Hash Default { get; } = new Hash();

		Hash() : this(Salt.Default.Then().Subject.Bind(16u), 16_384) {}

		readonly Func<Array<byte>> _salt;
		readonly ushort            _iterations;

		public Hash(Func<Array<byte>> salt, ushort iterations)
		{
			_salt       = salt;
			_iterations = iterations;
		}

		public string Get(string parameter)
		{
			var salt = _salt();

			var bytes = KeyDerivation.Pbkdf2(parameter, salt, KeyDerivationPrf.HMACSHA512, _iterations,
			                                 salt.Length.Degrade());

			var result = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(bytes)}";
			return result;
		}
	}
}