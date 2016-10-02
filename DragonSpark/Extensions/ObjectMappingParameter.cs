using AutoMapper;

namespace DragonSpark.Extensions
{
	public struct ObjectMappingParameter<T>
	{
		public ObjectMappingParameter( object source, T destination = default(T) )
		{
			Source = source;
			Destination = destination;

			var sourceType = Source.GetType();
			Pair = new TypePair( sourceType, Destination?.GetType() ?? ( typeof(T) == typeof(object) ? sourceType : typeof(T) ) );
		}

		public object Source { get; }

		public T Destination { get; }
		public TypePair Pair { get; }
	}
}