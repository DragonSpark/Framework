using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Synchronization
{
	public static class Expression
	{
		static readonly char[]
			ExpressionPartSeparator = new[] { '.' },
			IndexExprStartChars = new[] { '[', '(' },
			IndexExprEndChars = new[] { ']', ')' };

		#region DataBinding Hackist:
		public static object EvaluateValue( this object container, string expression )
		{
			Contract.Requires( expression != null && !string.IsNullOrWhiteSpace( expression ) );
			var result = container.EvaluateValue<object>( expression );
			return result;
		}

		public static TResult EvaluateValue<TResult>( this object container, string expression )
		{
			Contract.Requires( expression != null && !string.IsNullOrWhiteSpace( expression ) );
			var result = Evaluate( container, expression ).Transform( x => x.Last.Value.Value.To<TResult>() );
			return result;
		}

		public static LinkedList<PropertyContext> Evaluate( this object container, string expression, bool extractValue = true, bool throwOnException = false )
		{
			Contract.Requires( expression != null && !string.IsNullOrWhiteSpace( expression ) );
			expression = expression.Trim();
			if ( container == null )
			{
				return null;
			}

			if ( expression == ExpressionPartSeparator.First().ToString() )
			{
				var containerInstance = new Container( container );
				return new LinkedList<PropertyContext>( new PropertyContext( containerInstance, Container.ContentsProperty, container, null ).ToEnumerable() );
			}

			try
			{
				var expressionParts = expression.Split( ExpressionPartSeparator );
				var result = Evaluate( container, expressionParts, extractValue );
				return result;
			}
			catch ( Exception )
			{
				if ( throwOnException )
				{
					throw;
				}
				return null;
			}
		}

		static LinkedList<PropertyContext> Evaluate( object container, string[] expressionParts, bool processValue )
		{
			Contract.Assume( expressionParts != null );
			var result = new LinkedList<PropertyContext>();
			var propertyValue = container;
			for ( var i = 0; ( i < expressionParts.Length ) && ( propertyValue != null ); i++ )
			{
				var propName = expressionParts[ i ];
				PropertyInfo descriptor;
				var currentContainer = propertyValue;
				object[] index = null;
				propertyValue = propName.IndexOfAny( IndexExprStartChars ) < 0
				                	? GetPropertyValue( propertyValue, propName, out descriptor, processValue )
				                	: GetIndexedPropertyValue( propertyValue, propName, out descriptor, out index, processValue );
				result.AddLast( new PropertyContext( currentContainer, descriptor, propertyValue, index ) );
			}
			return result;
		}

		static object GetPropertyValue( object container, string propName, out PropertyInfo descriptor, bool processValue )
		{
			Contract.Assume( container != null );
			Contract.Assume( propName != null );

			descriptor = ResolveDescriptor( container, propName );
			if ( descriptor == null )
			{
				throw new InvalidOperationException( string.Format( "Property '{0}' Not found on type '{1}'", propName,
				                                                    container.GetType().FullName ) );
			}
			return processValue ? descriptor.GetValue( container, null ) : false;
		}

		static PropertyInfo ResolveDescriptor( object container, string propName )
		{
			Contract.Assume( container != null );
			Contract.Assume( propName != null );

			var result = container.GetType().GetProperty( propName, DragonSparkBindingOptions.AllProperties );
			if ( result == null )
			{
				var type = container as Type;
				if ( type != null )
				{
					var property = type.GetProperty( propName, DragonSparkBindingOptions.AllProperties );
					return property;
				}
			}
			return result;
		}

		static object GetIndexedPropertyValue( object container, string expr, out PropertyInfo descriptor, out object[] index, bool processValue )
		{
			Contract.Assume( expr != null );

			index = null;
			descriptor = null;
			var flag = false;
			var length = expr.IndexOfAny( IndexExprStartChars );
			var startIndex = length + 1;
			var num2 = expr.IndexOfAny( IndexExprEndChars, startIndex );
			if ( ( ( length < 0 ) || ( num2 < 0 ) ) || ( num2 == startIndex ) )
			{
				throw new ArgumentException( string.Format( "Invalid Indexed Expression: {0}.", new object[] { expr } ) );
			}
			string propName = null;
			object obj3 = null;
			Contract.Assume( startIndex <= expr.Length );
			var length1 = ( num2 - length ) - 1;
			Contract.Assume( 0 <= length1 );
			var s = expr.Substring( startIndex, length1 ).Trim();
			if ( length != 0 )
			{
				propName = expr.Substring( 0, length );
			}
			if ( s.Length != 0 )
			{
				if ( ( ( s[ 0 ] == '"' ) && ( s[ s.Length - 1 ] == '"' ) ) ||
				     ( ( s[ 0 ] == '\'' ) && ( s[ s.Length - 1 ] == '\'' ) ) )
				{
					var i1 = s.Length - 2;
					Contract.Assume( 0 <= i1 );
					obj3 = s.Substring( 1, i1 );
				}
				else if ( char.IsDigit( s[ 0 ] ) )
				{
					int result;
					flag = int.TryParse( s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result );
					if ( flag )
					{
						obj3 = result;
					}
					else
					{
						obj3 = s;
					}
				}
				else
				{
					obj3 = s;
				}
			}
			if ( obj3 == null )
			{
				throw new ArgumentException( string.Format( "Invalid Indexed Expression: {0}.", new object[] { expr } ) );
			}
			var propertyValue = !string.IsNullOrEmpty( propName ) ? GetPropertyValue( container, propName, out descriptor, processValue ) : container;
			if ( propertyValue == null )
			{
				return null;
			}
			var array = propertyValue as Array;
			var i = (int)obj3;
			if ( ( array != null ) && flag )
			{
				Contract.Assume( 0 < array.Rank );
				Contract.Assume( i >= array.GetLowerBound( 0 ) );
				Contract.Assume( i <= array.GetUpperBound( 0 ) );
				return array.GetValue( i );
			}
			if ( ( propertyValue is IList ) && flag )
			{
				var list = propertyValue.To<IList>();
				Contract.Assume( i >= 0 && i < list.Count );
				return list[ i ];
			}
			var info =
				propertyValue.GetType().GetProperty( "Item", BindingFlags.Public | BindingFlags.Instance, null, null,
				                                     new[] { obj3.GetType() }, null );
			if ( info == null )
			{
				throw new ArgumentException(
					string.Format( "No Indexed Accessor: {0}", new object[] { propertyValue.GetType().FullName } ) );
			}
			index = new[] { obj3 };
			return info.GetValue( propertyValue, index );
		}
		#endregion
	}
}