using System;

namespace DragonSpark.Application.Presentation.Entity
{
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = true )]
	public sealed class EntityFieldViewAttribute : Attribute
	{
		public EntityFieldViewAttribute() : this ( null )
		{}

		public EntityFieldViewAttribute( string viewName )
		{
			ViewName = viewName;
			IsViewable = true;
			IsEditable = true;
		}

		public string ViewName { get; private set; }
		public string AuthorizedRoles { get; set; }
		
		public bool IsViewable { get; set; }

		public bool IsEditable { get; set; }

		public string FieldName { get; set; }

		public Type ModelType { get; set; }

		public string ModelName { get; set; }
	}
}