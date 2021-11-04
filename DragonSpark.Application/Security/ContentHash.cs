using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public sealed class ContentHash : HashBase
{
	public static ContentHash Default { get; } = new();

	ContentHash() : base(MD5.Create, Encoding.ASCII) {}
}