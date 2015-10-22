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
		public static TResult Clone<TResult>( this TResult @this ) where TResult : class
		{
			var result = @this.MapInto<TResult>();
			return result;
		}

		public static IMappingExpression IgnoreUnassignable( this IMappingExpression expression, Type sourceType, Type destinationType )
		{
			var existingMaps = Mapper.FindTypeMapFor( sourceType, destinationType );
			foreach ( var map in existingMaps.GetPropertyMaps() )
			{
				var source = map.SourceMember.To<PropertyInfo>().PropertyType.GetTypeInfo();
				if ( !map.DestinationPropertyType.GetTypeInfo().IsAssignableFrom( source ) )
				{
					expression.ForMember( map.SourceMember.Name, opt => opt.Ignore() );
				}
				
			}
			return expression;
		}

		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = (LambdaExpression)expression;
			var result = ( lambda.Body.AsTo<UnaryExpression, Expression>( unaryExpression => unaryExpression.Operand ) ?? lambda.Body ).To<MemberExpression>().Member;
			return result;
		}

		public static TResult MapInto<TResult>( this object source, TResult existing = null ) where TResult : class 
		{
			var type = source.GetType();
				
			Mapper.FindTypeMapFor( type, typeof(TResult) ).Null( () =>
			{
				Mapper.CreateMap( type, typeof(TResult) ).IgnoreUnassignable( type, typeof(TResult) );
				Mapper.FindTypeMapFor( type, typeof(TResult) ).DestinationCtor = x => existing ?? Activation.Activator.CreateInstance<object>( x.DestinationType );
			} );
			var result = Mapper.Map<TResult>( source );
			return result;
		}

		public static TItem ThrowIfNull<TItem>( this TItem @this, string parameterName = null ) where TItem : class 
		{
			if ( @this == null )
			{
				throw new ArgumentNullException( parameterName ?? "parameter" );
			}
			return @this;
		}

		public static TItem InvalidIfNull<TItem>( this TItem @this, string message = null ) where TItem : class 
		{
			if ( @this == null )
			{
				throw new InvalidOperationException( message ?? "This object is null." );
			}
			return @this;
		}

		public static void TryDispose( this object target )
		{
			target.As<IDisposable>( x => x.Dispose() );
		}

		public static TItem With<TItem>( this TItem target, Action<TItem> action )
		{
			if ( action != null )
			{
				target.NotNull( action );
			}
			return target;
		}

		public static TItem WithValue<TItem>( this TItem? target, Action<TItem> action ) where TItem : struct 
		{
			if ( action != null && target.HasValue )
			{
				target.Value.With( action );
			}
			return target.GetValueOrDefault();
		}

		public static void NotNull<TItem>( this TItem target, Action<TItem> action )
		{
			if ( !Equals( target, default(TItem) ) )
			{
				action( target );
			}
		}

		public static void Null<TItem>( this TItem target, Action action )
		{
			if ( Equals( target, default(TItem) ) )
			{
				action();
			}
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
			// extract = extract ?? ( x => x is TResult ? x.To<TResult>() : default(TResult) );

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
			var result = target.GetAllPropertyValuesOf( typeof(TItem) ).Cast<TItem>().ToArray();
			return result;
		}

		public static IEnumerable GetAllPropertyValuesOf( this object target, Type propertyType )
		{
			var result = target.GetType().GetRuntimeProperties().Where( x => !x.GetIndexParameters().Any() && propertyType.GetTypeInfo().IsAssignableFrom( x.PropertyType.GetTypeInfo() ) ).Select( x => x.GetValue( target, null ) ).ToArray();
			return result;
		}

		public static TResult FromMetadata<TAttribute,TResult>( this object target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TAttribute,bool> condition = null ) where TAttribute : Attribute
		{
			var result = target.Transform( x => ( x as MemberInfo ?? x.GetType().GetTypeInfo() ).FromMetadata( resolveValue, resolveDefault, condition ) );
			return result;
		}

		public static TResult FromMetadata<TAttribute,TResult>( this MemberInfo target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TAttribute,bool> condition = null ) where TAttribute : Attribute
		{
			var result = target.GetCustomAttribute<TAttribute>().Transform( resolveValue, resolveDefault ?? DetermineDefault<TResult>, condition );
			return result;
		}

		public static TResult FromMetadata<TAttribute,TResult>( this Assembly target, Func<TAttribute,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TAttribute,bool> condition = null ) where TAttribute : Attribute
		{
			var result = target.GetCustomAttribute<TAttribute>().Transform( resolveValue, resolveDefault ?? DetermineDefault<TResult>, condition );
			return result;
		}

		public static TItem WithDefaults<TItem>( this TItem target ) where TItem : class
		{
			var provider = Services.Locate<IDefaultValueProvider>() ?? DefaultValueProvider.Instance;
			provider.With( x => x.Apply( target ) );
			return target;
		}

		public static object Evaluate( this object container, string expression )
		{
			var result = Services.With<IExpressionEvaluator, object>( x => x.Evaluate( container, expression ) );
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
					var result = typeArguments.Transform( x => Activator.CreateInstance( x.MakeArrayType(), 0 ) ).To<TResult>();
					return result;
				}
			}
			return default(TResult);
		}

		public static TResult Transform<TItem,TResult>( this TItem target, Func<TItem,TResult> resolveValue, Func<TResult> resolveDefault = null, Func<TItem,bool> condition = null )
		{
			resolveDefault = resolveDefault ?? DetermineDefault<TResult>;
			var result = !Equals( target, default(TItem) ) && ( condition == null || condition( target ) ) ? resolveValue( target ) : resolveDefault();
			return result;
		}

		/*public static TResult Clone<TResult>( this TResult source )
		{
			var result = (TResult)Activator.CreateInstance( source.GetType() );
			var properties = source.GetType().GetRuntimeProperties().Where( x => x.CanRead && x.CanWrite ).NotNull();
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

		public static TResult As<TResult>( this object target ) where TResult : class
		{
			return As<TResult, Exception>( target, null );
		}

		public static TResult As<TResult>( this object target, Action<TResult> action ) where TResult : class
		{
			return As<TResult, Exception>( target, action );
		}

		public static TResult As<TResult, TException>( this object target, Action<TResult> action, Func<TException> resolveException = null ) where TResult : class where TException : Exception
		{
			var result = target as TResult;

			// Apply for exception:
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

		public static TResult AsTo<TSource, TResult>( this object target, Func<TSource,TResult> transform, Func<TResult> resolve = null )
		{
			resolve = resolve ?? DetermineDefault<TResult>;
			var result = target is TSource ? transform( target.To<TSource>() ) : resolve();
			return result;
		}


		public static TResult To<TResult>( this object target )
		{
			var result = (TResult)target;
			return result;
		}

		public static T ConvertTo<T>( this object @this )
		{
			var result = @this.Transform( x => (T)ConvertTo( @this, typeof(T) ) );
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