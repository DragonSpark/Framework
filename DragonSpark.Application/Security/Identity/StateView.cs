namespace DragonSpark.Application.Security.Identity
{
	public sealed class StateView<T> where T : class
	{
		public static StateView<T> Default { get; } = new StateView<T>();

		StateView() : this(AuthenticationState<T>.Default, null) {}

		public StateView(AuthenticationState<T> state, string hash)
		{
			State = state;
			Hash  = hash;
		}

		public AuthenticationState<T> State { get; }

		public string Hash { get; }
	}
}