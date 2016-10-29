using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using PostSharp.Aspects.Internals;
using System;
using System.Runtime.CompilerServices;
using Defaults = DragonSpark.Activation.Defaults;

namespace DragonSpark.Application
{
	public sealed class ApplicationTypeSpecification : SpecificationBase<Type>
	{
		public static ISpecification<Type> Default { get; } = new DelegatedSpecification<Type>( new DecoratedSourceCache<Type, bool>( new ApplicationTypeSpecification().IsSatisfiedBy ).Get );
		ApplicationTypeSpecification() {}

		public override bool IsSatisfiedBy( Type parameter ) => Defaults.Instantiable.IsSatisfiedBy( parameter ) && !typeof(MethodBinding).Adapt().IsAssignableFrom( parameter ) && !parameter.Has<CompilerGeneratedAttribute>();
	}
}