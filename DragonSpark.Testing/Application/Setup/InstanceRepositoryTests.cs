using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Testing.Objects;
using Serilog;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Setup
{
	public class InstanceRepositoryTests
	{
		[Fact]
		public void BasicTest()
		{
			var target = new Class();
			var composite = new CompositeActivator( new InstanceRepository( target ) );
			var result = composite.Get<Class>();
			Assert.Same( target, result );
		}

		[Fact]
		public void Factory()
		{
			var result = new InstanceRepository( new ClassFactory() ).Get<Class>();
			Assert.IsType<ClassFactory.ClassFromFactory>( result );
		}

		[Fact]
		public void Logger()
		{
			var result = DefaultServices.Default.Get<ILogger>();
			Assert.NotNull( result );
			Assert.Same( DragonSpark.Diagnostics.Logger.Default.Get( Execution.Current() ), result );
		}

		[Fact]
		public void SystemParts()
		{
			var system = DefaultServices.Default.Get<SystemParts>();
			Assert.Empty( system.Assemblies.AsEnumerable() );
			Assert.Empty( system.Types.AsEnumerable() );

			var types = new[] { typeof(ClassFactory), typeof(Class) }.AsApplicationParts();
			
			var after = DefaultServices.Default.Get<SystemParts>();
			Assert.NotEmpty( after.Assemblies.AsEnumerable() );
			Assert.NotEmpty( after.Types.AsEnumerable() );
			Assert.Equal( types.AsEnumerable(), after.Types.ToArray() );

		}

		class ClassFactory : SourceBase<Class>
		{
			public override Class Get() => new ClassFromFactory();

			public class ClassFromFactory : Class {}
		}
	}
}