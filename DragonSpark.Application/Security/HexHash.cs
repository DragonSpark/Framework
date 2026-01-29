using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public sealed class HexHash : HexHashBase
{
	public static HexHash Default { get; } = new();

	HexHash() : base(SHA256.Create, Encoding.UTF8) {}
}