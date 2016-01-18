using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Reflection;
using PostSharp.Serialization;
using System;
using System.ComponentModel;

namespace DragonSpark.Aspects
{
	[MulticastAttributeUsage( MulticastTargets.Property,  PersistMetaData = false ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )]
	[PSerializable, ProvideAspectRole( "Default Object Values" ), LinesOfCodeAvoided( 6 )]
	[AttributeUsage( AttributeTargets.Assembly | AttributeTargets.Class )]
	public sealed class ApplyDefaultValues : LocationInterceptionAspect, IInstanceScopedAspect
	{
		public override bool CompileTimeValidate( LocationInfo locationInfo ) => DefaultValuePropertySpecification.Instance.IsSatisfiedBy( locationInfo.PropertyInfo );

		public override void OnGetValue( LocationInterceptionArgs args )
		{
			var apply = new Checked( this ).Item.Apply();
			if ( apply )
			{
				var parameter = new DefaultValueParameter( args.Instance ?? args.Location.DeclaringType, args.Location.PropertyInfo );
				var value = DefaultPropertyValueFactory.Instance.Create( parameter );
				args.SetNewValue( args.Value = value );
			}
			else
			{
				base.OnGetValue( args );
			}
		}

		public override void OnSetValue( LocationInterceptionArgs args )
		{
			new Checked( this ).Item.Apply();
			base.OnSetValue( args );
		}

		object IInstanceScopedAspect.CreateInstance( AdviceArgs adviceArgs ) => MemberwiseClone();

		void IInstanceScopedAspect.RuntimeInitializeInstance() {}
	}
}