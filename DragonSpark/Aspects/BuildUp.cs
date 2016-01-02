using DragonSpark.Extensions;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[PSerializable, AttributeUsage( AttributeTargets.Method | AttributeTargets.Class ), LinesOfCodeAvoided( 1 )]
	public class BuildUp : Attribute, IAspectProvider
	{
		public IEnumerable<AspectInstance> ProvideAspects( object targetElement )
		{
			var items = targetElement.AsTo<TypeInfo, IEnumerable<object>>( info => info.DeclaredConstructors ).NullIfEmpty()
						??
						targetElement.AsTo<MethodInfo, IEnumerable<object>>( info => info.ToItem() );

			var result = items.Select( o => new AspectInstance( o, new BuildUpMethodBoundaryAspect() ) ).ToArray();
			return result;
		}
	}

	[PSerializable, AttributeUsage( AttributeTargets.Method )]
	public class BuildUpMethodBoundaryAspect : OnMethodBoundaryAspect
	{
		public sealed override void OnEntry( MethodExecutionArgs args )
		{
			base.OnEntry( args );

			args.Instance.BuildUp();
		}
	}
}