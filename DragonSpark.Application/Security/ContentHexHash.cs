using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public sealed class ContentHexHash : HexHashBase
{
	public static ContentHexHash Default { get; } = new();

	ContentHexHash() : base(MD5.Create, Encoding.ASCII) {}
}