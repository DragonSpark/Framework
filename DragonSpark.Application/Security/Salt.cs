using DragonSpark.Model.Sequences.Memory;
using System.Security.Cryptography;

namespace DragonSpark.Application.Security;

/// <summary>
/// Attribution: https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
/// </summary>
sealed class Salt : ILease<uint, byte>
{
	public static Salt Default { get; } = new();

	Salt() : this(NewLeasing<byte>.Default) {}

	readonly INewLeasing<byte> _new;

	public Salt(INewLeasing<byte> @new) => _new = @new;

	public Leasing<byte> Get(uint parameter)
	{
		var       result = _new.Get(parameter);
		using var random = RandomNumberGenerator.Create();
		random.GetBytes(result.AsSpan());
		return result;
	}
}