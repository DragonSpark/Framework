namespace DragonSpark.TypeSystem.Metadata
{
	public static class Attributes
	{
		public static IAttributeProvider Get( object target ) => target as IAttributeProvider ?? AttributeProviders.Default.Get( target );
	}
}