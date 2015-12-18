using System;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationParameter
	{
		protected ActivationParameter( Type type )
		{
			Type = type;
		}

		public Type Type { get; }

	}
}