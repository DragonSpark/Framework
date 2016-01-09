using AutoMapper;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class ObjectExtensions
	{
		public static TResult Clone<TResult>( this TResult @this, Action<IMappingExpression> configure = null ) where TResult : class => @this.MapInto<TResult>( configure: configure );

		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = (LambdaExpression)expression;
			var result = ( lambda.Body.AsTo<UnaryExpression, Expression>( unaryExpression => unaryExpression.Operand ) ?? lambda.Body ).To<MemberExpression>().Member;
			return result;
		}

		public static void TryDispose( this object target ) => target.As<IDisposable>( x => x.Dispose() );

		public static void Null<TItem>( this TItem target, Action action ) => target.IsNull().IsTrue( action );

		public static bool IsNull<T>( this T @this ) => Equals( @this, default(T) );

		public static IEnumerable<TItem> Enumerate<TItem>( this IEnumerator<TItem> target )
		{
			var result = new List<TItem>();
			while ( target.MoveNext() )
			{
				result.Add( target.Current );
			}
			return result;
		}

		public static TResult Loop<TItem,TResult>( this TItem current, Func<TItem,TItem> resolveParent, Func<TItem, bool> condition, Func<TItem, TResult> extract = null, TResult defaultValue = default(TResult) )
		{
			do
			{
				if ( condition( current ) )
				{
					var result = extract( current );
					return result;
				}
				current = resolveParent( current );
			}
			while ( current != null );
			return defaultValue;
		}

		public static IEnumerable<TItem> GetAllPropertyValuesOf<TItem>( this object target ) => target.GetAllPropertyValuesOf( typeof( TItem ) ).Cast<TItem>().ToArray();

		public static IEnumerable GetAllPropertyValuesOf( this object target, Type propertyType ) => target.GetType().GetRuntimeProperties().Where( x => !x.GetIndexParameters().Any() && propertyType.GetTypeInfo().IsAssignableFrom( x.PropertyType.GetTypeInfo() ) ).Select( x => x.GetValue( target, null ) ).ToArray();

		public static TItem Use<TItem>( this Func<TItem> @this, Action<TItem> function )
		{
			var item = @this();
			var with = item.With( function );
			return with;
		}

		public static TResult Use<TItem, TResult>( this Func<TItem> @this, Func<TItem, TResult> function, Func<TResult> defaultFunction = null )
		{
			var item = @this();
			return item.With( function, defaultFunction );
		}

		public static TResult With<TItem, TResult>( this TItem target, Func<TItem, TResult> function, Func<TResult> defaultFunction = null )
		{
			var getDefault = defaultFunction ?? DefaultFactory<TResult>.Instance.Create;
			var result = target != null ? function( target ) : getDefault();
			return result;
		}

		public static TItem With<TItem>( this TItem @this, Action<TItem> action )
		{
			var result = @this.With( item =>
			{
				action?.Invoke( item );
				return item;
			} );
			return result;
		}

		public static bool Is<T>( [Required] this object @this ) => @this is T;
		public static bool Not<T>( [Required] this object @this ) => !@this.Is<T>();

		public static TItem WithSelf<TItem>( this TItem @this, Func<TItem, object> action )
		{
			@this.With( action );
			return @this;
		}

		public static TItem With<TItem>( this TItem? @this, Action<TItem> action ) where TItem : struct => @this?.With( action ) ?? default( TItem );

		public static TResult With<TItem, TResult>( this TItem? @this, Func<TItem, TResult> action ) where TItem : struct => @this != null ? @this.Value.With( action ) : default( TResult );

		public static TItem BuildUp<TItem>( [Required]this TItem target ) where TItem : class => ObjectBuilder.Instance.BuildUp<TItem>( target );

		public static TItem BuildUp<TItem>( [Required]this IObjectBuilder @this, TItem target ) where TItem : class
		{
			@this.BuildUp( target );
			return target;
		}

		public static TResult Evaluate<TResult>( [Required]this object container, string expression ) => Evaluate<TResult>( ExpressionEvaluator.Instance, container, expression );

		public static TResult Evaluate<TResult>( [Required]this IExpressionEvaluator @this, object container, string expression ) => (TResult)@this.Evaluate( container, expression );

		// public static TResult AsValid<TItem, TResult>( this object @this, Func<TItem, TResult> with ) => @this.AsValid<TItem>( _ => { } ).With( with );

		public static TItem AsValid<TItem>( this TItem @this, Action<TItem> with ) => AsValid( @this, with, null );

		public static TItem AsValid<TItem>( this object @this, Action<TItem> with ) => AsValid( @this, with, null );

		public static TItem AsValid<TItem>( this object @this, Action<TItem> with, string message )
		{
			var result = @this.As( with );
			result.Null( () =>
			{
				throw new InvalidOperationException( message ?? $"'{@this.GetType().FullName}' is not of type {typeof(TItem).FullName}." );
			} );
			return result;
		}

		public static TResult As<TResult>( this object target ) => As( target, (Action<TResult>)null );

		/*public static TResult As<TResult, TReturn>( this object target, Func<TResult, TReturn> action ) => target.As<TResult>( x => { action( x ); } );*/

		public static TResult As<TResult>( this object target, Action<TResult> action )
		{
			if ( target is TResult )
			{
				var result = (TResult)target;
				result.With( action );
				return result;
			}
			return default(TResult);
		}

		public static TResult AsTo<TSource, TResult>( this object target, Func<TSource,TResult> transform, Func<TResult> resolve = null )
		{
			var @default = resolve ?? DefaultFactory<TResult>.Instance.Create;
			var result = target is TSource ? transform( (TSource)target ) : @default();
			return result;
		}

		public static TResult To<TResult>( this object target ) => (TResult)target;

		public static T ConvertTo<T>( this object @this ) => @this.With( x => (T)ConvertTo( @this, typeof( T ) ) );

		public static object ConvertTo( this object @this, Type to )
		{
			var info = to.GetTypeInfo();
			return !info.IsAssignableFrom( @this.GetType().GetTypeInfo() ) ? ( info.IsEnum ? Enum.Parse( to, @this.ToString() ) : ChangeType( @this, to ) ) : @this;
		}

		static object ChangeType( object @this, Type to )
		{
			try
			{
				return Convert.ChangeType( @this, to );
			}
			catch ( InvalidCastException )
			{
				return null;
			}
		}
	}
}