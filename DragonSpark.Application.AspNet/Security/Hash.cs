using System.Security.Cryptography;
using System.Text;

namespace DragonSpark.Application.Security;

public sealed class Hash : HashBase
{
	public static Hash Default { get; } = new();

	Hash() : base(SHA256.Create, Encoding.UTF8) {}
}