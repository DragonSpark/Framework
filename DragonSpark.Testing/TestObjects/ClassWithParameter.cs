using DragonSpark.Activation;
using Microsoft.Practices.Unity;
using System;
using DragonSpark.Runtime;

namespace DragonSpark.Testing.TestObjects
{
	class Factory : Factory<ClassWithParameter>
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
					throw new ResolutionFailedException( GetType(), null, new InvalidOperationException( message ), null );
				default:
					AmbientValues.Remove( GetType() );
					break;
			}
		}
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