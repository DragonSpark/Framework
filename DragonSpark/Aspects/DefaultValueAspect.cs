using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Patterns.Contracts;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PostSharp;
using PostSharp.Aspects.Advices;
using PostSharp.Reflection;

namespace DragonSpark.Aspects
{
	[AttributeUsage( AttributeTargets.Property )]
	public class TriggerAttribute : Attribute
	{
		public TriggerAttribute( DefaultValueTrigger trigger )
		{
			Trigger = trigger;
		}

		public DefaultValueTrigger Trigger { get; }
	}

	public enum DefaultValueTrigger
	{
		OnDemand,
		OnLoad
	}

	[PSerializable, ProvideAspectRole( "Default Object Values" ), LinesOfCodeAvoided( 6 )]
	public class DefaultValueAspect : LocationInterceptionAspect
	{
		/*readonly DefaultValueTrigger trigger;

		public DefaultValueAspect( DefaultValueTrigger trigger )
		{
			this.trigger = trigger;
		}

		public override void RuntimeInitialize( LocationInfo locationInfo )
		{
			base.RuntimeInitialize( locationInfo );
			switch ( trigger )
			{
				case DefaultValueTrigger.OnLoad:
					locationInfo.
					// locationInfo.PropertyInfo.GetValue( )
					break;
			}
		}*/

		public override void OnGetValue( LocationInterceptionArgs args )
		{
			var context = new Context( args );
			var apply = context.Monitor.Apply();
			if ( apply )
			{
				args.SetNewValue( args.Value = DefaultPropertyValueFactory.Instance.Create( new DefaultValueParameter( context.Invocation.Item1, args.Location.PropertyInfo ) ) );
			}
			else
			{
				base.OnGetValue( args );
			}
		}

		class Context
		{
			public Context( LocationInterceptionArgs args ) : this( new Invocation( args.Instance ?? args.Location.PropertyInfo.DeclaringType, args.Location.PropertyInfo, new EqualityList() ) ) { }

			Context( [Required]Invocation invocation ) : this( new InvocationReference( invocation ) ) { }

			Context( InvocationReference reference ) : this( reference.Item, new Checked( reference.Item ).Item ) { }

			Context( [Required]Invocation invocation, [Required]ConditionMonitor monitor )
			{
				Invocation = invocation;
				Monitor = monitor;
			}

			public Invocation Invocation { get; }
			public ConditionMonitor Monitor { get; }
		}

		public override void OnSetValue( LocationInterceptionArgs args )
		{
			new Context( args ).Monitor.Apply();
			base.OnSetValue( args );
		}
	}
	
	[PSerializable]
	public class DefaultValueHost : InstanceLevelAspect
	{
		/*public DefaultValueHost( string initialize )
		{
			Initialize = initialize;
		}

		public string Initialize { get; set; }*/

		/*[OnInstanceConstructedAdvice]
		public void OnInstanceConstructed()
		{
			/*var type = Instance.GetType();
			Initialize.ToStringArray().Select( type.GetRuntimeProperty ).Each( info =>
			{
				info.GetValue( Instance );
			} );#1#
		}*/
	}

	[MulticastAttributeUsage( MulticastTargets.Class/*, TargetMemberAttributes = MulticastAttributes.Instance, TargetExternalTypeAttributes = MulticastAttributes.Instance, AllowExternalAssemblies = true, AllowMultiple = false, PersistMetaData = true */)]
	[AttributeUsage( AttributeTargets.Assembly ), LinesOfCodeAvoided( 6 )]
	public class DefaultValueAspectProvider : MulticastAttribute, IAspectProvider
	{
		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			var type = (TypeInfo)targetElement;
			if ( !type.IsDefined(typeof(SerializableAttribute) ) )
			// Message.Write( new Message( MessageLocation.Unknown, SeverityType.Error, "0001", $"HELLO {targetElement.Is<Type>()}", null, null, null ) );
			yield return new AspectInstance( type, new DefaultValueHost() );
		}

		IEnumerable<AspectInstance> Create( Type info )
		{
			var properties = BuildablePropertyCollectionFactory.Instance.Create( info );
			var result = properties.Any() ? 
				new AspectInstance( info, 
					new DefaultValueHost( /*string.Join( ";", properties.Where( x => x.From<TriggerAttribute, bool>( attribute => attribute.Trigger == DefaultValueTrigger.OnLoad ) ).Select( x => x.Name ) )*/ 
						) 
				)/*.Append( properties.Select( propertyInfo => new AspectInstance( propertyInfo, new DefaultValueAspect() ) ) )*/.ToItem()
				: Enumerable.Empty<AspectInstance>();
			return result;
		}
	}
}