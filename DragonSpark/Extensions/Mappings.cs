namespace DragonSpark.Extensions
{
	public static class Mappings
	{
		public static TResult MapInto<TResult>( this object source, TResult destination = null ) where TResult : class 
		{
			var context = new ObjectMappingParameter<TResult>( source, destination );
			var result = ObjectMapper<TResult>.Default.Get( context );
			return result;
		}
	}
}