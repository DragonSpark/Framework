using DragonSpark.ComponentModel;
using DragonSpark.TypeSystem;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Type = System.Type;

namespace DragonSpark.Extensions
{
	public static class ObjectExtensions
	{
		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = (LambdaExpression)expression;
			var result = ( lambda.Body.AsTo<UnaryExpression, Expression>( unaryExpression => unaryExpression.Operand ) ?? lambda.Body ).To<MemberExpression>().Member;
			return result;
		}

		public static void TryDispose( this object target ) => target.As<IDisposable>( x => x.Dispose() );

		public static bool IsAssignedOrValue<T>( [Optional]this T @this ) => IsAssigned( @this, true );

		public static bool IsAssigned<T>( [Optional]this T @this ) => IsAssigned( @this, false );

		static bool IsAssigned<T>( [Optional]this T @this, bool value )
		{
			var type = @this?.GetType() ?? typeof(T);
			var result = type.GetTypeInfo().IsValueType ? value || !SpecialValues.DefaultOrEmpty( type ).Equals( @this ) : !Equals( @this, default(T) );
			return result;
		}

		public static bool IsAssignedOrContains<T>( [Optional]this T @this ) => !Equals( @this, SpecialValues.DefaultOrEmpty<T>() );

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
			while ( current.IsAssigned() );
			return defaultValue;
		}

		public static TResult With<TItem, TResult>( [Optional]this TItem target, Func<TItem, TResult> function, Func<TResult> defaultFunction = null )
		{
			var getDefault = defaultFunction ?? Support<TResult>.Default;
			var result = target.IsAssigned() ? function( target ) : getDefault();
			return result;
		}

		static class Support<T>
		{
			public static Func<T> Default { get; } = SpecialValues.DefaultOrEmpty<T>;
		}

		public static TItem With<TItem>( [Optional]this TItem @this, Action<TItem> action = null )
		{
			if ( @this.IsAssigned() )
			{
				action?.Invoke( @this );
				return @this;
			}
			return default(TItem);
		}

		public static bool Is<T>( this object @this ) => @this is T;
		public static bool Not<T>( this object @this ) => !@this.Is<T>();

		public static TItem WithSelf<TItem>( [Optional]this TItem @this, Func<TItem, object> action )
		{
			if ( @this.IsAssigned() )
			{
				action( @this );
			}
			return @this;
		}

		public static T With<T>( [Optional]this T? @this, Action<T> action ) where T : struct => @this?.With( action ) ?? default(T);

		public static TResult With<TItem, TResult>( [Optional]this TItem? @this, Func<TItem, TResult> action ) where TItem : struct => @this != null ? @this.Value.With( action ) : default( TResult );

		public static TResult Evaluate<TResult>( this object container, string expression ) => Evaluate<TResult>( ExpressionEvaluator.Default, container, expression );

		public static TResult Evaluate<TResult>( this IExpressionEvaluator @this, object container, string expression ) => (TResult)@this.Evaluate( container, expression );

		public static T AsValid<T>( this object @this, Action<T> with = null, string message = null )
		{
			if ( !( @this is T ) )
			{
				throw new InvalidOperationException( message ?? $"'{@this.GetType().FullName}' is not of type {typeof(T).FullName}." );
			}

			var result = with != null ? @this.As( with ) : (T)@this;
			return result;
		}

		public static T As<T>( [Optional]this object target ) => target is T ? (T)target : default(T);

		public static T As<T>( [Optional]this object target, Action<T> action )
		{
			if ( target is T )
			{
				var result = (T)target;
				action( result );
				return result;
			}
			return default(T);
		}

		public static TResult AsTo<TSource, TResult>( this object target, Func<TSource,TResult> transform, Func<TResult> resolve = null )
		{
			var @default = resolve ?? ( () => default(TResult) );
			var result = target is TSource ? transform( (TSource)target ) : @default();
			return result;
		}

		public static TResult To<TResult>( this object target ) => (TResult)target;

		public static T ConvertTo<T>( this object @this ) => @this.IsAssigned() ? (T)ConvertTo( @this, typeof(T) ) : default(T);

		public static object ConvertTo( this object @this, Type to ) => !to.Adapt().IsInstanceOfType( @this ) ? ( to.GetTypeInfo().IsEnum ? Enum.Parse( to, @this.ToString() ) : ChangeType( @this, to ) ) : @this;

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