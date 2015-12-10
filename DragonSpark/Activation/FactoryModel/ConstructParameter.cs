using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructParameter : ActivationParameter
	{
		public ConstructParameter( Type type ) : this( type, new object[] { } )
		{}

		public ConstructParameter( Type type, params object[] arguments ) : base( type )
		{
			Arguments = arguments;
		}

		public object[] Arguments { get; }
	}
}