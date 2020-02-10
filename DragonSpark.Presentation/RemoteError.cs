namespace DragonSpark.Presentation
{
	public sealed class RemoteError : Text.Text
	{
		public static RemoteError Default { get; } = new RemoteError();

		RemoteError() : base("remoteError") {}
	}
}