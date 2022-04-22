using DragonSpark.Model.Selection.Conditions;
using System.Net;

namespace DragonSpark.Server.Requests.Warmup;

sealed class IsInternalAddress : ICondition<IPAddress>
{
	public static IsInternalAddress Default { get; } = new();

	IsInternalAddress() : this(10) { }

	readonly byte _expected;

	public IsInternalAddress(byte expected) => _expected = expected;

	public bool Get(IPAddress parameter) => parameter.GetAddressBytes()[0] == _expected;
}