using System;
using System.Collections.Generic;
using DragonSpark.Activation.Location;
using DragonSpark.Aspects.Alteration;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
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
}