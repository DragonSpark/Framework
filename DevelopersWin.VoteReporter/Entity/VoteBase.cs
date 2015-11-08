using DragonSpark.ComponentModel;
using System;

namespace DevelopersWin.VoteReporter.Entity
{
	public abstract class EntityBase
	{
		[NewGuid]
		public Guid Id { get; set; }

		[CurrentTime]
		public DateTimeOffset Created { get; set; }

		public DateTimeOffset? Modified { get; set; }
	}

	public abstract class VoteBase : EntityBase
	{
		public string Title { get; set; }

		public int Order { get; set; }
	}
}