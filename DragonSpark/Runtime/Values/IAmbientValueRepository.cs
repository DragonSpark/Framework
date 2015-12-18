namespace DragonSpark.Runtime.Values
{
	public interface IAmbientValueRepository
	{
		void Add( IAmbientKey key, object instance );

		object Get( IAmbientRequest request );

		void Remove( object context );
	}
}