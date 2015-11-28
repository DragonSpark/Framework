using DragonSpark.Activation;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Testing.TestObjects
{
	class ConstructFactory : ConstructFactory<ClassWithParameter>
	{}

	public class ClassWithParameter : IClassWithParameter, IInterface
	{
		public object Parameter { get; set; }

		public ClassWithParameter( object parameter )
		{
			Parameter = parameter;
		}
	}

	class ClassCreatedFromDefault
	{
		public ClassCreatedFromDefault( string message )
		{
			var count = AmbientValues.Get<int>( GetType() );
			switch ( count )
			{
				case 0:
					AmbientValues.RegisterFor( 1, GetType() );
					throw new InvalidOperationException( message );
				default:
					Message = message;
					AmbientValues.Remove( GetType() );
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