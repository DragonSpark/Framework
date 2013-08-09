using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Reflection;
using Activator = System.Activator;

namespace DragonSpark.Objects
{
	public class Factory<TResultType> : Factory<object,TResultType> where TResultType : class
	{}

	public class Factory<TMaterial,TResult> : IFactory where TResult : class
	{
		public event EventHandler<ObjectCreatingEventArgs<TMaterial,TResult>> Creating = delegate { };
		public event EventHandler<ObjectCreatedEventArgs<TResult>> Created = delegate { };

		protected virtual TResult CreateItem( TMaterial parameter )
		{
			var result = FromLocation( parameter ) ?? FromActivation( parameter );
			return result;
		}

		protected virtual TResult FromLocation( TMaterial source )
		{
			var result = ServiceLocation.Locate<TResult>();
			return result;
		}

		protected virtual TResult FromActivation( TMaterial source )
		{
			try
			{
				var result = Activator.CreateInstance<TResult>();
				return result;
			}
			catch ( TargetInvocationException error )
			{
				Console.WriteLine( error );
				return null;
			}
		}

		public TResult Create( TMaterial source = default(TMaterial) )
		{
			var args = new ObjectCreatingEventArgs<TMaterial,TResult>( source );
			OnCreating( args );
			var result = args.Result ?? CreateItem( source );
			OnCreated( new ObjectCreatedEventArgs<TResult>( result ) );
			return result;
		}

		protected virtual void OnCreating( ObjectCreatingEventArgs<TMaterial,TResult> args )
		{
			Creating( this, args );
		}

		protected virtual void OnCreated( ObjectCreatedEventArgs<TResult> args )
		{
			Created( this, args );
		}

		object IFactory.Create( Type resultType, object source )
		{
			var result = Create( source.To<TMaterial>() );
			return result;
		}
	}
}