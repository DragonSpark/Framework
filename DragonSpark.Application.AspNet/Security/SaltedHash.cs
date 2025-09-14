using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Application.AspNet.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
public sealed class SaltedHash : IAlteration<string>
{
	public static SaltedHash Default { get; } = new();

	SaltedHash() : this(Salt.Default.Then().Bind(16u), 16_384, DataAsText.Default.Get) {}

	readonly Func<Leasing<byte>>  _salt;
	readonly ushort               _iterations;
	readonly Func<byte[], string> _text;

	public SaltedHash(Func<Leasing<byte>> salt, ushort iterations, Func<byte[], string> text)
	{
		_salt       = salt;
		_iterations = iterations;
		_text       = text;
	}

	public string Get(string parameter)
	{
		using var salt = _salt();

		var data = salt.ToArray();
		var bytes = KeyDerivation.Pbkdf2(parameter, data, KeyDerivationPrf.HMACSHA512, _iterations,
		                                 salt.Length.Degrade());

		var result = $"{_text(data)}:{_text(bytes)}";
		return result;
	}
}

// TODO

sealed class HttpContextNonce : ReferenceValueStore<HttpContext, string>
{
	public static HttpContextNonce Default { get; } = new();

	HttpContextNonce() : base(_ => ContentPolicyNonce.Default.Get()) {}
}

sealed class ContentPolicyNonce : Result<string>
{
	public static ContentPolicyNonce Default { get; } = new();

	ContentPolicyNonce() : base(HexNonce.Default.Then().Bind(16)) {}
}