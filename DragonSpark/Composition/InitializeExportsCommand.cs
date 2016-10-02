using DragonSpark.Application;
using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;
using System.Composition;
using DragonSpark.Aspects.Validation;

namespace DragonSpark.Composition
{
	[ApplyAutoValidation]
	public sealed class InitializeExportsCommand : CommandBase<IServiceProvider>
	{
		public static InitializeExportsCommand Default { get; } = new InitializeExportsCommand();
		InitializeExportsCommand()  {}

		public override void Execute( IServiceProvider parameter ) => Exports.Default.Assign( new ExportProvider( parameter.Get<CompositionContext>() ) );
	}
}