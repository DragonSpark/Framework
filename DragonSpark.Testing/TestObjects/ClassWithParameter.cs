using DragonSpark.Activation;
using Microsoft.Practices.Unity;
using System;

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
		static int count = 0;

		public ClassCreatedFromDefault( string message )
		{
			switch ( count++ )
			{
				case 0:
					throw new ResolutionFailedException( GetType(), null, new InvalidOperationException( message ), null );
			}
		}
	}

	class ClassWithManyParameters
	{
		readonly string s;
		readonly int integer;
		readonly Class @class;

		public ClassWithManyParameters( string @string, int integer, Class @class )
		{
			s = @string;
			this.integer = integer;
			this.@class = @class;
		}

		public string String
		{
			get { return s; }
		}

		public int Integer
		{
			get { return integer; }
		}

		public Class Class
		{
			get { return @class; }
		}
	}
}