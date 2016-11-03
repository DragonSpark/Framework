using DragonSpark.Sources.Coercion;
using Xunit;

namespace DragonSpark.Testing.Sources.Coercion
{
	public class StringCoercerTests
	{
		[Fact]
		public void Verify()
		{
			Assert.Equal( Subject.Message, StringCoercer.Default.Get( new Subject() ) );
		}

		sealed class Subject
		{
			public const string Message = "123";

			public override string ToString() => Message;
		}
	}
}