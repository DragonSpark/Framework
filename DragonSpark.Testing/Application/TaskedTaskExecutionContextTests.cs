using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Runtime;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.Application
{
	[Trait( Traits.Category, Traits.Categories.ServiceLocation )]
	// ReSharper disable once TestFileNameWarning
	public class TaskedTaskExecutionContextTests : TestCollectionBase
	{
		public TaskedTaskExecutionContextTests( ITestOutputHelper output ) : base( output ) {}

		[Fact]
		public Task Fact()
		{
			var current = ExecutionContext.Default.Get();
			Assert.Equal( ExecutionContext.Default.Get().Origin, Identifier.Current() );
			Assert.Null( CurrentMethod.Default.Get() );
			return Task.Run( () =>
							 {
								 Assert.Same( current, ExecutionContext.Default.Get() );
								 Assert.NotEqual( ExecutionContext.Default.Get().Origin, Identifier.Current() );
								 Assert.Null( CurrentMethod.Default.Get() );
							 } );
		}

		[Theory, ExecutionContextAutoData]
		public Task Theory()
		{
			var current = ExecutionContext.Default.Get();
			Assert.Equal( Identification.Default.Get(), Identifier.Current() );
			var method = new Func<Task>( Theory ).Method;
			Assert.Equal( method, CurrentMethod.Default.Get() );
			return Task.Run( () =>
							 {
								 Assert.Same( current, ExecutionContext.Default.Get() );
								 ExecutionContextTests.Verify( method );
								 Assert.NotEqual( ExecutionContext.Default.Get().Origin, Identifier.Current() );
								 Assert.NotNull( CurrentMethod.Default.Get() );
								 Assert.Equal( method, CurrentMethod.Default.Get() );
							 } );
		}
	}
}