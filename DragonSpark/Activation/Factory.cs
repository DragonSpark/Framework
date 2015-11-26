using DragonSpark.Extensions;
using System;

namespace DragonSpark.Activation
{
	public class Factory<TResult> : FactoryBase<Type, TResult> where TResult : class
	{
		readonly IActivator activator;

		public Factory() : this( SystemActivator.Instance )
		{}

		public Factory( IActivator activator )
		{
			this.activator = activator;
		}

		protected override TResult CreateItem( Type parameter )
		{
			var result = activator.Construct<TResult>( parameter );
			return result;
		}
	}

	public static class DelegateSupport
	{
		public static Func<T, T> Self<T>()
		{
			return self => self;
		}
	}

	public class ActivateFactory<TResult> : ActivateFactoryBase<Type, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory() : base( DelegateSupport.Self<Type>() )
		{}
		
		public ActivateFactory( IActivator activator ) : base( activator, DelegateSupport.Self<Type>() )
		{}
	}

	public class ActivateFactoryBase<TParameter, TResult> : FactoryBase<TParameter, TResult> where TResult : class
	{
		readonly Func<TParameter, Type> determineType;
		
		public ActivateFactoryBase( Func<TParameter, Type> determineType ) : this( Activation.Activator.Current, determineType )
		{}

		public ActivateFactoryBase( IActivator activator, Func<TParameter, Type> determineType )
		{
			this.determineType = determineType;
			Activator = activator;
		}

		protected IActivator Activator { get; }

		protected override TResult CreateItem( TParameter parameter )
		{
			var type = determineType( parameter ).Extend().GuardAsAssignable<TResult>( nameof(parameter) );
			var result = Activate<TResult>( type );
			return result;
		}

		protected T Activate<T>( Type type )
		{
			var result = Activator.Activate<T>( type );
			return result;
		}
	}
}