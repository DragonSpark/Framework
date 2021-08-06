namespace DragonSpark.Application.Security.Identity.Authentication
{
	public sealed class StateView<T> where T : class
	{
		public static StateView<T> Default { get; } = new StateView<T>();

		StateView() : this(AuthenticationState<T>.Default, null) {}

		public StateView(AuthenticationState<T> state, string? hash)
		{
			State = state;
			Hash  = hash;
		}

		public AuthenticationState<T> State { get; }

		public string? Hash { get; }

		public void Deconstruct(out AuthenticationState<T> state, out string? hash)
		{
			state = State;
			hash  = Hash;
		}
	}
}