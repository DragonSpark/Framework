using PostSharp.Aspects;
using System;
using System.Linq;

namespace DragonSpark.Aspects.Build
{
	public class SupportDefinition<T> : SupportDefinitionBase where T : IAspect
	{
		public SupportDefinition( params ITypeDefinition[] definitions ) : this( SpecificationFactory.Default.Get( definitions ), definitions.AsEnumerable().Concat().ToArray() ) {}
		public SupportDefinition( params IMethodStore[] methods ) : this( SpecificationFactory.Default.Get( methods ), methods ) {}
		public SupportDefinition( Func<Type, bool> specification, params IMethodStore[] methods ) : this( specification, methods.Select( methodStore => new MethodBasedAspectInstanceLocator<T>( methodStore ) ).ToArray() ) {}
		public SupportDefinition( Func<Type, bool> specification, params IAspectInstanceLocator[] locators ) : base( specification, locators ) {}
	}
}