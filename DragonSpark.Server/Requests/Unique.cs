using System;

namespace DragonSpark.Server.Requests
{
	public readonly struct Unique<T>
	{
		public Unique(string userName, Guid identity, T subject)
		{
			UserName = userName;
			Identity = identity;
			Subject  = subject;
		}

		public string UserName { get; }

		public Guid Identity { get; }
		public T Subject { get; }

		public void Deconstruct(out string userName, out Guid identity, out T subject)
		{
			userName = UserName;
			identity = Identity;
			subject  = Subject;
		}
	}

	public readonly struct Unique
	{
		public Unique(string userName, Guid identity)
		{
			UserName = userName;
			Identity = identity;
		}

		public string UserName { get; }

		public Guid Identity { get; }

		public void Deconstruct(out string userName, out Guid identity)
		{
			userName = UserName;
			identity = Identity;
		}
	}
}