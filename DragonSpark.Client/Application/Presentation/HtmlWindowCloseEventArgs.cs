using System.ComponentModel;

namespace DragonSpark.Application.Presentation
{
	public class HtmlWindowCloseEventArgs : CancelEventArgs
	{
		public string DialogMessage { get; set; }
	}
}