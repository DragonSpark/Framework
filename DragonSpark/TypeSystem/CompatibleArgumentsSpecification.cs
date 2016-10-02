using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.Specifications;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	sealed class CompatibleArgumentsSpecification : SpecificationWithContextBase<Type[], CompatibleArgumentsSpecification.Parameter[]>
	{
		readonly int required;
		public static ICache<MethodBase, ISpecification<Type[]>> Default { get; } = new Cache<MethodBase, ISpecification<Type[]>>( method => new CompatibleArgumentsSpecification( method ) );

		readonly static Func<ValueTuple<Parameter, Type[]>, int, bool> SelectCompatible = Compatible;

		CompatibleArgumentsSpecification( MethodBase method ) : this( method.GetParameters().Select( info => new Parameter( info.ParameterType.Adapt(), info.IsOptional ) ).ToArray() ) {}

		CompatibleArgumentsSpecification( Parameter[] parameters ) : this( parameters, parameters.Count( info => !info.Optional ) ) {}

		CompatibleArgumentsSpecification( Parameter[] parameters, int required ) : base( parameters )
		{
			this.required = required;
		}

		public override bool IsSatisfiedBy( Type[] parameter )
		{
			var result = parameter.Length >= required && parameter.Length <= Context.Length
						 &&
						 Context.Introduce( parameter ).Select( SelectCompatible ).All();
			return result;
		}

		static bool Compatible( ValueTuple<Parameter, Type[]> context, int i )
		{
			var type = context.Item2.ElementAtOrDefault( i );
			var result = type != null ? context.Item1.ParameterType.IsAssignableFrom( type ) : i < context.Item2.Length || context.Item1.Optional;
			return result;
		}

		public struct Parameter
		{
			public Parameter( TypeAdapter parameterType, bool optional )
			{
				ParameterType = parameterType;
				Optional = optional;
			}

			public TypeAdapter ParameterType { get; }
			public bool Optional { get; }
		}
	}
}