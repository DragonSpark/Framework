using System.Windows;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Commands
{
	[Singleton]
	public class CopyToClipboardCommand : CommandBase<string>
	{
		protected override void Execute( string parameter )
		{
			parameter.NullIfEmpty().NotNull( x => DragonSpark.Runtime.Logging.Try( () => Clipboard.SetText( x ) ) );
		}
	}
}