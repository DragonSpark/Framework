using System.Security.Cryptography;

namespace DragonSpark.Server.Security;

public sealed class HmacSha256Hasher : Hasher
{
	public HmacSha256Hasher(string key) : base(k => new HMACSHA256(k), key) {}
}