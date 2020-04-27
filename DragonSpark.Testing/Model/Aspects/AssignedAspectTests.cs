using DragonSpark.Compose;
using DragonSpark.Model.Aspects;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Model.Aspects
{
	public class AssignedAspectTests
	{
		[Fact]
		public void RuntimeRegistration()
		{
			var single    = new Registration(typeof(AssignedAspect<,>));
			var parameter = new[] {A.Type<string>(), A.Type<string>()};
			single.Get(parameter).Should().BeSameAs(AssignedAspect<string, string>.Default);
		}

		[Fact]
		public void Verify()
		{
			var registry = new AspectRegistry();
			registry.Execute(new Registration(typeof(AssignedAspect<,>)));

			var subject = Start.A.Selection<string>().By.Self.Get();
			subject.Invoking(x => x.Get(null)).Should().NotThrow();

			new Aspects<string, string>(registry).Get(subject)
			                                     .Get(subject)
			                                     .Invoking(x => x.Get(null))
			                                     .Should()
			                                     .Throw<ArgumentNullException>();
		}
	}
}