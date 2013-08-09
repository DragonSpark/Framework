using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DragonSpark;

namespace DragonSpark.Objects.Synchronization
{
	public class SimpleProperty : ISynchronizationExpressionResolver
	{
		readonly string fromPropertyName;
		readonly string toPropertyName;

		public SimpleProperty( string name ) : this( name, name )
		{}

		public SimpleProperty( string fromPropertyName, string toPropertyName )
		{
			this.fromPropertyName = fromPropertyName;
			this.toPropertyName = toPropertyName;
		}

/*
		object ExtractValue( object source )
		{
			var type = source.GetType();
			var property = type.GetProperty( fromPropertyName, DragonSparkBindingFlags.AllProperties );
			var result = property.GetValue( source, DragonSparkBindingFlags.AllProperties, null, null, CultureInfo.CurrentCulture );
			return result;
		}

		public IEnumerable<SynchronizationContext> Expressions
		{
			get { return expressions ?? ( expressions = ResolveExpressions() ); }
		}	IEnumerable<SynchronizationContext> expressions;
*/

		/*ISynchronizationExpressionResolver ISynchronizationExpressionResolver.CreateMirror()
		{
			var result = new SimpleProperty( toPropertyName, fromPropertyName );
			return result;
		}*/

		/*public IEnumerable<PropertyContext> ResolveContexts( SynchronizationContainerContext context )
		{
			var property = context.Target.GetType().GetProperty( toPropertyName, DragonSparkBindingFlags.AllProperties );
			var value = ConversionSupport.CheckForConversion( ExtractValue( context.Source ), property, null );
			var result = new PropertyContext( context.Target, property, value, null );
			yield return result;
		}*/

		#region Implementation of ISynchronizationExpressionResolver
		public IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key )
		{
			yield return new SynchronizationContext( fromPropertyName, toPropertyName );
		}
		#endregion
	}
}