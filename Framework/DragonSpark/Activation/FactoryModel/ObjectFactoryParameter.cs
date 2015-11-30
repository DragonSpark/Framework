using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ObjectFactoryParameter : ActivateParameter
	{
		public ObjectFactoryParameter( Type factoryType ) : this( factoryType, null )
		{}

		public ObjectFactoryParameter( Type factoryType, object context ) : base( factoryType )
		{
			Context = context;
		}

		public object Context { get; }

		public static implicit operator ObjectFactoryParameter( Type type )
		{
			var result = new ObjectFactoryParameter( type );
			return result;
		}
	}
}