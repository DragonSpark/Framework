using DragonSpark.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources.Coercion;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation
{
	public interface IConstructor : IParameterizedSource<ConstructTypeRequest, object>, ISpecification<ConstructTypeRequest>, IActivator {}
	public sealed class Constructor : ActivatorBase, IConstructor
	{
		public static IConstructor Default { get; } = new Constructor();
		Constructor() : this( ConstructorSpecification.Default, Source.Implementation ){}

		readonly ISpecification<ConstructTypeRequest> specification;
		readonly IParameterizedSource<ConstructTypeRequest, object> source;

		[UsedImplicitly]
		public Constructor( ISpecification<ConstructTypeRequest> specification, IParameterizedSource<ConstructTypeRequest, object> source ) : base( specification.Coerce( ConstructorCoercer.Default ), source.Accept( ConstructorCoercer.Default ).Get )
		{
			this.specification = specification;
			this.source = source;
		}

		public bool IsSatisfiedBy( ConstructTypeRequest parameter ) => specification.IsSatisfiedBy( parameter );

		public object Get( ConstructTypeRequest parameter ) => source.Get( parameter );

		sealed class Source : ParameterizedSourceBase<ConstructTypeRequest, object>
		{
			public static Source Implementation { get; } = new Source();
			Source() : this( Constructors.Default.Get, ConstructorDelegateFactory<Invoke>.Default.Get ) {}

			readonly Func<ConstructTypeRequest, ConstructorInfo> constructorSource;
			readonly Func<ConstructorInfo, Invoke> activatorSource;

			Source( Func<ConstructTypeRequest, ConstructorInfo> constructorSource, Func<ConstructorInfo, Invoke> activatorSource )
			{
				this.constructorSource = constructorSource;
				this.activatorSource = activatorSource;
			}

			public override object Get( ConstructTypeRequest parameter ) => LocateAndCreate( parameter ) ?? SpecialValues.DefaultOrEmpty( parameter.RequestedType );

			object LocateAndCreate( ConstructTypeRequest parameter )
			{
				var info = constructorSource( parameter );
				var result = info != null ? activatorSource( info )?.Invoke( WithOptional( parameter.Arguments.ToArray(), info.GetParameters() ) ) : null;
				return result;
			}

			static object[] WithOptional( IReadOnlyCollection<object> arguments, IEnumerable<ParameterInfo> parameters )
			{
				var optional = parameters.Skip( arguments.Count ).Where( info => info.IsOptional ).Select( info => info.DefaultValue );
				var result = arguments.Concat( optional ).Fixed();
				return result;
			}
		}
	}

	sealed class ConstructorSpecification : SpecificationBase<ConstructTypeRequest>
	{
		public static ConstructorSpecification Default { get; } = new ConstructorSpecification();
		ConstructorSpecification() : this( Constructors.Default ) {}

		readonly Constructors cache;

		ConstructorSpecification( Constructors cache )
		{
			this.cache = cache;
		}

		public override bool IsSatisfiedBy( ConstructTypeRequest parameter ) => 
			parameter.RequestedType.GetTypeInfo().IsValueType || cache.Get( parameter ) != null;
	}

	public sealed class ConstructorCoercer : CoercerBase<Type, ConstructTypeRequest>
	{
		public static ConstructorCoercer Default { get; } = new ConstructorCoercer();
		ConstructorCoercer() {}

		protected override ConstructTypeRequest Coerce( Type parameter ) => new ConstructTypeRequest( parameter );
	}
}