using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Commands
{
	public class CommandMonitorOptions : ViewObject
	{
		public static CommandMonitorOptions Default
		{
			get { return DefaultField; }
		}	static readonly CommandMonitorOptions DefaultField = new CommandMonitorOptions().WithDefaults();

		public string Title { get; set; }

		[DefaultPropertyValue( true )]
		public bool? AllowCancel { get; set; }

		[DefaultPropertyValue( true )]
		public bool? CloseOnCompletion { get; set; }

		public bool AnyExceptionIsFatal { get; set; }
	}
}