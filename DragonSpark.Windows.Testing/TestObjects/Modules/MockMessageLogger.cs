using DragonSpark.Diagnostics;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	class MockMessageLogger : MessageLoggerBase
	{
		public string LastMessage { get; private set; }
		public string LastMessageCategory;

		protected override void OnLog( Message message )
		{
			LastMessage = message.Text;
			LastMessageCategory = message.Category;
		}
	}
}