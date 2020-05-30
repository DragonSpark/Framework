namespace DragonSpark.Composition.Compose
{
	sealed class LinkedRegistrationContext : IRegistration
	{
		readonly IRegistration _next, _previous;

		public LinkedRegistrationContext(IRegistration previous, IRegistration next)
		{
			_previous = previous;
			_next     = next;
		}

		public RegistrationResult Singleton()
		{
			_previous.Singleton();
			var result = _next.Singleton();
			return result;
		}

		public RegistrationResult Transient()
		{
			_previous.Transient();
			var result = _next.Transient();
			return result;
		}

		public RegistrationResult Scoped()
		{
			_previous.Scoped();
			var result = _next.Scoped();
			return result;
		}
	}
}