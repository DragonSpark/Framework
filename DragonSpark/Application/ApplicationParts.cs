using DragonSpark.Sources;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public sealed class ApplicationParts : Scope<SystemParts>
	{
		public static ImmutableArray<Type> Assign( params Type[] parts )
		{
			var assigned = SystemPartsFactory.Default.Get( parts );
			Default.Configured( assigned ).Execute();
			var result = assigned.Types;
			return result;
		}

		public static IScope<SystemParts> Default { get; } = new ApplicationParts();
		ApplicationParts() : base( () => SystemParts.Default ) {}

		public static bool IsAssigned => !Equals( Default.Get(), SystemParts.Default );
	}
}