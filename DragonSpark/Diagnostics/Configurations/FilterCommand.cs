using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Serilog.Configuration;
using Serilog.Core;
using System.Windows.Markup;

namespace DragonSpark.Diagnostics.Configurations
{
	[ContentProperty( nameof(Items) )]
	public class FilterCommand : FilterCommandBase
	{
		public DeclarativeCollection<ILogEventFilter> Items { get; } = new DeclarativeCollection<ILogEventFilter>();

		protected override void Configure( LoggerFilterConfiguration configuration ) => configuration.With( Items.Fixed() );
	}
}