using System;
using AutoMapper;

namespace DragonSpark.Extensions
{
	public class ObjectMappingParameter<T>
	{
		public ObjectMappingParameter( object source, T existing, Action<IMappingExpression> configuration )
		{
			Source = source;
			Existing = existing;
			Configuration = configuration;
		}

		public object Source { get; }

		public T Existing { get; }

		public Action<IMappingExpression> Configuration { get; }
	}
}