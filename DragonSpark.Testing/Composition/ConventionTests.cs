using DragonSpark.Application;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using JetBrains.Annotations;
using System;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class ConventionTests
	{
		[Fact]
		public void Convention()
		{
			var parts = new[] { typeof(IHelloWorld), typeof(HelloWorld) }.AsApplicationParts();
			

			var container = new ContainerConfiguration().WithParts( parts.ToArray(), ConventionBuilderFactory.Default.Get() ).CreateContainer();
			var export = container.GetExport<IHelloWorld>();
			Assert.IsType<HelloWorld>( export );
			Assert.NotSame( export, container.GetExport<IHelloWorld>() );
			Assert.NotSame( container.GetExport<HelloWorld>(), container.GetExport<HelloWorld>() );
			Assert.NotSame( container.GetExport<HelloWorld>(), container.GetExport<IHelloWorld>() );
		}

		[Fact]
		public void OrderOfSelection()
		{
			new[] { typeof(ICurrentTime), typeof(CurrentTime), typeof(DragonSpark.Application.CurrentTime) }.AsApplicationParts();

			Assert.Equal( typeof(CurrentTime), ApplicationTypes.Default.Get().First() );

			var handler = CompositionHostFactory.Default.Get().GetExport<ICurrentTime>();
			Assert.IsType<CurrentTime>( handler );
		}

		public sealed class CurrentTime : ICurrentTime
		{
			[UsedImplicitly]
			public static ICurrentTime Default { get; } = new CurrentTime();
			CurrentTime() {}

			public DateTimeOffset Now => new DateTimeOffset();
		}

		[Fact]
		public void WithoutConvention()
		{
			var parts = new[] { typeof(IHelloWorld), typeof(HelloWorld) }.AsApplicationParts();
			var container = new ContainerConfiguration().WithParts( parts.ToArray() ).CreateContainer();
			Assert.Throws<CompositionFailedException>( () => container.GetExport<IHelloWorld>() );
		}

		[Fact]
		public void Shared()
		{
			var parts = new[] { typeof(IHelloWorldShared), typeof(HelloWorldShared) }.AsApplicationParts();
			
			var container = new ContainerConfiguration().WithParts( parts.ToArray(), ConventionBuilderFactory.Default.Get() ).CreateContainer();
			var export = container.GetExport<IHelloWorldShared>();
			Assert.IsType<HelloWorldShared>( export );
			Assert.Same( export, container.GetExport<IHelloWorldShared>() );
			Assert.Same( container.GetExport<HelloWorldShared>(), container.GetExport<HelloWorldShared>() );
			Assert.Same( container.GetExport<HelloWorldShared>(), container.GetExport<IHelloWorldShared>() );
			
		}

		public interface IHelloWorld {}
		public class HelloWorld : IHelloWorld {}


		public interface IHelloWorldShared {}
		[Shared]
		public class HelloWorldShared : IHelloWorldShared {}
	}
}