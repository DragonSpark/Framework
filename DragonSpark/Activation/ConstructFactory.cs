using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Activation
{
	public class ConstructFactoryParameterQualifier<TResult> : FactoryParameterQualifier<ConstructParameter>
	{
		public new static ConstructFactoryParameterQualifier<TResult> Instance { get; } = new ConstructFactoryParameterQualifier<TResult>();

		protected override ConstructParameter Construct( object parameter )
		{
			var result = parameter.AsTo<Type, ConstructParameter>( type => typeof(TResult).Extend().IsAssignableFrom( type ) ? base.Construct( parameter ) : null ) ?? new ConstructParameter( typeof(TResult), parameter );
			return result;
		}
	}

	public class ConstructFactory<TResult> : ActivationFactory<ConstructParameter, TResult> where TResult : class
	{
		public ConstructFactory() : this( SystemActivator.Instance )
		{}

		public ConstructFactory( IActivator activator ) : this( activator, ConstructFactoryParameterQualifier<TResult>.Instance )
		{}

		public ConstructFactory( IActivator activator, IFactoryParameterQualifier<ConstructParameter> qualifier ) : base( activator, qualifier )
		{}

		protected override TResult Activate( Type qualified, ConstructParameter parameter )
		{
			var result = Activator.Construct<TResult>( qualified, parameter.Arguments );
			return result;
		}
	}

	public class ConstructParameter : ActivationParameter
	{
		public ConstructParameter( TypeExtension type ) : this( type, new object[] { } )
		{}

		public ConstructParameter( TypeExtension type, params object[] arguments ) : base( type )
		{
			Arguments = arguments;
		}

		public object[] Arguments { get; }

		/*public static explicit operator ConstructParameter( Type type )
		{
			var result = new ConstructParameter( type );
			return result;
		}*/
	}

	public class ActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory()
		{}

		public ActivateFactory( IActivator activator ) : base( activator )
		{}

		/*protected override ActivateParameter QualifyParameter( object parameter )
		{
			return base.QualifyParameter( parameter ) ?? new ActivateParameter( typeof(TResult) );
		}*/
	}

	public static class ActivateFactoryExtensions
	{
		public static T CreateUsing<T>( this IFactory<ActivateParameter, T> @this, Type type )
		{
			var result = @this.Create( new ActivateParameter( type ) );
			return result;
		}
	}

	public class ActivateFactory<TParameter, TResult> : ActivationFactory<TParameter, TResult> where TResult : class where TParameter : ActivateParameter
	{
		public ActivateFactory()
		{}

		public ActivateFactory( IActivator activator ) : base( activator )
		{}

		protected override TResult Activate( Type qualified, TParameter parameter )
		{
			var result = Activator.Activate<TResult>( qualified, parameter.Name );
			return result;
		}
	}

	public class ActivateParameter : ActivationParameter
	{
		public ActivateParameter( TypeExtension type ) : this( type, null )
		{}

		public ActivateParameter( TypeExtension type, string name ) : base( type )
		{
			Name = name;
		}

		public string Name { get; }
	}

	public abstract class ActivationParameter
	{
		protected ActivationParameter( TypeExtension type )
		{
			Type = type;
		}

		public TypeExtension Type { get; }

	}

	public abstract class ActivationFactory<TParameter, TResult> : FactoryBase<TParameter, TResult> where TParameter : ActivationParameter where TResult : class
	{
		protected ActivationFactory() : this( Activation.Activator.Current )
		{}

		protected ActivationFactory( IActivator activator ) : this( activator, FactoryParameterQualifier<TParameter>.Instance )
		{
		}

		protected ActivationFactory( IActivator activator, IFactoryParameterQualifier<TParameter> qualifier ) : base( qualifier )
		{
			Activator = activator;
		}

		protected IActivator Activator { get; }

		protected override TResult CreateItem( TParameter parameter )
		{
			var qualified = DetermineType( parameter ).Transform( extension => extension.GuardAsAssignable<TResult>( nameof(parameter) ) );
			var result = qualified.Transform( type => Activate( type, parameter ) );
			return result;
		}

		protected virtual TypeExtension DetermineType( TParameter parameter )
		{
			return parameter.Type;
		}

		protected abstract TResult Activate( Type qualified, TParameter parameter );

		/*protected override TParameter DetermineParameter( object parameter )
		{
			var result = parameter.AsTo<Type, TParameter>( type => (TParameter)type ) ?? base.DetermineParameter( parameter );
			/*var asdf = parameter.Transform( o => typeof(TParameter).Extend().IsAssignableFrom( o.GetType() ) );
			var qualified = typeof(TParameter).Extend().IsInstanceOfType( parameter ) ? (TParameter)parameter : default(TParameter);#1#

			return result;
		}*/
	}
}