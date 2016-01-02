using DragonSpark.Extensions;
using DragonSpark.Runtime.Specifications;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation.IoC
{
	public abstract class StrategyValidator<TStrategy> : SpecificationBase<StrategyValidatorParameter> where TStrategy : BuilderStrategy
	{
		protected override bool IsSatisfiedByParameter( StrategyValidatorParameter parameter )
		{
			return base.IsSatisfiedByParameter( parameter ) && parameter.Strategies.FirstOrDefaultOfType<TStrategy>().With( strategy => Check( parameter.Key ) );
		}

		protected abstract bool Check( NamedTypeBuildKey key );
	}

	public class ArrayStrategyValidator : StrategyValidator<ArrayResolutionStrategy>
	{
		public static ArrayStrategyValidator Instance { get; } = new ArrayStrategyValidator();

		protected override bool Check( NamedTypeBuildKey key )
		{
			return key.Type.IsArray;
		}
	}

	public class EnumerableStrategyValidator : StrategyValidator<EnumerableResolutionStrategy>
	{
		public static EnumerableStrategyValidator Instance { get; } = new EnumerableStrategyValidator();

		protected override bool Check( NamedTypeBuildKey key )
		{
			var result = key.Type.Adapt().IsGenericOf<IEnumerable<object>>();
			return result;
		}
	}

	public class StrategyValidatorParameter
	{
		public StrategyValidatorParameter( IEnumerable<IBuilderStrategy> strategies, NamedTypeBuildKey key )
		{
			Strategies = strategies;
			Key = key;
		}

		public IEnumerable<IBuilderStrategy> Strategies { get; }
		public NamedTypeBuildKey Key { get; }
	}

	class ResolutionSpecification : SpecificationBase<ResolutionSpecificationParameter>
	{
		static readonly ISpecification<StrategyValidatorParameter>[] DefaultValidators = { ArrayStrategyValidator.Instance, EnumerableStrategyValidator.Instance };

		readonly IEnumerable<ISpecification<StrategyValidatorParameter>> validators;

		public ResolutionSpecification() : this( DefaultValidators )
		{}

		public ResolutionSpecification( IEnumerable<ISpecification<StrategyValidatorParameter>> validators )
		{
			this.validators = validators;
		}

		static bool CheckInstance( ResolutionSpecificationParameter parameter )
		{
			var result = parameter.Context.Policies.Get<ILifetimePolicy>( parameter.Key ).With( policy => policy.GetValue() ) != null;
			return result;
		}

		static bool CheckRegistered( ResolutionSpecificationParameter parameter )
		{
			var result = parameter.Context.Container.IsRegistered( parameter.Key.Type, parameter.Key.Name ) && !( parameter.Context.Policies.GetNoDefault<IBuildPlanPolicy>( parameter.Key, false ) is DynamicMethodBuildPlan );
			return result;
		}

		bool Check( ResolutionSpecificationParameter parameter )
		{
			var result = CheckInstance( parameter ) || CheckRegistered( parameter ) 
				|| 
				new StrategyValidatorParameter( parameter.Context.Strategies.MakeStrategyChain(), parameter.Key ).With( p => validators.Any( specification => specification.IsSatisfiedBy( p ) ) );
			return result;
		}

		protected override bool IsSatisfiedByParameter( ResolutionSpecificationParameter parameter )
		{
			return base.IsSatisfiedByParameter( parameter ) && Check( parameter );
		}
	}

	public class ResolutionSpecificationParameter
	{
		public ResolutionSpecificationParameter( ExtensionContext context, NamedTypeBuildKey key )
		{
			Context = context;
			Key = key;
		}
		public ExtensionContext Context { get; }
		public NamedTypeBuildKey Key { get; }
	}

	class ResolutionSupport : IResolutionSupport
	{
		readonly ExtensionContext context;
		readonly ISpecification<ResolutionSpecificationParameter> specification;
		readonly IList<NamedTypeBuildKey> resolvable = new List<NamedTypeBuildKey>();

		public ResolutionSupport( ExtensionContext context ) : this( context, new ResolutionSpecification() )
		{}

		public ResolutionSupport( ExtensionContext context, ISpecification<ResolutionSpecificationParameter> specification )
		{
			this.context = context;
			this.specification = specification;
		}
		
		public bool CanResolve( Type type, string name, params object[] parameters )
		{
			var key = new NamedTypeBuildKey( type, name );
			var result = resolvable.Contains( key ) || Validate( key, parameters.NotNull().Select( o => o.GetType() ).ToArray() );
			return result;
		}

		bool Validate( NamedTypeBuildKey key, IEnumerable<Type> parameters )
		{
			var result = specification.IsSatisfiedBy( new ResolutionSpecificationParameter( context, key ) ) || GetConstructor( key ).With( x => Validate( x, parameters ) );
			result.IsTrue( () => resolvable.Add( key ) );
			return result;
		}

		bool Validate( MethodBase constructor, IEnumerable<Type> parameters )
		{
			var result = constructor
				.GetParameters()
				.Where( x => !x.ParameterType.GetTypeInfo().IsValueType )
				.Select( parameterInfo => new NamedTypeBuildKey( parameterInfo.ParameterType ) )
				.All( key => parameters.Any( key.Type.Adapt().IsAssignableFrom ) || specification.IsSatisfiedBy( new ResolutionSpecificationParameter( context, key ) ) );
			return result;
		}

		public ConstructorInfo GetConstructor( NamedTypeBuildKey key )
		{
			var mapped = context.Policies.Get<IBuildKeyMappingPolicy>( key ).With( policy => policy.Map( key, null ) ) ?? key;
			return context.Policies.Get<IConstructorSelectorPolicy>( mapped ).With( policy =>
			{
				var builder = new BuilderContext( context.BuildPlanStrategies.MakeStrategyChain(), context.Lifetime, context.Policies, mapped, null );
				var constructor = policy.SelectConstructor( builder, context.Policies );
				var result = constructor.With( selected => selected.Constructor ); 
				return result;
			} );
		}
	}
}