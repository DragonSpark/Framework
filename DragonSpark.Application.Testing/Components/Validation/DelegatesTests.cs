using DragonSpark.Application.Components.Validation;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Components.Validation
{
	public sealed class DelegatesTests
	{
		[Theory, AutoData]
		public void Verify(string message)
		{
			var instance = new Subject { Message = message };
			var actual   = Delegates.Default.Get(A.Type<Subject>(), nameof(Subject.Message))(instance);
			actual.Should().Be(message);
		}

		[Theory, AutoData]
		public void VerifyInheritance(string message)
		{
			{
				var instance = new BaseSubject { Message = message };
				var actual   = Delegates.Default.Get(instance.GetType(), nameof(BaseSubject.Message))(instance);
				actual.Should().Be(message);
			}
			{
				var instance = new ExtendedSubject { Message = message };
				var actual   = Delegates.Default.Get(instance.GetType(), nameof(BaseSubject.Message))(instance);
				actual.Should().Be(message);
			}
		}

		sealed class Subject
		{
			public string Message { get; set; } = default!;
		}

		class BaseSubject
		{
			public string Message { get; set; } = default!;
		}

		sealed class ExtendedSubject : BaseSubject {}
	}
}