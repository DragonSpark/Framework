using Ploeh.AutoFixture;
using System;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class MetadataCommand : AutoDataCommandBase
	{
		readonly static Func<MethodBase, ImmutableArray<ICustomization>> Factory = MetadataCustomizationFactory<ICustomization>.Default.Get;

		public static MetadataCommand Default { get; } = new MetadataCommand();
		MetadataCommand() : this( Factory ) {}

		readonly Func<MethodBase, ImmutableArray<ICustomization>> factory;
		
		public MetadataCommand( Func<MethodBase, ImmutableArray<ICustomization>> factory )
		{
			this.factory = factory;
		}

		public override void Execute( AutoData parameter )
		{
			foreach ( var customization in factory( parameter.Method ) )
			{
				customization.Customize( parameter.Fixture );
			}
		}
	}
}