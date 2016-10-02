using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using DragonSpark.Sources.Parameterized.Caching;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[LocationInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	[MulticastAttributeUsage( MulticastTargets.Property, PersistMetaData = false )]
	[PSerializable, ProvideAspectRole( "Data" ), LinesOfCodeAvoided( 6 )]
	[AttributeUsage( AttributeTargets.Assembly )]
	[AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading )]
	public sealed class ApplyDefaultValues : LocationInterceptionAspect
	{
		readonly static ICache<Delegate, ConditionMonitor> Property = new ActivatedCache<Delegate, ConditionMonitor>();
		readonly static Func<PropertyInfo, bool> DefaultSpecification = DefaultValuePropertySpecification.Default.IsSatisfiedBy;
		readonly static Func<DefaultValueParameter, object> DefaultFactory = DefaultPropertyValueFactory.Default.Get;

		readonly Func<PropertyInfo, bool> specification;
		readonly Func<DefaultValueParameter, object> source;

		public ApplyDefaultValues() : this( DefaultSpecification, DefaultFactory ) {}

		ApplyDefaultValues( Func<PropertyInfo, bool> specification, Func<DefaultValueParameter, object> source )
		{
			this.specification = specification;
			this.source = source;
		}

		static bool Apply( object instance, PropertyInfo info ) => Property.Get( Delegates.Default.Get( instance ).Get( info.GetMethod ) ).Apply();

		public override bool CompileTimeValidate( LocationInfo locationInfo ) => specification( locationInfo.PropertyInfo );

		public override void OnGetValue( LocationInterceptionArgs args )
		{
			var instance = args.Instance ?? args.Location.PropertyInfo.DeclaringType;
			var apply = Apply( instance, args.Location.PropertyInfo );
			if ( apply )
			{
				var parameter = new DefaultValueParameter( instance, args.Location.PropertyInfo );
				var value = source( parameter );
				args.SetNewValue( args.Value = value );
			}
			else
			{
				base.OnGetValue( args );
			}
		}

		public override void OnSetValue( LocationInterceptionArgs args )
		{
			Apply( args.Instance ?? args.Location.PropertyInfo.DeclaringType, args.Location.PropertyInfo );
			base.OnSetValue( args );
		}
	}
}