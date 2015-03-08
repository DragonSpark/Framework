
namespace System.ComponentModel.DataAnnotations
{
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false )]
	public sealed class MetadataTypeAttribute : Attribute
	{
		// Fields
		readonly Type metadataClassType;

		// Methods
		public MetadataTypeAttribute( Type metadataClassType )
		{
			this.metadataClassType = metadataClassType;
		}

		// Properties
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DataAnnotationsResources"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "MetadataTypeAttributeTypeCannotBeNull")]
		public Type MetadataClassType
		{
			get
			{
				if ( this.metadataClassType == null )
				{
					throw new InvalidOperationException( "DataAnnotationsResources.MetadataTypeAttribute_TypeCannotBeNull" );
				}
				return this.metadataClassType;
			}
		}
	}
}
