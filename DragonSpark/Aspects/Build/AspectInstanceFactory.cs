using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Aspects.Build
{
	public sealed class ObjectConstructionFactory<T> : ParameterizedSourceBase<IEnumerable<object>, ObjectConstruction>
	{
		public static ObjectConstructionFactory<T> Default { get; } = new ObjectConstructionFactory<T>();
		ObjectConstructionFactory() {}

		public ObjectConstruction Get() => GetUsing( Items<object>.Default );
		public ObjectConstruction GetUsing( params object[] parameter ) => Get( parameter );

		public override ObjectConstruction Get( IEnumerable<object> parameter ) => new ObjectConstruction( typeof(T), parameter.Fixed() );
	}

	public sealed class AspectInstanceFactory<TAspect, TTarget> : SpecificationParameterizedSource<TTarget, AspectInstance>
	{
		public static AspectInstanceFactory<TAspect, TTarget> Default { get; } = new AspectInstanceFactory<TAspect, TTarget>();
		AspectInstanceFactory() : base( HasAspectSpecification.DefaultNested.Inverse(), Factory.DefaultNested.Get ) {}
		
		sealed class HasAspectSpecification : SpecificationBase<TTarget>
		{
			readonly static Type AspectType = typeof(TAspect);

			public static HasAspectSpecification DefaultNested { get; } = new HasAspectSpecification();
			HasAspectSpecification() : this( () => PostSharpEnvironment.CurrentProject.GetService<IAspectRepositoryService>() ) {}

			readonly Func<IAspectRepositoryService> repositorySource;

			HasAspectSpecification( Func<IAspectRepositoryService> repositorySource )
			{
				this.repositorySource = repositorySource;
			}

			public override bool IsSatisfiedBy( TTarget parameter ) => repositorySource().HasAspect( parameter, AspectType );
		}

		sealed class Factory : ParameterizedSourceBase<TTarget, AspectInstance>
		{
			public static Factory DefaultNested { get; } = new Factory();
			Factory() : this( ObjectConstructionFactory<TAspect>.Default.Get() ) {}

			readonly ObjectConstruction construction;

			Factory( ObjectConstruction construction )
			{
				this.construction = construction;
			}

			public override AspectInstance Get( TTarget parameter ) => new AspectInstance( parameter, construction, null );
		}
	}
}