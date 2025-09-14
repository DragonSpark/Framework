using System;

namespace DragonSpark.Application.Security;

public sealed class HexNonce : NonceBase
{
	public static HexNonce Default { get; } = new();

	HexNonce() : base(Convert.ToHexString) {}
}