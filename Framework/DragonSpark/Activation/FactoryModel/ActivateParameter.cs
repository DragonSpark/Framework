using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateParameter : ActivationParameter
	{
		public ActivateParameter( TypeExtension type ) : this( type, null )
		{}

		public ActivateParameter( TypeExtension type, string name ) : base( type )
		{
			Name = name;
		}

		public string Name { get; }
	}
}