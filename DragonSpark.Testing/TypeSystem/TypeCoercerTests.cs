using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class TypeCoercerTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.Equal( TypeCoercer.Default.Get( this ), GetType() );
			Assert.Equal( TypeCoercer.Default.Get( GetType() ), GetType() );
			var methodInfo = new Action<string>( Subject.Default.Method ).Method;
			Assert.Equal( TypeCoercer.Default.Get( methodInfo ), typeof(void) );
			Assert.Equal( TypeCoercer.Default.Get( methodInfo.GetParameters().Only() ), typeof(string) );
		}

		sealed class Subject
		{
			public static Subject Default { get; } = new Subject();
			Subject() {}

			public void Method( string parameter ) {}
		}
	}
}