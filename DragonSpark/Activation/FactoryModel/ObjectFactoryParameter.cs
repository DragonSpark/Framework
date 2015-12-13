using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ObjectFactoryParameter : ActivateParameter
	{
		public ObjectFactoryParameter( Type factoryType, object context = null ) : base( factoryType )
		{
			Context = context;
		}

		public object Context { get; }
	}
}