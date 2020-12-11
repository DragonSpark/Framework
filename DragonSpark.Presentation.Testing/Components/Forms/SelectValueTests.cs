using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Presentation.Components.Forms;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;

namespace DragonSpark.Presentation.Testing.Components.Forms
{
	public sealed class SelectValueTests
	{
		[Theory, AutoData]
		public void Verify(string message)
		{
			var instance  = new Subject { Message = message };
			var parameter = FieldIdentifier.Create(() => instance.Message);
			var actual    = SelectValue<string>.Default.Get(parameter);
			actual.Should().Be(message);
		}

		[Theory, AutoData]
		public void VerifyInheritance(string message)
		{
			{
				var instance  = new BaseSubject { Message = message };
				var parameter = FieldIdentifier.Create(() => instance.Message);
				var actual    = SelectValue<string>.Default.Get(parameter);
				actual.Should().Be(message);
			}
			{
				var instance  = new ExtendedSubject { Message = message };
				var parameter = FieldIdentifier.Create(() => instance.Message);
				var actual    = SelectValue<string>.Default.Get(parameter);
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