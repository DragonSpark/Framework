using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System.Collections.Generic;
using System.Composition;
using Xunit;
using Xunit.Abstractions;
using Constructor = DragonSpark.Activation.Constructor;

namespace DragonSpark.Testing.Activation
{
	public class ActivatorTests : TestCollectionBase
	{
		public ActivatorTests( ITestOutputHelper output ) : base( output ) {}

		[Fact]
		public void Default()
		{
			var activator = GlobalServiceProvider.GetService<IActivator>();
			Assert.Same( Activator.Default, activator );
			var instance = activator.Get<IInterface>( typeof(Class) );
			Assert.IsType<Class>( instance );

			Assert.NotSame( instance, activator.Get<IInterface>( typeof(Class) ) );
		}

		[Fact]
		public void Specifications()
		{
			var specific = Constructor.Default;
			var request = new ConstructTypeRequest( typeof(object) );
			Assert.True( specific.IsSatisfiedBy( request ) );
			Assert.Same( Singleton.Default, SingletonLocator.Default.Get( typeof(Singleton) ) );
			Assert.True( SingletonLocator.Default.IsSatisfiedBy( typeof(Singleton) ) );

			Assert.False( Activator.Default.IsSatisfiedBy( typeof(IInner) ) );
			Assert.True( Activator.Default.IsSatisfiedBy( typeof(Class) ) );
			Assert.False( Activator.Default.IsSatisfiedBy( typeof(Closed) ) );
		}

		interface IInner {}

		class Closed
		{
			Closed() {}
		}

		[UsedImplicitly]
		sealed class Singleton : ItemSourceBase<Class>
		{
			[Export( typeof(IEnumerable<Class>) ), UsedImplicitly]
			public static Singleton Default { get; } = new Singleton();
			Singleton() {}

			protected override IEnumerable<Class> Yield()
			{
				yield return new Class();
			}
		}

		[Theory, AutoData]
		public void DefaultCreate( string parameter )
		{
			var activator = GlobalServiceProvider.GetService<IConstructor>();
			Assert.Same( Constructor.Default, activator );
			
			var instance = activator.Construct<ClassWithParameter>( parameter );
			Assert.NotNull( instance );
			Assert.Equal( parameter, instance.Parameter );
		}
	}
}