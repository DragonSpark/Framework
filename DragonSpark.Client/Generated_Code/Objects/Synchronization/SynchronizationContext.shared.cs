using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationContext : ISynchronizationContext
	{
		readonly string firstExpression;
		readonly string secondExpression;
		readonly Type typeConverterType;
		readonly bool allowMirroring;

		public SynchronizationContext( string firstExpression, string secondExpression, Type typeConverterType = null, bool allowMirroring = true )
		{
			Contract.Requires( !string.IsNullOrWhiteSpace( firstExpression ) );
			Contract.Requires( !string.IsNullOrWhiteSpace( secondExpression ) );

			this.firstExpression = firstExpression;
			this.secondExpression = secondExpression;
			this.typeConverterType = typeConverterType;
			this.allowMirroring = allowMirroring;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( !string.IsNullOrWhiteSpace( FirstExpression ) );
			Contract.Invariant( !string.IsNullOrWhiteSpace( SecondExpression ) );
		}*/

		public string SecondExpression
		{
			get { return secondExpression; }
		}

		public string FirstExpression
		{
			get { return firstExpression; }
		}

		public Type TypeConverterType
		{
			get { return typeConverterType; }
		}

		ISynchronizationContext ISynchronizationContext.CreateMirror()
		{
			var result = allowMirroring ? CreateMirror() : this;
			return result;
		}

		protected virtual ISynchronizationContext CreateMirror()
		{
			var result = new SynchronizationContext( SecondExpression, FirstExpression, TypeConverterType );
			return result;
		}

		protected virtual PropertyContext CreateContext( SynchronizationContextInformation context )
		{
			Contract.Assume( context != null && context.Source != null && context.Target != null && context.Target.Property != null );

			var value = ConversionHelper.ConvertTo( context.Source.Value, context.Target.Property, context.ConverterType );
			var result = new PropertyContext( context.Target.Container, context.Target.Property, value, context.Target.Index );
			return result;
		}

		protected PropertyContext ResolvePropertyContext( SynchronizationContainerContext context )
		{
			var information = ResolveInformation( context );
			var result = CreateContext( information );
			return result;
		}

		protected SynchronizationContextInformation ResolveInformation( SynchronizationContainerContext context )
		{
			Contract.Assume( context != null && context.Target != null && context.Source != null );

			var source = context.Source.Evaluate( FirstExpression ).Last.Value;
			var target = context.Target.Evaluate( SecondExpression ).Last.Value;
			var result = new SynchronizationContextInformation( context, source, target, TypeConverterType );
			return result;
		}

		public virtual void Synchronize( SynchronizationContainerContext context )
		{
			var mappingContext = ResolvePropertyContext( context );
			Contract.Assume( mappingContext.Property != null && mappingContext.Container != null );
			try
			{
				mappingContext.Property.SetValue( mappingContext.Container, mappingContext.Value, DragonSparkBindingOptions.AllProperties, null, mappingContext.Index.Transform( x => x.ToArray() ), CultureInfo.CurrentCulture );
			}
			catch ( TargetInvocationException error )
			{
				var message = string.Format( 
					Resources.Message_SynchronizationContext_LoggingMessage, 
					mappingContext.Property.Name,
					mappingContext.Container.GetType(),
					mappingContext.Transform( item => mappingContext.GetType().ToString(), () => "null" ),
					mappingContext,
					error.InnerException.Message
					);

				Logging.Warning( message );

				if ( !context.Container.ContinueOnMappingException )
				{
					throw;
				}
			}
		}

		public override bool Equals(object obj)
		{
			if ( ReferenceEquals( null, obj ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, obj ) )
			{
				return true;
			}
			if ( obj.GetType() != typeof(SynchronizationContext) )
			{
				return false;
			}
			return Equals( (SynchronizationContext)obj );
		}

		public bool Equals( SynchronizationContext other )
		{
			if ( ReferenceEquals( null, other ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, other ) )
			{
				return true;
			}
			return Equals( other.firstExpression, firstExpression ) && Equals( other.secondExpression, secondExpression );
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ( ( firstExpression != null ? firstExpression.GetHashCode() : 0 ) * 397 ) ^ ( secondExpression != null ? secondExpression.GetHashCode() : 0 );
			}
		}
	}
}