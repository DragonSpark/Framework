using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Patterns.Contracts;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System.ComponentModel;

namespace DragonSpark.Aspects
{
	public class TrueObject
	{
		public string Other { get; set; }

		[DefaultValue( "Hello World" )]
		public string PropertyName { get; set; }
	}
	
	[MulticastAttributeUsage( MulticastTargets.Property, TargetMemberAttributes = MulticastAttributes.Instance, TargetExternalTypeAttributes = MulticastAttributes.Instance )]
	[PSerializable, ProvideAspectRole( "Default Object Values" ), LinesOfCodeAvoided( 6 )]
	// [AttributeUsage( AttributeTargets.Assembly )]
	public sealed class DefaultValueAspectProvider : LocationInterceptionAspect
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

		[OnInstanceConstructedAdvice]
		public void OnInstanceConstructed()
		{}

/*[OnInstanceConstructedAdvice]
public void OnInstanceConstructed()
{
	/*var type = Instance.GetType();
	Initialize.ToStringArray().Select( type.GetRuntimeProperty ).Each( info =>
	{
		info.GetValue( Instance );
	} );#1#
}*/

		public override bool CompileTimeValidate( LocationInfo locationInfo ) => locationInfo.PropertyInfo.Has<DefaultValueAttribute>() || locationInfo.PropertyInfo.Has<DefaultValueBase>();

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
	public class ApplyDefaultValues : InstanceLevelAspect
	{
		/*public ApplyDefaultValues( string initialize )
		{
			Initialize = initialize;
		}

		public string Initialize { get; set; }*/

		
	}

	// [MulticastAttributeUsage( MulticastTargets.Property/*, TargetMemberAttributes = MulticastAttributes.Instance, TargetExternalTypeAttributes = MulticastAttributes.Instance, AllowExternalAssemblies = true, AllowMultiple = false, PersistMetaData = true */)]
	
	/*public class DefaultValueAspectProvider : MulticastAttribute// , IAspectProvider
	{
		/*public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			var type = (TypeInfo)targetElement;
			// if ( !type.IsDefined(typeof(SerializableAttribute) ) )
			// Message.Write( new Message( MessageLocation.Unknown, SeverityType.Error, "0001", $"HELLO {targetElement.Is<Type>()}", null, null, null ) );
			yield return new AspectInstance( type, new ApplyDefaultValues() );
		}#1#

		IEnumerable<AspectInstance> Create( Type info )
		{
			var properties = BuildablePropertyCollectionFactory.Instance.Create( info );
			var result = properties.Any() ? 
				new AspectInstance( info, 
					new ApplyDefaultValues( /*string.Join( ";", properties.Where( x => x.From<TriggerAttribute, bool>( attribute => attribute.Trigger == DefaultValueTrigger.OnLoad ) ).Select( x => x.Name ) )#1# 
						) 
				)/*.Append( properties.Select( propertyInfo => new AspectInstance( propertyInfo, new DefaultValueAspect() ) ) )#1#.ToItem()
				: Enumerable.Empty<AspectInstance>();
			return result;
		}
	}*/
}