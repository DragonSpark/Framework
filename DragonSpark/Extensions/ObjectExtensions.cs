using AutoMapper;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Activator = System.Activator;

namespace DragonSpark.Extensions
{
	public static class ObjectExtensions
	{
		public static TResult Clone<TResult>( this TResult @this, Action<IMappingExpression> configure = null ) where TResult : class
		{
			var result = @this.MapInto<TResult>( configure: configure );
			return result;
		}

		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = (LambdaExpression)expression;
			var result = ( lambda.Body.AsTo<UnaryExpression, Expression>( unaryExpression => unaryExpression.Operand ) ?? lambda.Body ).To<MemberExpression>().Member;
			return result;
		}

		public static void TryDispose( this object target )
		{
			target.As<IDisposable>( x => x.Dispose() );
		}

		public static void Null<TItem>( this TItem target, Action action )
		{
			target.IsNull().IsTrue( action );
		}

		public static bool IsNull<T>( this T @this )
		{
			var result = Equals( @this, default(T) );
			return result;
		}

		/*public static IEnumerable<TItem> ToEnumerable<TItem>( this TItem target, IEnumerable<TItem> others = null )
		{
			var second = others ?? Enumerable.Empty<TItem>();

			var result = new[] { target }.Union( second ).ToArray();
			return result;
		}*/

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

		public static IEnumerable<TItem> GetAllPropertyValuesOf<TItem>( this object target )
		{
			var result = target.GetAllPropertyValuesOf( typeof(TItem) ).Cast<TItem>().ToArray();
			return result;
		}

		public static IEnumerable GetAllPropertyValuesOf( this object target, Type propertyType )
		{
			var result = target.GetType().GetRuntimeProperties().Where( x => !x.GetIndexParameters().Any() && propertyType.GetTypeInfo().IsAssignableFrom( x.PropertyType.GetTypeInfo() ) ).Select( x => x.GetValue( target, null ) ).ToArray();
			return result;
		}

		public static TResult With<TItem, TResult>( this TItem target, Func<TItem, TResult> function, Func<TResult> defaultFunction = null )
		{
			var getDefault = defaultFunction ?? DetermineDefault<TResult>;
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

		public static TItem WithSelf<TItem>( this TItem @this, Func<TItem, object> action )
		{
			@this.With( action );
			return @this;
		}

		public static TItem With<TItem>( this TItem? @this, Action<TItem> action ) where TItem : struct
		{
			var result = @this?.With( action ) ?? default(TItem);
			return result;
		}

		public static TResult With<TItem, TResult>( this TItem? @this, Func<TItem, TResult> action ) where TItem : struct 
		{
			var result = @this != null ? @this.Value.With( action ) : default(TResult);
			return result;
		}

		public static TResult FromMetadata<TAttribute, TResult>( this object target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute
		{
			var result = target.With( x => 
				target.AsTo<Assembly, TResult>( assembly => assembly.FromMetadata( resolveValue, resolveDefault ), () => ( x as MemberInfo ?? x.GetType().GetTypeInfo() ).FromMetadata( resolveValue, resolveDefault ) ) 
			);
			return result;
		}

		public static TResult FromMetadata<TAttribute, TResult>( this MemberInfo target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute
		{
			var result = target.GetCustomAttributes<TAttribute>().WithFirst( resolveValue, resolveDefault ?? DetermineDefault<TResult> );
			return result;
		}

		public static TResult FromMetadata<TAttribute, TResult>( this Assembly target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null ) where TAttribute : Attribute
		{
			var result = target.GetCustomAttributes<TAttribute>().WithFirst( resolveValue, resolveDefault ?? DetermineDefault<TResult> );
			return result;
		}

		public static TItem BuildUp<TItem>( this TItem target ) where TItem : class
		{
			var builder = Services.Location.Locate<IObjectBuilder>() ?? ObjectBuilder.Instance;
			builder.BuildUp( target );
			return target;
		}

		public static object Evaluate( this object container, string expression )
		{
			var result = Services.Location.With<IExpressionEvaluator, object>( x => x.Evaluate( container, expression ) );
			return result;
		}

		public static TResult Evaluate<TResult>( this object container, string expression )
		{
			return (TResult)container.Evaluate( expression );
		}

		public static TResult DetermineDefault<TResult>()
		{
			var type = typeof(TResult).GetTypeInfo();
			if ( type.IsGenericType )
			{
				var typeArguments = type.GenericTypeArguments.First();
				var genericType = typeof(IEnumerable<>).MakeGenericType( typeArguments ).GetTypeInfo();
				if ( genericType.IsAssignableFrom( type ) )
				{
					var result = typeArguments.With( x => Activator.CreateInstance( x.MakeArrayType(), 0 ) ).To<TResult>();
					return result;
				}
			}
			return default(TResult);
		}

		/*public static TResult Clone<TResult>( this TResult source )
		{
			var result = (TResult)Activator.CreateInstance( source.GetType() );
			var properties = source.GetType().GetRuntimeProperties().Where( x => x.CanRead && x.CanWrite ).NotNull();
			properties.Each( x =>
			{
			    if ( typeof(IList).IsAssignableFrom( x.PropertyType ) )
			    {
			        var add = x.ReflectedType.GetMethod( "Add" );
			        if ( add != null )
			        {
			            var enumerable = To<IEnumerable>( x.GetValue( source, null ) );
			            var count = enumerable.Cast<object>().Count();
			            for ( var i = 0; i < count; i++ )
			            {
			                var value = ResolveValue( source, x, new object[]{ i } );
			                add.Invoke( enumerable, new[]{ value } );
			            }
			        }
			    }
			    else
			    {
			        var value = ResolveValue( source, x, null );
			        try
			        {
			            x.SetValue( result, value, null );
			        }
			        catch ( TargetInvocationException error )
			        {
			            Debug.WriteLine( "Could not write property '{0}'.  Error: '{1}'.", x.Name, error.Message );
			        }
			    }                		
			} );
			return result;
		}

		static object ResolveValue( object source, PropertyInfo property, object[] parameters )
		{
			var current = property.GetValue( source, parameters );
			var result = current.As<DependencyObject>().Transform( Clone, () => current );
			return result;
		}*/

		public static TItem AsValid<TItem>( this object @this, Action<TItem> with )
		{
			var result = AsValid( @this, with, null );
			return result;
		}

		public static TItem AsValid<TItem>( this object @this, Action<TItem> with, string message )
		{
			var result = @this.As( with );
			result.Null( () =>
			{
				throw new InvalidOperationException( message ?? $"This object is not of type {typeof(TItem).FullName}." );
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
			var @default = resolve ?? DetermineDefault<TResult>;
			var result = target is TSource ? transform( (TSource)target ) : @default();
			return result;
		}


		public static TResult To<TResult>( this object target )
		{
			var result = (TResult)target;
			return result;
		}

		public static T ConvertTo<T>( this object @this )
		{
			var result = @this.With( x => (T)ConvertTo( @this, typeof(T) ) );
			return result;
		}

		public static object ConvertTo( this object @this, Type to )
		{
			var info = to.GetTypeInfo();
			if ( !info.IsAssignableFrom( @this.GetType().GetTypeInfo() ) )
			{
				return info.IsEnum ? Enum.Parse( to, @this.ToString() ) : ChangeType( @this, to );
			}
			return @this;
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