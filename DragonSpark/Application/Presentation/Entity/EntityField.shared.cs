using System;

namespace DragonSpark.Application.Presentation.Entity
{
	public struct EntityField : IEquatable<EntityField>
	{
		readonly Type entityType;
		readonly string viewName;
		readonly string fieldName;

		public EntityField( Type entityType, string viewName, string fieldName )
		{
			this.entityType = entityType;
			this.viewName = viewName;
			this.fieldName = fieldName;
		}

		public Type EntityType
		{
			get { return entityType; }
		}

		public string ViewName
		{
			get { return viewName; }
		}

		public string FieldName
		{
			get { return fieldName; }
		}

		public override bool Equals(object obj)
		{
			return !ReferenceEquals( null, obj ) && ( obj.GetType() == typeof(EntityField) && Equals( (EntityField)obj ) );
		}

		public bool Equals( EntityField other )
		{
			return Equals( other.entityType, entityType ) && Equals( other.viewName, viewName ) && Equals( other.fieldName, fieldName );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var result = ( entityType != null ? entityType.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( viewName != null ? viewName.GetHashCode() : 0 );
				result = ( result * 397 ) ^ ( fieldName != null ? fieldName.GetHashCode() : 0 );
				return result;
			}
		}

		public static bool operator ==( EntityField left, EntityField right )
		{
			return left.Equals( right );
		}

		public static bool operator !=( EntityField left, EntityField right )
		{
			return !left.Equals( right );
		}
	}
}