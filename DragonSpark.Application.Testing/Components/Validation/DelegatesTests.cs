using DragonSpark.Application.Components.Validation;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using DragonSpark.Reflection.Members;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Components.Validation;

public sealed class DelegatesTests
{
	[Theory, AutoData]
	public void Verify(string message)
	{
		var instance = new Subject { Message = message };
		var actual   = PropertyDelegates.Default.Get(A.Type<Subject>(), nameof(Subject.Message)).Verify()(instance);
		actual.Should().Be(message);
	}

	[Fact]
	public void VerifyExtended()
	{
		var subject  = new Delegates();
		var instance = new ExtendedPropertySubject { Subject = new() { Message = "Hello World!" } };
		var message = subject.Get(new (instance, "Subject.Message"));
		message.Should().Be(instance.Subject.Message);

	}

	[Theory, AutoData]
	public void VerifyInheritance(string message)
	{
		{
			var instance = new BaseSubject { Message = message };
			var actual   = PropertyDelegates.Default.Get(instance.GetType(), nameof(BaseSubject.Message)).Verify()(instance);
			actual.Should().Be(message);
		}
		{
			var instance = new ExtendedSubject { Message = message };
			var actual   = PropertyDelegates.Default.Get(instance.GetType(), nameof(BaseSubject.Message)).Verify()(instance);
			actual.Should().Be(message);
		}
	}

	sealed class ExtendedPropertySubject
	{
		public Subject Subject { get; set; } = new();
	}

	sealed class Subject
	{
		public string Message { get; set; } = default!;
	}

	class BaseSubject
	{
		public string Message { get; set; } = default!;
	}

	sealed class ExtendedSubject : BaseSubject;
}