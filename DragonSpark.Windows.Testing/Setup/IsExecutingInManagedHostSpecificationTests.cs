using DragonSpark.Activation.Location;
using DragonSpark.Aspects.Alteration;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class IsExecutingInManagedHostSpecificationTests
	{
		/*[Fact]
		public void Verify()
		{
			Assert.False( IsExecutingInManagedHostSpecification.Default.IsSatisfiedBy( AppDomain.CurrentDomain ) );
		}*/

		[Theory, DragonSpark.Testing.Framework.Application.AutoData]
		void Sandbox( /*Mock<ApplicationSettingsBase> sut*/ )
		{
			
		}

		[Theory, IsExecutingInManagedHostSpecificationTests.AutoData]
		void VerifySingleton( SingletonSubject sut )
		{
			Assert.Null( sut.SomeObject );
			Assert.Same( sut, SingletonSubject.Default );
		}

		[Theory, IsExecutingInManagedHostSpecificationTests.AutoData]
		void VerifySourcedSingleton( SingletonScopedSubject sut )
		{
			Assert.Null( sut.SomeObject );
			Assert.Same( sut, SingletonScopedSubject.Default.Get() );
		}

		[Theory, IsExecutingInManagedHostSpecificationTests.AutoData]
		void VerifyNoSingleton( [Frozen]object parameter, NoSingleton sut )
		{
			Assert.Same( parameter, sut.SomeObject );
			
		}

		class SingletonSubject
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

		class SingletonScopedSubject
		{
			public static IScope<SingletonScopedSubject> Default { get; } = new Scope<SingletonScopedSubject>( Factory.GlobalCache( () => new SingletonScopedSubject() ) );
			SingletonScopedSubject() {}

			[UsedImplicitly]
			public SingletonScopedSubject( object someObject )
			{
				SomeObject = someObject;
			}

			public object SomeObject { get; }
		}

		class NoSingleton
		{
			[UsedImplicitly]
			public NoSingleton( object someObject )
			{
				SomeObject = someObject;
			}

			public object SomeObject { get; }
		}

		[ApplyAutoValidation, ApplySpecification( typeof(ContainsSingletonPropertySpecification) ), ApplyResultAlteration( typeof(EnumerableResultAlteration<IMethod>) )]
		sealed class SingletonQuery : ParameterizedSourceBase<Type, IEnumerable<IMethod>>, IMethodQuery, ISpecification<Type>
		{
			public static SingletonQuery Default { get; } = new SingletonQuery();
			SingletonQuery() {}

			public override IEnumerable<IMethod> Get( Type parameter )
			{
				yield return new SingletonMethod( parameter );
			}

			IEnumerable<IMethod> IMethodQuery.SelectMethods( Type type ) => Get( type );

			bool ISpecification<Type>.IsSatisfiedBy( Type parameter ) => false;
		}

		sealed class SingletonCustomization : CustomizationBase
		{
			readonly static MethodInvoker MethodInvoker = new MethodInvoker( SingletonQuery.Default );

			public static SingletonCustomization Default { get; } = new SingletonCustomization();
			SingletonCustomization() {}

			protected override void OnCustomize( IFixture fixture )
			{
				fixture.Customizations.Insert( 0, MethodInvoker );
			}
		}

		sealed class SingletonMethod : SuppliedSource<Type, object>, IMethod
		{
			public SingletonMethod( Type parameter ) : base( SingletonLocator.Default.Get, parameter ) {}

			public IEnumerable<ParameterInfo> Parameters { get; } = Items<ParameterInfo>.Default;

			object IMethod.Invoke( IEnumerable<object> parameters ) => Get();
		}

		class AutoData : AutoDataAttribute
		{
			public AutoData() : base( new Fixture().Customize( SingletonCustomization.Default ) ) {}
		}


	}
}