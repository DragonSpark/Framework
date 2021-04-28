using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests
{
	public readonly struct Query<T>
	{
		public Query(ControllerBase owner, T subject)
		{
			Owner   = owner;
			Subject = subject;
		}

		public ControllerBase Owner { get; }

		public T Subject { get; }

		public void Deconstruct(out ControllerBase owner, out T subject)
		{
			owner   = Owner;
			subject = Subject;
		}
	}
}