using DragonSpark.Model.Sequences;
using System.Security.Cryptography;

namespace DragonSpark.Application.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
sealed class Salt : IArray<uint, byte>
{
	public static Salt Default { get; } = new();

	Salt() {}

	public Array<byte> Get(uint parameter)
	{
		var       result = new byte[parameter];
		using var random = RandomNumberGenerator.Create();
		random.GetBytes(result);
		return result;
	}
}