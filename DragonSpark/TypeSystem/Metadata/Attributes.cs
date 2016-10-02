namespace DragonSpark.TypeSystem.Metadata
{
	public static class Attributes
	{
		//readonly static Func<object, IAttributeProvider> Source = AttributeProviders.Default.Delegate();
		public static IAttributeProvider Get( object target ) => target as IAttributeProvider ?? AttributeProviders.Default.Get( target );
	}
}