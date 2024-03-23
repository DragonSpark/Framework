using JetBrains.Annotations;
using System.Security.Cryptography;

namespace DragonSpark.Server.Security;

[UsedImplicitly]
public sealed class HmacSha512Hasher : Hasher
{
	public HmacSha512Hasher(string key) : base(x => new HMACSHA512(x), key) {}
}