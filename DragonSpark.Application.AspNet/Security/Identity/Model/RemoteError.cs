namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public sealed class RemoteError : Text.Text
{
	public static RemoteError Default { get; } = new();

	RemoteError() : base("remoteError") {}
}