using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Values;
using System;

namespace DragonSpark.Testing.TestObjects
{
	class ConstructFactory : ConstructFactory<ClassWithParameter>
	{}

	public class ClassWithParameter : IClassWithParameter, IInterface
	{
		public ClassWithParameter( object parameter )
		{
			Parameter = parameter;
		}

		public object Parameter { get; }
	}

	class ClassCreatedFromDefault
	{
		public ClassCreatedFromDefault( string message )
		{
			var associated = new AssociatedValue<Type, int>( GetType() );
			var count = associated.Item;
			switch ( count )
			{
				case 0:
					associated.Assign( 1 );
					throw new InvalidOperationException( message );
				default:
					Message = message;
					associated.Property.TryDisconnect();
					break;
			}
		}
		public string Message { get; private set; }
	}

	class ClassWithManyParameters
	{
		public ClassWithManyParameters( string @string, int integer, Class @class )
		{
			String = @string;
			Integer = integer;
			Class = @class;
		}

		public string String { get; }

		public int Integer { get; }

		public Class Class { get; }
	}
}