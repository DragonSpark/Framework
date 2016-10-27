using DragonSpark.Activation.Location;
using DragonSpark.Aspects.Alteration;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Runtime;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class AutoDataCustomization : CompositeCustomization
	{
		public AutoDataCustomization() : base( ServicesCustomization.Default, SingletonCustomization.Default, new AutoMoqCustomization() ) {}
	}

	[ApplyAutoValidation, ApplySpecification( typeof(ContainsSingletonPropertySpecification) ), ApplyResultAlteration( typeof(EnumerableResultAlteration<IMethod>) )]
	public sealed class SingletonQuery : ParameterizedSourceBase<Type, IEnumerable<IMethod>>, IMethodQuery, ISpecification<Type>
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

	public sealed class SingletonCustomization : CustomizationBase
	{
		readonly static MethodInvoker MethodInvoker = new MethodInvoker( SingletonQuery.Default );

		public static SingletonCustomization Default { get; } = new SingletonCustomization();
		SingletonCustomization() {}

		protected override void OnCustomize( IFixture fixture ) => fixture.Customizations.Insert( 0, MethodInvoker );
	}

	public sealed class SingletonMethod : SuppliedSource<Type, object>, IMethod
	{
		public SingletonMethod( Type parameter ) : this( parameter, SourceAccountedValues.Defaults.Get( parameter ) ) {}

		[UsedImplicitly]
		public SingletonMethod( Type parameter, Func<object, object> account ) : base( SingletonLocator.Default.Apply( account ).Get, parameter ) {}

		public IEnumerable<ParameterInfo> Parameters { get; } = Items<ParameterInfo>.Default;

		object IMethod.Invoke( IEnumerable<object> parameters ) => Get();
	}
}