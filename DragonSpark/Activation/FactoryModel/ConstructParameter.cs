using DragonSpark.TypeSystem;

namespace DragonSpark.Activation.FactoryModel
{
	public class ConstructParameter : ActivationParameter
	{
		public ConstructParameter( TypeExtension type ) : this( type, new object[] { } )
		{}

		public ConstructParameter( TypeExtension type, params object[] arguments ) : base( type )
		{
			Arguments = arguments;
		}

		public object[] Arguments { get; }

		/*public static explicit operator ConstructParameter( Type type )
		{
			var result = new ConstructParameter( type );
			return result;
		}*/
	}
}