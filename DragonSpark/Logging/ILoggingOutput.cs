using System.Collections.ObjectModel;

namespace DragonSpark.Logging
{
	public interface ILoggingOutput
	{
		ReadOnlyObservableCollection<string> Output { get;  }
	}
}