using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace DragonSpark.Extensions
{
	public static class ObjectExtensions
	{
		[Pure]
		public static bool IsDefault<TItem>( this TItem target )
		{
			var result = Equals( target, default(TItem) );
			return result;
		}

		public static void TryDispose( this object target )
		{
			target.As<IDisposable>( x => x.Dispose() );
		}

		public static TItem With<TItem>( this TItem target, Action<TItem> action )
		{
			action( target );
			return target;
		}

		public static void NotNull<TItem>( this TItem target, Action<TItem> action )
		{
			Contract.Requires( action != null );

			if ( !Equals( target, default(TItem) ) )
			{
				action( target );
			}
		}

		public static void Null<TItem>( this TItem target, System.Action action )
		{
			Contract.Requires( action != null );

			if ( Equals( target, default(TItem) ) )
			{
				action();
			}
		}

		public static IEnumerable<TItem> ToEnumerable<TItem>( this TItem target, IEnumerable<TItem> others = null )
		{
			Contract.Requires( !Equals( target, default(TItem) ) );
			var second = others ?? Enumerable.Empty<TItem>();

			var result = new[] { target }.Union( second ).ToArray();
			return result;
		}

		public static IEnumerable<TItem> Enumerate<TItem>( this IEnumerator<TItem> target )
		{
			var result = new List<TItem>();
			while ( target.MoveNext() )
			{
				result.Add( target.Current );
			}

			// reset.IsTrue( target.Reset );
			return result;
		}

		public static TResult ResolveFromParent<TItem,TResult>( this TItem current, Func<TItem,TItem> resolveParent, Func<TItem,bool> condition, Func<TItem,TResult> extract = null, TResult defaultValue = default(TResult) )
		{
			Contract.Requires( !Equals( current, default(TItem) ) );
			Contract.Requires( resolveParent != null );
			Contract.Requires( condition != null );

			extract = extract ?? new Func<TItem, TResult>( x => x is TResult ? x.To<TResult>() : default(TResult) );

			do
			{
				if ( condition( current ) )
				{
					var result = extract( current );
					return result;
				}
				current = resolveParent( current );
			}
			while ( !Equals( current, default( TItem ) ) );
			return defaultValue;
		}

		public static IEnumerable<TItem> GetAllPropertyValuesOf<TItem>( this object target )
		{
			var result = target.GetAllPropertyValuesOf( typeof(TItem) ).Cast<TItem>();
			return result;
		}

		public static IEnumerable GetAllPropertyValuesOf( this object target, Type propertyType, BindingFlags? propertyBindings = null )
		{
			var result = target.GetType().GetProperties( propertyBindings ?? DragonSparkBindingOptions.PublicProperties ).Where( x => !x.GetIndexParameters().Any() && propertyType.IsAssignableFrom( x.PropertyType ) ).Select( x => x.GetValue( target, null ) ).ToArray();
			return result;
		}

		public static TResult FromMetadata<TAttribute,TResult>( this object target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TAttribute,bool> condition = null ) where TAttribute : Attribute
		{
			Contract.Requires( target != null );
			Contract.Requires( resolveValue != null );

			var result = target.Transform( x => x.GetType().FromMetadata( resolveValue, resolveDefault, condition ) );
			return result;
		}
		
		public static TResult FromMetadata<TAttribute,TResult>( this ICustomAttributeProvider target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TAttribute,bool> condition = null ) where TAttribute : Attribute
		{
			Contract.Requires( target != null );
			Contract.Requires( resolveValue != null );

			resolveDefault = resolveDefault ?? ResolveResult<TResult>;
			var result = target.GetAttribute<TAttribute>().Transform( resolveValue, resolveDefault, condition );
			return result;
		}

		/*public static TResult TranslateOrNew<TItem,TResult>( this TItem target, Func<TItem,bool> condition, Func<TItem,TResult> resolve ) where TResult : new()
		{
			Contract.Requires( resolve != null );

			var result = target.Transform( resolve, () => new TResult(), condition );
			return result;
		}*/

		static TResult ResolveResult<TResult>()
		{
			var type = typeof(TResult);
			if ( type.IsGenericType )
			{
				var typeArguments = type.GetGenericArguments().First();
				var genericType = typeof(IEnumerable<>).MakeGenericType( typeArguments );
				if ( genericType.IsAssignableFrom( type ) )
				{
					var result = typeArguments.Transform( x => Activator.CreateInstance( x.MakeArrayType(), 0 ) ).To<TResult>();
					return result;
				}
			}
			return default(TResult);
		}

		public static TResult Transform<TItem,TResult>( this TItem target, Func<TItem,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TItem,bool> condition = null )
		{
			Contract.Requires( resolveValue != null );

			resolveDefault = resolveDefault ?? ResolveResult<TResult>;
			var result = !Equals( target, default(TItem) ) && ( condition == null || condition( target ) ) ? resolveValue( target ) : resolveDefault();
			return result;
		}

		public static TResult Clone<TResult>( this TResult source )
		{
			Contract.Requires( !Equals( source, default(TResult) ) );

			var result = (TResult)Activator.CreateInstance( source.GetType() );
			var properties = source.GetType().GetProperties().Where( x => x.CanRead && x.CanWrite ).NotNull();
			properties.Apply( x =>
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
			            Debug.WriteLine( string.Format( "Could not write property '{0}'.  Error: '{1}'.", x.Name, error.Message ) );
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
		}

		public static TResult As<TResult>( this object target ) where TResult : class
		{
			return As<TResult,Exception>( target, null, null );
		}

		public static TResult As<TResult>( this object target, Action<TResult> action ) where TResult : class
		{
			return As<TResult,Exception>( target, action, null );
		}

		public static TResult AsTo<TSource,TResult>( this object target, Func<TSource,TResult> transform )
		{
			var result = target is TSource ? transform( target.To<TSource>() ) : ResolveResult<TResult>();
			return result;
		}

		/*public static TResult As<TResult,TException>( this object target, Func<TException> resolveException ) where TResult : class where TException : Exception
		{
			return As<TResult, TException>( target, resolveException, null );
		}*/

		public static TResult As<TResult,TException>( this object target, Action<TResult> action, Func<TException> resolveException = null ) where TResult : class where TException : Exception
		{
			var result = target as TResult;

			// Check for exception:
			if ( result == null && resolveException != null )
			{
				throw resolveException();
			}

			if ( action != null && result != null )
			{
				action( result );
			}
			return result;
		}

		public static TResult To<TResult>( this object target )
		{
			var result = (TResult)target;
			return result;
		}
	}
}