using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Activation.Location
{
	public class SingletonLocatorTests
	{
		[Theory, Framework.Application.AutoData]
		void VerifySingleton( SingletonSubject sut )
		{
			Assert.Null( sut.SomeObject );
			Assert.Same( sut, SingletonSubject.Default );
		}

		[Theory, Framework.Application.AutoData]
		void VerifySourcedSingleton( SingletonScopedSubject sut )
		{
			Assert.Null( sut.SomeObject );
			Assert.Same( sut, SingletonScopedSubject.Default.Get() );
		}

		[Theory, Framework.Application.AutoData]
		void VerifyNoSingleton( [Frozen]object parameter, NoSingleton sut ) => Assert.Same( parameter, sut.SomeObject );

		[Theory, Framework.Application.AutoData]
		void VerifySingletonWithGreedy( [Frozen]object parameter, [Frozen]IEnumerable<object> parameters, SingletonWithConstructorsSubject singleton, [Modest]SingletonWithConstructorsSubject modest, [Greedy]SingletonWithConstructorsSubject greedy )
		{
			Assert.Same( SingletonWithConstructorsSubject.Default, singleton );

			Assert.NotSame( SingletonWithConstructorsSubject.Default, greedy );
			Assert.Null( greedy.Parameter );
			Assert.NotNull( greedy.DateTime );
			Assert.NotNull( greedy.Number );
			Assert.Same( parameters, greedy.Parameters );

			Assert.NotSame( SingletonWithConstructorsSubject.Default, modest );
			Assert.Null( modest.Parameters );
			Assert.Same( parameter, modest.Parameter );
		}

		sealed class SingletonSubject
		{
			public static SingletonSubject Default { get; } = new SingletonSubject();
			SingletonSubject() {}

			[UsedImplicitly]
			public SingletonSubject( object someObject )
			{
				SomeObject = someObject;
			}

			public object SomeObject { get; }
		}

		sealed class SingletonWithConstructorsSubject
		{
			public static SingletonWithConstructorsSubject Default { get; } = new SingletonWithConstructorsSubject();
			SingletonWithConstructorsSubject() {}

			[UsedImplicitly]
			public SingletonWithConstructorsSubject( object parameter )
			{
				Parameter = parameter;
			}

			[UsedImplicitly]
			public SingletonWithConstructorsSubject( IEnumerable<object> parameters, int? number, DateTime? dateTime )
			{
				Parameters = parameters;
				Number = number;
				DateTime = dateTime;
			}

			public object Parameter { get; }
			public IEnumerable<object> Parameters { get; }
			public int? Number { get; }
			public DateTime? DateTime { get; }
		}

		sealed class SingletonScopedSubject
		{
			public static IScope<SingletonScopedSubject> Default { get; } = new SingletonScope<SingletonScopedSubject>( () => new SingletonScopedSubject() );
			SingletonScopedSubject() {}

			[UsedImplicitly]
			public SingletonScopedSubject( object someObject )
			{
				SomeObject = someObject;
			}

			public object SomeObject { get; }
		}

		sealed class NoSingleton
		{
			[UsedImplicitly]
			public NoSingleton( object someObject )
			{
				SomeObject = someObject;
			}

			public object SomeObject { get; }
		}
	}
}