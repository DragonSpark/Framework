using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server.Requests
{
	public readonly struct Query
	{
		public Query(ControllerBase owner, Guid subject)
		{
			Owner   = owner;
			Subject = subject;
		}

		public ControllerBase Owner { get; }

		public Guid Subject { get; }

		public void Deconstruct(out ControllerBase owner, out Guid subject)
		{
			owner   = Owner;
			subject = Subject;
		}
	}


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