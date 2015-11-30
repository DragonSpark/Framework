using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.FactoryModel
{
	public abstract class ActivationParameter
	{
		protected ActivationParameter( TypeExtension type )
		{
			Type = type;
		}

		public TypeExtension Type { get; }

	}
}