namespace DragonSpark.Activation.FactoryModel
{
	public static class Factory
	{
		public static T Create<T>( object context = null )
		{
			var type = FactoryReflectionSupport.Instance.GetFactoryType( typeof(T) );
			var result = (T)new FactoryBuiltObjectFactory().Create( new ObjectFactoryParameter( type, context ) );
			return result;
		}
	}
}