using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace DragonSpark.Testing.Framework.Application
{
	public sealed class ApplicationCommandSource : DragonSpark.Application.ApplicationCommandSource
	{
		readonly static Func<MethodBase, ImmutableArray<ICommand<AutoData>>> Factory = MetadataCustomizationFactory<ICommand<AutoData>>.Default.Get;

		public static ApplicationCommandSource Default { get; } = new ApplicationCommandSource();
		ApplicationCommandSource() : base( MethodTypes.Default, Composition.ServiceProviderConfigurations.Default ) {}

		protected override IEnumerable<ICommand> Yield() => 
			base.Yield()
				.Append( MetadataCommand.Default )
				.Concat( Factory( MethodContext.Default.Get() ).AsEnumerable() );
	}
}