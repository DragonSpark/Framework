using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Communication.Entity
{
	public interface IEntitySetProfile : IAuthorizable
	{
		bool IsRoot { get;  }
		Type EntityType { get; }

		string Title { get; }
		string ItemName { get; }
		string ItemNamePlural { get; }
		string DisplayNamePath { get; }

		IEnumerable<IEntitySetOperationProfile> Operations { get; }

		int Order { get; }
	}
}