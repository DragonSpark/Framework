using System;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateParameter : ActivationParameter
	{
		public ActivateParameter( Type type, string name = null ) : base( type )
		{
			Name = name;
		}

		public string Name { get; }
	}
}