using System.Collections.Generic;
using DragonSpark.Application.Communication.Entity;

namespace DragonSpark.Application.Presentation.Entity
{
	class EntityView : IEntityView
	{
		readonly IEntitySetProfile entitySet;
		readonly string viewName;
		readonly IEnumerable<IEntityViewField> fields;

		public EntityView( IEntitySetProfile entitySet, string viewName, IEnumerable<IEntityViewField> fields )
		{
			this.entitySet = entitySet;
			this.viewName = viewName;
			this.fields = fields;
		}

		public IEntitySetProfile EntitySet
		{
			get { return entitySet; }
		}

		public string ViewName
		{
			get { return viewName; }
		}

		public IEnumerable<IEntityViewField> Fields
		{
			get { return fields; }
		}
	}
}