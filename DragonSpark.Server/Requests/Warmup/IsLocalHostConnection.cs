using System.Net;

namespace DragonSpark.Server.Requests.Warmup;

sealed class IsLocalHostConnection : Model.Selection.Conditions.Condition<IPAddress>
{
	public static IsLocalHostConnection Default { get; } = new();

	IsLocalHostConnection() : base(IPAddress.IsLoopback) { }
}