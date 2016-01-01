using System;

namespace DragonSpark.Windows.Testing.TestObjects
{
	public class ClassWithBrokenConstructor
	{
		public ClassWithBrokenConstructor()
		{
			throw new InvalidOperationException( "This is a broken constructor" );
		}
	}
}