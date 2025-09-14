using System;

namespace DragonSpark.Application.Security;

public sealed class Base64Nonce : NonceBase
{
	public static Base64Nonce Default { get; } = new();

	Base64Nonce() : base(x => Convert.ToBase64String(x)) {}
}