using System.Collections.Generic;
using DragonSpark.Application.Communication.Entity;

namespace DragonSpark.Application.Presentation.Entity
{
	public interface IEntityView
	{
		IEntitySetProfile EntitySet { get; }

		string ViewName { get; }

		IEnumerable<IEntityViewField> Fields { get; }
	}
}