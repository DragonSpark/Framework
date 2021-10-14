using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Activation
{
	public sealed class RecordTests
	{
		[Fact]
		public void Verify()
		{
			var instance = new Subject("Hello World", 123);
			var first    = instance;
			instance.Message = "Updated";

			instance.Should().BeSameAs(first);
			first.Message.Should().Be("Updated");
		}

		[Fact]
		public void VerifyWith()
		{
			var instance = new Subject("Hello World", 123);

			var next = instance with { Message = "Updated" };

			next.Should().NotBeSameAs(instance);

		}

		// ReSharper disable once NotAccessedPositionalProperty.Local
		sealed record Subject(string Message, int Number)
		{
			public string Message { get; set; } = Message;
		}
	}
}