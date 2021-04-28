using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server.Requests
{
	public readonly struct View
	{
		public static implicit operator Query(View instance) => new Query(instance.Owner, instance.Subject);

		public View(Controller owner, Guid subject)
		{
			Owner   = owner;
			Subject = subject;
		}

		public Controller Owner { get; }

		public Guid Subject { get; }

		public void Deconstruct(out Controller owner, out Guid subject)
		{
			owner   = Owner;
			subject = Subject;
		}
		
	}

	public readonly struct View<T>
	{
		public View(Controller owner, T subject)
		{
			Owner   = owner;
			Subject = subject;
		}

		public Controller Owner { get; }

		public T Subject { get; }

		public void Deconstruct(out Controller owner, out T subject)
		{
			owner   = Owner;
			subject = Subject;
		}
	}

}