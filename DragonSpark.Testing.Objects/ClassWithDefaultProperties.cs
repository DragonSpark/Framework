using DragonSpark.ComponentModel;
using DragonSpark.Runtime.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DragonSpark.Testing.Objects
{
	public class ClassWithDefaultProperties
	{
		public ClassWithDefaultProperties()
		{
			AlreadySet = "Already Set";
		}

		[Default( 'd' )]
		public char Char { get; set; }

		[Default( (byte)7 )]
		public byte Byte { get; set; }

		[Default( (short)8 )]
		public short Short { get; set; }

		[Default( 9 )]
		public int Int { get; set; }

		[Default( 6776L )]
		public long Long { get; set; }

		[Default( 6.7F )]
		public float Float { get; set; }

		[Default( 7.1D )]
		public double Double { get; set; }

		[Default( true )]
		public bool Boolean { get; set; }

		[Default( "Hello World" )]
		public string String { get; set; }

		[DefaultValue( "Legacy" )]
		public string Legacy { get; set; }

		[Default( typeof(ClassWithDefaultProperties) )]
		public object Object { get; set; }

		[CurrentTime]
		public DateTime CurrentDateTime { get; set; }

		[CurrentTime]
		public DateTimeOffset CurrentDateTimeOffset { get; set; }

		[Activate]
		public Class Activated { get; set; }

		[Factory( typeof(ConstructFactory) )]
		public object Factory { get; set; }

		[Collection]
		public IEnumerable<object> Collection { get; set; }

		[Collection( typeof(Class) )]
		public IEnumerable<Class> Classes { get; set; }
		
		[Value( typeof(Value) )]
		public int ValuedInt { get; set; }

		internal class Value : FixedValue<int>
		{
			public Value()
			{
				Assign( 6776 );
			}
		}

		[NewGuid( "66570344-BA99-4C90-A7BE-AEC903441F97" )]
		public Guid ProvidedGuid { get; set; }

		[NewGuid]
		public Guid Guid { get; set; }

		[NewGuid]
		public Guid AnotherGuid { get; set; }

		[DefaultValue( "This does not get set to this value." )]
		public string AlreadySet { get; set; }
	}
}