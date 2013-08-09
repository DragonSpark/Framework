using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.Entity
{
	class EntityViewField : IEntityViewField
	{
		readonly string fieldName;
		readonly string[] authorizedRoles;
		readonly bool isViewable;
		readonly bool isEditable;
		readonly Type modelType;
		readonly string modelName;

		public EntityViewField( string fieldName, Type modelType, string modelName, string[] authorizedRoles, bool isViewable, bool isEditable )
		{
			this.fieldName = fieldName;
			this.authorizedRoles = authorizedRoles;
			this.isViewable = isViewable;
			this.isEditable = isEditable;
			this.modelType = modelType;
			this.modelName = modelName;
		}

		public bool IsEditable
		{
			get { return isEditable; }
		}

		public string FieldName
		{
			get { return fieldName; }
		}

		public IEnumerable<string> AuthorizedRoles
		{
			get { return authorizedRoles; }
		}

		public bool IsViewable
		{
			get { return isViewable; }
		}

		public Type ItemType
		{
			get { return modelType; }
		}

		public string ItemName
		{
			get { return modelName; }
		}
	}
}